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
// ** Desc：SQLiteHelper.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace JasonWfDesigner.Common
{
    /// <summary>
    /// </summary>
    public class SQLiteHelper
    {
        private static string _connectionString = string.Empty;

        /// <summary>
        ///     根据数据源、密码、版本号设置连接字符串。
        /// </summary>
        /// <param name="datasource">数据源。</param>
        /// <param name="password">密码。</param>
        /// <param name="version">版本号（缺省为3）。</param>
        public static void SetConnectionString(string datasource, string password, int version = 3)
        {
            _connectionString = $"Data Source={datasource};Version={version};password={password}";
        }

        /// <summary>
        ///     创建一个数据库文件。如果存在同名数据库文件，则会覆盖。
        /// </summary>
        /// <param name="dbName">数据库文件名。为null或空串时不创建。</param>
        /// <param name="password">（可选）数据库密码，默认为空。</param>
        /// <exception cref="Exception"></exception>
        public static void CreateDb(string dbName)
        {
            if (!string.IsNullOrEmpty(dbName)) SQLiteConnection.CreateFile(dbName);
        }

        /// <summary>
        ///     对SQLite数据库执行增删改操作，返回受影响的行数。
        /// </summary>
        /// <param name="sql">要执行的增删改的SQL语句。</param>
        /// <param name="parameters">执行增删改语句所需要的参数，参数必须以它们在SQL语句中的顺序为准。</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int ExecuteNonQuery(string sql, params SQLiteParameter[] parameters)
        {
            var affectedRows = 0;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                using (var command = new SQLiteCommand(connection))
                {
                    try
                    {
                        connection.Open();
                        command.CommandText = sql;
                        if (parameters.Length != 0) command.Parameters.AddRange(parameters);
                        affectedRows = command.ExecuteNonQuery();
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return affectedRows;
        }

        /// <summary>
        ///     批量处理数据操作语句。
        /// </summary>
        /// <param name="list">SQL语句集合。</param>
        /// <exception cref="Exception"></exception>
        public void ExecuteNonQueryBatch(List<KeyValuePair<string, SQLiteParameter[]>> list)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    using (var cmd = new SQLiteCommand(conn))
                    {
                        try
                        {
                            foreach (var item in list)
                            {
                                cmd.CommandText = item.Key;
                                if (item.Value != null) cmd.Parameters.AddRange(item.Value);
                                cmd.ExecuteNonQuery();
                            }

                            tran.Commit();
                        }
                        catch (Exception)
                        {
                            tran.Rollback();
                            throw;
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     执行查询语句，并返回第一个结果。
        /// </summary>
        /// <param name="sql">查询语句。</param>
        /// <param name="parameters"></param>
        /// <returns>查询结果。</returns>
        /// <exception cref="Exception"></exception>
        public object ExecuteScalar(string sql, params SQLiteParameter[] parameters)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                using (var cmd = new SQLiteCommand(conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.CommandText = sql;
                        if (parameters.Length != 0) cmd.Parameters.AddRange(parameters);
                        return cmd.ExecuteScalar();
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        /// <summary>
        ///     执行一个查询语句，返回一个包含查询结果的DataTable。
        /// </summary>
        /// <param name="sql">要执行的查询语句。</param>
        /// <param name="parameters">执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准。</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public DataTable ExecuteQuery(string sql, params SQLiteParameter[] parameters)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                using (var command = new SQLiteCommand(sql, connection))
                {
                    if (parameters.Length != 0) command.Parameters.AddRange(parameters);
                    var adapter = new SQLiteDataAdapter(command);
                    var data = new DataTable();
                    try
                    {
                        adapter.Fill(data);
                    }
                    finally
                    {
                        connection.Close();
                    }

                    return data;
                }
            }
        }

        public DataTable ExecuteQuery(string sql)
        {
            return ExecuteQuery(sql, null);
        }

        /// <summary>
        ///     执行一个查询语句，返回一个关联的SQLiteDataReader实例。
        /// </summary>
        /// <param name="sql">要执行的查询语句。</param>
        /// <param name="parameters">执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准。</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public SQLiteDataReader ExecuteReader(string sql, params SQLiteParameter[] parameters)
        {
            var connection = new SQLiteConnection(_connectionString);
            var command = new SQLiteCommand(sql, connection);
            try
            {
                if (parameters.Length != 0) command.Parameters.AddRange(parameters);
                connection.Open();
                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        ///     查询数据库中的所有数据类型信息。
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public DataTable GetSchema()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    return connection.GetSchema("TABLES");
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}