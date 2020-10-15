// Name: Colin Campbell
// Class: Graded Unit
// Project description: A stock system to be used within the clothing department of Sainsburys (Tu Clothing)
// Version: 1.00
// Date: 07/02/2018

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tu_Stock_System
{
    public class Employee
    {
        // attributes
        protected ushort employeeID;
        protected string username;
        protected string password;
        protected string employeeType;
        protected bool isManager;

        // constructors

        /// <summary>
        /// Constructor that takes three arguments that set username, password and employee type.
        /// </summary>
        /// <param name="usernameIn">Text for specifying username.</param>
        /// <param name="passwordIn">Text for specifying password.</param>
        /// <param name="employeeType">Text for specifying employee type.</param>
        public Employee(string usernameIn, string passwordIn, string employeeTypeIn)
        {
            employeeID = 0;
            username = usernameIn;
            password = passwordIn;
            employeeType = employeeTypeIn;

            if (employeeTypeIn == "Manager")
            {
                isManager = true;
            }

            else
            {
                isManager = false;
            }
        }

        /// <summary>
        /// Constructor that takes two arguments that set username and password.
        /// </summary>
        /// <param name="usernameIn">Text for specifying username.</param>
        /// <param name="passwordIn">Text for specifying password.</param>
        public Employee(string usernameIn, string passwordIn)
        {
            employeeID = 0;
            username = usernameIn;
            password = passwordIn;
            employeeType = "";
            isManager = false;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Employee()
        {
            employeeID = 0;
            username = "";
            password = "";
            employeeType = "";
            isManager = false;
        }

        // methods

            /* GET METHODS */

        /// <summary>
        /// Used to retrieve the employee ID.
        /// </summary>
        /// <returns></returns>
        public ushort GetEmployeeID()
        {
            return employeeID;
        }

        /// <summary>
        /// Used to retrieve the username.
        /// </summary>
        /// <returns></returns>
        public string GetUsername()
        {
            return username;
        }

        /// <summary>
        /// Used to retrieve the password.
        /// </summary>
        /// <returns></returns>
        public string GetPassword()
        {
            return password;
        }

        /// <summary>
        /// Used to retrieve the employee type.
        /// </summary>
        /// <returns></returns>
        public string GetEmployeeType()
        {
            return employeeType;
        }

        /// <summary>
        /// Used to retrieve whether the employee is a manager or not.
        /// </summary>
        /// <returns></returns>
        public bool GetIsManager()
        {
            return isManager;
        }

            /* SET METHODS */

        /// <summary>
        /// Used to set the employee ID.
        /// </summary>
        /// <param name="idIN">ID to set the ID as.</param>
        public void SetID(string idIn)
        {
            employeeID = ushort.Parse(idIn);
        }

        /// <summary>
        /// Used to set the username.
        /// </summary>
        /// <param name="usernameIn">Username to set the username as.</param>
        public void SetUsername(string usernameIn)
        {
            username = usernameIn;
        }

        /// <summary>
        /// Used to set the password.
        /// </summary>
        /// <param name="passwordIn">Password to set the password as.</param>
        public void SetPassword(string passwordIn)
        {
            password = passwordIn;
        }

        /// <summary>
        /// Used to set the employee type.
        /// </summary>
        /// <param name="employeeTypeIn">Employee type to set the employee type as.</param>
        public void SetEmployeeType(string employeeTypeIn)
        {
            employeeType = employeeTypeIn;
        }

        /// <summary>
        /// Used to set the whether the employee is a manager.
        /// </summary>
        /// <param name="employeeTypeIn">Employee type that determines whether the employee is a manager.</param>
        public void SetIsManager(string employeeTypeIn)
        {
            if (employeeTypeIn == "Manager" || employeeTypeIn == "mgr")
            {
                isManager = true;
            }

            else
            {
                isManager = false;
            }
        }
    }
}