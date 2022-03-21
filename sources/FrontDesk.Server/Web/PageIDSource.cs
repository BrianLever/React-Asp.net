using System;

namespace FrontDesk.Server.Web
{
    /// <summary>
    /// The source if the searched collection
    /// </summary>
    [Flags]
    public enum PageIDSource
    {
        None = 0,
        QueryString = 1,
        Request = 2,
        Session = 4,
        ViewState = 8,
        Form = 16,
        /// <summary>
        /// Search in all sources
        /// </summary>
        Any = QueryString | Request | Session | ViewState | Form,
        /// <summary>
        /// Search in source that couldn't be modified by user
        /// </summary>
        NonUserInteractive = Session | ViewState,
        /// <summary>
        /// Search in source that could be modified by user
        /// </summary>
        UserIteractive = QueryString | Request | Form
    }

}