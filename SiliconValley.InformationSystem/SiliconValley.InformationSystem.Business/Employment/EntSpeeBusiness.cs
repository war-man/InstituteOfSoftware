using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Employment
{
    /// <summary>
    /// 企业专业表业务类
    /// </summary>
    public class EntSpeeBusiness : BaseBusiness<EntSpee>
    {

        public List<EntSpee> GetEntSpees() {
         return   this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }
        public EntSpee GetEntSpeeByID(int ID)
        {
            return this.GetEntSpees().Where(a => a.Id == ID).FirstOrDefault();
        }


        public List<EntSpee> GetEntSpeesByEntid(int entid) {
          return  this.GetEntSpees().Where(a => a.EntID == entid).ToList();
        }
        /// <summary>
        /// 根据公司id 返回出这个公司涉及到专业 拼接出string
        /// </summary>
        /// <param name="entid"></param>
        /// <returns></returns>
        public string Getentstrintg(int entid) {
            var data= this.GetEntSpeesByEntid(entid);
            string result = "";
            foreach (var item in data)
            {
                result = result + item.SpeID + "-";
            }
            return result.Substring(0, result.Length - 1);
        }
    }
}
