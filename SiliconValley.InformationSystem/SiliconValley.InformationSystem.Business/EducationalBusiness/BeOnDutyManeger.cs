using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Entity.MyEntity;

namespace SiliconValley.InformationSystem.Business.EducationalBusiness
{
   public class BeOnDutyManeger:BaseBusiness<BeOnDuty>
    {
        //这是一个值班，加班费用的业务类

        /// <summary>
        /// 处理之后的根据名称或者主键查询值班，加班费用
        /// </summary>
        /// <param name="id">主键或名称</param>
        /// <param name="IsKey">是否是主键</param>
        /// <returns></returns>
        public BeOnDuty GetSingleBeOnButy(string id,bool IsKey)
        {
            BeOnDuty find_b = new BeOnDuty();
            if (IsKey)
            {
                //是主键
                int beonbuty_Id = Convert.ToInt32(id);
                find_b= this.GetEntity(beonbuty_Id);                
            }
            else
            {
                //不是主键
                find_b= this.GetList().Where(b => b.TypeName == id).FirstOrDefault();
            }
            if (string.IsNullOrEmpty(find_b.TypeName))
            {
                //没找到
                find_b.TypeName = "空";
                return find_b;
            }
            else
            {
                //找到了
                return find_b;
            }
        }
    }
}
