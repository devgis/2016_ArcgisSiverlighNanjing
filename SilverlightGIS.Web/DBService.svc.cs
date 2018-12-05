using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SilverlightGIS.Common;

namespace SilverlightGIS.Web
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“DBService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 DBService.svc 或 DBService.svc.cs，然后开始调试。
    public class DBService : IDBService
    {
        #region  信息
        public List<Info> GetInfoList(string Where)
        {
            List<Info> list = new List<Info>();
            string sql = "select * from t_Info";
            if (!string.IsNullOrEmpty(Where))
            {
                sql += (" where " + Where);
            }
            try
            {
                DataTable dt = SQLHelper.Instance.GetDataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        try
                        {
                            Info info = new Info();
                            info.ID = row["ID"].ToString();
                            info.Title = row["Title"].ToString();
                            info.Content = row["InfoContent"].ToString();
                            info.Author = row["Author"].ToString();
                            info.Time = Convert.ToDateTime(row["InfoTime"]);
                            list.Add(info);
                        }
                        catch
                        { }
                        
                    }
                }
            }
            catch
            { }
            return list;
        }
        public bool AddInfo(Info info)
        {
            string sql = string.Format("insert into t_Info (ID,Title,InfoContent,Author,InfoTime) values('{0}','{1}','{2}','{3}','{4}')",
                Guid.NewGuid().ToString(), info.Title, info.Content, info.Author, info.Time.ToString());
            try
            {
                return SQLHelper.Instance.ExecuteSql(sql);
            }
            catch
            { }
            return false;
        }
        public bool EditInfo(Info info)
        {
            string sql = string.Format("update t_Info set Title='{1}',InfoContent='{2}',Author='{3}',InfoTime='{4}' where ID='{0}'",
                info.ID, info.Title, info.Content, info.Author, info.Time.ToString());
            try
            {
                return SQLHelper.Instance.ExecuteSql(sql);
            }
            catch
            { }
            return false;
        }
        public bool DeleteInfo(string ID)
        {
            string sql = string.Format("delete from t_Info where ID='{0}'",
                ID);
            try
            {
                return SQLHelper.Instance.ExecuteSql(sql);
            }
            catch
            { }
            return false;
        }
        #endregion

        #region 订票
        public List<OrderInfo> GetOrderList(string Where)
        {
            List<OrderInfo> list = new List<OrderInfo>();
            string sql = "select * from t_OrderInfo";
            if (!string.IsNullOrEmpty(Where))
            {
                sql += (" where " + Where);
            }
            try
            {
                DataTable dt = SQLHelper.Instance.GetDataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        try
                        {
                            OrderInfo info = new OrderInfo();
                            info.ID = row["ID"].ToString();
                            info.OrderTime = Convert.ToDateTime(row["OrderTime"]);
                            info.TrainNO = row["TrainNO"].ToString();
                            info.Start = row["Start"].ToString();
                            info.End = row["End"].ToString();
                            info.PsgName = row["PsgName"].ToString();
                            info.UserName = row["UserName"].ToString();
                            list.Add(info);
                        }
                        catch
                        { }

                    }
                }
            }
            catch
            { }
            return list;
        }
        public bool AddOrderInfo(OrderInfo orderInfo)
        {
            string sql = string.Format("insert into t_OrderInfo (ID,OrderTime,TrainNO,[Start],[End],PsgName,UserName) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')",
                Guid.NewGuid().ToString(), orderInfo.OrderTime, orderInfo.TrainNO, orderInfo.Start, orderInfo.End, orderInfo.PsgName, orderInfo.UserName);
            try
            {
                return SQLHelper.Instance.ExecuteSql(sql);
            }
            catch
            { }
            return false;
        }
        public bool EditOrderInfo(OrderInfo orderInfo)
        {
            string sql = string.Format("update t_OrderInfo set OrderTime='{1}',TrainNO='{2}',[Start]='{3}',[End]='{4}',PsgName='{5}',UserName='{6}' where ID='{0}'",
                orderInfo.ID, orderInfo.OrderTime, orderInfo.TrainNO, orderInfo.Start, orderInfo.End, orderInfo.PsgName, orderInfo.UserName);
            try
            {
                return SQLHelper.Instance.ExecuteSql(sql);
            }
            catch
            { }
            return false;
        }
        public bool DeleteOrderInfo(string ID)
        {
            string sql = string.Format("delete from t_OrderInfo where ID='{0}'",
                ID);
            try
            {
                return SQLHelper.Instance.ExecuteSql(sql);
            }
            catch
            { }
            return false;
        }
        #endregion

        #region 用户
        public bool AddUser(string UserName, string UserPassword)
        {
            string sql = string.Format("insert into t_User (ID,UserName,UserPassword) values('{0}','{1}','{2}')",
                Guid.NewGuid().ToString(), UserName, UserPassword);
            try
            {
                return SQLHelper.Instance.ExecuteSql(sql);
            }
            catch
            { }
            return false;
        }
        public bool EditUser(string UserName, string UserPassword)
        {
            string sql = string.Format("update t_User set UserPassword='{1}' where UserName='{0}'",
                UserName, UserPassword);
            try
            {
                return SQLHelper.Instance.ExecuteSql(sql);
            }
            catch
            { }
            return false;
        }
        public bool CheckUser(string UserName, string UserPassword)
        {
            string sql = string.Format("select UserPassword from t_User where UserName='{0}'",
                UserName);
            try
            {
                DataTable tb= SQLHelper.Instance.GetDataTable(sql);
                if (tb != null && tb.Rows.Count > 0)
                {
                    if (UserPassword.Equals(tb.Rows[0][0].ToString()))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            { }
            return false;
        }
        public bool UserExists(string UserName, string UserPassword)
        {
            string sql = string.Format("select * from t_User where UserName='{0}'",
                UserName);
            try
            {
                DataTable tb = SQLHelper.Instance.GetDataTable(sql);
                if (tb != null && tb.Rows.Count > 0)
                {
                    return true;
                }
            }
            catch
            { }
            return false;
        }

        #endregion
    }
}
