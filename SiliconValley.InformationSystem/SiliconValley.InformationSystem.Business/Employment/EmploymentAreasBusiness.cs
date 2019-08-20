
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Employment
{
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Entity.ViewEntity;

    /// <summary>
    /// 区域业务类
    /// </summary>
    public class EmploymentAreasBusiness : BaseBusiness<EmploymentAreas>
    {
        /// <summary>
        /// 查询全部就业区域
        /// </summary>
        /// <returns></returns>
        public List<EmploymentAreas> GetAll()
        {
            var bb = this.GetIQueryable().Where(a => a.IsDel == false).ToList();
            return bb;
        }
        /// <summary>
        /// 根据id查询对象
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public EmploymentAreas GetObjByID(int? ID)
        {
            var bb = this.GetAll().Where(a => a.ID == ID).FirstOrDefault();
            if (bb==null)
            {
                EmploymentAreas areas = new EmploymentAreas();
                areas.AreaName = "待定";
                return areas;
            }
            else
            {
                return bb;
            }
            
        }

        /// <summary>
        /// 返回穿梭组件数据
        /// </summary>
        /// <returns></returns>
        public List<TransferData> TransferData()
        {
            var alldata = this.GetAll();
            
            EmploymentStaffBusiness dbstaff = new EmploymentStaffBusiness();
            List<TransferData> resultdata = new List<TransferData>();
            
            foreach (var item in alldata)
            {
                TransferData transfer = new TransferData();
                transfer.value = item.ID;
                transfer.title = item.AreaName;
                transfer.mychecked = "";
                transfer.disabled = "";
                var staffdata= dbstaff.GetEmploymentByAreasID(item.ID);
                if (staffdata!=null)
                {
                    transfer.disabled = "true";
                }
                resultdata.Add(transfer);
            }

            return resultdata;
        }
    }
}
