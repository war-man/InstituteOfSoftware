using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    public class EmployeeInfoView
    {
        public string excelid { get; set; }//excel数据的序号
        public string name { get; set; }//姓名
        public string dept { get; set; }//部门
        public string position { get; set; }//岗位
        public string ddid { get; set; }//工号
        public string original { get; set; }//招聘来源
        public string idcardnum { get; set; }//身份证号码
        public string phonenum { get; set; }//电话号码
        public string empsex { get; set; }//性别
        //public Nullable<int> empage { get; set; }//年龄
        public string nation { get; set; }//民族
        public System.DateTime? entertime { get; set; }//入职时间
        public System.DateTime? positivetime { get; set; }//转正时间
        public Nullable<decimal> probationsalary { get; set; }//试用期工资
        public Nullable<decimal> salary { get; set; }//转正后工资
        public string education { get; set; }//学历
        public System.DateTime? contractStartTime { get; set; }//合同起止日期
        public System.DateTime? contractEndTime { get; set; }//合同终止日期
        public string birthday { get; set; }//生日
        public string urgentphone { get; set; }//紧急联系电话
        public string domicileAddress { get; set; }//户籍地址
        public string address { get; set; }//现居地址
        public Nullable<bool> maritalStatus { get; set; }//婚姻状况
        public Nullable<System.DateTime> idcardIndate { get; set; }//身份证有效期
        public string politicsStatus { get; set; }//政治面貌
        public Nullable<System.DateTime> SSstartTime { get; set; }//社保起始月份
        public string bankCardnum { get; set; }//银行卡号
        public string paperyMaterial { get; set; }//纸质材料
        //public Nullable<System.DateTime> Birthdate { get; set; }
        public string Remark { get; set; }//备注
        //public string WorkExperience { get; set; }
        //public Nullable<bool> IsDel { get; set; }
        //public string Image { get; set; }

    }
}
