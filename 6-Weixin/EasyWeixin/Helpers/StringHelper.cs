using System.Text.RegularExpressions;

namespace EasyWeixin.Helpers
{
    public static class StringHelper
    {
        public static string repElement(string el, string str)
        {
            string pat = @"<" + el + "[^>]+>";
            string rep = "<" + el + ">";
            str = Regex.Replace(str.ToString(), pat, rep);
            return str;
        }

        public static string ParseHtml(string html)
        {
            //html = HttpUtility.HtmlDecode(html);

            html = System.Web.HttpUtility.HtmlDecode(html);
            string[] el = new string[] { "p", "span", "strong", "table", "div", "tr", "td" };
            foreach (string s in el)
            {
                html = repElement(s, html);
            }
            //删除脚本
            html = Regex.Replace(html, @"<script(.*)</script>", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            Regex reg = new Regex("<[^>]*>");
            html = reg.Replace(html, "");
            html = Regex.Replace(html, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"-->", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"<!--.*", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "  ", "", RegexOptions.IgnoreCase);
            html = html.Replace("<", "");
            html = html.Replace(">", "");
            html = html.Replace("\r\n", "");
            return html;
        }
    }
}