using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Business.ClassesBusiness;
using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Business.StudentBusiness;
using SiliconValley.InformationSystem.Entity.Entity;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Util;

namespace SiliconValley.InformationSystem.Business.StudentmanagementBusinsess
{
    public class StudentFeeStandardBusinsess : BaseBusiness<StudentFeeStandard>
    {
        //专业表
        BaseBusiness<Specialty> special = new BaseBusiness<Specialty>();

        //班主任表
        HeadmasterBusiness headmasters = new HeadmasterBusiness();
        //学员班级
        ScheduleForTraineesBusiness scheduleForTraineesBusiness = new ScheduleForTraineesBusiness();
        //阶段专业表
        BaseBusiness<StageGrade> stagegrade = new BaseBusiness<StageGrade>();
        //阶段表
        BaseBusiness<Grand> geand = new BaseBusiness<Grand>();

        //学费详情
        BaseBusiness<Studenttuitionfeestandard> Feestandard = new BaseBusiness<Studenttuitionfeestandard>();
        //学员信息
        StudentInformationBusiness studentInformationBusiness = new StudentInformationBusiness();

        //获取所有数据
        public object GetDate(int page, int limit, string Name, string Sex, string StudentNumber, string identitydocument)
        {
            //班主任带班
            BaseBusiness<HeadClass> Hoadclass = new BaseBusiness<HeadClass>();
            //    List<StudentInformation>list=  dbtext.GetPagination(dbtext.GetIQueryable(),page,limit, dbtext)
            List<StudentInformation> list = studentInformationBusiness.Mylist("StudentInformation").Where(a => a.IsDelete ==false).ToList();
           

                if (!string.IsNullOrEmpty(Name))
                {
                    list = list.Where(a => a.Name.Contains(Name)).ToList();
                }
                if (!string.IsNullOrEmpty(Sex))
                {
                    bool sex = Convert.ToBoolean(Sex);
                    list = list.Where(a => a.Sex == sex).ToList();
                }
                if (!string.IsNullOrEmpty(StudentNumber))
                {
                    list = list.Where(a => a.StudentNumber.Contains(StudentNumber)).ToList();
                }
                if (!string.IsNullOrEmpty(identitydocument))
                {
                    list = list.Where(a => a.identitydocument.Contains(identitydocument)).ToList();
                }


           var xz= list.Select(a => new
            {
                a.StudentNumber,
                a.Name,
                a.Sex,
                a.BirthDate,
                a.identitydocument,
                ClassName= scheduleForTraineesBusiness.SutdentCLassName(a.StudentNumber).ClassID,
                Headmasters =headmasters.Listheadmasters(a.StudentNumber).EmpName

           }).ToList();
            var dataList = xz.OrderBy(a => a.StudentNumber).Skip((page - 1) * limit).Take(limit).ToList();
            //  var x = dbtext.GetList();
            var data = new
            {
                code = "",
                msg = "",
                count = xz.Count,
                data = dataList
            };
            return data;

        }
    }
    
}
