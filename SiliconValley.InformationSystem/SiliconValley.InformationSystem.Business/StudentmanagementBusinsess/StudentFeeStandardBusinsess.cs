using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Util;

namespace SiliconValley.InformationSystem.Business.StudentmanagementBusinsess
{
    public class StudentFeeStandardBusinsess : BaseBusiness<StudentFeeStandard>
    {
        //专业表
        BaseBusiness<Specialty> special = new BaseBusiness<Specialty>();
        //阶段专业表
        BaseBusiness<StageGrade> stagegrade = new BaseBusiness<StageGrade>();
        //阶段表

        BaseBusiness<Grand> geand = new BaseBusiness<Grand>();
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="page">第几个</param>
        /// <param name="limit">当前几页</param>
        /// <returns></returns>
        public object StudentFeeList(int page, int limit)
        {

          var list=  this.GetList().Where(a => a.IsDelete == false).Select(a => new
            {

              a.ID,
                a.Foodandlodging,
                a.Tuition,
                grade_Id = geand.GetEntity(stagegrade.GetList().Where(c => c.IsDelete == false && c.Id == a.Stage).FirstOrDefault().Grand_Id).GrandName,
                Major_Id = special.GetEntity(stagegrade.GetList().Where(c => c.IsDelete == false && c.Id == a.Stage).FirstOrDefault().Major_Id).SpecialtyName
            }).ToList();
            var dataList = list.OrderBy(a => a.ID).Skip((page - 1) * limit).Take(limit).ToList();
            //  var x = dbtext.GetList();
            var data = new
            {
                code = "",
                msg = "",
                count = list.Count,
                data = dataList
            };
            return data;
        }
        /// <summary>
        /// 查询是否数据库存在学员学费单
        /// </summary>
        /// <param name="Grand_Id">阶段id</param>
        /// <param name="Major_Id">专业id</param>
        /// <returns></returns>
        public bool BoolFeeStude(int Grand_Id,int Major_Id)
        {
            bool str = false;
          var id=  stagegrade.GetList().Where(a => a.IsDelete == false && a.Grand_Id == Grand_Id && a.Major_Id == Major_Id).FirstOrDefault();
            if (id!=null)
            {
                var count = this.GetList().Where(a => a.IsDelete == false && a.Stage == id.Id).Count();
                if (count>0)
                {
                    str = true;
                }
            }
            return str;
         
        }
        /// <summary>
        /// 录入学员学费价格单
        /// </summary>
        /// <param name="Foodandlodging">伙食费</param>
        /// <param name="Tuition">学费</param>
        /// <param name="Grand_Id">阶段id</param>
        /// <param name="Major_Id">专业id</param>
        /// <returns></returns>
        public AjaxResult AddFeeStudent(decimal Foodandlodging,decimal Tuition,int Grand_Id,int Major_Id)
        {
            AjaxResult retus = null;

            try
            {
                var x = stagegrade.GetList().Where(a => a.IsDelete == false && a.Grand_Id == Grand_Id && a.Major_Id == Major_Id).FirstOrDefault();
                StudentFeeStandard studentFeeStandard = new StudentFeeStandard();
                if (x == null)
                {
                    StageGrade stage = new StageGrade();
                    stage.Major_Id = Major_Id;
                    stage.Grand_Id = Grand_Id;
                    stage.IsDelete = false;
                    stage.AddTime = DateTime.Now;
                    stagegrade.Insert(stage);
                    var my_stage = stagegrade.GetList().Where(a => a.IsDelete == false && a.Grand_Id == Grand_Id && a.Major_Id == Major_Id).FirstOrDefault();
                    x.Id = my_stage.Id;
                }
                studentFeeStandard.Stage = x.Id;
                studentFeeStandard.Foodandlodging = Foodandlodging;
                studentFeeStandard.Tuition = Tuition;
                studentFeeStandard.Addtime = DateTime.Now;
                studentFeeStandard.IsDelete = false;
                this.Insert(studentFeeStandard);
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
    }
    
}
