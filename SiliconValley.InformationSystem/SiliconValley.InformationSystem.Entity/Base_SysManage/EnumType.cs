namespace SiliconValley.InformationSystem.Entity.Base_SysManage
{
    /// <summary>
    /// 枚举类型
    /// </summary>
    public class EnumType
    {
        /// <summary>
        /// 系统日志类型
        /// </summary>
        public enum LogType
        {
            系统异常,
            系统用户管理,
            系统角色管理,
            接口密钥管理,
            加载数据异常,
            添加数据异常
        }

        /// <summary>
        /// 系统角色类型
        /// </summary>
        public enum RoleType
        {
            超级管理员 = 1
        }
    }
}
