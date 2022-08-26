using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MedicineEshop.DAL;
using MedicineEshop.Utility;
using MedicineEshop.ViewModel;
using WebGrease.Css.Ast.Selectors;

namespace MedicineEshop.Controllers
{
   
    public class AuthController : Controller
    {
      readonly AuthenticationDAL _authentication = new AuthenticationDAL();

        // GET: Auth
        public ActionResult Index(bool? userExist)
        {
            ViewBag.UserId = _authentication.GetSystemGeneratedUserId();
            var employee = Session["authentication"] as AuthModel;
            if (employee != null)
            { 
              return RedirectToAction("About", "Home");
            }
            var b = !userExist;
            if (b != null && (bool)b)
            {
                ViewBag.Message = TempData["message"];
            }
            return View("Index");
        }

        [LogAction]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(AuthModel authModel)
        {
            if (authModel.EmployeeId !=null && authModel.EmployeePassword != null)
            {
                authModel = await _authentication.Login(authModel.EmployeeId, authModel.EmployeePassword);
                if (authModel.Message)
                {
                    Session["authentication"] = authModel;
                    return RedirectToAction("About", "Home");
                }
                TempData["message"] = "Unauthorize access denied for this system !! ";
                return RedirectToAction("Index", new { userExist = false });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Registeration(AuthModel authModel)
        {
            if (ModelState.IsValid)
            {
                string strMessage = await _authentication.UserRegistrationSave(authModel);
                if(strMessage == "INSERTED SUCCESSFULLY !!")
                {
                    TempData["message"] = "Registration Successful";
                }
                else
                {
                    TempData["message"] = "Registration Faild! Try agian.";
                }
            }
            return RedirectToAction("Index", new { userExist = false });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotePassword(AuthModel authModel)
        {
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> LoadTopMenue()
        {
            var employee = Session["authentication"] as AuthModel;
            if (employee != null)
            {
                return PartialView("_TopMenuPartial", employee);
            }
            return null;
        }

        public ActionResult RedirectActionResult()
        {
            Response.Clear();
            Response.Redirect("~/Auth/Index");
            return null;
        }

        [LogAction]
        public ActionResult LogOut()
        {
            var employee = Session["authentication"] as AuthModel;
            if (employee != null)
            {
                Session.Abandon();
            }
            return RedirectToAction("Index");
        }

        [LogAction]
        public ActionResult UserPermisionAlert()
        {
            var employee = Session["authentication"] as AuthModel;
            if (employee != null)
            {
                Session.Abandon();
            }
            return View();
        }

        #region UserList
        [RoleFilter]
        public async Task<ActionResult> UserList()
        {
            ModelState.Clear();

            EmployeeDistributionModel model = new EmployeeDistributionModel();

            ViewBag.UserList = await _authentication.GetUserList();

            return View(model);
        }
        #endregion

    }
}