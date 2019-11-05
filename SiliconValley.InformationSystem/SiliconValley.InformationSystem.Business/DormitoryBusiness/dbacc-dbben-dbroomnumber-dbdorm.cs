using SiliconValley.InformationSystem.Entity.Entity;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.DormitoryBusiness
{
   
   public class dbacc_dbben_dbroomnumber_dbdorm
    {
        private AccdationinformationBusiness dbacc;
        private BenNumberBusiness dbben;
        private RoomStayNumberBusiness dbroomnumber;
        private DormInformationBusiness dbdorm;
        private StaffAccdationBusiness dbstaffacc;

        /// <summary>
        /// 根据房间号返回对应的床位
        /// </summary>
        /// <param name="DorminfoID"></param>
        /// <returns></returns>
        public List<BenNumber> GetBensByDorminfoID(int DorminfoID) {

            dbben = new BenNumberBusiness();
            dbroomnumber = new RoomStayNumberBusiness();
            dbdorm = new DormInformationBusiness();

            //查询房间对象
            var querydorm = dbdorm.GetDormByDorminfoID(DorminfoID);

            //查询房间居住数量对象
            var queryroomnumber = dbroomnumber.GetRoomStayNumberByRoomStayNumberId(querydorm.RoomStayNumberId);

            //床位对象
            List<BenNumber> querybenlist = dbben.Getbennumber(queryroomnumber.StayNumber);
            return querybenlist;
        }

        /// <summary>
        /// 根据房间id 返回房间剩余床位
        /// </summary>
        /// <param name="DorminfoID"></param>
        /// <returns></returns>
        public List<BenNumber> GetSurplusbyDorminfoID(int DorminfoID, string roomtype) {
            dbacc = new AccdationinformationBusiness();
            dbstaffacc = new StaffAccdationBusiness();

            List<BenNumber> querybenlist = this.GetBensByDorminfoID(DorminfoID);

            if (roomtype=="staff")
            {
                //员工居住信息
                List<StaffAccdation> queryacclist = dbstaffacc.GetStaffAccdationsByDorminfoID(DorminfoID);
                if (querybenlist.Count!= queryacclist.Count)
                {
                    for (int i = querybenlist.Count - 1; i >= 0; i--)
                    {
                        foreach (var item in queryacclist)
                        {
                            if (querybenlist[i].Id == item.BedId)
                            {
                                querybenlist.Remove(querybenlist[i]);
                                break;
                            }
                        }
                    }
                }
                

            }

            if (roomtype=="student")
            {
                //学生居住信息
                List<Accdationinformation> queryacclist = dbacc.GetAccdationinformationByDormId(DorminfoID);
                if (querybenlist.Count!= queryacclist.Count)
                {
                    for (int i = querybenlist.Count - 1; i >= 0; i--)
                    {
                        foreach (var item in queryacclist)
                        {
                            if (querybenlist[i].Id == item.BedId)
                            {
                                querybenlist.Remove(querybenlist[i]);
                                break;
                            }

                        }
                    }
                }
                
            }
            return querybenlist;

        }

 

    }
}
