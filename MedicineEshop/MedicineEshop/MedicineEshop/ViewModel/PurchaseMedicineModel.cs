using System.Collections.Generic;

namespace MedicineEshop.ViewModel
{
    public class PurchaseMedicineModel
    {
        public int PurchaseInfoId { get; set; }
        public string PurchaeNumber { get; set; }
        public string PurchaseDate { get; set; }
        public int TotalItem { get; set; }
        public double TotalPurchaseAmount { get; set; }
        public string PurchaedBy { get; set; }

        public List<PurchaseMedicineItem> PurchaseMedicineItemList { get; set; }
    }

    public class PurchaseMedicineItem
    {
        public int PurchaseInfoId { get; set; }
        public int PurchaseItemId { get; set; }
        public int MedicineId { get; set; }
        public string MedicineName { get; set; }
        public string CompanyName { get; set; }
        public string MedicineTypeName { get; set; }
        public double PurchasePrice { get; set; }
        public double SalePrice { get; set; }
        public int Quantity { get; set; }
        public double TotalAmount { get; set; }
    }
}