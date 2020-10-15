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
    public partial class StockView : Form, IFormMethods
    {
        // connection string used to connect to local database
        // used in all SQL queries
        SqlConnection myConnection = new SqlConnection("Server=.;Database=Tu;Trusted_Connection=True;");

        // global employee object to be used in the constructor of any other form opened
        Employee currentUser = new Employee();

        // global clothing object used to store values of stock being viewed.
        Clothing stockBeingViewed = new Clothing();

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StockView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Second constructor which takes one employee object.
        /// </summary>
        /// <param name="employeeIn">Employee that's currently logged in.</param>
        public StockView(Employee employeeIn)
        {
            InitializeComponent();

            currentUser = employeeIn;
        }

        // Events

        /// <summary>
        /// Populates each text box with relevant information from database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewStock_Click(object sender, EventArgs e)
        {
            bool itemFound = false;

            // only carry out the following code if a barcode has been entered
            if (AllFieldsCompleted())
            {
                myConnection.Open();
                SqlDataReader myReader = null;

                SQLGetStockDetails(myReader, ref itemFound);

                // populate fields with information from database if the item has been found
                if (itemFound)
                {
                    SQLGetDepartmentDetails(myReader);
                    SQLGetLocationDetails(myReader);

                    myConnection.Close();

                    txtStyle.Text = stockBeingViewed.GetStyleNo();
                    txtSKU.Text = stockBeingViewed.GetSKU();
                    txtSize.Text = stockBeingViewed.GetSize();
                    txtPrice.Text = stockBeingViewed.GetPrice().ToString();
                    txtShopFloor.Text = stockBeingViewed.GetOnShopFloor();
                    txtLocation.Text = stockBeingViewed.GetLocation();
                    txtQuantity.Text = stockBeingViewed.GetQuantity().ToString();
                    txtDepartment.Text = stockBeingViewed.GetDepartment();

                    // Only display location specific controls if the item is on the shop floor
                    if (stockBeingViewed.GetOnShopFloor().ToLower() == "yes")
                    {
                        txtLocation.Visible = true;
                        lblLocation.Visible = true;
                    }

                    // otherwise hide them
                    else
                    {
                        txtLocation.Visible = false;
                        lblLocation.Visible = false;
                    }
                }

                // alert user that no item was found
                else
                {
                    MessageBox.Show(this, "No stock found - item has not been added to database.", "Stock not found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    myConnection.Close();
                    ClearForm();
                    
                    // code used to ensure that the cursor is set to the start
                    // of the text box. Will not work without BeginInvoke...
                    this.BeginInvoke((MethodInvoker)delegate ()
                    {
                        txtBarcode.ResetText();
                        txtBarcode.Select(0, 0);
                    });
                }
            }

            else
            {
                MessageBox.Show(this, "Please enter a full barcode.", "Barcode not complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Opens the edit stock page with the current stock being viewed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditStock_Click(object sender, EventArgs e)
        {
            // allow access to form if user is a manager
            if (currentUser.GetEmployeeType() == "mgr")
            {
                StockEdit stockEdit = new StockEdit(stockBeingViewed, currentUser);
                this.Hide();

                stockEdit.Show();
            }

            // otherwise deny access
            else
            {
                MessageBox.Show(this, $"Access denied - please get a manager or team leader to access this page.", "Access denied", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        /// <summary>
        /// Once a barcode has been entered, simulate the btnViewStock button being clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBarcode_TextChanged(object sender, EventArgs e)
        {
            if (txtBarcode.Text.Length == 13)
            {
                btnViewStock_Click(sender, e);
            }
        }

        /// <summary>
        /// Kill the application if the user closes the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StockView_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        // Custom methods

        /// <summary>
        /// This function contains the SQL command necessary to retrieve the stock details of the chosen piece of clothing from the database using a barcode.
        /// </summary>
        /// <param name="myReaderIn">Reader object used to read rows from table.</param>
        /// <param name="itemFoundIn">Bool variable used to identify whether the item exists in the database or not.</param>
        private void SQLGetStockDetails(SqlDataReader myReaderIn, ref bool itemFoundIn)
        {
            SqlCommand cmdGetStockDetails = new SqlCommand("dbo.spGetStockDetailsByBarcode", myConnection);
            cmdGetStockDetails.CommandType = CommandType.StoredProcedure;
            cmdGetStockDetails.Parameters.AddWithValue("@barcodeIn", SqlDbType.VarChar).Value = txtBarcode.Text;

            myReaderIn = cmdGetStockDetails.ExecuteReader();

            while (myReaderIn.Read())
            {
                itemFoundIn = true;
                stockBeingViewed.SetBarcode(txtBarcode.Text);
                stockBeingViewed.SetClothingID(myReaderIn["clothing_id"].ToString());
                stockBeingViewed.SetStyleNum(myReaderIn["style_number"].ToString());
                stockBeingViewed.SetSKU(myReaderIn["SKU"].ToString());
                stockBeingViewed.SetSize(myReaderIn["Size"].ToString());
                stockBeingViewed.SetPrice(Convert.ToDecimal(myReaderIn["Price"].ToString()));
                stockBeingViewed.SetOnShopFloor(myReaderIn["on_shop_floor"].ToString());
                stockBeingViewed.SetLocation(myReaderIn["lcn_id"].ToString());
                stockBeingViewed.SetQuantity(ushort.Parse(myReaderIn["quantity"].ToString()));
                stockBeingViewed.SetDepartment(myReaderIn["dpt_id"].ToString());
            }
            myReaderIn.Close();
        }

        /// <summary>
        /// This function contains the SQL command necessary to retrieve the department name from the database using a department ID.
        /// </summary>
        /// <param name="myReaderIn">Reader object used to read rows from table.</param>
        private void SQLGetDepartmentDetails(SqlDataReader myReaderIn)
        {
            SqlCommand cmdGetDepartmentDetails = new SqlCommand("dbo.spGetDepartmentNameByID", myConnection);
            cmdGetDepartmentDetails.CommandType = CommandType.StoredProcedure;
            cmdGetDepartmentDetails.Parameters.AddWithValue("@IDIn", SqlDbType.VarChar).Value = stockBeingViewed.GetDepartment();

            myReaderIn = cmdGetDepartmentDetails.ExecuteReader();

            while (myReaderIn.Read())
            {
                stockBeingViewed.SetDepartment(myReaderIn["name"].ToString());
            }
            myReaderIn.Close();
        }

        /// <summary>
        /// This function contains the SQL command necessary to retrieve the location name from the database using a location ID.
        /// </summary>
        /// <param name="myReaderIn">Reader object used to read rows from table.</param>
        private void SQLGetLocationDetails(SqlDataReader myReaderIn)
        {
            SqlCommand cmdGetLocationDetails = new SqlCommand("dbo.spGetLocationNameByID", myConnection);
            cmdGetLocationDetails.CommandType = CommandType.StoredProcedure;
            cmdGetLocationDetails.Parameters.AddWithValue("@IDIn", SqlDbType.VarChar).Value = stockBeingViewed.GetLocation();

            myReaderIn = cmdGetLocationDetails.ExecuteReader();

            while (myReaderIn.Read())
            {
                stockBeingViewed.SetLocation(myReaderIn["name"].ToString());
            }
            myReaderIn.Close();
        }

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
            txtDepartment.ResetText();
            txtLocation.ResetText();
            txtShopFloor.ResetText();
        }

        /// <summary>
        /// This method ensures that each input field has some sort of input from the user.
        /// </summary>
        /// <returns>Returns true if each input field has input and vice versa.</returns>
        public bool AllFieldsCompleted()
        {
            if (txtBarcode.Text.Length == 13)
            {
                return true;
            }

            else
            {
                return false;
            }
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

            this.Close();
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