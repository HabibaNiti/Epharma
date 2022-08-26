using MedicineEshop.DAL;
using MedicineEshop.Utility;
using MedicineEshop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MedicineEshop.Controllers
{
    public class UserSubActionPermissionController : Controller
    {
        private readonly UserSubActionPermissionDal _objSubActionPermissionDal = new UserSubActionPermissionDal();

        //For All Dropdown Load for this Object.
        private readonly DropdownDAL _objDropdownDal = new DropdownDAL();

        // GET: UserSubActionPermission
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

        //[RoleFilter]
        public async Task<ActionResult> Index(int? permissionId)
        {
            ModelState.Clear();

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"];
            }

            LoadSession();
            UserSubActionPermissionModel model = new UserSubActionPermissionModel();

            ViewBag.SubActionPermissionList = await _objSubActionPermissionDal.GetSubActionPermissionList();

            ViewBag.RoleList = UtilityClass.GetSelectListByDataTable(await _objDropdownDal.GetUserRoleListDropdown(), "EMPLOYEE_ROLE", "EMPLOYEE_ROLE");

            if (permissionId != null && permissionId != 0)
            {
                model = await _objSubActionPermissionDal.GetASubActionPermission((int)permissionId);
            }
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveAndUpdateSubActionPermission(UserSubActionPermissionModel objSubPermissionModel)
        {
            LoadSession();
            if (ModelState.IsValid)
            {
                objSubPermissionModel.CreateBy = _strEmployeeId;
                objSubPermissionModel.Active_YN = objSubPermissionModel.ActiveStatus ? "Y" : "N";

                string strMessage = await _objSubActionPermissionDal.SaveAndUpdateSubActionPermission(objSubPermissionModel);
                TempData["message"] = strMessage;
            }
            return RedirectToAction("Index");
        }

    }
}