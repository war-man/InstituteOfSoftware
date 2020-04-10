using SiliconValley.InformationSystem.Business.EducationalBusiness;
using System;
using Xunit;

namespace XUnitTestProject1
{
    public class UnitTest1
    {
       

        public UnitTest1()
        {
            
        }

        [Fact]
        public void Test1()
        {
            //第一阶段 创建实例
            Staff_Cost_StatisticssBusiness db_Const = new Staff_Cost_StatisticssBusiness();

            //第二阶段 Act 调用
            var result = db_Const.Staff_CostData("202001030008", DateTime.Parse("2020-04-04"));

        }
    }
}
