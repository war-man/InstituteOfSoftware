using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.Base_SysManage.ViewEntity
{
    /// <summary>
    /// 微信账号用户信息
    /// </summary>
    public class Weixin_info
    {

        public string OpenId { get; set; }//普通用户的标识，对当前开发者帐号唯一
        public string Nickname { get; set; }//普通用户昵称
        public string Sex { get; set; }//普通用户性别，1为男性，2为女性
        public string Province { get; set; }//普通用户个人资料填写的省份

        public string City { get; set; }//普通用户个人资料填写的城市

        public string Headimgurl { get; set; }//用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空
        public string Unionid { get; set; }//用户统一标识。针对一个微信开放平台帐号下的应用，同一用户的unionid是唯一的。
    }

}
