using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Employment
{
    public class EmploymentAreasBuiness : BaseBusiness<EmploymentAreas>
    {
        /// <summary>
        ///验证名字是否重复
        /// </summary>
        /// <param name="param0"></param>
        /// <returns></returns>
        public bool verificationname(string param0)
        {
            var vcc = false;
            var param1 = param0.Replace(" ","");
            var aa = this.GetIQueryable().Where(a => a.AreaName == param1).FirstOrDefault();
            if (aa != null)
            {
                vcc = true;
            }
            return vcc;
        }
    }
}
