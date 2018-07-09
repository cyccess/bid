using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OAuth.Web.Models
{ 
    /// <summary>
    /// 授权的用户信息
    /// </summary>
    [XmlRoot("Response")]
    public class IdentityUser
    {
        /// <summary>
        /// 是否验证了用户
        /// </summary>
        public int IsAuthenticated { get; set; }

        /// <summary>
        /// 会员id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 登录IP
        /// </summary>
        public string LoginIP { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public string LoginTime { get; set; }

        /// <summary>
        /// 令牌编号
        /// </summary>
        public string Tokenid { get; set; }

        /// <summary>
        /// 应用程序id
        /// </summary>
        public string Appid { get;set; }

        /// <summary>
        /// 登录地址
        /// </summary>
        public string LoginUrl { get; set; }
    }
}
