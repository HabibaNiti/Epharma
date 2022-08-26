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
    [LogAction]
    public class SubMenuSetUpController : Controller
    {
        private readonly MenuAndSubMenuDal _objSubMenuSetupDal = new MenuAndSubMenuDal();

        //For All Dropdown Load for this Object.
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

        // GET: SubMenuSetUp
        [RoleFilter]
        public async Task<ActionResult> Index(int? menuId, int? subMenuId)
        {
            ModelState.Clear();

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"];
            }

            LoadSession();
            SubMenuSetUpModel model = new SubMenuSetUpModel();

            ViewBag.SubMenuList = await _objSubMenuSetupDal.GetSubMenuList();

           // ViewBag.MaxOrderNumber = await _objSetupDal.GetMaxOrderNumberForSubMenu(menuId);

            if (menuId != null && menuId != 0 && subMenuId != null && subMenuId != 0)
            {
                model = await _objSubMenuSetupDal.GetASubMenu((int)menuId, (int)subMenuId);
            }

            ViewBag.MenuList = UtilityClass.GetSelectListByDataTable(await _objDropdownDal.GetUserMenuListDropdown(), "MENU_ID", "MENU_NAME");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveAndUpdateSubMenu(SubMenuSetUpModel objSubMenuModel)
        {
            LoadSession();
            if (ModelState.IsValid)
            {
                objSubMenuModel.UpdateBy = _strEmployeeId;

                string strMessage = await _objSubMenuSetupDal.SaveAndUpdateSubMenu(objSubMenuModel);
                TempData["message"] = strMessage;
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> DeleteSubMenu(int menuId, int subMenuId)
        {
            LoadSession();
            string message = await _objSubMenuSetupDal.DeleteSubMenu(menuId, subMenuId);
            TempData["message"] = message;

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> GetMenuIdFromView(int menuId)
        {
            var data = await _objSubMenuSetupDal.GetMaxOrderNumberForSubMenu(menuId);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}