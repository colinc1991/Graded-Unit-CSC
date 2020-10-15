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
    public class Clothing
    {
        // attributes
        private uint clothingID;
        private string barcode;
        private string styleNum;
        private string SKU;
        private string department;
        private string size;
        private decimal price;
        private string onShopFloor;
        private ushort quantity;
        private string location;

        // constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Clothing()
        {
            clothingID = 0;
            barcode = "";
            styleNum = "";
            SKU = "";
            department = "";
            size = "";
            price = 0;
            onShopFloor = "No";
            quantity = 0;
            location = "";
        }

        // methods

            /* GET METHODS */
        
        /// <summary>
        /// Used to retrieve the clothing ID.
        /// </summary>
        /// <returns></returns>
        public uint GetID()
        {
            return clothingID;
        }

        /// <summary>
        /// Used to retrieve the barcode.
        /// </summary>
        /// <returns></returns>
        public string GetBarcode()
        {
            return barcode;
        }

        /// <summary>
        /// Used to retrieve the style number.
        /// </summary>
        /// <returns></returns>
        public string GetStyleNo()
        {
            return styleNum;
        }

        /// <summary>
        /// Used to retrieve the SKU.
        /// </summary>
        /// <returns></returns>
        public string GetSKU()
        {
            return SKU;
        }

        /// <summary>
        /// Used to retrieve the department.
        /// </summary>
        /// <returns></returns>
        public string GetDepartment()
        {
            return department;
        }

        /// <summary>
        /// Used to retrieve the size.
        /// </summary>
        /// <returns></returns>
        public string GetSize()
        {
            return size;
        }

        /// <summary>
        /// Used to retrieve the price.
        /// </summary>
        /// <returns></returns>
        public decimal GetPrice()
        {
            return price;
        }

        /// <summary>
        /// Used to retrieve whether the item is on the shop floor or not.
        /// </summary>
        /// <returns></returns>
        public string GetOnShopFloor()
        {
            return onShopFloor;
        }

        /// <summary>
        /// Used to retrieve the quantity.
        /// </summary>
        /// <returns></returns>
        public ushort GetQuantity()
        {
            return quantity;
        }

        /// <summary>
        /// Used to retrieve the location.
        /// </summary>
        /// <returns></returns>
        public string GetLocation()
        {
            return location;
        }


            /* SET METHODS */

        /// <summary>
        /// Used to set the clothingID.
        /// </summary>
        /// <param name="idIn">ID to set the ID as.</param>
        public void SetClothingID(string idIn)
        {
            clothingID = uint.Parse(idIn);
        }

        /// <summary>
        /// Used to set the barcode.
        /// </summary>
        /// <param name="barcodeIn">Barcode to set the barcode as.</param>
        public void SetBarcode(string barcodeIn)
        {
            barcode = barcodeIn;
        }

        /// <summary>
        /// Used to set the style number.
        /// </summary>
        /// <param name="styleNoIn">Style number to set the style number as.</param>
        public void SetStyleNum(string styleNoIn)
        {
            styleNum = styleNoIn;
        }

        /// <summary>
        /// Used to set the SKU.
        /// </summary>
        /// <param name="SKUIn">SKU to set the SKU as.</param>
        public void SetSKU(string SKUIn)
        {
            SKU = SKUIn;
        }

        /// <summary>
        /// Used to set the department.
        /// </summary>
        /// <param name="departmentIn">Department to set the department as.</param>
        public void SetDepartment(string departmentIn)
        {
            department = departmentIn;
        }

        /// <summary>
        /// Used to set the size.
        /// </summary>
        /// <param name="sizeIn">Size to set the size as.</param>
        public void SetSize(string sizeIn)
        {
            size = sizeIn;
        }

        /// <summary>
        /// Used to set the price.
        /// </summary>
        /// <param name="priceIn">Price to set the price as.</param>
        public void SetPrice(decimal priceIn)
        {
            price = priceIn;
        }

        /// <summary>
        /// Used to set whether the item is on the shop floor or not.
        /// </summary>
        /// <param name="OnShopFloorIn">Value that sets whether the item is on the shop floor or not.</param>
        public void SetOnShopFloor(string onShopFloorIn)
        {
            onShopFloor = onShopFloorIn;
        }

        /// <summary>
        /// Used to set the quantity.
        /// </summary>
        /// <param name="quantityIn">Quantity to set the quantity as.</param>
        public void SetQuantity(ushort quantityIn)
        {
            quantity = quantityIn;
        }

        /// <summary>
        /// Used to set the location.
        /// </summary>
        /// <param name="locationIn">Location to set the location as.</param>
        public void SetLocation(string locationIn)
        {
            location = locationIn;
        }
    }
}