using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public AjaxResult Cost(StudentFeeRecord studentFeeRecord)
        {
            AjaxResult retus = null;
            try
            {
                //studentFeeRecord.Addtime = DateTime.Now;
                studentFeeRecord.IsDelete = false;
                StudeRecord.Insert(studentFeeRecord);
                retus = new SuccessResult();
                retus.Success = true;
                retus.Msg = "添加数据";
                BusHelper.WriteSysLog("添加数据", Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            catch (Exception ex)
            {
                retus = new ErrorResult();
                retus.Msg = "服务器错误";
                retus.Success = false;
                retus.ErrorCode = 500;
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            return retus;
        }

        public object Studenttuitionfeestandard(int id)
        {
            var x = feestandard.GetList().Where(a => a.IsDelete == false && a.Stage == id).Select(a => new {
                code = a.id,
                name = a.Unitpricename
            }).ToList();
            return x;
        }
    }
}
