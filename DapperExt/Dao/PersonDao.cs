using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using DapperExt.Model;

namespace DapperExt.Dao
{
    /// <summary>
    /// Person
    /// DAO(Data Access Object) 数据访问对象是第一个面向对象的数据库接口
    /// </summary>
    public class PersonDao
    {
        public int Add(Person model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Person(");
            strSql.Append("username,password,age,registerDate,address)");
            strSql.Append(" values (");
            strSql.Append("@username,@password,@age,@registerDate,@address)");
            //
            DbHelperSql.Insert(strSql.ToString(), model);
            //
            DbHelperSql.InsertReturnId(strSql.ToString(), model);
            //
            var mList=new List<Person>();
            mList.Add(new Person() {age = 1});
            mList.Add(new Person() {age = 1});
            mList.Add(new Person() {age = 1});
            DbHelperSql.InsertBatch(strSql.ToString(), mList);
            //
            var modeList = new List<Person>();
            for (int i = 0; i < 1000000; i++)
            {
                modeList.Add(new Person() {age = i,username = "username",address = "address",password = "password" });
            }
            DbHelperSql.InsertBatchBySqlBulkCopy(modeList);
            //
            return 1;
        }

        public int DeleteById(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Person ");
            strSql.Append("where id=@id");
            //
            DbHelperSql.Delete(strSql.ToString(), new Person {id = id});
            //
            DbHelperSql.Delete(strSql.ToString(), new Person {username = "dapper-test"});
            return 1;
        }

        public int UpdateById(Person model, int id)
        {
            model.id = id;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Person set ");
            strSql.Append("username=@username,");
            strSql.Append("password=@password,");
            strSql.Append("age=@age,");
            strSql.Append("registerDate=@registerDate,");
            strSql.Append("address=@address");
            strSql.Append(" where id=@id");
            //
            DbHelperSql.Update(strSql.ToString(), model);
            return 1;
        }

        public int Update(Person model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Person set ");
            strSql.Append("username=@username,");
            strSql.Append("password=@password,");
            strSql.Append("age=@age,");
            strSql.Append("registerDate=@registerDate,");
            strSql.Append("address=@address");
            strSql.Append(" where id=@id");
            //
            DbHelperSql.Update(strSql.ToString(), model);
            return 1;
        }

        public Person FindById(int id,int top=1)
        {
            //默认top 1--1条记录
            var model = new Person {id = id};
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select  top {0} id,username,password,age,registerDate,address from Person ",top);
            strSql.Append(" where id=@id");
            //
            var result = DbHelperSql.Find(strSql.ToString(), model);
            return result;
        }

        public IList<Person> FindList(Person model,string where,string orderBy, int pageIndex,int pageSize)
        {
            var startIndex = pageIndex*pageSize;
            var size = pageSize;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  id,username,password,age,registerDate,address from Person ");
            strSql.Append(" where 1=1 and ");
            strSql.Append(where);
            strSql.AppendFormat("ORDER BY {0} OFFSET {1} ROWS FETCH NEXT {2} ROWS ONLY",orderBy,startIndex,size);
            var result = DbHelperSql.FindList(strSql.ToString(), model);
            return result;
        }

        public int Count(Person model, string where)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  count(*) from Person");
            strSql.Append(" where 1=1 and ");
            strSql.Append(where);
            var result = DbHelperSql.Count(strSql.ToString(), model);
            return result;
        }
    }
}