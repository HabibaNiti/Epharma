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
    public class MenuSetUpController : Controller
    {
        private readonly MenuAndSubMenuDal _objMenuSetupDal = new MenuAndSubMenuDal();

        //For All Dropdown Load for this Object.
        private readonly DropdownDAL _objDropdownDal = new DropdownDAL();


        // GET: MenuSetUp
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
        public async Task<ActionResult> Index(int? menuId)
        {
            ModelState.Clear();

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"];
            }

            LoadSession();
            MenuSetUpModel model = new MenuSetUpModel();

            var objMenuModel = await _objMenuSetupDal.GetMenuList();
            ViewBag.MenuList = objMenuModel;

            
            ViewBag.MaxOrderNumber = await _objMenuSetupDal.GetMaxOrderNumberForMenu();

            if (menuId != null && menuId != 0)
            {
                model = await _objMenuSetupDal.GetAMenu((int)menuId);
                ViewBag.MaxOrderNumber = model.MenuOrder;
            }
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveAndUpdateMenu(MenuSetUpModel objMenuModel)
        {
            LoadSession();
            if (ModelState.IsValid)
            {
                objMenuModel.UpdateBy = _strEmployeeId;
                objMenuModel.Active_YN = objMenuModel.ActiveStatus ? "Y" : "N";

                string strMessage = await _objMenuSetupDal.SaveAndUpdateMenu(objMenuModel);
                TempData["message"] = strMessage;
            }
            return RedirectToAction("Index");
        }


        public async Task<ActionResult> DeleteMenu(int menuId)
        {
            LoadSession();
            string message = await _objMenuSetupDal.DeleteMenu(menuId);
            TempData["message"] = message;

            return RedirectToAction("Index");
        }
    }
}