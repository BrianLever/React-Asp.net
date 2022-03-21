using System;

namespace FrontDesk.Server.Web
{
    /// <summary>
    /// Page access permissions
    /// </summary>
    [Flags]
    public enum AccessPermissions
    {
        None = 0x0,
        Read = 0x1,
        Write = 0x2,
        Delete = 0x4,
        Print = 0x8
    }

}