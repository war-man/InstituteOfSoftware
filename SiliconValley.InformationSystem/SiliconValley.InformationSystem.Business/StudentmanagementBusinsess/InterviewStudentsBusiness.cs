using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Util;
namespace SiliconValley.InformationSystem.Business.StudentmanagementBusinsess
{
  public  class InterviewStudentsBusiness:BaseBusiness<InterviewStudents>
    {
        //使用缓存存储数据
        public List<InterviewStudents> Mylist(string Name)
        {
            List<InterviewStudents> list = new List<InterviewStudents>();
          var x=  this.GetList();
            RedisCache redis = new RedisCache();
           list= redis.GetCache<List<InterviewStudents>>(Name);
            if (list != null)
            {
                return list;
            }
            else
            {
                redis.SetCache("Name", x);
                list = redis.GetCache<List<InterviewStudents>>(Name);
                
                return list;

            }
        }
        public void Remove(string Name)
        {
            RedisCache redis = new RedisCache();
            redis.RemoveCache(Name);
        }
    }
}
