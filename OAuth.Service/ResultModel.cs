using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAuth.Service
{
    /// <summary>
    /// 结果模型
    /// </summary>
    public class ResultModel
    {
        
        public ResultModel(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public ResultModel() { }

        public int Code { get; set; }

        public string Message { get; set; }
    }
}