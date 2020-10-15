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
    public partial class LocationAdd : Form, IFormMethods
    {
        // connection string used to connect to local database
        // used in all SQL queries
        SqlConnection myConnection = new SqlConnection("Server=.;Database=Tu;Trusted_Connection=True;");

        // global employee object to be used in the constructor of any other form opened
        Employee currentUser = new Employee();

        /// <summary>
        /// Default constructor.
        /// </summary>
        public LocationAdd()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Second constructor which takes one employee object.
        /// </summary>
        /// <param name="employeeIn">Employee that's currently logged in.</param>
        public LocationAdd(Employee employeeIn)
        {
            InitializeComponent();

            currentUser = employeeIn;
        }

        // Events

        /// <summary>
        /// This adds a location to the database assuming both input fields have a value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddLocation_Click(object sender, EventArgs e)
        {
            // check to make sure user doesn't leave input fields blank
            if (AllFieldsCompleted())
            {
                MessageBox.Show(this, "Please ensure that location has both a type and a name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                Location locationBeingAdded = new Location(comboBoxLocations.Text, txtLocationName.Text);

                SQLAddLocation(locationBeingAdded);

                MessageBox.Show(this, "Location added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearForm();
            }
        }

        /// <summary>
        /// Kills the application if user closes form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LocationAdd_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Alert users to naming guidelines for location name when the control has focus.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLocationName_Enter(object sender, EventArgs e)
        {
            ToolTip toolTip1 = new ToolTip();
            toolTip1.ToolTipTitle = "Naming guidelines";
            toolTip1.Show("Please try to use the format \"type + number\" \n(e.g. \"Wall 1\", \"Wall 2\" etc.)", txtLocationName, -70, 40, 5000);
        }

        // Custom methods

        /// <summary>
        /// This function contains the SQL command necessary to add a location to the database.
        /// </summary>
        /// <param name="locationIn">The location that will be added to the database.</param>
        private void SQLAddLocation(Location locationIn)
        {
            myConnection.Open();

            SqlCommand cmdAddLocation = new SqlCommand("dbo.spAddLocation", myConnection);
            cmdAddLocation.CommandType = CommandType.StoredProcedure;
            cmdAddLocation.Parameters.AddWithValue("@NameIn", SqlDbType.VarChar).Value = locationIn.GetName();
            cmdAddLocation.Parameters.AddWithValue("@TypeIn", SqlDbType.VarChar).Value = locationIn.GetType();
            
            cmdAddLocation.ExecuteNonQuery();

            myConnection.Close();
        }

        /// <summary>
        /// This function will clear all input fields on the form.
        /// </summary>
        public void ClearForm()
        {
            comboBoxLocations.SelectedIndex = -1;
            txtLocationName.Clear();
        }

        /// <summary>
        /// This method ensures that each input field has some sort of input from the user.
        /// </summary>
        /// <returns>Returns true if each input field has input and vice versa.</returns>
        public bool AllFieldsCompleted()
        {
            if (comboBoxLocations.Text == "" || txtLocationName.Text == "")
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