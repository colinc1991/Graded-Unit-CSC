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
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tu_Stock_System
{
    public partial class SplashScreen : Form
    {
        // connection string used to connect to local database
        // used in all SQL queries
        SqlConnection myConnection = new SqlConnection("Server=.;Database=Tu;Trusted_Connection=True;");

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SplashScreen()
        {
            InitializeComponent();
        }

        // Events

        /// <summary>
        /// Log in button checks username and password.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogIn_Click(object sender, EventArgs e)
        {
            Employee employee1 = new Employee(txtUsername.Text.ToLower(), txtPassword.Text);
            bool validEmployee = false;

            SQLValidateUser(employee1, ref validEmployee);

            // only allow access to application if users credentials check out
            if (validEmployee)
            {
                employee1.SetIsManager(employee1.GetEmployeeType());

                MessageBox.Show(this, "Login successful.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // set password back to blank value as it is never used again
                employee1.SetPassword("");

                StockFind stockFind = new StockFind(employee1);

                stockFind.Show();
                this.Hide();
            }

            else
            {
                MessageBox.Show(this, "Login unsuccessful.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.ResetText();
            }
        }

        /// <summary>
        /// If user closes the form, kill the application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SplashScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        // Custom method

        /// <summary>
        /// This function contains the SQL command necessary to check a users credentials.
        /// </summary>
        /// <param name="employeeIn">Employee whose credentials are being checked.</param>
        /// <param name="validEmployeeIn">Bool variable used to identify whether an employee has been found or not.</param>
        private void SQLValidateUser(Employee employeeIn, ref bool validEmployeeIn)
        {
            myConnection.Open();

            SqlCommand cmdValidateUser = new SqlCommand("dbo.spGetEmployeeDetails", myConnection);
            cmdValidateUser.CommandType = CommandType.StoredProcedure;
            cmdValidateUser.Parameters.AddWithValue("@UsernameIn", SqlDbType.VarChar).Value = employeeIn.GetUsername();

            SqlDataReader myReader = null;
            DESEncrypt decryptor = new DESEncrypt();
            string hash = "SDifopIrltwwaIhthalafc";

            myReader = cmdValidateUser.ExecuteReader();

            while (myReader.Read())
            {
                // if the decrypted password and the username matches what the user has already entered, allow access to rest of application
                if (decryptor.Decrypt(myReader["password"].ToString(), hash) == employeeIn.GetPassword() && myReader["username"].ToString() == employeeIn.GetUsername())
                {
                    validEmployeeIn = true;
                    employeeIn.SetEmployeeType(myReader["epe_type"].ToString());
                    employeeIn.SetID(myReader["employee_id"].ToString());
                }
            }
            myReader.Close();
            myConnection.Close();
        }
    }
}