using System.Threading.Tasks;
using System.Web.Mvc;
using MedicineEshop.DAL;
using MedicineEshop.ViewModel;

namespace MedicineEshop.Controllers
{

    public class HomeController : Controller
    {
        private readonly HomeDal _objHomeDal = new HomeDal();

        #region "Common"

        private string _strEmployeeId = "";
        private string _strEmployeeRole = "";

        public void LoadSession()
        {
            if (Session["authentication"] is AuthModel auth)
            {
                _strEmployeeId = auth.EmployeeId;
                _strEmployeeRole = auth.EmployeeRole;
            }
            else
            {
                Response.Headers.Clear();
                string url = Url.Action("Index", "Auth");
                if (url != null) Response.Redirect(url);
            }
        }
        #endregion

        public ActionResult About()
        {
            var employee = Session["authentication"] as AuthModel;

            if (employee != null)
            {
                    return View();
            }
           
            return RedirectToAction("Index", "Auth");
        }

        [ChildActionOnly]
        public async Task<ActionResult> Menu()
        {
            LoadSession();

            MenuModel menuModel = new MenuModel();
            //role wise menu
            if (_strEmployeeRole != "null")
            {
                menuModel.MenuMains = await _objHomeDal.GetMenuMainRoleWise(_strEmployeeRole);
                foreach (var menuMain in menuModel.MenuMains)
                {
                    menuMain.MenuSubs = await _objHomeDal.GetMenuSubRoleWise(menuMain.MenuMainId, _strEmployeeRole);
                }
            }
            else
            {
                ViewBag.MenuPermisionMessage = "You Have No Permision.Please contact with concern Person!.";
            }
            return PartialView("_MenuPartial", menuModel);
        }

       
    }
}