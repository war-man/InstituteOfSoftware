using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.DormitoryBusiness
{
    /// <summary>
    /// 用于卫生登记显示表格的业务类
    /// </summary>
    public class HealthRegistrationtableviewBusiness
    {
        private DormInformationBusiness dbdorm;

        private DormitoryhygieneBusiness dbdormhygiene;

        private RoomdeWithPageXmlHelp dbroomhelpxml;

        private EmployeesInfoManage dbemp;

        private InstructorListBusiness dbinstructorlist;
        /// <summary>
        /// 根据栋楼层id，以及开始时间结束时间进行的一个查询返回的是view视图。
        /// </summary>
        /// <param name="tungfloorid"></param>
        /// <param name="startime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public List<HealthRegistrationtableview> GetHealthRegistrationtableviews(int tungfloorid, DateTime startime, DateTime endtime) {
            dbdormhygiene = new DormitoryhygieneBusiness();
            dbdorm = new DormInformationBusiness();
            dbroomhelpxml = new RoomdeWithPageXmlHelp();
            dbemp = new EmployeesInfoManage();
            dbinstructorlist = new InstructorListBusiness();
            int studentroom= dbroomhelpxml.GetRoomType(RoomTypeEnum.RoomType.StudentRoom);
           List<DormInformation> querydormlist= dbdorm.GetStudentDormsByTungfloorid(tungfloorid);
            List<HealthRegistrationtableview> result = new List<HealthRegistrationtableview>();
            foreach (var item in querydormlist)
            {
               DormInformation querydorm=  dbdorm.GetDormByDorminfoID(item.ID);
                List<Dormitoryhygiene> queryDormitoryhygiene = dbdormhygiene.GetDormitoryhygienesByDormID(item.ID);
              
                queryDormitoryhygiene=queryDormitoryhygiene.Where(a => a.RegisterTime > startime && a.RegisterTime <= endtime).ToList().OrderByDescending(a=>a.Addtime).ToList();
                foreach (var item1 in queryDormitoryhygiene)
                {
                    int girl= dbroomhelpxml.Getmale(RoomTypeEnum.SexType.Female);

                    EmployeesInfo queryemp= dbemp.GetEntity(dbinstructorlist.GetEntity(item1.Inspector).EmployeeNumber);
                    HealthRegistrationtableview healthRegistrationtableview = new HealthRegistrationtableview();
                    healthRegistrationtableview.DormInfoName = querydorm.DormInfoName;
                    healthRegistrationtableview.EmpinfoName = queryemp.EmpName;
                    healthRegistrationtableview.RecordTime =item1.RegisterTime;
                    healthRegistrationtableview.Remark =item1.Remarks;
                    if (item.SexType==girl)
                    {
                        healthRegistrationtableview.SexType ="女寝室";
                    }
                    else
                    {
                        healthRegistrationtableview.SexType = "男寝室";
                    }

                    healthRegistrationtableview.Causeofdeduction = this.ResultCauseofdeduction(item1) ;
                    result.Add(healthRegistrationtableview);
                }

            }
            return result;
        }

        /// <summary>
        /// 传入一个卫生登记对象，将这个对象涉及的违规项总结出来，进行一个返回。
        /// </summary>
        /// <param name="dormitoryhygiene"></param>
        /// <returns></returns>
        public string ResultCauseofdeduction(Dormitoryhygiene dormitoryhygiene) {

            dbroomhelpxml = new RoomdeWithPageXmlHelp();

            Type t = dormitoryhygiene.GetType();//获得该类的Type

            string result = "扣分项目有：";
            foreach (PropertyInfo pi in t.GetProperties())
            {
                string name = pi.Name;//获得属性的名字,后面就可以根据名字判断来进行些自己想要的操作

                var value = pi.GetValue(dormitoryhygiene, null);//用pi.GetValue获得值

                if (value.ToString().ToUpper()=="TRUE")
                {
                    string query = dbroomhelpxml.GetPointsdeductionproject(name);
                    result = result + query + ",";
                }
                
                //string newVal = "新值";
                //pi.SetValue(dormitoryhygiene, newVal);//设置属性值
            }
            
            return result.Substring(0,result.Length-1);
        }
    }
}
