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
    public partial class LocationAddStock : Form
    {
        // connection string used to connect to local database
        // used in all SQL queries
        SqlConnection myConnection = new SqlConnection("Server=.;Database=Tu;Trusted_Connection=True;");

        // global employee object to be used in the constructor of any other form opened
        Employee currentUser = new Employee();

        // global list object used to populate the locations list box
        List<string> locations = new List<string>();

        /// <summary>
        /// Default constructor
        /// </summary>
        public LocationAddStock()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Second constructor which takes one employee object.
        /// </summary>
        /// <param name="employeeIn">Employee that's currently logged in.</param>
        public LocationAddStock(Employee employeeIn)
        {
            InitializeComponent();

            currentUser = employeeIn;
        }

        // Events

        /// <summary>
        /// When the form loads, populate the locations list box with database values and have the barcode text box immediately ready for user input.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LocationAddStock_Load(object sender, EventArgs e)
        {
            listBoxLocations.Items.Clear();
            listBoxBarcodes.Items.Clear();

            myConnection.Open();
            SQLGetLocations(ref locations);
            myConnection.Close();

            foreach (var item in locations)
            {
                listBoxLocations.Items.Add(item);
            }

            txtBarcode.Focus();
        }

        /// <summary>
        /// This button will remove the selected barcode from the attached listbox and will then select the right index for said list box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, EventArgs e)
        {
            SelectAppropriateIndex(listBoxBarcodes);
        }

        /// <summary>
        /// This will update each barcode with the new location chosen by the user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddToLocation_Click(object sender, EventArgs e)
        {
            // make sure the user has actually entered in at least one barcode
            if (listBoxBarcodes.Items.Count == 0)
            {
                MessageBox.Show(this, "Please enter at least one barcode.", "No barcodes entered", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // allow the operation to carry on if they have
            else
            {
                myConnection.Open();

                // used to store each successful addition of style number to location
                string successfulAddition = "";

                try
                {
                    // call the SQL function for each barcode in the list box
                    // but only if the barcode hasn't already been allocated a location
                    // and it actually exists in the database
                    foreach (var item in listBoxBarcodes.Items)
                    {
                        if (SQLItemHasLocation(item.ToString()))
                        {
                            MessageBox.Show(this, $"{item.ToString()} already has a location: {SQLGetLocationNameByBarcode(item.ToString())}.", "Stock not added to location", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        else if (SQLItemExists(item.ToString()) == false)
                        {
                            MessageBox.Show(this, $"Barcode: {item.ToString()} does not exist in database.", "Barcode does not exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        else
                        {
                            SQLAddStockToLocation(SQLGetLocationIDByName(listBoxLocations.SelectedItem.ToString()), item.ToString());
                            successfulAddition += item.ToString() + "\n";
                        }
                    }
                    myConnection.Close();
                }

                // an item in the location listbox hasn't been selected
                catch (NullReferenceException)
                {
                    MessageBox.Show(this, "Please select a location.", "No location selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // close the connection if an error is thrown
                finally
                {
                    myConnection.Close();
                }

                // only display success message if barcodes were actually added to a location
                if (successfulAddition != "")
                {
                    MessageBox.Show(this, $"The following barcode(s):\n{successfulAddition}has/have been successfully added to {listBoxLocations.SelectedItem.ToString()}.", "Stock added to location", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    listBoxBarcodes.Items.Clear();
                    txtBarcode.ResetText();
                }
            }
        }

        /// <summary>
        /// Kill application if user closes the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LocationAddStock_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Adds the entered barcode (barcodes are always 13 characters) to the listbox only if it already hasn't been added.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBarcode_TextChanged(object sender, EventArgs e)
        {
            // making sure the barcode is sufficient length and hasn't already been added
            if (txtBarcode.Text.Length == 13 && !listBoxBarcodes.Items.Contains(txtBarcode.Text))
            {
                // add the barcode, clear the text box and focus the text box for the next barcode
                listBoxBarcodes.Items.Add(txtBarcode.Text);

                // code used to ensure that the cursor is set to the start
                // of the text box. Will not work without BeginInvoke...
                this.BeginInvoke((MethodInvoker)delegate ()
                {
                    txtBarcode.ResetText();
                    txtBarcode.Select(0, 0);
                });
            }

            // if the entered barcode already exists, display message to user and reset contents of text box
            else if (txtBarcode.Text.Length == 13 && listBoxBarcodes.Items.Contains(txtBarcode.Text))
            {
                MessageBox.Show(this, "Unable to add barcode - this barcode has already been entered.", "Barcode duplicate", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                // code used to ensure that the cursor is set to the start
                // of the text box. Will not work without BeginInvoke...
                this.BeginInvoke((MethodInvoker)delegate ()
                {
                    txtBarcode.ResetText();
                    txtBarcode.Select(0, 0);
                });
            }
        }

        // Custom methods

        /// <summary>
        /// This function contains the SQL command necessary to check if an item already has a location using a given barcode.
        /// </summary>
        /// <param name="barcodeIn">The barcode associated with the piece of clothing being checked.</param>
        /// <returns>Returns true if the item has a location and vice versa.</returns>
        private bool SQLItemHasLocation(string barcodeIn)
        {
            SqlCommand cmdGetStockDetails = new SqlCommand("dbo.spGetStockDetailsByBarcode", myConnection);
            cmdGetStockDetails.CommandType = CommandType.StoredProcedure;
            cmdGetStockDetails.Parameters.AddWithValue("@barcodeIn", SqlDbType.VarChar).Value = barcodeIn;

            SqlDataReader myReader = null;
            myReader = cmdGetStockDetails.ExecuteReader();
            bool itemHasLocation = false;

            while (myReader.Read())
            {
                if (myReader["on_shop_floor"].ToString() == "No")
                {
                    itemHasLocation = false;
                }

                else
                {
                    itemHasLocation = true;
                }
            }
            myReader.Close();

            return itemHasLocation;
        }

        /// <summary>
        /// This function contains the SQL command necessary to check if an item actually exists in the database using a given barcode.
        /// </summary>
        /// <param name="barcodeIn">The barcode associated with the piece of clothing being checked.</param>
        /// <returns>Returns true if the item exists and vice versa.</returns>
        private bool SQLItemExists(string barcodeIn)
        {
            SqlCommand cmdGetStockDetails = new SqlCommand("dbo.spGetStockDetailsByBarcode", myConnection);
            cmdGetStockDetails.CommandType = CommandType.StoredProcedure;
            cmdGetStockDetails.Parameters.AddWithValue("@barcodeIn", SqlDbType.VarChar).Value = barcodeIn;

            SqlDataReader myReader = null;
            myReader = cmdGetStockDetails.ExecuteReader();
            bool itemExists = false;

            if (myReader.HasRows)
            {
                itemExists = true;
            }

            else
            {
                itemExists = false;
            }

            myReader.Close();

            return itemExists;
        }

        /// <summary>
        /// This function contains the SQL command necessary to check if an item already has a location using a given barcode.
        /// </summary>
        /// <param name="barcodeIn">The barcode associated with the piece of clothing being checked.</param>
        /// <returns>Returns the name of the location that the barcode is assigned to.</returns>
        private string SQLGetLocationNameByBarcode(string barcodeIn)
        {
            SqlDataReader myReader = null;
            string output = "";
            SqlCommand cmdGetLocationName = new SqlCommand("dbo.spGetLocationNameByBarcode", myConnection);
            cmdGetLocationName.CommandType = CommandType.StoredProcedure;
            cmdGetLocationName.Parameters.AddWithValue("@BarcodeIn", SqlDbType.VarChar).Value = barcodeIn;

            myReader = cmdGetLocationName.ExecuteReader();

            while (myReader.Read())
            {
                output = myReader["name"].ToString();
            }
            myReader.Close();

            return output;
        }

        /// <summary>
        /// This function contains the SQL command necessary to retrieve all locations names in the database and populate the list parameter with said names.
        /// </summary>
        /// <param name="listIn">The list that will have the location names fed in.</param>
        private void SQLGetLocations(ref List<string> listIn)
        {
            SqlCommand cmdGetLocations = new SqlCommand("dbo.spGetAllLocations", myConnection);
            cmdGetLocations.CommandType = CommandType.StoredProcedure;

            SqlDataReader myReader = null;
            myReader = cmdGetLocations.ExecuteReader();

            while (myReader.Read())
            {
                listIn.Add(myReader["name"].ToString());
            }
            myReader.Close();
        }

        /// <summary>
        /// This function contains the SQL command necessary to update the clothing item with the new location ID.
        /// </summary>
        /// <param name="locationIDIn"></param>
        /// <param name="barcodeIn"></param>
        private void SQLAddStockToLocation(string locationIDIn, string barcodeIn)
        {
            SqlCommand cmdAddStockToLocation = new SqlCommand("dbo.spAddStockToLocation", myConnection);
            cmdAddStockToLocation.CommandType = CommandType.StoredProcedure;
            cmdAddStockToLocation.Parameters.AddWithValue("@LocationIDIn", SqlDbType.VarChar).Value = locationIDIn;
            cmdAddStockToLocation.Parameters.AddWithValue("@BarcodeIn", SqlDbType.VarChar).Value = barcodeIn;

            cmdAddStockToLocation.ExecuteNonQuery();
        }

        /// <summary>
        /// This function contains the SQL command necessary to get the location id of the location name supplied.
        /// </summary>
        /// <param name="locationNameIn">The name of the location that the ID is required for.</param>
        /// <returns>Returns the location ID in a string format.</returns>
        private string SQLGetLocationIDByName(string locationNameIn)
        {
            SqlDataReader myReader = null;
            string output = "";
            SqlCommand cmdGetLocationDetails = new SqlCommand("dbo.spGetLocationIDByName", myConnection);
            cmdGetLocationDetails.CommandType = CommandType.StoredProcedure;
            cmdGetLocationDetails.Parameters.AddWithValue("@NameIn", SqlDbType.VarChar).Value = locationNameIn;

            myReader = cmdGetLocationDetails.ExecuteReader();

            while (myReader.Read())
            {
                output = myReader["location_id"].ToString();
            }
            myReader.Close();

            return output;
        }

        /// <summary>
        /// Deletes selected item from listbox and updates the index accordingly.
        /// </summary>
        /// <param name="listBoxIn">Listbox to have values deleted from.</param>
        private void SelectAppropriateIndex(ListBox listBoxIn)
        {
            int newIndex = listBoxIn.SelectedIndex;

            listBoxIn.Items.Remove(listBoxIn.SelectedItem);

            if (newIndex >= listBoxIn.Items.Count)
            {
                listBoxIn.SelectedIndex = newIndex - 1;
            }

            else
            {
                listBoxIn.SelectedIndex = newIndex;
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