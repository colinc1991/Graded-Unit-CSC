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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tu_Stock_System
{
    public partial class AddUser : Form
    {
        // connection string used to connect to local database
        // used in all SQL queries
        SqlConnection myConnection = new SqlConnection("Server=.;Database=Tu;Trusted_Connection=True;");

        // global employee object to be used in the constructor of any other form opened
        Employee currentUser = new Employee();

        /// <summary>
        /// Default constructor.
        /// </summary>
        public AddUser()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Second constructor which takes one employee object.
        /// </summary>
        /// <param name="employeeIn">Employee that's currently logged in.</param>
        public AddUser(Employee employeeIn)
        {
            InitializeComponent();

            currentUser = employeeIn;
        }

        // Events

        /// <summary>
        /// Adds the user subject to conditions being met
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddUser_Click(object sender, EventArgs e)
        {
            // Check for input in all necessary fields
            if (txtUsername.Text == "" || txtPassword.Text == "" || comboBoxUserTypes.Text == "")
            {
                MessageBox.Show(this, "Please make sure a username, password and employee type have been selected.", "User not added", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Make sure password meets strength requirements
            else if (PasswordIsStrong() == false)
            {
                ToolTip toolTip1 = new ToolTip();
                toolTip1.ToolTipTitle = "Invalid password";
                toolTip1.Show("Password must contain a capital letter \nand is at least 7 characters long.", txtPassword, 0, 40, 5000);
            }

            else if (SQLEmployeeExists(txtUsername.Text))
            {
                MessageBox.Show(this, "User already exists - please select a different username.", "User already exists", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Add the user
            else
            {
                string hash = "SDifopIrltwwaIhthalafc";
                DESEncrypt encryptor = new DESEncrypt();
                Employee newEmployee = new Employee(txtUsername.Text.ToLower(), encryptor.Encrypt(txtPassword.Text, hash), comboBoxUserTypes.Text);

                myConnection.Open();

                // Adding a colleague to database
                if (newEmployee.GetEmployeeType() == "Colleague")
                {
                    // Set employee type to 3 character long abbreviations for SQL input
                    newEmployee.SetEmployeeType("clg");

                    SQLAddColleague(newEmployee);
                }

                // Adding a manager to database
                else
                {
                    // make sure manager type has been chosen
                    // otherwise don't execute SQL INSERT
                    if (comboBoxManagerTypes.Text == "")
                    {
                        MessageBox.Show(this, "Please select a manager type.", "Manager not added", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    else
                    {
                        // Set employee type to 3 character long abbreviations for SQL input
                        newEmployee.SetEmployeeType("mgr");

                        Manager newManager = new Manager();
                        newManager.SetUsername(newEmployee.GetUsername());
                        newManager.SetPassword(newEmployee.GetPassword());
                        newManager.SetEmployeeType(newEmployee.GetEmployeeType());
                        newManager.SetManagerType(comboBoxManagerTypes.Text);

                        SQLAddManager(newManager);
                    }
                }

                myConnection.Close();

                MessageBox.Show(this, $"New user ({newEmployee.GetUsername()}) added successfully.", "User added", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtUsername.ResetText();
                txtPassword.ResetText();
            }
        }

        /// <summary>
        /// Only display Manager related information if user being added is a manager.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxUserTypes_TextChanged(object sender, EventArgs e)
        {
            if (comboBoxUserTypes.SelectedIndex == 1)
            {
                comboBoxManagerTypes.Visible = true;
                lblManagerType.Visible = true;
            }

            else
            {
                comboBoxManagerTypes.Visible = false;
                lblManagerType.Visible = false;
            }
        }
        
        /// <summary>
        /// Kill the application if user closes form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddUser_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Inform the user as to what the naming format for usernames should be via tooltip.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUsername_Enter(object sender, EventArgs e)
        {
            ToolTip toolTip1 = new ToolTip();
            toolTip1.ToolTipTitle = "Naming guidelines";
            toolTip1.Show("Username should formatted: first initial+surname (e.g. Adam Henry = ahenry).", txtUsername, 0, 40, 5000);
        }

        /// <summary>
        /// Explain what is meant by combo box (requested via user feedback)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxManagerTypes_Enter(object sender, EventArgs e)
        {
            ToolTip toolTip1 = new ToolTip();
            toolTip1.ToolTipTitle = "Manager guidelines";
            toolTip1.Show("Is the user being added a manager or team leader?", comboBoxManagerTypes, 0, -40, 5000);
        }

        // Custom methods

        /// <summary>
        /// This function contains the SQL command necessary to add a colleague to the database.
        /// </summary>
        /// <param name="employeeIn">The employee that will be added to the database.</param>
        private void SQLAddColleague(Employee employeeIn)
        {
            SqlCommand cmdAddColleague = new SqlCommand("dbo.spAddColleague", myConnection);
            cmdAddColleague.CommandType = CommandType.StoredProcedure;
            cmdAddColleague.Parameters.AddWithValue("@UsernameIn", SqlDbType.VarChar).Value = employeeIn.GetUsername();
            cmdAddColleague.Parameters.AddWithValue("@PasswordIn", SqlDbType.VarChar).Value = employeeIn.GetPassword();
            cmdAddColleague.Parameters.AddWithValue("@EmployeeTypeIn", SqlDbType.VarChar).Value = employeeIn.GetEmployeeType();

            cmdAddColleague.ExecuteNonQuery();
        }

        /// <summary>
        /// This function contains the SQL command necessary to add a manager to the database.
        /// </summary>
        /// <param name="employeeIn">The employee that will be added to the database.</param>
        private void SQLAddManager(Manager managerIn)
        {
            SqlCommand cmdAddManager = new SqlCommand("dbo.spAddManager", myConnection);
            cmdAddManager.CommandType = CommandType.StoredProcedure;
            cmdAddManager.Parameters.AddWithValue("@UsernameIn", SqlDbType.VarChar).Value = managerIn.GetUsername();
            cmdAddManager.Parameters.AddWithValue("@PasswordIn", SqlDbType.VarChar).Value = managerIn.GetPassword();
            cmdAddManager.Parameters.AddWithValue("@ManagerTypeIn", SqlDbType.VarChar).Value = managerIn.GetManagerType();
            cmdAddManager.Parameters.AddWithValue("@EmployeeTypeIn", SqlDbType.VarChar).Value = managerIn.GetEmployeeType();

            cmdAddManager.ExecuteNonQuery();
        }

        /// <summary>
        /// This function contains the SQL command necessary to check if a username has already been added to the database.
        /// </summary>
        /// <param name="usernameIn">The username being checked.</param>
        /// <returns>Returns true if a username exists and vice versa.</returns>
        private bool SQLEmployeeExists(string usernameIn)
        {
            myConnection.Open();

            SqlCommand cmdCheckUsername = new SqlCommand("dbo.spGetEmployeeDetails", myConnection);
            cmdCheckUsername.CommandType = CommandType.StoredProcedure;
            cmdCheckUsername.Parameters.AddWithValue("@UsernameIn", SqlDbType.VarChar).Value = usernameIn;

            SqlDataReader myReader = null;
            myReader = cmdCheckUsername.ExecuteReader();
            bool employeeExists = false;

            if (myReader.HasRows)
            {
                employeeExists = true;
            }
            
            else
            {
                employeeExists = false;
            }

            myReader.Close();
            myConnection.Close();

            return employeeExists;
        }

        /// <summary>
        /// A function that checks the password is sufficiently strong (at least 7 characters long with a capital letter) for entry into database.
        /// </summary>
        /// <returns></returns>
        private bool PasswordIsStrong()
        {
            if (txtPassword.Text.Length < 7 || txtPassword.Text == txtPassword.Text.ToLower())
            {
                return false;
            }

            else
            {
                return true;
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