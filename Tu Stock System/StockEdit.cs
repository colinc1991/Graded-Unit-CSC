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
    public partial class StockEdit : Form, IFormMethods
    {
        // connection string used to connect to local database
        // used in all SQL queries
        SqlConnection myConnection = new SqlConnection("Server=.;Database=Tu;Trusted_Connection=True;");

        // global employee object to be used in the constructor of any other form opened
        Employee currentUser = new Employee();

        // global clothing object used to store values of piece of clothing being modified
        Clothing stockBeingEdited = new Clothing();

        // Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StockEdit()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Second constructor which takes one employee object.
        /// </summary>
        /// <param name="employeeIn">Employee that's currently logged in.</param>
        public StockEdit(Employee employeeIn)
        {
            InitializeComponent();
            
            currentUser = employeeIn;
        }
        
        /// <summary>
        /// Third constructor which takes one clothing object and one employee object.
        /// </summary>
        /// <param name="clothingIn">Clothing to be modified.</param>
        /// <param name="employeeIn">Employee that's currently logged in.</param>
        public StockEdit(Clothing clothingIn, Employee employeeIn)
        {
            InitializeComponent();

            stockBeingEdited = clothingIn;
            currentUser = employeeIn;
        }

        // Events

        /// <summary>
        /// When the form loads, populate all input fields with necessary information.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StockEdit_Load(object sender, EventArgs e)
        {
            myConnection.Open();
            SQLLoadComboBoxDepartments();
            SQLLoadComboBoxLocations();
            myConnection.Close();

            txtBarcode.Text = stockBeingEdited.GetBarcode();
            txtStyle.Text = stockBeingEdited.GetStyleNo();
            txtSKU.Text = stockBeingEdited.GetSKU();
            comboBoxDepartments.SelectedItem = stockBeingEdited.GetDepartment();
            txtSize.Text = stockBeingEdited.GetSize();
            txtPrice.Text = stockBeingEdited.GetPrice().ToString();

            // if the item of clothing is on the shop floor
            // adjust controls accordingly
            if (stockBeingEdited.GetOnShopFloor().ToLower() == "yes")
            {
                radioButtonYes.Checked = true;
                comboBoxLocations.Text = stockBeingEdited.GetLocation();
            }

            // if the item of clothing is not on the shop floor
            // adjust controls accordingly
            else
            {
                radioButtonNo.Checked = true;
                comboBoxLocations.Visible = false;
                lblLocation.Visible = false;
            }

            comboBoxLocations.Text = stockBeingEdited.GetLocation();
            txtQuantity.Text = stockBeingEdited.GetQuantity().ToString();
        }

        /// <summary>
        /// Populate each input field with a value from the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadItem_Click(object sender, EventArgs e)
        {
            if (txtBarcode.Text.Length == 13)
            {
                myConnection.Open();
                SqlDataReader myReader = null;

                SQLGetStockDetails(myReader);
                SQLGetDepartmentDetails(myReader);
                SQLGetLocationDetailsByID(myReader);

                myConnection.Close();

                txtStyle.Text = stockBeingEdited.GetStyleNo();
                txtSKU.Text = stockBeingEdited.GetSKU();
                txtSize.Text = stockBeingEdited.GetSize();
                txtPrice.Text = stockBeingEdited.GetPrice().ToString();

                // if the item of clothing is on the shop floor
                // adjust controls accordingly
                if (stockBeingEdited.GetOnShopFloor().ToLower() == "yes")
                {
                    radioButtonYes.Checked = true;
                    comboBoxLocations.Text = stockBeingEdited.GetLocation();
                }

                // if the item of clothing is not on the shop floor
                // adjust controls accordingly
                else
                {
                    radioButtonNo.Checked = true;
                    comboBoxLocations.Visible = false;
                    lblLocation.Visible = false;
                }

                txtQuantity.Text = stockBeingEdited.GetQuantity().ToString();
                comboBoxDepartments.Text = stockBeingEdited.GetDepartment();
            }

            else
            {
                MessageBox.Show(this, "Please make sure the barcode is 13 numbers long.", "Invalid barcode", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Updates the stock with new information and updates record in database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmitChanges_Click(object sender, EventArgs e)
        {
            SqlDataReader myReader = null;

            if (txtBarcode.Text.Length != 13 || txtStyle.Text.Length != 9 || txtSKU.Text.Length != 9)
            {
                MessageBox.Show(this, "Please make sure the barcode is 13 numbers long and the style number/SKU are both 9 numbers long.", "Invalid field length", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                if (AllFieldsCompleted() == true)
                {
                    try
                    {
                        // only proceed if the user has supplied correct input into the two numeric fields
                        if (FieldsAreValid(txtQuantity, txtPrice))
                        {
                            stockBeingEdited.SetBarcode(txtBarcode.Text);
                            stockBeingEdited.SetStyleNum(txtStyle.Text);
                            stockBeingEdited.SetSKU(txtSKU.Text);
                            stockBeingEdited.SetDepartment(comboBoxDepartments.SelectedItem.ToString());
                            stockBeingEdited.SetSize(txtSize.Text);
                            stockBeingEdited.SetPrice(decimal.Parse(txtPrice.Text));

                            myConnection.Open();

                            // Only assign location if the item is on shop floor
                            if (radioButtonYes.Checked == true)
                            {
                                stockBeingEdited.SetOnShopFloor("Yes");
                                stockBeingEdited.SetLocation(comboBoxLocations.SelectedItem.ToString());

                                SQLGetLocationDetailsByName(myReader);
                            }

                            // Set location to blank string if item isn't on shop floor
                            else
                            {
                                stockBeingEdited.SetOnShopFloor("No");
                                stockBeingEdited.SetLocation("");
                            }

                            stockBeingEdited.SetQuantity(ushort.Parse(txtQuantity.Text));

                            SQLGetDepartmentDetailsByName(myReader);
                            SQLUpdateStock();
                            SQLUpdateStockAdd(stockBeingEdited);
                            myConnection.Close();

                            MessageBox.Show(this, "Stock modified successfully", "Stock modified", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearForm();
                        }

                        else
                        {
                            MessageBox.Show(this, "Stock not modified - please make sure that quantity is within the range of 0 - 65535 and price is within 0.01 - 99.99.", "Stock not modified", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    catch (OverflowException)
                    {
                        MessageBox.Show(this, "Please make sure that quantity is within the range of 0 - 65535.", "Stock not modified", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    catch (FormatException)
                    {
                        MessageBox.Show(this, "Please make sure that both quantity and price are numbers.", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                else
                {
                    MessageBox.Show(this, "Please make sure all fields have been filled in where appropriate.", "Fields missing input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        /// <summary>
        /// Hide the location field if user says the item isn't on shop floor and vice versa.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonYes_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonYes.Checked == true)
            {
                comboBoxLocations.Show();
                lblLocation.Show();
            }

            else
            {
                comboBoxLocations.Hide();
                lblLocation.Hide();
            }
        }

        /// <summary>
        /// Kill the application if the user closes the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StockEdit_FormClosing(object sender, FormClosingEventArgs e)
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

        /// <summary>
        /// If a user enters a space, alert them this isn't allowed and clear the text box.
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
        }

        /// <summary>
        /// If a user enters a space, alert them this isn't allowed and clear the text box.
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
        }

        /// <summary>
        /// If a user enters a space, alert them this isn't allowed and clear the text box.
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
        }

        // Custom methods

        /// <summary>
        /// This method will clear all input fields within the form and returns focus to the barcode text box.
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
            txtBarcode.Focus();
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
        /// This function contains the SQL command necessary to retrieves all stock information from the database associated with supplied barcode.
        /// Will then populate clothing object with values found in row.
        /// </summary>
        /// <param name="myReaderIn">The SQL data reader necessary for reading rows.</param>
        private void SQLGetStockDetails(SqlDataReader myReaderIn)
        {
            SqlCommand cmd = new SqlCommand("dbo.spGetStockDetailsByBarcode", myConnection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@barcodeIn", SqlDbType.VarChar).Value = txtBarcode.Text;

            myReaderIn = cmd.ExecuteReader();

            while (myReaderIn.Read())
            {
                stockBeingEdited.SetBarcode(txtBarcode.Text);
                stockBeingEdited.SetClothingID(myReaderIn["clothing_id"].ToString());
                stockBeingEdited.SetStyleNum(myReaderIn["style_number"].ToString());
                stockBeingEdited.SetSKU(myReaderIn["SKU"].ToString());
                stockBeingEdited.SetSize(myReaderIn["Size"].ToString());
                stockBeingEdited.SetPrice(Convert.ToDecimal(myReaderIn["Price"].ToString()));
                stockBeingEdited.SetOnShopFloor(myReaderIn["on_shop_floor"].ToString());
                stockBeingEdited.SetLocation(myReaderIn["lcn_id"].ToString());
                stockBeingEdited.SetQuantity(ushort.Parse(myReaderIn["quantity"].ToString()));
                stockBeingEdited.SetDepartment(myReaderIn["dpt_id"].ToString());
            }
            myReaderIn.Close();
        }

        /// <summary>
        /// This function contains the SQL command necessary to retrieve the name of the department using the given ID.
        /// </summary>
        /// <param name="myReaderIn">The SQL data reader necessary for reading rows.</param>
        private void SQLGetDepartmentDetails(SqlDataReader myReaderIn)
        {
            SqlCommand cmdGetDepartmentDetails = new SqlCommand("dbo.spGetDepartmentNameByID", myConnection);
            cmdGetDepartmentDetails.CommandType = CommandType.StoredProcedure;
            cmdGetDepartmentDetails.Parameters.AddWithValue("@IDIn", SqlDbType.VarChar).Value = stockBeingEdited.GetDepartment();

            myReaderIn = cmdGetDepartmentDetails.ExecuteReader();

            while (myReaderIn.Read())
            {
                stockBeingEdited.SetDepartment(myReaderIn["name"].ToString());
            }
            myReaderIn.Close();
        }

        /// <summary>
        /// This function contains the SQL command necessary to retrieve the name of the location using the given ID.
        /// </summary>
        /// <param name="myReaderIn">The SQL data reader necessary for reading rows.</param>
        private void SQLGetLocationDetailsByID(SqlDataReader myReaderIn)
        {
            SqlCommand cmdGetLocationDetails = new SqlCommand("dbo.spGetLocationNameByID", myConnection);
            cmdGetLocationDetails.CommandType = CommandType.StoredProcedure;
            cmdGetLocationDetails.Parameters.AddWithValue("@IDIn", SqlDbType.VarChar).Value = stockBeingEdited.GetLocation();

            myReaderIn = cmdGetLocationDetails.ExecuteReader();

            while (myReaderIn.Read())
            {
                stockBeingEdited.SetLocation(myReaderIn["name"].ToString());
            }
            myReaderIn.Close();
        }

        /// <summary>
        /// This function contains the SQL command necessary to update the stock information held in the database.
        /// </summary>
        private void SQLUpdateStock()
        {
            SqlCommand cmdUpdateStock = new SqlCommand("dbo.spUpdateStock", myConnection);
            cmdUpdateStock.CommandType = CommandType.StoredProcedure;
            cmdUpdateStock.Parameters.AddWithValue("@IDIn", SqlDbType.Int).Value = int.Parse(stockBeingEdited.GetID().ToString());
            cmdUpdateStock.Parameters.AddWithValue("@barcodeIn", SqlDbType.VarChar).Value = stockBeingEdited.GetBarcode();
            cmdUpdateStock.Parameters.AddWithValue("@styleNumIn", SqlDbType.VarChar).Value = stockBeingEdited.GetStyleNo();
            cmdUpdateStock.Parameters.AddWithValue("@SKUIn", SqlDbType.VarChar).Value = stockBeingEdited.GetSKU();
            cmdUpdateStock.Parameters.AddWithValue("@SizeIn", SqlDbType.VarChar).Value = stockBeingEdited.GetSize();
            cmdUpdateStock.Parameters.AddWithValue("@PriceIn", SqlDbType.Decimal).Value = stockBeingEdited.GetPrice();
            cmdUpdateStock.Parameters.AddWithValue("@OnShopFloorIn", SqlDbType.VarChar).Value = stockBeingEdited.GetOnShopFloor();

            // if the stock being edited has no location
            // set the lcn_id column value to null
            if (stockBeingEdited.GetLocation() == "")
            {
                cmdUpdateStock.Parameters.AddWithValue("@lcnIn", SqlDbType.Int).Value = DBNull.Value;
            }

            // otherwise add the location id
            else
            {
                cmdUpdateStock.Parameters.AddWithValue("@lcnIn", SqlDbType.Int).Value = stockBeingEdited.GetLocation();
            }

            cmdUpdateStock.Parameters.AddWithValue("@quantityIn", SqlDbType.Int).Value = int.Parse(stockBeingEdited.GetQuantity().ToString());
            cmdUpdateStock.Parameters.AddWithValue("@dptIn", SqlDbType.Int).Value = stockBeingEdited.GetDepartment();

            cmdUpdateStock.ExecuteNonQuery();
        }

        /// <summary>
        /// This function contains the SQL command necessary to get the department ID using the department name.
        /// </summary>
        /// <param name="myReaderIn">The SQL data reader necessary for reading rows.</param>
        private void SQLGetDepartmentDetailsByName(SqlDataReader myReaderIn)
        {
            SqlCommand cmdGetDepartmentDetails = new SqlCommand("dbo.spGetDepartmentIDByName", myConnection);
            cmdGetDepartmentDetails.CommandType = CommandType.StoredProcedure;
            cmdGetDepartmentDetails.Parameters.AddWithValue("@NameIn", SqlDbType.VarChar).Value = stockBeingEdited.GetDepartment();

            myReaderIn = cmdGetDepartmentDetails.ExecuteReader();

            while (myReaderIn.Read())
            {
                stockBeingEdited.SetDepartment(myReaderIn["department_id"].ToString());
            }
            myReaderIn.Close();
        }

        /// <summary>
        /// This function contains the SQL command necessary to get the location id using the location name.
        /// </summary>
        /// <param name="myReaderIn">The SQL data reader necessary for reading rows.</param>
        private void SQLGetLocationDetailsByName(SqlDataReader myReaderIn)
        {
            SqlCommand cmdGetLocationDetails = new SqlCommand("dbo.spGetLocationIDByName", myConnection);
            cmdGetLocationDetails.CommandType = CommandType.StoredProcedure;
            cmdGetLocationDetails.Parameters.AddWithValue("@NameIn", SqlDbType.VarChar).Value = stockBeingEdited.GetLocation();

            myReaderIn = cmdGetLocationDetails.ExecuteReader();

            while (myReaderIn.Read())
            {
                stockBeingEdited.SetLocation(myReaderIn["location_id"].ToString());
            }
            myReaderIn.Close();
        }

        /// <summary>
        /// This function contains the SQL command necessary to add a new record into the stock updates table.
        /// </summary>
        /// <param name="clothingIn">The piece of clothing that has been added to the database.</param>
        private void SQLUpdateStockAdd(Clothing clothingIn)
        {
            SqlCommand cmdUpdateStockAdd = new SqlCommand("dbo.spAddStockUpdate", myConnection);
            cmdUpdateStockAdd.CommandType = CommandType.StoredProcedure;
            cmdUpdateStockAdd.Parameters.AddWithValue("@ClothingIDIn", SqlDbType.VarChar).Value = clothingIn.GetID().ToString();
            cmdUpdateStockAdd.Parameters.AddWithValue("@UserIDIn", SqlDbType.VarChar).Value = currentUser.GetEmployeeID().ToString();
            cmdUpdateStockAdd.Parameters.AddWithValue("@UpdateTypeIn", SqlDbType.VarChar).Value = "Edit";

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