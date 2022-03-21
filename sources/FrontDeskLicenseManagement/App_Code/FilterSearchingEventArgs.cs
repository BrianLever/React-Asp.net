using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// FilterSearchingEventArgs
/// </summary>
public class FilterSearchingEventArgs : EventArgs
{

    #region Properties
    /// <summary>
    /// Filter condition
    /// </summary>
    public string FilterBy;
    /// <summary>
    /// Filter value
    /// </summary>
    public object Value;

    #endregion
    /// <summary>
    /// default parametreless contructor
    /// </summary>
    public FilterSearchingEventArgs()
    {

    }

    public FilterSearchingEventArgs(string filterBy, object filterValue)
    {
        this.FilterBy = filterBy;
        this.Value = filterValue;
    }
}
