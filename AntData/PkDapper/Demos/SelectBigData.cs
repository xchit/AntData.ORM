﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using AntData.ORM.Data;
using AntData.ORM.DataProvider.SqlServer;
using Dapper;
using Dapper.Contrib;
using Dapper.Contrib.Extensions;
using DbModels.SqlServer;
using SqlSugar;
using SyntacticSugar;
namespace PkDapper.Demos
{
    public class SelectBigData : IDemos
    {
        private static DbContext<Entitys> DB = new DbContext<Entitys>("testorm_sqlserver", new SqlServerDataProvider(SqlServerVersion.v2008));
        /// <summary>
        /// 测试一次读取100万条数据的速度
        /// </summary>
        public void Init()
        {
            Console.WriteLine("测试一次读取100万条数据的速度");
            var eachCount = 1;

            /*******************车轮战是性能评估最准确的一种方式***********************/
            for (int i = 0; i < 10; i++)
            {

                //dapper
                Dapper(eachCount);

                //sqlSugar
                //SqlSugar(eachCount); 

                //AntData
                AntData(eachCount);
            }
 
        }

        private static void SqlSugar(int eachCount)
        {
            GC.Collect();//回收资源
            System.Threading.Thread.Sleep(2000);//休息2秒

            PerHelper.Execute(eachCount, "SqlSugar", () =>
            {
                using (SqlSugarClient conn = new SqlSugarClient(PubConst.connectionString))
                {
                    var list = conn.Queryable<Models.PkDapper.Test>().ToList();
                }
            });
        }

        private static void Dapper(int eachCount)
        {
            GC.Collect();//回收资源
            System.Threading.Thread.Sleep(2000);//休息2秒

            //正试比拼
            PerHelper.Execute(eachCount, "Dapper", () =>
            {
                using (SqlConnection conn = new SqlConnection(PubConst.connectionString))
                {
                    var list = conn.GetAll<Test>();
                }
            });
        }

        private static void AntData(int eachCount)
        {
            GC.Collect();//回收资源
            System.Threading.Thread.Sleep(2000);//休息2秒

            //正试比拼
            PerHelper.Execute(eachCount, "AntData", () =>
            {
                var list = DB.Tables.Tests.ToList();
            });
        }
    }
}
