using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FrontDesk.Server.Web.Notification
{
    public class NotificationMessagesHub
    {
        public List<NotificationMessage> Notifications = new List<NotificationMessage>();

        public NotificationMessagesHub Add(string message, NotificationCategory category)
        {
            Notifications.Add(new NotificationMessage(message, category));
            return this;
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(Notifications);
        }

        public static implicit operator NotificationMessagesHub(string value)
        {
            var model = new NotificationMessagesHub();
            var messages = JsonConvert.DeserializeObject<List<NotificationMessage>>(value);
            if (messages != null)
            {
                model.Notifications.AddRange(messages);
            }
            return model;
        }


        public string ToHtml(bool renderScriptBlock = false)
        {
            if (Notifications.Count == 0) return string.Empty;

            StringBuilder jsBlock = new StringBuilder();
            if (renderScriptBlock)
            {
                jsBlock.AppendLine("<script type=\"text/javascript\">");
            }

            foreach (var item in Notifications)
            {
                jsBlock.Append(item.ToJS());
            }
            if (renderScriptBlock)
            {
                jsBlock.AppendLine("</script>");
            }
            return jsBlock.ToString();
        }
        
        public void Reset()
        {
            Notifications.Clear();
        }


    }

    public static class NotificationMessageExtentions
    {
        internal static string ToJS(this NotificationMessage message)
        {
            return @"
$.notify({{
	message: ""{0}"", icon: ""glyphicon glyphicon-{2}-sign"" 
}},{{
	type: '{1}'
}});".FormatWith(
message.Message.Replace("'", @"\'"),
CategoryToHtmlString(message.Category),
CategoryToBootstrapIconName(message.Category)
);
        }

        internal static string CategoryToHtmlString(NotificationCategory category)
        {
            switch (category)
            {
                case NotificationCategory.Success:
                    return "success";
                case NotificationCategory.Failure:
                    return "danger";
                case NotificationCategory.Warning:
                    return "warning";
                case NotificationCategory.Info:
                    return "info";
                default:
                    return "info";
            }
        }

        internal static string CategoryToBootstrapIconName(NotificationCategory category)
        {
            switch (category)
            {
                case NotificationCategory.Success:
                case NotificationCategory.Info:
                    return "info";
                case NotificationCategory.Failure:
                case NotificationCategory.Warning:
                    return "warning";
               
                default:
                    return "warning";
            }
        }
    }




}
