using FrontDesk.Server.Data.BhsVisits;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FrontDesk.Server.Web.RestApi
{
    //typeahead.ashx?list=tribe
    public class TypeaheadRestHttpHandler : IHttpHandler
    {
        public bool IsReusable => true;

        public void ProcessRequest(HttpContext context)
        {
            string listName = (context.Request.QueryString["list"]?? string.Empty).Trim();
            string filter = (context.Request.QueryString["q"] ?? string.Empty).Trim();

            List<string> result = new List<string>();

            if (listName == "tribe")
            {
                result = new TypeaheadDataSource().GetTribe(filter);
            }
            else if (listName == "county")
            {
                result = new TypeaheadDataSource().GetCounty(filter);
            }

            context.Response.ContentType = "application/json";
            context.Response.ContentEncoding = Encoding.UTF8;

            context.Response.Cache.SetExpires(DateTime.Now.AddHours(1));
            context.Response.Cache.SetCacheability(HttpCacheability.Public);

            context.Response.Write(JsonConvert.SerializeObject(result));


        }
    }
}
