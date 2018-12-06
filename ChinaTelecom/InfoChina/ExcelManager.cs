using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;

namespace InfoChina
{
    /// <summary>
    /// Execl操作
    /// </summary>
    public class ExcelManager
    {
        /// <summary>
        /// 解析Excel数据，返回DataSet
        /// </summary>
        /// <param name="path">Excel文件路径</param>
        /// <param name="SheetName">工作簿名</param>
        /// <returns></returns>
        public DataSet Excel(string path, string SheetName)
        {
            //using System.Data.OleDb;
            //此连接可以操作.xls与.xlsx文件 (支持Excel2003 和 Excel2007 的连接字符串)
            //HDR=Yes  表示表格第一行是标题
            string strConn = "Provider=Microsoft.Ace.OleDb.12.0;"
                              + "data source="
                              + path
                              + ";Extended Properties='Excel 12.0; HDR=Yes; IMEX=1'";
            OleDbConnection conn = new OleDbConnection(strConn);
            OleDbDataAdapter adp = new OleDbDataAdapter("select * from [" + SheetName + "$]", conn);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds;
        }
    }
}
