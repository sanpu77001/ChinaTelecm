using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace InfoChina
{
    public class DataBase
    {
        private string constr = string.Empty;
        public DataBase()
        {
            constr = System.Configuration.ConfigurationManager.AppSettings["constr"];
        }
        /// <summary>
        /// 批量上传
        /// </summary>
        /// <returns></returns>
        public bool Bulk(string tabName, DataTable dt)
        {
            try
            {
                SqlBulkCopy bulk = new SqlBulkCopy(constr);
                bulk.BatchSize = dt.Rows.Count;
                bulk.DestinationTableName = tabName;
                bulk.WriteToServer(dt);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
