using System;
using System.Web.Mvc;
using System.Web.WebPages;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Globalization;

namespace MvcHtmlHelpers
{
    public class HtmlResources
    {
        public List<string> Scripts;
        public List<string> Stylesheets;

        public HtmlResources()
        {
            Scripts = new List<String>();
            Stylesheets = new List<String>();
        }
    }

    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString Resource(this HtmlHelper html, string value)
        {
            var server = html.ViewContext.RequestContext.HttpContext.Server;
            var resources = (HtmlResources)html.ViewData["Resources"];
            if (resources == null)
            {
                resources = new HtmlResources();
                html.ViewData["Resources"] = resources;
            }

            if (File.Exists(server.MapPath(value)))
            {
                if (value.EndsWith(".js"))
                {
                    if (!resources.Scripts.Contains(value))
                    {
                        resources.Scripts.Insert(0, value);
                    }
                    else
                    {
                        throw new HttpException(String.Format("Script already included {0}", value));
                    }
                }
                else if (value.EndsWith(".css"))
                {
                    if (!resources.Stylesheets.Contains(value))
                    {
                        resources.Stylesheets.Insert(0, value);
                    }
                    else
                    {
                        throw new HttpException(String.Format("Stylesheet already included {0}", value));
                    }
                }
            }
            else
            {
                throw new HttpException(404, String.Format("Could not find file {0}", value));
            }
            return null;
        }

        public static MvcHtmlString RenderResources(this HtmlHelper html)
        {
            var url = new UrlHelper(html.ViewContext.RequestContext);
            var server = html.ViewContext.RequestContext.HttpContext.Server;
            var resources = (HtmlResources)html.ViewData["Resources"];
            string result = "";
            
            if (resources != null)
            {
                foreach (string resource in resources.Stylesheets)
                {
                    DateTime dt = File.GetLastWriteTime(server.MapPath(resource));
                    result += "<link href=\"" + url.Content(resource) + "?" + String.Format("{0:yyyyddHHss}", dt) + "\" rel=\"stylesheet\" type=\"text/css\" />\n";
                }
                foreach(string resource in resources.Scripts) {
                    DateTime dt = File.GetLastWriteTime(server.MapPath(resource));
                    result += "<link href=\"" + url.Content(resource) + "?" + String.Format("{0:yyyyddHHss}", dt) + "\" rel=\"stylesheet\" type=\"text/css\" />\n";
                }
            }

            return MvcHtmlString.Create(result);
        }

        public static HelperResult RenderSection(this WebPageBase webPage, string name, string stuff)
        {
            return webPage.RenderSection(name);
        }
    }
}
