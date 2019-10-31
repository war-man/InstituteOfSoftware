using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.EducationalBusiness
{
   public class PurchaseApplyManeger:BaseBusiness<PurchaseApply>
    {
        /// <summary>
        /// 添加采购数据
        /// </summary>
        /// <param name="new_p">采购对象</param>
        /// <returns></returns>
        public bool AddData(PurchaseApply new_p)
        {
            bool s = false;
            try
            {
                this.Insert(new_p);
                s = true;
            }
            catch (Exception ex)
            {
                s = false;
            }
            return s;
        }
        /// <summary>
        /// false--没有正在操作的数据,true--有正在操作的数据
        /// </summary>
        /// <returns></returns>
        public bool IsWartherData()
        {
            bool s = true;
           int count= this.GetList().Where(p => p.IsWhether == true).ToList().Count;
            if (count<=0)
            {
                s = false;
            }
            return s;
        }
    }
}
