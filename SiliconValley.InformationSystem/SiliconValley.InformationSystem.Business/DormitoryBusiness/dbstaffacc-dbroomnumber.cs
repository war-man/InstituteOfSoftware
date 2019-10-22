using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.DormitoryBusiness
{
    /// <summary>
    /// 员工居住信息跟房间数量
    /// </summary>
    public class dbstaffacc_dbroomnumber
    {
        private StaffAccdationBusiness dbstaffacc;
        private RoomStayNumberBusiness dbroomnumber;
        /// <summary>
        /// 确定房间是否满人
        /// </summary>
        /// <param name="dormId">房间id</param>
        /// <returns></returns>
        public bool IsFull(int dormId, int numberid)
        {
            dbstaffacc = new StaffAccdationBusiness();
            dbroomnumber = new RoomStayNumberBusiness();
            var staffaccdata = dbstaffacc.GetStaffAccdationsByDorminfoID(dormId);
            var number = dbroomnumber.GetRoomStayNumberByRoomStayNumberId(numberid);
            if (staffaccdata.Count == number.StayNumber)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
