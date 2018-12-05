﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace SilverlightGIS.Common
{
    public class SQLHelper
    {
        private SqlConnection StyleConnection;

        #region 构造方法
        private SQLHelper()
        {
            #region 初始化连接信息
            string strConStr = System.Configuration.ConfigurationManager.AppSettings["SQLServer"].ToString();
            StyleConnection = new SqlConnection(strConStr);
            #endregion
        }
        #endregion

        #region 单例
        private static SQLHelper _instance = null;

        /// <summary>
        /// PGHelper的实例
        /// </summary>
        public static SQLHelper Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new SQLHelper();
                }
                return _instance;
            }
        }
        #endregion

        #region 公有方法
        /// <summary>
        /// 从型号基础库获取数据
        /// </summary>
        /// <param name="SQL">查询的SQL语句</param>
        /// <returns>查询的结果</returns>
        public DataTable GetDataTable(String SQL)
        {
            SqlCommand cmd = new SqlCommand(SQL, StyleConnection);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        /// <summary>
        /// 执行单个SQL
        /// </summary>
        /// <param name="Sql">需要执行的SQL</param>
        /// <returns>执行结果</returns>
        public bool ExecuteSql(String Sql)
        {
            StyleConnection.Open();
            try
            {
                SqlCommand oc = new SqlCommand(Sql, StyleConnection);
                if (oc.ExecuteNonQuery() > 0)
                {
                    return true;
                }
            }
            catch(Exception ex)
            {
            }
            finally
            {
                StyleConnection.Close();
            }
            return false;
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="ListSql">SQL语句集合</param>
        /// <returns>执行结果</returns>
        public bool ExecuteSqlTran(List<String> ListSql)
        {
            StyleConnection.Open();
            SqlTransaction sqlTran = StyleConnection.BeginTransaction();
            try
            {
                //OracleTransaction tx=conn.BeginTransaction();	
                foreach (String sql in ListSql)
                {
                    try
                    {
                        SqlCommand oc = new SqlCommand(sql, StyleConnection);
                        oc.Transaction = sqlTran;
                        oc.ExecuteNonQuery();
                    }
                    catch (System.Data.SqlClient.SqlException E)
                    {
                        sqlTran.Rollback();
                        throw new Exception(E.Message);
                    }

                }
                sqlTran.Commit();
                return true;
            }
            catch
            {
                sqlTran.Rollback();
            }
            finally
            {
                StyleConnection.Close();
            }
            return false;
        }
        #endregion

    }
}
