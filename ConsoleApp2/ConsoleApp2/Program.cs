using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using Newtonsoft.Json;
namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            redis_help db = new redis_help();

            //db.Set("name", "zhonglunpanLOVEzhongtongtong", 10000);
            //string name=db.Get<string>("name");

            //string hahs = db.GetHash<string>("Person", "name");

            //HashEntry hashEntry = new HashEntry("name","nihao");

            //db.SetHahs<string>("Person", hashEntry);
            //var name=db.GetHash<string>("Person", "name");

            Person p = new Person();
            p.name = "小红";
            p.age = 18;
            p.sex = "男";

            //HashEntry hashEntry = new HashEntry("teache", p.name);
            JsonConvert.SerializeObject(p);

            HashEntry hashEntry1 = new HashEntry("xiaohong", JsonConvert.SerializeObject(p) );

            db.SetHahs<Person>("Person", hashEntry1);

            //var person = db.GetHash<Person>("Person", "xiaohong");

        }
    }
}
