using MedicineEshop.DAL;
using MedicineEshop.Utility;
using MedicineEshop.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MedicineEshop.Controllers
{
    [LogAction]
    public class SaleMedicineController : Controller
    {
        private readonly PurchaseMedicineDal _objPurchaseMedicineDal = new PurchaseMedicineDal();
        private readonly SaleMedicineDal _objSaleMedicineDal = new SaleMedicineDal();
        private readonly DropdownDAL _objDropdownDal = new DropdownDAL();

        #region "Common"       
        private string _strEmployeeId = "";
        private string _strEmployeeRole = "";

        public void LoadSession()
        {
            var auth = Session["authentication"] as AuthModel;
            if (auth != null)
            {
                _strEmployeeId = auth.EmployeeId;
                _strEmployeeRole = auth.EmployeeRole;
            }
            else
            {
                string url = Url.Action("Index", "Auth");
                if (url != null) Response.Redirect(url);
            }
        }

        #endregion

        [RoleFilter]
        public async Task<ActionResult> SaleMedicineList()
        {
            LoadSession();
            var customerId = _strEmployeeId;
            ViewBag.SaleMedicineList = await _objSaleMedicineDal.SaleMedicineList(customerId);
            return View();
        }

        public async Task<JsonResult> GetSaleMedicineList(int saleInfoId)
        {
            var objSaleMedicineModel = await _objSaleMedicineDal.GetSaleMedicineList(saleInfoId);

            return Json(objSaleMedicineModel, JsonRequestBehavior.AllowGet);
        }

        [RoleFilter]
        public async Task<ActionResult> SaleMedicine()
        {
            ViewBag.PaymentTypeList = UtilityClass.GetSelectListByDataTable(await _objDropdownDal.GetPaymentTypeListDropdown(), "PAYMENT_TYPE_ID", "PAYMENT_TYPE_NAME");
            return View();
        }

        public async Task<ActionResult> GetAllMedicineForSale(DataTableAjaxPostModel model)
        {
            var data = await _objPurchaseMedicineDal.GetAllMedicineForPurchase(model);

            var recordsFiltered = data.Count > 0 ? data[0].TotalItem : data.Count;

            int recordsTotal = data.Count;

            if (recordsTotal < model.length)
            {
                recordsTotal = model.length;
            }

            return Json(new { model.draw, recordsTotal, recordsFiltered, data }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> SaveAllSoldMedicineItem(SaleMedicineModel objSaleMedicineModel)
        {
            LoadSession();
            string returnMessage = "";

            objSaleMedicineModel.ImageString = UtilityClass.ImageName != null ? UtilityClass.ImageName : "";

            objSaleMedicineModel.CustomerId = _strEmployeeId;

            try
            {

                if (objSaleMedicineModel.SaleMedicineItemList != null)
                {
                    var strMessageData = await _objSaleMedicineDal.SaveSaleInfo(objSaleMedicineModel);

                    if (strMessageData != null)
                    {
                        foreach (var tableData in objSaleMedicineModel.SaleMedicineItemList)
                        {
                            tableData.SaleInfoId = Convert.ToInt32(strMessageData);
                            returnMessage = await _objSaleMedicineDal.SaveSaleItem(tableData);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error : " + ex.Message);
            }

            var messageAndReload = new
            {
                m = returnMessage,
                isRedirect = true,
                redirectUrl = Url.Action("SaleMedicineList")
            };
            return Json(messageAndReload, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult SavePrescriptionImage()
        //{
        //    HttpFileCollectionBase filebase = Request.Files;
        //    if (filebase != null)
        //    {
        //        HttpPostedFileBase file = filebase[0];
        //        string path = Path.Combine(Server.MapPath("~/Files/Prescriptions/"), file.FileName);
        //        file.SaveAs(path);
        //    }

        //    return Json("operation completed");
        //}

        public void GetHttpPostedFileBase()
        {
            string filePath = Server.MapPath("~/Files/Prescriptions/");
            string vFileName = Guid.NewGuid().ToString();

            var imageFile = HttpContext.Request.Files[0];
            if (imageFile != null)
            {
                var imageExtension = Path.GetExtension(imageFile.FileName);
                if (imageExtension != null)
                {
                    imageExtension = imageExtension.ToUpper();

                    if (imageExtension == ".JPG" || imageExtension == ".JPEG" || imageExtension == ".PNG")
                    {
                        filePath += vFileName;
                        filePath += imageExtension.ToLower();

                        imageFile.SaveAs(filePath);

                        UtilityClass.ImageName = vFileName + imageExtension.ToLower();
                    }
                }
            }
        }

    }
}