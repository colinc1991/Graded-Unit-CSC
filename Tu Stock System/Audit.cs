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
    public partial class Audit : Form
    {
        // connection string used to connect to local database
        // used in all SQL queries
        SqlConnection myConnection = new SqlConnection("Server=.;Database=Tu;Trusted_Connection=True;");

        // global employee object to be used in the constructor of any other form opened
        Employee currentUser = new Employee();

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Audit()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Second constructor which takes one employee object.
        /// </summary>
        /// <param name="employeeIn">Employee that's currently logged in.</param>
        public Audit(Employee employeeIn)
        {
            InitializeComponent();

            currentUser = employeeIn;
        }

        // Events

        /// <summary>
        /// When the form loads, populate the users combo box with values from database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Audit_Load(object sender, EventArgs e)
        {
            SQLFillUsersBox();
        }
        
        /// <summary>
        /// Call SQL function when the audit button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAudit_Click(object sender, EventArgs e)
        {
            // make sure a user has actually been selected first
            if (comboBoxUsernames.SelectedIndex >= 0)
            {
                txtAuditResult.Clear();
                SQLAuditUser(comboBoxUsernames.SelectedItem.ToString());
            }

            else
            {
                MessageBox.Show(this, "Please select a user to audit.", "User not selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Kill the application if user closes the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Audit_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        // Custom method

        /// <summary>
        /// This function contains the SQL necessary to retrieve the activity of the chosen colleague.
        /// </summary>
        /// <param name="usernameIn">The user whose activity is being checked.</param>
        private void SQLAuditUser(string usernameIn)
        {
            SqlCommand cmdAuditUser = new SqlCommand("dbo.spAuditUser", myConnection);
            cmdAuditUser.CommandType = CommandType.StoredProcedure;
            cmdAuditUser.Parameters.AddWithValue("@UsernameIn", SqlDbType.VarChar).Value = usernameIn;

            myConnection.Open();

            SqlDataReader myReader = null;
            myReader = cmdAuditUser.ExecuteReader();
            int auditCount = 1;

            // as rows are being read, populate the text box with the values of the rows
            while (myReader.Read())
            {
                if (myReader["update_type"].ToString() == "Add")
                {
                    txtAuditResult.Text += $"{auditCount.ToString()}) {usernameIn} added barcode {myReader["barcode"]} on {myReader["date"]}.\n";
                }

                else
                {
                    txtAuditResult.Text += $"{auditCount.ToString()}) {usernameIn} modified barcode {myReader["barcode"]} on {myReader["date"]}.\n";
                }

                auditCount++;
            }
            myReader.Close();
            myConnection.Close();
        }

        /// <summary>
        /// This function contains the necessary SQL to populate the combo box with all usernames attached to managers
        /// </summary>
        /// <param name="listIn"></param>
        private void SQLFillUsersBox()
        {
            SqlCommand cmdGetLocations = new SqlCommand("dbo.spGetUsernames", myConnection);
            cmdGetLocations.CommandType = CommandType.StoredProcedure;

            myConnection.Open();
            SqlDataReader myReader = null;
            myReader = cmdGetLocations.ExecuteReader();

            while (myReader.Read())
            {
                comboBoxUsernames.Items.Add(myReader["username"].ToString());
            }
            myReader.Close();
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