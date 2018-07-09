using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using OAuth.Service.Common;
using OAuth.Service.Interfaces;
using OAuth.Service.ModelDto;

namespace OAuth.Service
{
    public class SessionService
    {
        private readonly static ICacheManager Cache;

        [ThreadStatic]
        private static UserInfo _current;

        static SessionService()
        {
            Cache = new CacheManager();
        }

        public static UserInfo SessionInfo
        {
            get
            {
                if (_current == null)
                {
                    string sessionid = GetCookieKey();
                    var obj = Cache.Get<UserInfo>(sessionid);

                    if (obj != null)
                    {
                        if (obj.LastRequestTime == null || (DateTime.Now - obj.LastRequestTime.Value).Minutes > 3)
                        {
                            obj.LastRequestTime = DateTime.Now;
                            Cache.Set(sessionid, obj, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 30, 0));
                        }
                    }

                    _current = obj;
                }

                if (_current == null)
                {
                    _current = NewSessionInfo();
                    Save();
                }

                return _current;

            }
            internal set { _current = value; }
        }


        public static void Save()
        {
            Cache.Set(_current.SessionId, _current, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 30, 0));
        }

        public static void Clear()
        {
            _current = NewSessionInfo();
            Save();
        }

        /// <summary>
        /// 销毁当前登录用户信息
        /// </summary>
        public static void Destroy()
        {
            _current = null;
        }

        public static string GetCookieKey()
        {
            string cookieName = "sessionid";
            string cookieInfo = "";

            if (HttpContext.Current.Request.Cookies[cookieName] != null)
            {
                cookieInfo = HttpContext.Current.Request.Cookies[cookieName].Value;
            }
            else
            {
                cookieInfo = Guid.NewGuid().ToString("N");
                HttpCookie cookie = new HttpCookie(cookieName)
                {
                    HttpOnly = true,
                    Path = "/",
                    Value = cookieInfo
                };

                HttpContext.Current.Response.SetCookie(cookie);

                var httpCookie = HttpContext.Current.Request.Cookies[cookieName];
                if (httpCookie != null)
                    cookieInfo = httpCookie.Value;
            }

            return cookieInfo;
        }


        private static UserInfo NewSessionInfo()
        {
            string sessionId = GetCookieKey();
            var info = new UserInfo
            {
                Id = 0,
                SessionId = sessionId,
                UserName = "none",
                LastRequestTime = DateTime.Now
            };

            return info;
        }
    }
}
