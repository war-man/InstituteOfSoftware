using SiliconValley.InformationSystem.Entity.Entity;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.EducationalBusiness
{
    public class EvningSelfStudyManeger : BaseBusiness<EvningSelfStudy>
    {
        static readonly RedisCache redisCache = new RedisCache();
        private ClassroomManeger ClassRoom_Entity;
        private BaseDataEnumManeger BaseDataEnum_Entity;

        /// <summary>
        /// 从缓存中获取所有数据
        /// </summary>
        /// <returns></returns>
        public List<EvningSelfStudy> EvningSelfStudyGetAll()
        {
            EvningSelfStudyManeger.redisCache.RemoveCache("EvningSelfStudyList");
            List<EvningSelfStudy> EvningSelfStudy_list = new List<EvningSelfStudy>();
            EvningSelfStudy_list = EvningSelfStudyManeger.redisCache.GetCache<List<EvningSelfStudy>>("EvningSelfStudyList");
            if (EvningSelfStudy_list == null || EvningSelfStudy_list.Count == 0)
            {
                EvningSelfStudy_list = this.GetList();
                Reconcile_Com.redisCache.SetCache("EvningSelfStudyList", EvningSelfStudy_list);
            }
            return EvningSelfStudy_list;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public AjaxResult Add_Data(EvningSelfStudy e)
        {
            AjaxResult a = new AjaxResult();
            try
            {
                this.Insert(e);
                EvningSelfStudyManeger.redisCache.RemoveCache("EvningSelfStudyList");
                a.Success = true;
            }
            catch (Exception ex)
            {
                a.Msg = ex.Message;
                a.Success = false;
            }
            return a;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="eid"></param>
        /// <returns></returns>
        public AjaxResult Delete_Data(int eid)
        {
            AjaxResult a = new AjaxResult();
            EvningSelfStudy find_e = this.GetEntity(eid);
            try
            {
                if (find_e != null)
                {
                    this.Delete(find_e);
                    EvningSelfStudyManeger.redisCache.RemoveCache("EvningSelfStudyList");
                    a.Success = true;
                }
            }
            catch (Exception ex)
            {
                a.Msg = ex.Message;
                a.Success = false;
            }
            return a;
        }
        /// <summary>
        /// 修改(只修改安排的日期跟上课时间段)
        /// </summary>
        /// <param name="new_e"></param>
        /// <returns></returns>
        public AjaxResult Update_Data(EvningSelfStudy new_e)
        {
            AjaxResult a = new AjaxResult();
            EvningSelfStudy old_e = this.GetEntity(new_e.id);
            try
            {
                if (old_e != null)
                {
                    old_e.Anpaidate = new_e.Anpaidate;
                    old_e.curd_name = new_e.curd_name;
                    this.Update(old_e);
                     redisCache.RemoveCache("EvningSelfStudyList");
                    a.Success = true;
                }
            }
            catch (Exception ex)
            {
                a.Msg = ex.Message;
                a.Success = false;
            }
            return a;
        }       
        public AjaxResult Update_DataTwo(EvningSelfStudy new_e)
        {
            AjaxResult a = new AjaxResult();
            try
            {
                this.Update(new_e);
                redisCache.RemoveCache("EvningSelfStudyList");
                a.Success = true;
            }
            catch (Exception ex)
            {

                a.Msg = ex.Message;
                a.Success = false;
            }
            return a;
        }
        /// <summary>
        /// 判断XX班级是否安排了晚自习
        /// </summary>
        /// <param name="time">日期</param>
        /// <param name="class_id">班级编号</param>
        /// <returns></returns>
        public bool IsAlreadAnpai(DateTime time, int class_id)
        {
            int count = EvningSelfStudyGetAll().Where(e => e.Anpaidate == time && e.ClassSchedule_id == class_id).Count();
            return count > 0 ? true : false;
        }
        /// <summary>
        /// 查询晚自习安排数据
        /// </summary>
        /// <param name="timename">上课时间段</param>
        /// <param name="time">日期</param>
        /// <param name="roomlist">教室集合</param>
        /// <returns></returns>
        public List<AnPaiData> getAppoint(string timename,DateTime time,List<Classroom> roomlist){
            List<AnPaiData> list = new List<AnPaiData>();
            foreach (Classroom item in roomlist)
            {
                EvningSelfStudy fine= EvningSelfStudyGetAll().Where(e => e.curd_name == timename && e.Anpaidate == time && e.Classroom_id == item.Id).FirstOrDefault();
                AnPaiData a = new AnPaiData();             
                if (fine!=null)
                {
                    a.NeiRong = "晚自习";
                    a.ClassName =Reconcile_Com.ClassSchedule_Entity.GetEntity(fine.ClassSchedule_id).ClassNumber;
                    a.class_Id = fine.ClassSchedule_id;
                }
                list.Add(a);
            }
            return list;
        }
        /// <summary>
        /// 获取XX日期XX教室晚自习安排
        /// </summary>
        /// <param name="time">日期</param>
        /// <param name="classroom_id">教室</param>
        /// <returns></returns>
        public EvningSelfStudy GetNving(DateTime time,int class_id)
        {
          return  EvningSelfStudyGetAll().Where(e => e.Anpaidate == time && e.ClassSchedule_id == class_id).FirstOrDefault();
        }
        /// <summary>
        /// 获取晚自习在XX教室上课的班级
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="classoom_id"></param>
        /// <returns></returns>
        public List<EvningSelfStudy> GetOnCurrClass(DateTime dateTime, int classoom_id)
        {
            return EvningSelfStudyGetAll().Where(e => e.Anpaidate == dateTime && e.Classroom_id == classoom_id).ToList();
        }
        /// <summary>
        /// 获取空教室
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <param name="old">false-达康维嘉校区，true--继善高科校区</param>
        /// <returns></returns>
        public ClassRoom_AddCourse  GetEmptyClassroom(DateTime dateTime,bool old)
        {
            ClassRoom_Entity = new ClassroomManeger();
            ClassRoom_AddCourse result = new ClassRoom_AddCourse();
            BaseDataEnum_Entity = new BaseDataEnumManeger();
            List<EvningSelfStudy> getlist = EvningSelfStudyGetAll().Where(e=>e.Anpaidate== dateTime).ToList();
            int base_id = 0;
            if (old)
            { 
                //获取继善高科校区教室
                 base_id= BaseDataEnum_Entity.GetSingData("继善高科校区", false).Id;
            }
            else
            {
                base_id = BaseDataEnum_Entity.GetSingData("达嘉维康校区", false).Id;
            }
            if (base_id!=0)
            {
                List<Classroom> list_classroom1 = ClassRoom_Entity.GetAddreeClassRoom(base_id);
                foreach (var item in list_classroom1)
                {
                    //ClassRoom_AddCourse
                    List<EvningSelfStudy> list_ev = getlist.Where(e => e.Classroom_id == item.Id && e.Anpaidate == dateTime).ToList();
                    int count = list_ev.Count;
                    if (count == 0)
                    {
                        result.ClassRoomId = item.Id;
                        result.TimeName = "晚一";
                        break;
                    }
                    else if (count == 1)
                    {
                        result.ClassRoomId = item.Id;
                        result.TimeName = list_ev[0].curd_name == "晚一" ? "晚二" : "晚一";
                        break;
                    }
                }
            }            
            return result;
        }
        /// <summary>
        /// 日期往前挪或往后退
        /// </summary>
        /// <param name="s1ors3"></param>
        /// <param name="count"></param>
        /// <param name="starTime"></param>
        /// <returns></returns>
        public AjaxResult ALLDataADI(bool s1ors3,int count,DateTime starTime)
        {
            AjaxResult a = new AjaxResult();
            try
            {
                List<ClassSchedule> class_list = Reconcile_Com.GetClass(s1ors3);
                List<EvningSelfStudy> e_list = EvningSelfStudyGetAll().Where(e => e.Anpaidate >= starTime).ToList();
                foreach (EvningSelfStudy item in e_list)
                {
                    int c_count = class_list.Where(c => c.id == item.ClassSchedule_id).ToList().Count;
                    if (c_count > 0)
                    {
                        item.Anpaidate = item.Anpaidate.AddDays(count);
                        this.Update(item);
                    }
                }
                a.Success = true;
            }
            catch (Exception ex)
            {
                a.Success = false;
                a.Msg = ex.Message;
            }
            return a;
        }

        public AjaxResult ClassALLDataADI(int count, DateTime starTime,int class_id)
        {
            AjaxResult a = new AjaxResult();
            try
            {
                List<EvningSelfStudy> e_list = EvningSelfStudyGetAll().Where(e => e.Anpaidate >= starTime && e.ClassSchedule_id==class_id).ToList();
                foreach (EvningSelfStudy item in e_list)
                {                     
                    item.Anpaidate = item.Anpaidate.AddDays(count);
                    this.Update(item);        
                }
                a.Success = true;
            }
            catch (Exception ex)
            {
                a.Success = false;
                a.Msg = ex.Message;
            }
            return a;
        }

        /// <summary>
        /// 上课日期调换
        /// </summary>
        /// <param name="e"></param>
        /// <param name="d1"></param>
        /// <returns></returns>
        public AjaxResult ChangDate(List<EvningSelfStudy> e,DateTime d1)
        {
            AjaxResult a = new AjaxResult();
            try
            {
                foreach (EvningSelfStudy it in e)
                {
                    it.Anpaidate = d1;
                    this.Update(it);
                }
                EvningSelfStudyManeger.redisCache.RemoveCache("EvningSelfStudyList");
                a.Success = true;               
            }
            catch (Exception ex)
            {
                a.Success = false;
                a.Msg = ex.Message;
            }

            return a;
        }
    }
}
