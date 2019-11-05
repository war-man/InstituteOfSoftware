using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.DormitoryBusiness
{

    /// <summary>
    /// 房间视图页面model业务类
    /// </summary>
    public class ProDormInfoViewBusiness
    {

        private DormitoryLeaderBusiness dbleader;
        private ProStudentInformationBusiness dbprostudent;
        /// <summary>
        /// 将房间实体对象转化为页面model
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<DormInfoView> dormInfoViewsByStudent(List<DormInformation> data)
        {
            dbleader = new DormitoryLeaderBusiness();
            dbprostudent = new ProStudentInformationBusiness();
            List<DormInfoView> dormInfoViews = new List<DormInfoView>();
            foreach (var item in data)
            {
                DormInfoView myview = new DormInfoView();
                myview.ID = item.ID;
                myview.DormInfoName = item.DormInfoName;

                var queryleader = dbleader.GetLeader(item.ID);
                if (queryleader == null)
                {
                    myview.StudentName = "暂定";
                }
                else
                {
                    var querystudent = dbprostudent.GetStudent(queryleader.StudentNumber);
                    myview.StudentName = querystudent.Name;
                }

                dormInfoViews.Add(myview);
            }
            return dormInfoViews;
        }

        /// <summary>
        /// 用于员工
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<DormInfoView> dormInfoViewsByStaff(List<DormInformation> data)
        {
            dbprostudent = new ProStudentInformationBusiness();
            List<DormInfoView> dormInfoViews = new List<DormInfoView>();
            foreach (var item in data)
            {
                DormInfoView myview = new DormInfoView();
                myview.ID = item.ID;
                myview.DormInfoName = item.DormInfoName;
                dormInfoViews.Add(myview);
            }
            return dormInfoViews;
        }
    }
}
