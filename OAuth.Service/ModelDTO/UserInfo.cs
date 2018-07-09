using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OAuth.Domain.Model;

namespace OAuth.Service.ModelDto
{
    [Serializable]
    public class UserInfo
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int Id { get; set; }

        public string SessionId { get; set; }

        public string UserName { get; set; }

        public bool IsEnabled { get; set; }

        /// <summary>
        /// 用户类型 1 供应商 ，2 系统用户
        /// </summary>
        public UserType UserType { get; set; }

        public DateTime? LastRequestTime { get; set; }

        public IEnumerable<Module> Module { get; set; }
    }

    public enum UserType : byte
    {
        /// <summary>
        /// 后台用户
        /// </summary>
        User = 2,

        /// <summary>
        /// 供应商
        /// </summary>
        Supplier = 1
    }
}
