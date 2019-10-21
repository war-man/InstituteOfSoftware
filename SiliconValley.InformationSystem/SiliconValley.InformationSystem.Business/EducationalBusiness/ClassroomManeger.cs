using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.EducationalBusiness
{
   public class ClassroomManeger:BaseBusiness<Classroom>
    {
        /// <summary>
        /// 根据主键或名称查单条数据
        /// </summary>
        /// <param name="name">Id或名称</param>
        /// <param name="key">true--按主键查找,false---按名称查找</param>
        /// <returns></returns>
        public Classroom GetSingData(string name,bool key)
        {
            Classroom new_c = new Classroom();
            if (key)
            {
                //根据主键查询
               int id= Convert.ToInt32(name);
                new_c= this.GetEntity(id);
            }
            else
            {
                new_c= this.GetList().Where(c => c.ClassroomName == name).FirstOrDefault();
            }

            return new_c;
        }

        /// <summary>
        /// 获取有效数据
        /// </summary>
        /// <returns></returns>
        public List<Classroom> GetExitsData()
        {
           return this.GetList().Where(c => c.IsDelete == false).ToList();
        }

    }
}
