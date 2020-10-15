// Name: Colin Campbell
// Class: Graded Unit
// Project description: A stock system to be used within the clothing department of Sainsburys (Tu Clothing)
// Version: 1.00
// Date: 07/02/2018

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tu_Stock_System
{
    public partial class StockAdd : Form, IFormMethods
    {
        // connection string used to connect to local database
        // used in all SQL queries
        SqlConnection myConnection = new SqlConnection("Server=.;Database=Tu;Trusted_Connection=True;");

        // global employee object to be used in the constructor of any other form opened
        Employee currentUser = new Employee();

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StockAdd()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Second constructor which takes one employee object.
        /// </summary>
        /// <param name="employeeIn">Employee that's currently logged in.</param>
        public StockAdd(Employee employeeIn)
        {
            InitializeComponent();

            currentUser = employeeIn;
        }

        // Events

        /// <summary>
        /// When the form loads, populate both combo boxes with appropriate information from database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StockAdd_Load(object sender, EventArgs e)
        {
            //myConnection.Open();
            //SQLLoadComboBoxDepartments();
            //SQLLoadComboBoxLocations();
            //myConnection.Close();
        }

        /// <summary>
        /// If a user enters a space, alert them this isn't allowed and clear the text box.
        /// If a barcode has been entered, highlight the next text box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBarcode_TextChanged(object sender, EventArgs e)
        {
            if (txtBarcode.Text.Contains(" "))
            {
                MessageBox.Show(this, "Spaces are not allowed in the barcode text box.", "Invalid character", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // code used to ensure that the cursor is set to the start
                // of the text box. Will not work without BeginInvoke...
                this.BeginInvoke((MethodInvoker)delegate ()
                {
                    txtBarcode.ResetText();
                    txtBarcode.Select(0, 0);
                });
            }

            else if (txtBarcode.Text.Length == 13)
            {
                txtStyle.Focus();
            }
        }

        /// <summary>
        /// If a user enters a space, alert them this isn't allowed and clear the text box.
        /// If a style number has been entered, highlight the next text box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtStyle_TextChanged(object sender, EventArgs e)
        {
            if (txtStyle.Text.Contains(" "))
            {
                MessageBox.Show(this, "Spaces are not allowed in the style number text box.", "Invalid character", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // code used to ensure that the cursor is set to the start
                // of the text box. Will not work without BeginInvoke...
                this.BeginInvoke((MethodInvoker)delegate ()
                {
                    txtStyle.ResetText();
                    txtStyle.Select(0, 0);
                });
            }

            else if (txtStyle.Text.Length == 9)
            {
                txtSKU.Focus();
            }
        }

        /// <summary>
        /// If a user enters a space, alert them this isn't allowed and clear the text box.
        /// If a SKU has been entered, highlight the next text box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSKU_TextChanged(object sender, EventArgs e)
        {
            if (txtSKU.Text.Contains(" "))
            {
                MessageBox.Show(this, "Spaces are not allowed in the SKU text box.", "Invalid character", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // code used to ensure that the cursor is set to the start
                // of the text box. Will not work without BeginInvoke...
                this.BeginInvoke((MethodInvoker)delegate ()
                {
                    txtSKU.ResetText();
                    txtSKU.Select(0, 0);
                });
            }

            else if (txtSKU.Text.Length == 9)
            {
                comboBoxDepartments.Focus();
            }
        }

        /// <summary>
        /// Takes values from text boxes and populates attributes of Clothing object.
        /// Will then call custom SQL methods to add records to stock table and stock update table.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddStock_Click(object sender, EventArgs e)
        {
            Clothing stockBeingAdded = new Clothing();
            SqlDataReader myReader = null;

            // alert user if the masked text boxes are complete with insufficient length
            if (txtBarcode.Text.Length != 13 || txtStyle.Text.Length != 9 || txtSKU.Text.Length != 9)
            {
                MessageBox.Show(this, "Please make sure the barcode is 13 numbers long and the style number/SKU are both 9 numbers long.", "Invalid field length", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                try
                {
                    myConnection.Open();

                    if (AllFieldsCompleted() == false)
                    {
                        MessageBox.Show(this, "Please make sure all fields have been filled in where appropriate.", "Fields missing input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    else
                    {
                        if (SQLItemExists(txtBarcode.Text, myReader))
                        {
                            MessageBox.Show(this, "Unable to add stock - an item with this barcode has already been added.", "Item already exists", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        else
                        {
                            if (FieldsAreValid(txtQuantity, txtPrice))
                            {
                                stockBeingAdded.SetBarcode(txtBarcode.Text);
                                stockBeingAdded.SetStyleNum(txtStyle.Text);
                                stockBeingAdded.SetSKU(txtSKU.Text);
                                stockBeingAdded.SetDepartment(comboBoxDepartments.SelectedItem.ToString());
                                stockBeingAdded.SetSize(txtSize.Text);
                                stockBeingAdded.SetPrice(decimal.Parse(txtPrice.Text));

                                if (radioButtonYes.Checked == true)
                                {
                                    stockBeingAdded.SetOnShopFloor("Yes");
                                    stockBeingAdded.SetLocation(comboBoxLocations.SelectedItem.ToString());

                                    SQLGetLocation(stockBeingAdded, myReader);
                                }

                                else
                                {
                                    stockBeingAdded.SetOnShopFloor("No");
                                    stockBeingAdded.SetLocation("");
                                }

                                stockBeingAdded.SetQuantity(ushort.Parse(txtQuantity.Text));

                                SQLGetDepartment(stockBeingAdded, myReader);
                                SQLAddStock(stockBeingAdded);
                                SQLUpdateStockAdd(stockBeingAdded);

                                myConnection.Close();

                                MessageBox.Show(this, "Stock added successfully.", "Stock added", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                ClearForm();
                                txtBarcode.Focus();
                            }

                            else
                            {
                                MessageBox.Show(this, "Stock not added - please make sure that quantity is within the range of 0 - 65535 and price is within 0.01 - 99.99.", "Stock not added", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }

                catch (OverflowException)
                {
                    MessageBox.Show(this, "Please make sure that quantity is within the range of 0 - 65535.", "Invalid quantity", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                catch (FormatException)
                {
                    MessageBox.Show(this, "Stock not added - please make sure that both quantity and price are numbers.", "Stock not added", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                //close connection in case of error
                finally
                {
                    myConnection.Close();
                }
            }
        }

        /// <summary>
        /// If the stock will have a location, allow user to see the appropriate input fields.
        /// Otherwise hide them.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonYes_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonYes.Checked == true)
            {
                comboBoxLocations.Visible = true;
                lblLocation.Visible = true;
            }

            else
            {
                comboBoxLocations.Text = null;
                comboBoxLocations.Visible = false;
                lblLocation.Visible = false;
            }
        }

        /// <summary>
        /// When the quantity text box has focus, let the user know exactly what is meant by quantity.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtQuantity_Enter(object sender, EventArgs e)
        {
            ToolTip toolTip1 = new ToolTip();
            toolTip1.ToolTipTitle = "Quantity";
            toolTip1.Show("The quantity of clothing being added to the store.", txtQuantity, -80, 40, 5000);
        }

        /// <summary>
        /// Kill the application if the user closes the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StockAdd_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Alerts the user not to enter spaces if they try entering one.
        /// Will then clear the text box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBarcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 32)
            {
                MessageBox.Show(this, "Spaces are not allowed in the barcode text box.", "Invalid character", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // code used to ensure that the cursor is set to the start
                // of the text box. Will not work without BeginInvoke...
                this.BeginInvoke((MethodInvoker)delegate ()
                {
                    txtBarcode.ResetText();
                    txtBarcode.Select(0, 0);
                });
            }
        }

        /// <summary>
        /// Alerts the user not to enter spaces if they try entering one.
        /// Will then clear the text box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtStyle_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 32)
            {
                MessageBox.Show(this, "Spaces are not allowed in the style number text box.", "Invalid character", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // code used to ensure that the cursor is set to the start
                // of the text box. Will not work without BeginInvoke...
                this.BeginInvoke((MethodInvoker)delegate ()
                {
                    txtStyle.ResetText();
                    txtStyle.Select(0, 0);
                });
            }
        }

        /// <summary>
        /// Alerts the user not to enter spaces if they try entering one.
        /// Will then clear the text box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSKU_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 32)
            {
                MessageBox.Show(this, "Spaces are not allowed in the SKU text box.", "Invalid character", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // code used to ensure that the cursor is set to the start
                // of the text box. Will not work without BeginInvoke...
                this.BeginInvoke((MethodInvoker)delegate ()
                {
                    txtSKU.ResetText();
                    txtSKU.Select(0, 0);
                });
            }
        }
        
        // Custom methods

        /// <summary>
        /// This method will clear types of input within the form.
        /// </summary>
        public void ClearForm()
        {
            txtBarcode.ResetText();
            txtStyle.ResetText();
            txtSKU.ResetText();
            txtSize.ResetText();
            txtPrice.ResetText();
            txtQuantity.ResetText();
            comboBoxDepartments.SelectedIndex = -1;
            comboBoxLocations.SelectedIndex = -1;
            radioButtonYes.Checked = false;
            radioButtonNo.Checked = false;
        }

        /// <summary>
        /// This method ensures that each input field has some sort of input from the user.
        /// </summary>
        /// <returns>Returns true if each input field has input and vice versa.</returns>
        public bool AllFieldsCompleted()
        {
            if (txtBarcode.Text.Length == 0 || txtStyle.Text.Length == 0 || txtSKU.Text.Length == 0 || comboBoxDepartments.SelectedIndex < 0 || txtSize.Text.Length == 0 || txtPrice.Text.Length == 0 || (radioButtonNo.Checked == false && radioButtonYes.Checked == false) || txtQuantity.Text.Length == 0 || (radioButtonYes.Checked == true && comboBoxLocations.SelectedIndex < 0))
            {
                return false;
            }

            else
            {
                return true;
            }
        }

        /// <summary>
        /// Check to make sure that a user has entered appropriate values into the two supplied text boxes.
        /// </summary>
        /// <param name="quantityIn">The quantity text box to check.</param>
        /// <param name="priceIn">The price text box to check.</param>
        /// <returns>Returns false if the user supplies poor input and vice versa.</returns>
        private bool FieldsAreValid(TextBox quantityIn, TextBox priceIn)
        {
            if (int.Parse(quantityIn.Text) < 0 || int.Parse(quantityIn.Text) > 65535 || double.Parse(priceIn.Text) < 0.01 || double.Parse(priceIn.Text) > 99.99)
            {
                return false;
            }

            else
            {
                return true;
            }
        }
        
        /// <summary>
        /// This function contains the SQL command necessary to add stock to the database.
        /// </summary>
        /// <param name="clothingIn">The stock that will be added to the database.</param>
        private void SQLAddStock(Clothing clothingIn)
        {
            SqlCommand cmdAddStock = new SqlCommand("dbo.spAddStock", myConnection);
            cmdAddStock.CommandType = CommandType.StoredProcedure;
            cmdAddStock.Parameters.AddWithValue("@BarcodeIn", SqlDbType.VarChar).Value = clothingIn.GetBarcode();
            cmdAddStock.Parameters.AddWithValue("@StyleNumIn", SqlDbType.VarChar).Value = clothingIn.GetStyleNo();
            cmdAddStock.Parameters.AddWithValue("@SKUIn", SqlDbType.VarChar).Value = clothingIn.GetSKU();
            cmdAddStock.Parameters.AddWithValue("@DepartmentIn", SqlDbType.Int).Value = clothingIn.GetDepartment();
            cmdAddStock.Parameters.AddWithValue("@SizeIn", SqlDbType.VarChar).Value = clothingIn.GetSize();
            cmdAddStock.Parameters.AddWithValue("@PriceIn", SqlDbType.Decimal).Value = clothingIn.GetPrice();
            cmdAddStock.Parameters.AddWithValue("@OnShopFloorIn", SqlDbType.VarChar).Value = clothingIn.GetOnShopFloor();
            cmdAddStock.Parameters.AddWithValue("@QuantityIn", SqlDbType.Int).Value = int.Parse(clothingIn.GetQuantity().ToString());

            if (clothingIn.GetLocation() == "")
            {
                cmdAddStock.Parameters.AddWithValue("@LocationIn", SqlDbType.Int).Value = DBNull.Value;
            }

            else
            {
                cmdAddStock.Parameters.AddWithValue("@LocationIn", SqlDbType.Int).Value = clothingIn.GetLocation();
            }


            cmdAddStock.ExecuteNonQuery();
        }

        /// <summary>
        /// This function contains the SQL command necessary to retrieve the location ID of the chosen location from the database.
        /// </summary>
        /// <param name="clothingIn">The piece of stock being added.</param>
        /// <param name="myReaderIn">The SQL data reader necessary for reading rows.</param>
        private void SQLGetLocation(Clothing clothingIn, SqlDataReader myReaderIn)
        {
            SqlCommand cmdGetLocation = new SqlCommand("dbo.spGetLocationIDByName", myConnection);
            cmdGetLocation.CommandType = CommandType.StoredProcedure;
            cmdGetLocation.Parameters.AddWithValue("@NameIn", SqlDbType.VarChar).Value = clothingIn.GetLocation();

            myReaderIn = cmdGetLocation.ExecuteReader();

            while (myReaderIn.Read())
            {
                clothingIn.SetLocation(myReaderIn["location_id"].ToString());
            }
            myReaderIn.Close();
        }

        /// <summary>
        /// This function contains the SQL command necessary to retrieve the department ID of the chosen department from the database.
        /// </summary>
        /// <param name="clothingIn">The piece of stock being added.</param>
        /// <param name="myReaderIn">The SQL data reader necessary for reading rows.</param>
        private void SQLGetDepartment(Clothing clothingIn, SqlDataReader myReaderIn)
        {
            SqlCommand cmdGetDepartment = new SqlCommand("dbo.spGetDepartmentIDByName", myConnection);
            cmdGetDepartment.CommandType = CommandType.StoredProcedure;
            cmdGetDepartment.Parameters.AddWithValue("@NameIn", SqlDbType.VarChar).Value = clothingIn.GetDepartment();

            myReaderIn = cmdGetDepartment.ExecuteReader();

            while (myReaderIn.Read())
            {
                clothingIn.SetDepartment(myReaderIn["department_id"].ToString());
            }
            myReaderIn.Close();
        }

        /// <summary>
        /// This function contains the SQL command necessary to check if an item actually exists in the database using a given barcode.
        /// </summary>
        /// <param name="barcodeIn">The barcode associated with the piece of clothing being checked.</param>
        /// <returns>Returns true if the item exists and vice versa.</returns>
        private bool SQLItemExists(string barcodeIn, SqlDataReader myReaderIn)
        {
            SqlCommand cmdGetStockDetails = new SqlCommand("dbo.spGetStockDetailsByBarcode", myConnection);
            cmdGetStockDetails.CommandType = CommandType.StoredProcedure;
            cmdGetStockDetails.Parameters.AddWithValue("@barcodeIn", SqlDbType.VarChar).Value = barcodeIn;

            myReaderIn = null;
            myReaderIn = cmdGetStockDetails.ExecuteReader();
            bool itemExists = false;

            if (myReaderIn.HasRows)
            {
                itemExists = true;
            }

            else
            {
                itemExists = false;
            }

            myReaderIn.Close();

            return itemExists;
        }

        /// <summary>
        /// This function contains the SQL command necessary to populate the departments combo box with the department names held in the database.
        /// </summary>
        private void SQLLoadComboBoxDepartments()
        {
            SqlCommand cmdGetDepartments = new SqlCommand("dbo.spGetAllDepartments", myConnection);
            SqlDataReader myReader = null;

            cmdGetDepartments.CommandType = CommandType.StoredProcedure;

            myReader = cmdGetDepartments.ExecuteReader();

            while (myReader.Read())
            {
                comboBoxDepartments.Items.Add(myReader["name"].ToString());
            }
            myReader.Close();
        }

        /// <summary>
        /// This function contains the SQL command necessary to populate the locations combo box with the location names held in the database.
        /// </summary>
        private void SQLLoadComboBoxLocations()
        {
            SqlCommand cmdGetLocations = new SqlCommand("dbo.spGetAllLocations", myConnection);
            SqlDataReader myReader = null;

            cmdGetLocations.CommandType = CommandType.StoredProcedure;

            myReader = cmdGetLocations.ExecuteReader();

            while (myReader.Read())
            {
                comboBoxLocations.Items.Add(myReader["name"].ToString());
            }
            myReader.Close();
        }

        /// <summary>
        /// This function contains the SQL command necessary to retrieve the clothing id of the piece of clothing just added to the database.
        /// </summary>
        /// <param name="clothingIn">The piece of clothing whose ID is needed.</param>
        /// <returns>Returns the clothing ID in a string format.</returns>
        private string SQLGetClothingID(Clothing clothingIn)
        {
            SqlCommand cmdGetLocations = new SqlCommand("dbo.spGetStockDetailsByBarcode", myConnection);
            SqlDataReader myReader = null;
            string output = "";

            cmdGetLocations.CommandType = CommandType.StoredProcedure;
            cmdGetLocations.Parameters.AddWithValue("@barcodeIn", SqlDbType.VarChar).Value = clothingIn.GetBarcode();

            myReader = cmdGetLocations.ExecuteReader();

            while (myReader.Read())
            {
               output = myReader["clothing_id"].ToString();
            }
            myReader.Close();

            return output;
        }

        /// <summary>
        /// This function contains the SQL command necessary to add a new record into the stock updates table.
        /// </summary>
        /// <param name="clothingIn">The piece of clothing that has been added to the database.</param>
        private void SQLUpdateStockAdd(Clothing clothingIn)
        {
            SqlCommand cmdUpdateStockAdd = new SqlCommand("dbo.spAddStockUpdate", myConnection);
            cmdUpdateStockAdd.CommandType = CommandType.StoredProcedure;
            cmdUpdateStockAdd.Parameters.AddWithValue("@ClothingIDIn", SqlDbType.VarChar).Value = SQLGetClothingID(clothingIn);
            cmdUpdateStockAdd.Parameters.AddWithValue("@UserIDIn", SqlDbType.VarChar).Value = currentUser.GetEmployeeID().ToString();
            cmdUpdateStockAdd.Parameters.AddWithValue("@UpdateTypeIn", SqlDbType.VarChar).Value = "Add";

            cmdUpdateStockAdd.ExecuteNonQuery();
        }

        #region Menustrip code

        private void viewStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StockView stockView = new StockView(currentUser);
            this.Hide();

            stockView.Show();
        }

        private void addStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // allow access to form if user is a manager
            if (currentUser.GetEmployeeType() == "mgr")
            {
                StockAdd stockAdd = new StockAdd(currentUser);
                this.Hide();

                stockAdd.Show();
            }

            // otherwise deny access
            else
            {
                MessageBox.Show(this, $"Access denied - please get a manager or team leader to access this page.", "Access denied", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StockFind stockFind = new StockFind(currentUser);
            this.Hide();

            stockFind.Show();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // allow access to form if user is a manager
            if (currentUser.GetEmployeeType() == "mgr")
            {
                StockEdit stockEdit = new StockEdit(currentUser);
                this.Hide();

                stockEdit.Show();
            }

            // otherwise deny access
            else
            {
                MessageBox.Show(this, $"Access denied - please get a manager or team leader to access this page.", "Access denied", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LocationView locationView = new LocationView(currentUser);
            this.Hide();

            locationView.Show();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // allow access to form if user is a manager
            if (currentUser.GetEmployeeType() == "mgr")
            {
                LocationAdd locationAdd = new LocationAdd(currentUser);
                this.Hide();

                locationAdd.Show();
            }

            // otherwise deny access
            else
            {
                MessageBox.Show(this, $"Access denied - please get a manager or team leader to access this page.", "Access denied", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void addStockToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            LocationAddStock locationAddStock = new LocationAddStock(currentUser);
            this.Hide();

            locationAddStock.Show();
        }

        private void addUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // allow access to form if user is a manager
            if (currentUser.GetEmployeeType() == "mgr")
            {
                AddUser addUser = new AddUser(currentUser);
                this.Hide();

                addUser.Show();
            }

            // otherwise deny access
            else
            {
                MessageBox.Show(this, $"Access denied - please get a manager or team leader to access this page.", "Access denied", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void auditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // allow access to form if user is a manager
            if (currentUser.GetEmployeeType() == "mgr")
            {
                Audit audit = new Audit(currentUser);
                this.Hide();

                audit.Show();
            }

            // otherwise deny access
            else
            {
                MessageBox.Show(this, $"Access denied - please get a manager or team leader to access this page.", "Access denied", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "Logout successful.", "Logout", MessageBoxButtons.OK, MessageBoxIcon.Information);

            SplashScreen splashScreen = new SplashScreen();
            this.Hide();

            splashScreen.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion
    }
}