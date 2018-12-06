using InfoChina;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChinaTelecom.Controllers
{
    public class ImportController : Controller
    {
        //
        // GET: /Import/
        /// <summary>
        /// 批量导入施工人员界面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult importUser()
        {
            return View();
        }
        /// <summary>
        /// 上传文件操作
        /// </summary>
        /// <param name="file1"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult importUser(HttpPostedFileBase file1)
        {
            if (file1 == null)
                return Content("0");
            try
            {
                var path = Server.MapPath("~/Files/") + file1.FileName;
                file1.SaveAs(path);
                ExcelManager ex = new ExcelManager();
                DataSet ds = ex.Excel(path, "Sheet1");
                var dt = ds.Tables[0];
                dt.Columns.Add("ID", typeof(int));
                dt.Columns.Add("State", typeof(int));
                dt.Columns["ID"].SetOrdinal(0);
                DataBase db = new DataBase();
                db.Bulk("ConstructionPersonnelTable", dt);
            }
            catch (Exception)
            {
                return Content("-1");
            }
            return Content("1");
        }
    }
}
