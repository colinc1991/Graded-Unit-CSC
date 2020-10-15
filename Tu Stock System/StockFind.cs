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
    public partial class StockFind : Form
    {
        // connection string used to connect to local database
        // used in all SQL queries
        SqlConnection myConnection = new SqlConnection("Server=.;Database=Tu;Trusted_Connection=True;");

        // global employee object to be used in the constructor of any other form opened
        Employee currentUser = new Employee();

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StockFind()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Second constructor which takes one employee object.
        /// </summary>
        /// <param name="employeeIn">Employee that's currently logged in.</param>
        public StockFind(Employee employeeIn)
        {
            InitializeComponent();

            currentUser = employeeIn;
        }

        // Events

        /// <summary>
        /// Displays the location of the item of stock entered.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFind_Click(object sender, EventArgs e)
        {
            // make sure the user has entered in a complete barcode
            if (radioButtonBarcode.Checked == true && txtInput.Text.Length != 13)
            {
                MessageBox.Show(this, "Please make sure a complete barcode has been entered.", "Incomplete barcode", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // make sure the user has entered in a complete style number/SKU
            else if ((radioButtonStyleNum.Checked == true || radioButtonSKU.Checked == true) && txtInput.Text.Length != 9)
            {
                MessageBox.Show(this, "Please make sure a complete SKU or style number has been entered.", "Incomplete SKU/style number", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // carry out operations if they have supplied appropriate details
            else
            {

                myConnection.Open();
                SqlDataReader myReader = null;
                Location location = new Location();

                bool itemExists = false;

                try
                {
                    // Different SQL functions will be called depending on which radio button has been selected.

                    if (radioButtonBarcode.Checked == true)
                    {
                        SQLGetLocationDetailsByBarcode(myReader, location);
                    }

                    else if (radioButtonStyleNum.Checked == true)
                    {
                        SQLGetLocationDetailsByStyleNum(myReader, location);
                    }

                    else if (radioButtonSKU.Checked == true)
                    {
                        SQLGetLocationDetailsBySKU(myReader, location);
                    }

                    SQLGetLocationDetailsByID(myReader, location, ref itemExists);

                    // if item entered has location, display this to user
                    if (itemExists)
                    {
                        MessageBox.Show(this, $"Item is located on {location.GetName()}.", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    // item does not exist in the database
                    else
                    {
                        MessageBox.Show(this, "Item has not been added to database.", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                catch
                {
                    // item has no location
                    MessageBox.Show(this, "Item is not on shop floor.", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                myConnection.Close();

                // code used to ensure that the cursor is set to the start
                // of the text box. Will not work without BeginInvoke...
                this.BeginInvoke((MethodInvoker)delegate ()
                {
                    txtInput.ResetText();
                    txtInput.Select(0, 0);
                });
            }
        }

        /// <summary>
        /// If a barcode, style number or SKU has been entered, call the btnFind function to simulate the button being pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtInput_TextChanged(object sender, EventArgs e)
        {
            if (txtInput.Text.Length == 13 && radioButtonBarcode.Checked == true)
            {
                btnFind_Click(sender, e);
            }

            else if (txtInput.Text.Length == 9 && (radioButtonStyleNum.Checked == true || radioButtonSKU.Checked == true))
            {
                btnFind_Click(sender, e);
            }
        }

        /// <summary>
        /// Change the label text to what is appropriate depending on the radio button selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonBarcode_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonBarcode.Checked == true)
            {
                lblSearchChoice.Text = "Enter or scan barcode below:";
            }

            txtInput.Focus();
        }

        /// <summary>
        /// Change the label text to what is appropriate depending on the radio button selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonStyleNum_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonStyleNum.Checked == true)
            {
                lblSearchChoice.Text = "Enter style number below:";
            }

            txtInput.Focus();
        }

        /// <summary>
        /// Change the label text to what is appropriate depending on the radio button selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonSKU_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSKU.Checked == true)
            {
                lblSearchChoice.Text = "Enter SKU below:";
            }

            txtInput.Focus();
        }

        /// <summary>
        /// Kill the application if the user closes the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StockFind_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        // Custom methods

        /// <summary>
        /// This function contains the SQL command necessary to retrieve the location ID from the database using a barcode.
        /// </summary>
        /// <param name="myReaderIn">Reader object used to read rows from table.</param>
        /// <param name="locationIn">Location object that the ID will be held in.</param>
        private void SQLGetLocationDetailsByBarcode(SqlDataReader myReaderIn, Location locationIn)
        {
            SqlCommand cmdGetLocationDetails = new SqlCommand("dbo.spGetLocationIDByBarcode", myConnection);
            cmdGetLocationDetails.CommandType = CommandType.StoredProcedure;
            cmdGetLocationDetails.Parameters.AddWithValue("@barcodeIn", SqlDbType.VarChar).Value = txtInput.Text;

            myReaderIn = cmdGetLocationDetails.ExecuteReader();

            while (myReaderIn.Read())
            {
                locationIn.SetID(ushort.Parse(myReaderIn["lcn_id"].ToString()));
            }

            myReaderIn.Close();
        }

        /// <summary>
        /// This function contains the SQL command necessary to retrieve the location ID from the database using a barcode.
        /// </summary>
        /// <param name="myReaderIn">Reader object used to read rows from table.</param>
        /// <param name="locationIn">Location object that the ID will be held in.</param>
        private void SQLGetLocationDetailsByStyleNum(SqlDataReader myReaderIn, Location locationIn)
        {
            SqlCommand cmdGetLocationDetails = new SqlCommand("dbo.spGetLocationIDByStyleNum", myConnection);
            cmdGetLocationDetails.CommandType = CommandType.StoredProcedure;
            cmdGetLocationDetails.Parameters.AddWithValue("@StyleNumIn", SqlDbType.VarChar).Value = txtInput.Text.Substring(0, 9);

            myReaderIn = cmdGetLocationDetails.ExecuteReader();

            while (myReaderIn.Read())
            {
                locationIn.SetID(ushort.Parse(myReaderIn["lcn_id"].ToString()));
            }

            myReaderIn.Close();
        }

        /// <summary>
        /// This function contains the SQL command necessary to retrieve the location ID from the database using a barcode.
        /// </summary>
        /// <param name="myReaderIn">Reader object used to read rows from table.</param>
        /// <param name="locationIn">Location object that the ID will be held in.</param>
        private void SQLGetLocationDetailsBySKU(SqlDataReader myReaderIn, Location locationIn)
        {
            SqlCommand cmdGetLocationDetails = new SqlCommand("dbo.spGetLocationIDBySKU", myConnection);
            cmdGetLocationDetails.CommandType = CommandType.StoredProcedure;
            cmdGetLocationDetails.Parameters.AddWithValue("@SKUIn", SqlDbType.VarChar).Value = txtInput.Text.Substring(0, 9);

            myReaderIn = cmdGetLocationDetails.ExecuteReader();

            while (myReaderIn.Read())
            {
                locationIn.SetID(ushort.Parse(myReaderIn["lcn_id"].ToString()));
            }

            myReaderIn.Close();
        }

        /// <summary>
        /// This function contains the SQL command necessary to retrieve the location name from the database using an ID.
        /// </summary>
        /// <param name="myReaderIn">Reader object used to read rows from table.</param>
        /// <param name="locationIn">Location object that the location name will be held in.</param>
        /// <param name="itemExistsIn">Bool variable used to identify whether the item exists in the database or not.</param>
        private void SQLGetLocationDetailsByID(SqlDataReader myReaderIn, Location locationIn, ref bool itemExistsIn)
        {
            SqlCommand cmdGetLocationDetails = new SqlCommand("dbo.spGetLocationNameByID", myConnection);
            cmdGetLocationDetails.CommandType = CommandType.StoredProcedure;
            cmdGetLocationDetails.Parameters.AddWithValue("@IDIn", SqlDbType.VarChar).Value = locationIn.GetID().ToString();

            myReaderIn = cmdGetLocationDetails.ExecuteReader();

            while (myReaderIn.Read())
            {
                locationIn.SetName(myReaderIn["name"].ToString());
                itemExistsIn = true;
            }
            myReaderIn.Close();
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