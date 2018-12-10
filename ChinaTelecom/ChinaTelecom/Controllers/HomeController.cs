using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using InfoChina;
using ChinaTelecom.Models;
using System.Data.Entity.Infrastructure;

namespace ChinaTelecom.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        ChinaTelecomEntities obj = new ChinaTelecomEntities();
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>]
        [Authorize]
        public ActionResult Index()
        {

            return View();
        }
        /// <summary>
        /// 登录界面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// 登录验证!(密码MD5加密)
        /// </summary>
        /// <param name="userCode"></param>
        /// <param name="userPwd"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(string userCode, string userPwd)
        {
            var userpwd = md5.MD5Encrypt32(userPwd);
            var user = (from s in obj.Login
                        where s.userCode == userCode && s.userPwd == userpwd
                        select s).FirstOrDefault();
            if (user != null)
            {
                Session["LastTime"] = user.lastTime;
                //更新最后登录时间
                user.lastTime = DateTime.Now;
                obj.SaveChanges();
                //创建身份令牌            
                FormsAuthentication.SetAuthCookie(user.userCode, false);
                return View("Index");
            }
            ViewBag.userCode = userCode;
            ViewBag.msg = "账户密码不匹配";
            return View();
        }
        /// <summary>
        /// 注册页面
        /// </summary>
        /// <returns></returns>
        public ActionResult RegiUser()
        {
            return View();
        }
        /// <summary>
        /// 注册验证(用户名是否存在)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RegiUser(string userName)
        {
            var user = (from s in obj.Login
                        where s.userCode == userName
                        select s).FirstOrDefault();
            if (user != null)
                return Content("1");
            return Content("0");
        }
        /// <summary>
        /// 添加注册数据(用户)
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPwd"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("AddUser")]
        public ActionResult RegiUser(string userName, string userPwd)
        {
            try
            {
                obj.Login.Add(new Login()
                {
                    userCode = userName,
                    userPwd = md5.MD5Encrypt32(userPwd),
                    lastTime = DateTime.Now
                });
                obj.SaveChanges();
            }
            catch (Exception)
            {
                return Content("-1");
            }
            return Content("1");
        }
        /// <summary>
        /// 查询页面
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchList()
        {
            Session["list"] = null;
            UserList us = new UserList();
            //统计公司集合
            var list = (from s in obj.ConstructionPersonnelTable
                        group s by s.Company into dt
                        select dt);
            foreach (var item in list)
            {
                us.Company.Add(item.Key);
            }
            us.Company.Insert(0, "全部");
            //统计分支局集合
            var list1 = (from s in obj.ConstructionPersonnelTable
                         group s by s.BranchOffice into dt
                         select dt);
            foreach (var item in list1)
            {
                us.BranchOffice.Add(item.Key);
            }
            us.BranchOffice.Insert(0, "全部");
            //统计分页数据
            us.Count = obj.ConstructionPersonnelTable.Count();
            us.pageSize = us.Count % 10 == 0 ? us.Count / 10 : us.Count / 10 + 1;
            return View(us);
        }
        /// <summary>
        /// 分页显示数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult SearchList(int pageIndex)
        {
            List<User> list = null;
            if (Session["list"] == null)
            {
                list = (from s in obj.ConstructionPersonnelTable
                        join t in obj.UserState on s.State equals t.ID
                        orderby s.ID
                        select new User()
                        {
                            ID = s.ID,
                            Company = s.Company,
                            BranchOffice = s.BranchOffice,
                            Worknumber = s.Worknumber,
                            UserName = s.UserName,
                            State = s.State,
                            stateMsg = t.stateName
                        }).ToList<User>();
            }
            else
            {
                list = Session["list"] as List<User>;
            }
            if (list.Count > 0)
            {
                list[0].Count = list.Count;
                list[0].pageSize = list.Count % 10 == 0 ? list.Count / 10 : list.Count / 10 + 1;
            }
            var list3 = list.Skip((pageIndex - 1) * 10).Take(10).OrderBy(f => f.ID).ToList<User>();
            return Json(list3, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 根据条件查找数据
        /// </summary>
        /// <param name="userCode"></param>
        /// <param name="userName"></param>
        /// <param name="gsm"></param>
        /// <param name="fzj"></param>
        /// <param name="zzzt"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [ActionName("SearchListFilter")]
        public ActionResult SearchList(string userCode, string userName, string gsm, string fzj, int? zzzt)
        {
            var list = (from s in obj.ConstructionPersonnelTable
                        join f in obj.UserState on s.State equals f.ID
                        select new User()
                        {
                            ID = s.ID,
                            Company = s.Company,
                            BranchOffice = s.BranchOffice,
                            Worknumber = s.Worknumber,
                            UserName = s.UserName,
                            State = s.State,
                            stateMsg = f.stateName
                        });
            if (!string.IsNullOrWhiteSpace(userCode))
                list = list.Where(f => f.Worknumber.Contains(userCode));
            if (!string.IsNullOrWhiteSpace(userName))
                list = list.Where(f => f.UserName.Contains(userName));
            if (!string.IsNullOrWhiteSpace(gsm))
            {
                if (gsm != "全部")
                    list = list.Where(f => f.Company == gsm);
            }
            if (!string.IsNullOrWhiteSpace(fzj))
            {
                if (fzj != "全部")
                    list = list.Where(f => f.BranchOffice == fzj);
            }
            if (zzzt != null)
            {
                if (zzzt != 0)
                    list = list.Where(f => f.State == zzzt);
            }
            var list1 = list.ToList();
            if (list1.Count > 0)
            {
                list1[0].Count = list1.Count;
                list1[0].pageSize = list1.Count % 10 == 0 ? list1.Count / 10 : list1.Count / 10 + 1;
            }
            Session["list"] = list1;
            return Json(list1.Take(10).OrderBy(f => f.ID), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 目前还有问题(sanpu)
        /// </summary>
        /// <param name="IDNum"></param>
        /// <param name="State"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateState(string IDNum, string State)
        {
            try
            {
                int IDnum = Convert.ToInt32(IDNum);
                var list = obj.ConstructionPersonnelTable.Where(f => f.ID == IDnum).FirstOrDefault();
                if (State == "在职")
                {

                    list.State = 0;
                }
                else
                    list.State = 1;
                obj.SaveChanges();
            }
            catch (Exception)
            {

                return Content("-1");
            }
            return Content("1");
        }
        /// <summary>
        /// 登录信息维护(界面)
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ActionResult UpdateUserInfo()
        {
            return View();
        }
        /// <summary>
        /// 排名达标率(界面)
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ActionResult RankingRate()
        {
            return View();
        }
        /// <summary>
        /// Excel基础数据导入(界面)
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ActionResult ExeclBasicDataImport()
        {
            return View();
        }
        /// <summary>
        /// 查看透视数据(界面)
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ActionResult PerspectiveData()
        {
            return View();
        }
        /// <summary>
        /// 查看分母数据(界面)
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ActionResult DenominatorData()
        {
            return View();
        }
        /// <summary>
        /// 查看排名数据
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ActionResult RankingData()
        {
            return View();
        }
    }
}
