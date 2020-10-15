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
    public class Location
    {
        // attributes
        private ushort locationID;
        private string type;
        private string name;

        // constructors
        
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Location()
        {
            locationID = 0;
            type = "";
            name = "";
        }

        /// <summary>
        /// Constructor that takes two parameters that specify type and name of location.
        /// </summary>
        /// <param name="typeIn">Text for specifying type of location.</param>
        /// <param name="nameIn">Text for specifying name of location.</param>
        public Location(string typeIn, string nameIn)
        {
            locationID = 0;
            type = typeIn;
            name = nameIn;
        }

        // methods

            /*GET METHODS*/

        /// <summary>
        /// Returns the ID attribute.
        /// </summary>
        /// <returns></returns>
        public ushort GetID()
        {
            return locationID;
        }

        /// <summary>
        /// Returns the type attribute.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// "new" keyword required because GetType() method already exists
        /// </remarks>
        public new string GetType()
        {
            // note for method: "new" keyword was required as the object class already has a method known as GetType()
            return type;
        }

        /// <summary>
        /// Returns the name attribute.
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return name;
        }

            /*SET METHODS*/

        /// <summary>
        /// Used to set the ID.
        /// </summary>
        /// <param name="IDIn">ID to set the ID as.</param>
        public void SetID(ushort IDIn)
        {
            locationID = IDIn;
        }

        /// <summary>
        /// Used to set the location type.
        /// </summary>
        /// <param name="typeIn">Type to set the type as.</param>
        public void SetType(string typeIn)
        {
            type = typeIn;
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