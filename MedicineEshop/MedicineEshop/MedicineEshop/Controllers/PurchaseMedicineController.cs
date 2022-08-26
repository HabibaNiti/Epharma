﻿using MedicineEshop.Utility;
using MedicineEshop.ViewModel;
using MedicineEshop.DAL;
using System.Threading.Tasks;
using System.Web.Mvc;
using System;

namespace MedicineEshop.Controllers
{
    [LogAction]
    public class PurchaseMedicineController : Controller
    {
        private readonly PurchaseMedicineDal _objPurchaseMedicineDal = new PurchaseMedicineDal();

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

        // GET: PurchaseMedicine
        [RoleFilter]
        public async Task<ActionResult> PurchaseMedicineList()
        {
            LoadSession();
            ViewBag.PurchaseMedicineList = await _objPurchaseMedicineDal.PurchaseMedicineList();
            return View();
        }

        public async Task<JsonResult> GetPurchaseMedicineItem(int purchaseInfoId)
        {
            var objPurchaseMedicineModel = await _objPurchaseMedicineDal.GetPurchaseMedicineItem(purchaseInfoId);

            return Json(objPurchaseMedicineModel, JsonRequestBehavior.AllowGet);
        }

        [RoleFilter]
        public async Task<ActionResult> PurchaseMedicine()
        {
            return View();
        }

        public async Task<ActionResult> GetAllMedicineForPurchase(DataTableAjaxPostModel model)
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

        public async Task<ActionResult> SaveAllPurchaseMedicineItem(PurchaseMedicineModel objPurchaseMedicineModel)
        {
            LoadSession();
            string returnMessage = "";

            objPurchaseMedicineModel.PurchaedBy = _strEmployeeId;

            try
            {

                if (objPurchaseMedicineModel.PurchaseMedicineItemList != null)
                {
                    var strMessageData = await _objPurchaseMedicineDal.SavePurchaseInfo(objPurchaseMedicineModel);

                    if (strMessageData != null)
                    {
                        foreach (var tableData in objPurchaseMedicineModel.PurchaseMedicineItemList)
                        {
                            tableData.PurchaseInfoId = Convert.ToInt32(strMessageData);
                            returnMessage = await _objPurchaseMedicineDal.SavePurchaseItem(tableData);
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
                redirectUrl = Url.Action("PurchaseMedicineList")
            };
            return Json(messageAndReload, JsonRequestBehavior.AllowGet);
        }
    }
}