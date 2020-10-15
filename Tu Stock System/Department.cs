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
    class Department
    {
        // attributes
        private byte departmentID;
        private string name;

        // constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Department()
        {
            departmentID = 0;
            name = "";
        }

        // methods

            /* GET METHODS */

        /// <summary>
        /// Used to retrieve the department ID.
        /// </summary>
        /// <returns></returns>
        public byte GetID()
        {
            return departmentID;
        }

        /// <summary>
        /// Used to retrieve the name.
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return name;
        }

            /* SET METHODS */

        /// <summary>
        /// Used to set the department ID.
        /// </summary>
        /// <param name="IDIn">ID to set the department ID as.</param>
        public void SetID(byte IDIn)
        {
            departmentID = IDIn;
        }

        /// <summary>
        /// Used to set the name.
        /// </summary>
        /// <param name="nameIn">Name to set the name as.</param>
        public void SetName(string nameIn)
        {
            name = nameIn;
        }
    }
}