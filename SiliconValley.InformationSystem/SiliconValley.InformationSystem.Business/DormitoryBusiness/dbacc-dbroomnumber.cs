using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.DormitoryBusiness
{
   public  class dbacc_dbroomnumber
    {
        private AccdationinformationBusiness dbacc;

        private RoomStayNumberBusiness dbroomnumber;

        /// <summary>
        /// 确定房间是否满人
        /// </summary>
        /// <param name="dormId">房间id</param>
        /// <returns></returns>
        public bool IsFull(int dormId,int numberid) {
            dbacc = new AccdationinformationBusiness();
            dbroomnumber = new RoomStayNumberBusiness();
            var accdata= dbacc.GetAccdationinformationByDormId(dormId);
           var number= dbroomnumber.GetRoomStayNumberByRoomStayNumberId(numberid);
            if (accdata.Count==number.StayNumber)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 是否有学生居住
        /// </summary>
        /// <param name="dormId"></param>
        /// <returns></returns>
        public bool Somepeoplelivein(int dormId) {
            dbacc = new AccdationinformationBusiness();
            var accdata = dbacc.GetAccdationinformationByDormId(dormId);
            if (accdata.Count>0)
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
