using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Entity;
using SiliconValley.InformationSystem.Entity.MyEntity;

namespace SiliconValley.InformationSystem.Business.StuSatae_Maneger
{
   public class StuStateManeger:BaseBusiness<StuStatus>
    {
        /// <summary>
        /// 这是根据名称找状态
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public StuStatus GetStu(string name)
        {
           StuStatus find_s= this.GetList().Where(s => s.StatusName == name).FirstOrDefault();
            return find_s;
        }
        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="name">状态名称</param>
        /// <returns></returns>
        public StuStatus GetId(string name)
        {
           return this.GetList().Where(s => s.StatusName.Contains(name)).FirstOrDefault();
        }
    }
}
