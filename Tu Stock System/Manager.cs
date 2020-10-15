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
    class Manager : Employee
    {
        // attribute
        private string managerType;

        // constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Manager()
        {
            managerType = "";
        }

        // methods

        /// <summary>
        /// Returns the managerType attribute.
        /// </summary>
        /// <returns></returns>
        public string GetManagerType()
        {
            return managerType;
        }

        /// <summary>
        /// Used to set the manager type.
        /// </summary>
        /// <param name="managerTypeIn">The type of manager to set the managerType as.</param>
        public void SetManagerType(string managerTypeIn)
        {
            managerType = managerTypeIn;
        }
    }
}