using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SiliconValley.InformationSystem.Business.DormitoryBusiness
{
    public class RoomdeWithPageXmlHelp
    {
        /// <summary>
        /// 通过xml配置得出房间的数据
        /// </summary>
        /// <returns></returns>
        public List<RoomStayType> GetRoomStayTypesByXml()
        {
            List<RoomStayType> list = new List<RoomStayType>();
            XElement xe = XElement.Load(@"F:\Projects\硅谷信息平台版本更新\1.0.9\SiliconValley.InformationSystem\SiliconValley.InformationSystem.Web\xmlfile\RoomdeWithPage.xml");
            IEnumerable<XElement> elements = from ele in xe.Elements("RoomStayType")
                                             select ele;
            foreach (var ele in elements)
            {
                RoomStayType obj = new RoomStayType();
                //属性用Attribute  
                obj.Id = int.Parse(ele.Attribute("id").Value);
                //子节点用Element
                obj.RoomStayTypeName = ele.Element("RoomStayTypeName").Value;
                list.Add(obj);
            }
            return list;
        }
}
