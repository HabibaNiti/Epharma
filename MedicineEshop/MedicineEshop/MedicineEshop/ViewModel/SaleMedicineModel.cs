using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedicineEshop.ViewModel
{
    public class SaleMedicineModel
    {
        public int SaleInfoId { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public int TotalItem { get; set; }
        public double TotalSaleAmount { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int PaymentTypeID { get; set; }
        public string PaymentType { get; set; }
        public string PaymentRefNumber { get; set; }
        public bool PaymentVerificationStatus { get; set; }
        public string PaymentVerifyYN { get; set; }
        public bool DeliveryStatus { get; set; }
        public string DeliveryYN { get; set; }
        public string DeliveryDate { get; set; }
        public string DeliveryBy { get; set; }
        public bool PrescriptionVerificationStatus { get; set; }
        public string PrescriptionVerifyYN { get; set; }
        public string ImageString { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }

        public List<SaleMedicineItem> SaleMedicineItemList { get; set; }
    }

    public class SaleMedicineItem
    {
        public int SaleInfoId { get; set; }
        public int SaleItemId { get; set; }
        public int MedicineId { get; set; }
        public string MedicineName { get; set; }
        public string CompanyName { get; set; }
        public string MedicineTypeName { get; set; }
        public double SalePrice { get; set; }
        public int Quantity { get; set; }
        public double TotalAmount { get; set; }
    }
}