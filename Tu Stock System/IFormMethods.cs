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
    interface IFormMethods
    {
        /// <summary>
        /// Will contain code to clear the contents of all text boxes.
        /// </summary>
        void ClearForm();

        /// <summary>
        /// Will contain code to make sure all appropriate fields have input.
        /// </summary>
        /// <returns>Returns true if specified fields have input and vice versa.</returns>
        bool AllFieldsCompleted();
    }
}