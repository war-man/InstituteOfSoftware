using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
 public   class ProfessionalaView { 
   
            public int ID { get; set; }
            /// <summary>
            /// 培训人
            /// </summary>
            public int Trainee { get; set; }
            /// <summary>
            /// 培训人名字及部门，该属性不往数据库添加，业务查询数据使用
            /// </summary>
            public string EmpNameTraine { get; set; }
            /// <summary>
            /// 培训标题
            /// </summary 
            public string TrainingTitle { get; set; }
            /// <summary>
            /// 培训内容
            /// </summary>
            public string Trainingcontent { get; set; }
            /// <summary>
            /// 培训日期
            /// </summary>
            public Nullable<System.DateTime> TrainingDate { get; set; }
            /// <summary>
            /// 备注
            /// </summary>
            public string Remarks { get; set; }
            /// <summary>
            /// 是否删除
            /// </summary>
            public Nullable<bool> Dateofregistration { get; set; }
            /// <summary>
            /// 添加时间
            /// </summary>
            public Nullable<System.DateTime> AddTime { get; set; }
        }
    }



