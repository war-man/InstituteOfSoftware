using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

using SiliconValley.InformationSystem.Entity.Base_SysManage.ViewEntity;
using SiliconValley.InformationSystem.Business.Base_SysManage;

namespace SiliconValley.InformationSystem.Web.Common
{
    /// <summary>
    ///微信扫码登录工具类
    /// </summary>
    public class QQ_callback
    {
        public QQ_callback()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary>
        /// 获得Weixn回调信息字符串json
        /// </summary>
        /// <param name="code">Authorization Code</param>
        /// <returns></returns>
        public Weixin_info getWeixinUserInfoJSON(string code)
        {
            Weixin_info weixin_info = new Weixin_info();//自己定义的dto
            Base_SysLogBusiness base_SysLogBusiness = new Base_SysLogBusiness();
            try
            {
                string appid = "wx9a441f4d4496906a";
                string secret = "5cc48a40aee59287c3180a00c8441f79";


                string apiurl = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", appid, secret, code);

                System.GC.Collect();
                System.Net.ServicePointManager.DefaultConnectionLimit = 200;
                WebRequest request = WebRequest.Create(apiurl);

                WebResponse response = request.GetResponse();

                Stream stream = response.GetResponseStream();
                Encoding encode = Encoding.UTF8;
                StreamReader reader = new StreamReader(stream, encode);
                string jsonText = reader.ReadToEnd();

                JObject jo1 = (JObject)JsonConvert.DeserializeObject(jsonText);
                string access_token = jo1["access_token"].ToString();
                string refresh_token = jo1["refresh_token"].ToString();
                string openid = jo1["openid"].ToString();


                string url_me = string.Format("https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type=refresh_token&refresh_token={1}", appid, refresh_token);
                request = WebRequest.Create(url_me);
                response = request.GetResponse();
                stream = response.GetResponseStream();
                reader = new StreamReader(stream, encode);
                string openIdStr = reader.ReadToEnd();


                JObject jo = (JObject)JsonConvert.DeserializeObject(openIdStr);

                //string access_token = jo["access_token"].ToString();
                string openId = jo["openid"].ToString();

                ////根据OpenID获取用户信息 可以显示更多 用的就几个 需要的可以自己在下面加
                string getinfo = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}", access_token, openId);
                request = WebRequest.Create(getinfo);
                response = request.GetResponse();
                stream = response.GetResponseStream();
                reader = new StreamReader(stream, encode);
                string userStr = reader.ReadToEnd();

                JObject info = (JObject)JsonConvert.DeserializeObject(userStr);

                
                weixin_info.OpenId = openId;
                weixin_info.Nickname = info["nickname"].ToString();
                weixin_info.Sex = info["sex"].ToString();
                weixin_info.Province = info["province"].ToString();
                weixin_info.City = info["city"].ToString();
                weixin_info.Headimgurl = info["headimgurl"].ToString();//大小为30×30像素的QQ空间头像URL。
                weixin_info.Unionid = info["unionid"].ToString();//用户统一标识。针对一个微信开放平台帐号下的应用，同一用户的unionid是唯一的。
                reader.Close();
                stream.Flush();
                stream.Close();
                response.Close();

            }
            catch (Exception ex)
            {

                throw;
            }

            return weixin_info;

        }
    }


}