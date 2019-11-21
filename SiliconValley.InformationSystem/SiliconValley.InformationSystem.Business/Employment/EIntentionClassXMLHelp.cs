using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SiliconValley.InformationSystem.Business.Employment
{
    /// <summary>
    /// 就业意向班级  业务类
    /// </summary>
    public class EIntentionClassXMLHelp
    {
        /// <summary>
        /// 班级是否是可以填写的就业意向的班级
        /// </summary>
        /// <param name="param0"></param>
        /// <returns></returns>
        public bool isexistence(string param0)
        {
            bool result = false;
            try
            {
                
                XElement xe = XElement.Load(@"F:\Projects\硅谷信息平台版本更新\1.0.9\SiliconValley.InformationSystem\SiliconValley.InformationSystem.Web\xmlfile\EIntentionClass.xml");
                IEnumerable<XElement> elements = from ele in xe.Elements("ClassNO")
                                                 select ele;
                foreach (var ele in elements)
                {
                   var  value = ele.Attribute("val").Value;
                    if (value==param0)
                    {
                        result = true;
                        break;
                    }
                }
                
            }
            catch (Exception ex)
            {

                throw;
            }
            return result;
        }

        /// <summary>
        /// 添加可以填写就业意向的班级
        /// </summary>
        /// <param name="param0"></param>
        /// <returns></returns>
        public bool AddEIntentionClass(string param0)
        {
            bool result = false;
            try
            {
                XElement xe = XElement.Load(@"F:\Projects\硅谷信息平台版本更新\1.0.9\SiliconValley.InformationSystem\SiliconValley.InformationSystem.Web\xmlfile\EIntentionClass.xml");
                XElement record = new XElement(
                new XElement("ClassNO",
                new XAttribute("val", param0)));
                xe.Add(record);
                xe.Save(@"F:\Projects\硅谷信息平台版本更新\1.0.9\SiliconValley.InformationSystem\SiliconValley.InformationSystem.Web\xmlfile\EIntentionClass.xml");
                result = true;

            }
            catch (Exception ex)
            {

                throw;
            }

            return result;
        }
    }
}
