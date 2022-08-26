
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MedicineEshop.DAL;
using MedicineEshop.Utility;
using MedicineEshop.ViewModel;

namespace MedicineEshop.Controllers
{
    [LogAction]
    public class SetupController : Controller
    {

        private readonly SetupDAL _objSetupDal = new SetupDAL();
        
        //For All Dropdown Load for this Object.
        private readonly DropdownDAL _objDropdownDal = new DropdownDAL();

        //End
        
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

        #region CompanaySetup
        [RoleFilter]
        public async Task<ActionResult> CompanySetup(int? companyId)
        {
            ModelState.Clear();

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"];
            }

            LoadSession();
            CompanySetupModel model = new CompanySetupModel();

            ViewBag.CompanyList = await _objSetupDal.GetAllCompanyList();

            if(companyId != null && companyId != 0)
            {
                model = await _objSetupDal.GetACompany((int)companyId);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveOrUpdateCompany(CompanySetupModel objCompanySetupModel)
        {
            LoadSession();

            if (ModelState.IsValid)
            {
                objCompanySetupModel.CreateBy = _strEmployeeId;
                objCompanySetupModel.ActiveYN = objCompanySetupModel.ActiveStatus ? "Y" : "N";

                string strMessage = await _objSetupDal.SaveOrUpdateCompany(objCompanySetupModel);
                TempData["message"] = strMessage;
            }
            return RedirectToAction("CompanySetup");
        }

        public async Task<ActionResult> DeleteCompany(int companyId)
        {
            LoadSession();
            string message = await _objSetupDal.DeleteCompany(companyId);
            TempData["message"] = message;

            return RedirectToAction("CompanySetup");
        }
        #endregion

        #region MedicineTypeSetup
        [RoleFilter]
        public async Task<ActionResult> MedicineTypeSetup(int? medicineTypeId)
        {
            ModelState.Clear();

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"];
            }

            LoadSession();
            MedicineTypeSetupModel model = new MedicineTypeSetupModel();

            ViewBag.MedicineTypeList = await _objSetupDal.GetAllMedicineTypeList();

            if (medicineTypeId != null && medicineTypeId != 0)
            {
                model = await _objSetupDal.GetAMedicineType((int)medicineTypeId);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveOrUpdateMedicineType(MedicineTypeSetupModel objMedicineTypeSetupModel)
        {
            LoadSession();

            if (ModelState.IsValid)
            {
                objMedicineTypeSetupModel.CreateBy = _strEmployeeId;
                objMedicineTypeSetupModel.ActiveYN = objMedicineTypeSetupModel.ActiveStatus ? "Y" : "N";

                string strMessage = await _objSetupDal.SaveOrUpdateMedicineType(objMedicineTypeSetupModel);
                TempData["message"] = strMessage;
            }
            return RedirectToAction("MedicineTypeSetup");
        }

        public async Task<ActionResult> DeleteMedicineType(int medicineTypeId)
        {
            LoadSession();
            string message = await _objSetupDal.DeleteMedicineType(medicineTypeId);
            TempData["message"] = message;

            return RedirectToAction("MedicineTypeSetup");
        }
        #endregion

        #region PaymentTypeSetup
        [RoleFilter]
        public async Task<ActionResult> PaymentTypeSetup(int? paymentTypeId)
        {
            ModelState.Clear();

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"];
            }

            LoadSession();
            PaymentTypeSetupModel model = new PaymentTypeSetupModel();

            ViewBag.PaymentTypeList = await _objSetupDal.GetAllPaymentTypeList();

            if (paymentTypeId != null && paymentTypeId != 0)
            {
                model = await _objSetupDal.GetAPaymentType((int)paymentTypeId);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveOrUpdatePaymentType(PaymentTypeSetupModel objPaymentTypeSetupModel)
        {
            LoadSession();

            if (ModelState.IsValid)
            {
                objPaymentTypeSetupModel.CreateBy = _strEmployeeId;
                objPaymentTypeSetupModel.ActiveYN = objPaymentTypeSetupModel.ActiveStatus ? "Y" : "N";

                string strMessage = await _objSetupDal.SaveOrUpdatePaymentType(objPaymentTypeSetupModel);
                TempData["message"] = strMessage;
            }
            return RedirectToAction("PaymentTypeSetup");
        }

        public async Task<ActionResult> DeletePaymentType(int paymentTypeId)
        {
            LoadSession();
            string message = await _objSetupDal.DeletePaymentType(paymentTypeId);
            TempData["message"] = message;

            return RedirectToAction("PaymentTypeSetup");
        }
        #endregion

        #region MedicineSetup
        [RoleFilter]
        public async Task<ActionResult> MedicineSetup(int? medicineId)
        {
            ModelState.Clear();

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"];
            }

            LoadSession();
            MedicineSetupModel model = new MedicineSetupModel();

            ViewBag.MedicineList = await _objSetupDal.GetAllMedicineList();

            if (medicineId != null && medicineId != 0)
            {
                model = await _objSetupDal.GetAMedicine((int)medicineId);
            }

            ViewBag.CompanyList = UtilityClass.GetSelectListByDataTable(await _objDropdownDal.GetCompanyListDropdown(), "COMPANY_ID", "COMPANY_NAME");
            ViewBag.MedicineTypeList = UtilityClass.GetSelectListByDataTable(await _objDropdownDal.GetMedicineTypeListDropdown(), "MEDICINE_TYPE_ID", "MEDICINE_TYPE_NAME");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveOrUpdateMedicine(MedicineSetupModel objMedicineSetupModel)
        {
            LoadSession();

            if (ModelState.IsValid)
            {
                objMedicineSetupModel.CreateBy = _strEmployeeId;
                objMedicineSetupModel.ActiveYN = objMedicineSetupModel.ActiveStatus ? "Y" : "N";

                string strMessage = await _objSetupDal.SaveOrUpdateMedicine(objMedicineSetupModel);
                TempData["message"] = strMessage;
            }
            return RedirectToAction("MedicineSetup");
        }

        public async Task<ActionResult> DeleteMedicine(int medicineId)
        {
            LoadSession();
            string message = await _objSetupDal.DeleteMedicine(medicineId);
            TempData["message"] = message;

            return RedirectToAction("MedicineSetup");
        }
        #endregion
    }
}