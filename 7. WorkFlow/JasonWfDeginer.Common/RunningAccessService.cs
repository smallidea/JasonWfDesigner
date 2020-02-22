// ******************************************************************
// ** Copyright：Copyright (c) 2020
// ** Project：JasonWfDesigner.Common
// ** Create Date：2020-02-21 14:31
// ** Created by：陈晓平
// ** Blog：http://smallidea.cnblogs.com
// ** Git：http://smallidea.github.com
// ** Email: smallidea@126.com
// ** Version：v 1.0
// ** Last Modified: 2020-02-21 15:55
// ** Desc：RunningAccessService.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using JasonWfDesigner.Common.Lib;

namespace JasonWfDesigner.Common
{
    /// <summary>
    /// </summary>
    public class RunningAccessService
    {
        private static readonly SQLiteHelper _sqLiteHelper = new SQLiteHelper();

        //  string dbFileName = "running.db";
        private static readonly string _dbFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "running.db");

        private static readonly LockList<string> _list = new LockList<string>();
        private static readonly object _lock = new object();

        static RunningAccessService()
        {
            // 数据库文件是否存在
            if (false == File.Exists(_dbFilePath))
            {
                // 创建库
                SQLiteHelper.CreateDb("running.db");
                SQLiteHelper.SetConnectionString(_dbFilePath, null);

                // 创建表
                var sql = @"CREATE TABLE MovePath (
	                                                            [ID] integer PRIMARY KEY AUTOINCREMENT
	                                                            ,[nodekey] varchar(300) not null
	                                                            ,[productid] INT NOT NULL
	                                                            ,[intime] datetime default(datetime ('now','localtime'))
	                                                            )";
                _sqLiteHelper.ExecuteNonQuery(sql);

                /*  using (var db = new SQLiteConnection(_dbFilePath))
                  {
                      db.Execute(sql);
                  }*/
            }
            else
            {
                SQLiteHelper.SetConnectionString(_dbFilePath, null);
            }
        }

        /// <summary>
        ///     保存历史
        /// </summary>
        /// <param name="nodeKey"></param>
        /// <param name="productId"></param>
        /// <param name="createTime"></param>
        public static bool SaveLogWithQueue(string nodeKey, int productId, DateTime createTime)
        {
            var sql =
                $"insert into MovePath(nodekey, productid, intime)  values('{nodeKey}', {productId}, '{createTime:yyyy-MM-dd HH:mm:ss}')";
            _list.Insert(sql);

            //TODO: 如果遇到程序异常关闭，会丢失数据
            if (_list.Count() >= 10) // 累计10条提交一次，防止过于频繁
            {
                var executeSql = string.Join(";", _list);
                var result = _sqLiteHelper.ExecuteNonQuery(executeSql) == 1;
                _list.Clear();
                return result;
            }

            return true;
        }

        /// <summary>
        ///     保存历史
        /// </summary>
        /// <param name="nodeKey"></param>
        /// <param name="productId"></param>
        /// <param name="createTime"></param>
        public static bool SaveLog(string nodeKey, int productId, DateTime createTime)
        {
            lock (_lock)
            {
                var sql = "insert into MovePath(nodekey, productid, intime)  " +
                          $"values('{nodeKey}', {productId}, '{createTime:yyyy-MM-dd HH:mm:ss}')";
                return _sqLiteHelper.ExecuteNonQuery(sql) == 1;
            }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static bool CleanAll()
        {
            var sql = "delete from MovePath";
            _sqLiteHelper.ExecuteQuery(sql);
            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static bool Clean(int productId)
        {
            var sql = $"delete from MovePath where productid = {productId}";
            _sqLiteHelper.ExecuteQuery(sql);
            return true;
        }

        /// <summary>
        ///     获取历史
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static List<KeyValuePair<string, DateTime>> GetLogs(int productId)
        {
            var result = new List<KeyValuePair<string, DateTime>>();
            var sql = $"select nodekey, intime from movepath where productid = {productId} order by intime asc";
            var dt = _sqLiteHelper.ExecuteQuery(sql);
            foreach (DataRow dataRow in dt.Rows)
            {
                var keyValue = new KeyValuePair<string, DateTime>(dataRow["nodekey"].ToString(),
                    ConvertHelper.ObjToDate(dataRow["intime"]));
                result.Add(keyValue);
            }

            result = result.OrderBy(a => a.Value).ToList();
            return result;

            /* using (var db = new SQLiteConnection(_dbFilePath))
             {
                 var sql = $"select nodekey, intime from movepath where productid = {productId}";
                 var result = db.Query<KeyValuePair<string, DateTime>>(sql);
                 return result;
             }*/
        }
    }
}