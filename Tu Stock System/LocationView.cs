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
    public partial class LocationView : Form
    {
        // connection string used to connect to local database
        // used in all SQL queries
        SqlConnection myConnection = new SqlConnection("Server=.;Database=Tu;Trusted_Connection=True;");

        // global employee object to be used in the constructor of any other form opened
        Employee currentUser = new Employee();

        // global list object used to populate locations list box
        List<string> locations = new List<string>();

        /// <summary>
        /// Default constructor.
        /// </summary>
        public LocationView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Second constructor which takes one employee object.
        /// </summary>
        /// <param name="employeeIn">Employee that's currently logged in.</param>
        public LocationView(Employee employeeIn)
        {
            InitializeComponent();

            currentUser = employeeIn;
        }

        // Events

        /// <summary>
        /// When the form loads, populate the locations list box with values from database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LocationView_Load(object sender, EventArgs e)
        {
            listBoxLocations.Items.Clear();
            listBoxStyleNumbers.Items.Clear();

            SQLGetLocations(ref locations);

            foreach (var item in locations)
            {
                listBoxLocations.Items.Add(item);
            }
        }

        /// <summary>
        /// This will populate the second list box with all style numbers associated with the chosen location.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewLocation_Click(object sender, EventArgs e)
        {
            // only carry out the functionality if an item has actually been selected in the locations list box
            if (listBoxLocations.SelectedIndex >= 0)
            {
                listBoxStyleNumbers.Items.Clear();

                List<string> styleNumbers = new List<string>();

                SQLGetStyleNumbersByLocation(ref styleNumbers);

                foreach (var item in styleNumbers)
                {
                    listBoxStyleNumbers.Items.Add(item);
                }
            }

            else
            {
                MessageBox.Show(this, "Please select a location.", "No location selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// This will delete the chosen location, update the locations table in the database, reload the locations list box with the new information and select the appropriate index.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveLocation_Click(object sender, EventArgs e)
        {
            Location locationBeingEdited = new Location();

            // only carry out the necessary work if a location has been chosen
            if (listBoxLocations.SelectedIndex >= 0)
            {
                // only allow user to remove location if they're a manager
                if (currentUser.GetIsManager() == true)
                {

                    locationBeingEdited.SetName(listBoxLocations.SelectedItem.ToString());
                    SQLRemoveFromShopFloor(locationBeingEdited);
                    SQLRemoveLocation(locationBeingEdited);
                    
                    SQLGetLocations(ref locations);
                    listBoxLocations.Items.Clear();

                    foreach (var item in locations)
                    {
                        listBoxLocations.Items.Add(item);
                    }

                    SelectAppropriateIndex(listBoxLocations);
                    listBoxStyleNumbers.Items.Clear();

                    MessageBox.Show(this, "Location removed succesfully.", "Location removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
                else
                {
                    MessageBox.Show(this, $"Please get a manager or team leader to remove this location.", "Unable to remove location", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }

            else
            {
                MessageBox.Show(this, "Please select a location.", "No location selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        /// <summary>
        /// This will remove the chosen barcode from the selected location.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveStock_Click(object sender, EventArgs e)
        {
            Location locationBeingEdited = new Location();

            // only carry out necessary work if a location has been selected
            if (listBoxLocations.SelectedIndex >= 0)
            {
                locationBeingEdited.SetName(listBoxLocations.SelectedItem.ToString());

                // only carry out necessary work if a style number has been selected
                if (listBoxStyleNumbers.SelectedIndex >= 0)
                {
                    SQLRemoveStockFromLocation(locationBeingEdited, listBoxStyleNumbers.SelectedItem.ToString());

                    btnViewLocation_Click(sender, e);

                    SelectAppropriateIndex(listBoxStyleNumbers);
                }

                else
                {
                    MessageBox.Show(this, "Please select a style number.", "No style number selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            else
            {
                MessageBox.Show(this, "Please select a location.", "No location selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Kill the application if the user closes the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LocationView_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        // Custom methods
        
        /// <summary>
        /// Deletes selected item from listbox and will choose the appropriate index.
        /// </summary>
        /// <param name="listBoxIn">Listbox to have values deleted from.</param>
        private void SelectAppropriateIndex(ListBox listBoxIn)
        {
            int newIndex = listBoxIn.SelectedIndex;

            if (newIndex >= listBoxIn.Items.Count)
            {
                listBoxIn.SelectedIndex = newIndex - 1;
            }

            else
            {
                listBoxIn.SelectedIndex = newIndex;
            }
        }

        /// <summary>
        /// This function contains the SQL command necessary to retrieve all style numbers associated with a location.
        /// </summary>
        /// <param name="myReaderIn">Reader used to read rows in chosen table.</param>
        /// <param name="listIn">The list that will be populated with style numbers.</param>
        private void SQLGetStyleNumbersByLocation(ref List<string> listIn)
        {
            myConnection.Open();

            SqlCommand cmdGetStyleNumbers = new SqlCommand("dbo.spGetStyleNumbersByLocationName", myConnection);
            cmdGetStyleNumbers.CommandType = CommandType.StoredProcedure;
            cmdGetStyleNumbers.Parameters.AddWithValue("@LocationNameIn", SqlDbType.VarChar).Value = listBoxLocations.SelectedItem.ToString();
            
            SqlDataReader myReader = null;
            myReader = cmdGetStyleNumbers.ExecuteReader();

            while (myReader.Read())
            {
                listIn.Add(myReader["style_number"].ToString());
            }
            myReader.Close();
            myConnection.Close();
        }

        /// <summary>
        /// This function contains the SQL command necessary to retrieve all locations held in the database.
        /// </summary>
        /// <param name="listIn">The list that will be populated with location names.</param>
        private void SQLGetLocations(ref List<string> listIn)
        {
            listIn.Clear();
            //myConnection.Open();

            //SqlCommand cmdGetLocations = new SqlCommand("dbo.spGetAllLocations", myConnection);
            //cmdGetLocations.CommandType = CommandType.StoredProcedure;

            //SqlDataReader myReader = null;
            //myReader = cmdGetLocations.ExecuteReader();

            //while (myReader.Read())
            //{
            //    listIn.Add(myReader["name"].ToString());
            //}
            //myReader.Close();
            //myConnection.Close();
        }

        /// <summary>
        /// This function contains the SQL command necessary to alter any clothing rows so that they appear to be off the shop floor.
        /// </summary>
        /// <param name="locationIn">The location that is having stock removed.</param>
        private void SQLRemoveFromShopFloor(Location locationIn)
        {
            myConnection.Open();

            SqlCommand cmdRemoveFromShopFloor = new SqlCommand("dbo.spRemoveFromShopFloor", myConnection);
            cmdRemoveFromShopFloor.CommandType = CommandType.StoredProcedure;
            cmdRemoveFromShopFloor.Parameters.AddWithValue("@LocationNameIn", SqlDbType.VarChar).Value = locationIn.GetName();

            cmdRemoveFromShopFloor.ExecuteNonQuery();

            myConnection.Close();
        }

        /// <summary>
        /// This function contains the SQL command necessary to set the lcn_id of appropriate row to null.
        /// </summary>
        /// <param name="locationIn">The location that will have the stock removed.</param>
        /// <param name="styleNumIn">The style number to have its location set to null.</param>
        private void SQLRemoveStockFromLocation(Location locationIn, string styleNumIn)
        {
            myConnection.Open();

            SqlCommand cmdRemoveStockFromLocation = new SqlCommand("dbo.spRemoveStockFromLocation", myConnection);
            cmdRemoveStockFromLocation.CommandType = CommandType.StoredProcedure;
            cmdRemoveStockFromLocation.Parameters.AddWithValue("@LocationNameIn", SqlDbType.VarChar).Value = locationIn.GetName();
            cmdRemoveStockFromLocation.Parameters.AddWithValue("@StyleNumberIn", SqlDbType.VarChar).Value = styleNumIn;

            cmdRemoveStockFromLocation.ExecuteNonQuery();

            myConnection.Close();
        }

        /// <summary>
        /// This function contains the SQL command necessary to remove a location from the database.
        /// </summary>
        /// <param name="locationIn">The location that will be removed from the database.</param>
        private void SQLRemoveLocation(Location locationIn)
        {
            myConnection.Open();

            SqlCommand cmdRemoveLocation = new SqlCommand("dbo.spRemoveLocation", myConnection);
            cmdRemoveLocation.CommandType = CommandType.StoredProcedure;
            cmdRemoveLocation.Parameters.AddWithValue("@LocationNameIn", SqlDbType.VarChar).Value = locationIn.GetName();

            cmdRemoveLocation.ExecuteNonQuery();

            myConnection.Close();
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