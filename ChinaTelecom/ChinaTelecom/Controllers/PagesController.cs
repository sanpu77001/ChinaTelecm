using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using InfoChina;

namespace ChinaTelecom.Controllers
{
    [Authorize]
    public class PagesController : Controller
    {
        ChinaTelecomEntities obj = new ChinaTelecomEntities();
        //
        // GET: /Pages/
        /// <summary>
        /// top头部文件
        /// </summary>
        /// <returns></returns>
        public ActionResult top()
        {
            ViewBag.userCode = this.User.Identity.Name;
            return View();
        }
        /// <summary>
        /// Index内容界面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.userCode = this.User.Identity.Name;
            ViewBag.lastTime = Session["LastTime"];
            return View();
        }
        /// <summary>
        /// Left左导航栏界面
        /// </summary>
        /// <returns></returns>
        public ActionResult Left()
        {
            return View();
        }
        /// <summary>
        /// footer底部页面
        /// </summary>
        /// <returns></returns>
        public ActionResult footer()
        {
            return View();
        }
        public ActionResult Exit() 
        { 
            //清除身份凭证
            FormsAuthentication.SignOut();
            return Redirect("~/Home/Login");
        }
    }
}
