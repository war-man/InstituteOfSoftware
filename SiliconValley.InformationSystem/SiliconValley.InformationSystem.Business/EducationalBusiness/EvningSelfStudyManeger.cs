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

    }
}
