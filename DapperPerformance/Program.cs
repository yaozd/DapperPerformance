using DapperExt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DapperExt.Dao;

namespace DapperPerformance
{
    class Program
    {
        //原子计数
        public static int Count;
        //错误原子计数
        public static int ErrorCount;
        //测试数据
        public static Byte[] TestData;
        public static int TestDataSize = 1 * 1024;
        public static string LogPath = @"D:\temp-test";
        public static string LogName = "log.txt";

        /// <summary>
        /// sql语句的生成可以使用动软代码生成器
        /// ----
        /// 使用dapper时动态拼接查询sql有什么好的步骤吗
        /// 记得dapper 有个Dapper.SqlBuilder的功能，就是动态拼接查询sql的
        /// Dapper.SqlBuilder--------------------------------------------
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Init();
            //
            PerformanceTest.Time("Test_Set", 400, 1000, (Test_Set));
            //
            End();
        }

        static void Test_Set()
        {
            //
            var person = new Person
            {
                username = "dapper-test"
            };
            
            var dao=new PersonDao();
            dao.Count(new Person(), "1=1");
            dao.FindList(new Person(), "1=1", "id", 0, 3);
            dao.FindById(7);
            dao.Update(person);
            dao.UpdateById(person,7);
            dao.DeleteById(5);
            //批量插入等
            //var id = new PersonDao().Add(person);

        }

        static void Test_Get()
        {
            //
        }
        static void Example()
        {
            var current = Interlocked.Increment(ref Count);
            Logger.Info(current.ToString());
        }

        static void Example_Loop()
        {
            while (true)
            {
                try
                {
                    //
                    PerformanceTest.Time("Test_Set", 40, 5000, (Test_Set));
                    PerformanceTest.Time("Test_Get", 40, 5000, (Test_Get));
                    //
                }
                catch (Exception ex)
                {
                    //记录报错信息
                    var current = Interlocked.Increment(ref ErrorCount);
                    Logger.Info(string.Format("2-Error-Count-{0}-{1}", current, DateTime.Now));
                    Logger.Info(string.Format("2-Error-Count-{0}-{1}", current, ex.Message));
                }
            }
        }
        static void Init()
        {
            Logger.Initialize(LogPath, LogName);
            PerformanceTest.Initialize();
            BuildTestData();
        }
        private static void End()
        {
            Console.WriteLine("Test End");
            Logger.Close();
            Console.Read();
        }
        static void BuildTestData()
        {
            TestData = new byte[TestDataSize];
            for (int i = 0; i < TestDataSize; i++)
            {
                TestData[i] = 0x30;
            }
        }
    }
}
