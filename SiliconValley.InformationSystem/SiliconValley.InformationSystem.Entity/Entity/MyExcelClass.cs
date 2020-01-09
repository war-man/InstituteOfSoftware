using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Entity.MyEntity;

namespace SiliconValley.InformationSystem.Entity.Entity
{
    /// <summary>
    /// Excel实体类
    /// </summary>
   public class MyExcelClass:IEqualityComparer<MyExcelClass>
    {
       /// <summary>
       /// 学生姓名
       /// </summary>
       public string StuName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
       public string StuSex { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
       public string StuPhone { get; set; }
        /// <summary>
        /// 毕业学校
        /// </summary>
       public string StuSchoolName { get; set; }
        /// <summary>
        /// 家庭住址
        /// </summary>
       public string StuAddress { get; set; }
        /// <summary>
        /// 招生区域
        /// </summary>
       public string Region_id { get; set; }
        /// <summary>
        /// 信息来源
        /// </summary>
       public string StuInfomationType_Id { get; set; }
        /// <summary>
        /// 学历
        /// </summary>
       public string StuEducational { get; set; }
        /// <summary>
        /// 备案老师
        /// </summary>
       public string EmployeesInfo_Id { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
       public string Reak { get; set; }

        bool IEqualityComparer<MyExcelClass>.Equals(MyExcelClass x, MyExcelClass y)
        {
            if (x.StuName==y.StuName && y.StuPhone==x.StuPhone)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        int IEqualityComparer<MyExcelClass>.GetHashCode(MyExcelClass obj)
        {
            throw new NotImplementedException();
        }
    }
}
