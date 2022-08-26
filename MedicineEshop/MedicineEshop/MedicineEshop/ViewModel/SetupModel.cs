using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedicineEshop.ViewModel
{
    public class SetupModel
    {
        public string ActiveYN { get; set; }
        public string CreateBy { get; set; }
        public string CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public string UpdateDate { get; set; }

        [DisplayName("Active Status")]
        public bool ActiveStatus { get; set; }
    }

    public class CompanySetupModel : SetupModel
    {
        public int CompanyId { get; set; }

        [Required]
        [DisplayName("Company Name")]
        public string ComapnayName { get; set; }

        [Required]
        [DisplayName("Company Address")]
        public string Address { get; set; }

        [Required]
        [DisplayName("Contact Number")]
        public string ContactNo { get; set; }

        [Required]
        [DisplayName("Email")]
        public string ComapanyEmail { get; set; }
    }
    
    public class MedicineTypeSetupModel : SetupModel
    {
        public int MedicineTypeId { get; set; }

        [Required]
        [DisplayName("Medicine Type")]
        public string MedicineTypeName { get; set; }
    }

    public class PaymentTypeSetupModel : SetupModel
    {
        public int PaymentTypeId { get; set; }

        [Required]
        [DisplayName("Payment Type")]
        public string PaymentTypeName { get; set; }
    }

    public class MedicineSetupModel : SetupModel
    {
        public int MedicineId { get; set; }

        [Required]
        [DisplayName("Medicine Name")]
        public string MedicineName { get; set; }

        [Required]
        [DisplayName("Medicine Description")]
        public string MedicineDescription { get; set; }

        [Required]
        [DisplayName("Company")]
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }

        [Required]
        [DisplayName("Medicine Type")]
        public int MedicineTypeId { get; set; }
        public string MedicineTypeName { get; set; }

        [Required]
        [DisplayName("Purchase Price")]
        public double? PurchasePrice { get; set; }

        [Required]
        [DisplayName("Sale Price")]
        public double? SalePrice { get; set; }

        public int TotalPurchaseQty { get; set; }
        public int TotalSaleQty { get; set; }
        public int StockQty { get; set; }
        public int TotalItem { get; set; }
    }
}