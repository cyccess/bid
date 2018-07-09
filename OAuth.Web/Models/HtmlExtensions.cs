using OAuth.Domain.Model;
using OAuth.Service.ModelDto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace System.Web.Mvc
{
    public static class HtmlExtensions
    {
        /// <summary>
        /// 状态显示
        /// </summary>
        /// <param name="html"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static MvcHtmlString Label(this HtmlHelper html, bool status)
        {
            StringBuilder label = new StringBuilder();
            if (status)
            {
                label.AppendFormat("<span class=\"label label-success\">启用</span>");
            }
            else
            {
                label.AppendFormat("<span class=\"label label-default\">禁用</span>");
            }
            return new MvcHtmlString(label.ToString());
        }

        public static MvcHtmlString RadioGroup(this HtmlHelper html, bool status)
        {
            StringBuilder radioGroup = new StringBuilder();
            string yesChecked, noChecked;

            if (status)
            {
                yesChecked = "checked=\"checked\"";
                noChecked = "";
            }
            else
            {
                yesChecked = "";
                noChecked = "checked=\"checked\"";
            }

            radioGroup.AppendFormat("<label class=\"radio-inline\">");
            radioGroup.AppendFormat("<input id=\"rdoStatus1\" name=\"Status\" type=\"radio\" {0} value=\"1\" />是", yesChecked);
            radioGroup.AppendFormat("</label>");
            radioGroup.AppendFormat("<label class=\"radio-inline\">");
            radioGroup.AppendFormat("<input id=\"rdoStatus2\" name=\"Status\" type=\"radio\" {0} value=\"0\" />否", noChecked);
            radioGroup.AppendFormat("</label>");

            return new MvcHtmlString(radioGroup.ToString());
        }


        public static MvcHtmlString RadioGroup(this HtmlHelper html, bool status, string name)
        {
            StringBuilder radioGroup = new StringBuilder();
            string yesChecked, noChecked;

            if (status)
            {
                yesChecked = "checked=\"checked\"";
                noChecked = "";
            }
            else
            {
                yesChecked = "";
                noChecked = "checked=\"checked\"";
            }

            radioGroup.AppendFormat("<label class=\"radio-inline\">");
            radioGroup.AppendFormat("<input id=\"rdoStatus1\" name=\"{1}\" type=\"radio\" {0} value=\"True\" />是", yesChecked, name);
            radioGroup.AppendFormat("</label>");
            radioGroup.AppendFormat("<label class=\"radio-inline\">");
            radioGroup.AppendFormat("<input id=\"rdoStatus2\" name=\"{1}\" type=\"radio\" {0} value=\"False\" />否", noChecked, name);
            radioGroup.AppendFormat("</label>");

            return new MvcHtmlString(radioGroup.ToString());
        }
    }
}