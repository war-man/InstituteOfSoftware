using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
   public class NetClientRecordView
    { 
        public int Id { get; set; }
        public string EmpId { get; set; }//回访人编号
        public string Empname { get; set; }//跟踪回访人
        public string Channelemp { get; set; }//市场回访人员
        public Nullable<System.DateTime> NetClientDate { get; set; }//回访日期
        public string CallBackCase { get; set; }//回访记录
        public Nullable<bool> IsDel { get; set; }    
        public int SPRId { get; set; }//备案编号
        public string StuName { get; set; }//学生姓名（备案）
        public string StuSex { get; set; }//学生性别（备案）
        public string StuPhone { get; set; }//联系电话（备案）
        public string channeltea { get; set; }//市场对接老师（市场人员表）
        public string StuWeiXin { get; set; }//学生微信（备案）
        public string StuQQ { get; set; }//学生QQ（备案） 
        public string StuEducational { get; set; }//学历
        public string StuStatus_Id { get; set; }//学生状态（备案：是否报名）
        public string SprEmp { get; set; }//备案人（备案）
        public string IsFaceConsult { get; set; }//是否面咨
        public string StuStatus { get; set; }//学生状态
        public string RegionName { get; set; }//所在区域
        public string consultemp { get; set; }//咨询师
        public string Grade { get; set; }//等级
    }
}
