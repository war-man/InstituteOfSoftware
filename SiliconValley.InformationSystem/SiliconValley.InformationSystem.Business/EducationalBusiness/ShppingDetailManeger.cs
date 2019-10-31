using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.EducationalBusiness
{
   public class ShppingDetailManeger:BaseBusiness<ShppingDetail>
    {
        /// <summary>
        /// 将采购详情绑定到采购数据
        /// </summary>
        /// <param name="PurchaseApply_Id">采购的Id</param>
        /// <param name="update">要修改采购Id的采购详情集合</param>
        /// <returns></returns>
        public bool MyUpdateData(int PurchaseApply_Id, List<ShppingDetail> update)
        {
            bool s = false;
            try
            {
                foreach (ShppingDetail u in update)
                {
                    u.PurchaseApply_Id = PurchaseApply_Id;
                    //临时数据变为确定数据
                    u.Temporary = false;
                    this.Update(u);
                }
                s = true;
            }
            catch (Exception)
            {
                s=false;
               List<ShppingDetail> list= update.Where(u => u.PurchaseApply_Id != null).ToList();
                foreach (ShppingDetail l in list)
                {
                    l.PurchaseApply_Id = null;
                    this.Update(l);
                }
            }
            return s;
        }
        /// <summary>
        /// 删除临时数据
        /// </summary>
        /// <param name="tem">临时数据</param>
        /// <returns></returns>
        public bool DeleteList(List<ShppingDetail> tem)
        {
            bool s = false;
            try
            {
                foreach (ShppingDetail item in tem)
                {
                    this.Delete(item);
                }
                s = true;
            }
            catch (Exception)
            {
                s = false;
            }
            return s;
        }
    }
}
