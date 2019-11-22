using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Business.ClassesBusiness;
using SiliconValley.InformationSystem.Business.ClassSchedule_Business;
using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Util;

namespace SiliconValley.InformationSystem.Business.StudentBusiness
{
 public   class StudentInformationBusiness:BaseBusiness<StudentInformation>
    {
        //阶段专业表
        BaseBusiness<StageGrade> tagegrade = new BaseBusiness<StageGrade>();
        //专业
        BaseBusiness<Specialty> specia = new BaseBusiness<Specialty>();
        //学员学费标准
        BaseBusiness<StudentFeeStandard> StudentFee = new BaseBusiness<StudentFeeStandard>();
        //迟交配置
        BaseBusiness<Tuitionallocation> tuitionalloca = new BaseBusiness<Tuitionallocation>();
        //学员缴费
        BaseBusiness<StudentFeeRecord> StudeRecord = new BaseBusiness<StudentFeeRecord>();
        //学费明细
        BaseBusiness<Studenttuitionfeestandard> feestandard = new BaseBusiness<Studenttuitionfeestandard>();
        //学员班级表
        ScheduleForTraineesBusiness scheduleForTraineesBusiness = new ScheduleForTraineesBusiness();
        /// <summary>
        /// 根据选择的阶段弹出专业选项
        /// </summary>
        /// <param name="id">阶段id</param>
        /// <returns></returns>
        public object Stage(int id)
        {
            var x = tagegrade.GetList().Where(a => a.IsDelete == false && a.Grand_Id == id).Select(a=>new {
                code= specia.GetEntity(a.Major_Id).Id,
                name=specia.GetEntity(a.Major_Id).SpecialtyName
            }).ToList();
            return x;
        }

        /// <summary>
        /// 获取当前学费
        /// </summary>
        /// <param name="Stage">阶段id</param>
        /// <param name="Major_Id">专业id</param>
        /// <returns></returns>
        public StudentFeeStandard GetFeestandard(int Stage, int Major_Id)
        {
           var x= tagegrade.GetList().Where(a => a.IsDelete == false && a.Grand_Id == Stage && a.Major_Id == Major_Id).FirstOrDefault();
           return StudentFee.GetList().Where(a => a.IsDelete == false && a.Stage == x.Id).FirstOrDefault();
        }
        /// <summary>
        /// 迟交数据显示
        /// </summary>
        /// <param name="Stage">阶段id</param>
        /// <returns></returns>
        public Tuitionallocation Latetuitionfee(int Stage)
        {
          return  tuitionalloca.GetList().Where(a => a.IsDelete == false && a.Stage == Stage).FirstOrDefault();
        }

  

        public object Studenttuitionfeestandard(int id)
        {
            var x = feestandard.GetList().Where(a => a.IsDelete == false && a.Stage == id).Select(a => new {
                code = a.id,
                name = a.Unitpricename
            }).ToList();
            return x;
        }
        /// <summary>
        /// 学员照片添加，返回true为成功
        /// </summary>
        /// <param name="studenid">学号</param>
        /// <param name="imgurl">照片名称</param>
        /// <returns></returns>
        public bool StudentAddImg(string studenid, string imgurl)
         {
            bool bo = true;
            try
            {
                var x = this.GetEntity(studenid);
                x.Picture = imgurl;
                this.Update(x);
                BusHelper.WriteSysLog("修改学员信息图片", Entity.Base_SysManage.EnumType.LogType.编辑数据);
            }
            catch (Exception ex)
            {
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.编辑数据);
                bo = false;

            }
            return bo;
        }

        /// <summary>
        /// 根据学号获取学员姓名，班级
        /// </summary>
        /// <param name="studentid">学号</param>
        /// <returns></returns>
        public object StuClass(string studentid)
        {
            //学员班级
            ClassScheduleBusiness classScheduleBusiness = new ClassScheduleBusiness();
            var x = this.GetEntity(studentid);
            var Enti = new
            {
                x.Name,
                x.StudentNumber,
                ClassName = scheduleForTraineesBusiness.SutdentCLassName(studentid).ClassID
            };
            return Enti;
        }
        /// <summary>
        /// 验证是否有照片了
        /// </summary>
        /// <param name="studentid">学号</param>
        /// <returns></returns>
        public int boolImg(string studentid)
        {
          if(this.GetEntity(studentid).Picture!=null)
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 获取学在校学员
        /// </summary>
        /// <returns></returns>
        public List<StudentInformation> StudentList()
        {
            return this.GetList().Where(a => a.IsDelete != true && a.State == null).ToList();
        }
    }
}
