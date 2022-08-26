using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Zen.Barcode;
using ZXing;

namespace MedicineEshop.Utility
{
    public class UtilityClass
    {
        //For dropdown list
        public static SelectList GetSelectListByDataTable(DataTable objDataTable, string pValueField, string pTextField)
        {
            List<SelectListItem> objSelectListItems = new List<SelectListItem>
                {
                    new SelectListItem() {Value = "", Text = "--Select Item--"},
                };


            objSelectListItems.AddRange(from DataRow dataRow in objDataTable.Rows
                                        select new SelectListItem()
                                        {
                                            Value = dataRow[pValueField].ToString(),
                                            Text = dataRow[pTextField].ToString()
                                        });

            return new SelectList(objSelectListItems, "Value", "Text");
        }
 
    }
}