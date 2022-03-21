using System;

namespace FrontDesk.Server.Web.Notification
{
    public struct NotificationMessage
    {
        public NotificationCategory Category;
        public string Message;

        public NotificationMessage(string message, NotificationCategory category)
        {
            Message = message;
            Category = category;
        }
        
    }

}
