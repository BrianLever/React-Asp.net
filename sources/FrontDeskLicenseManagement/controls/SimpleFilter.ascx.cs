using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
//using FrontDesk.Server.Web.Controls;
using FrontDesk.Server.LicenseManagerWeb.Controls;

[ParseChildren(true, "FilterByValues")]
[PersistChildren(false)]
public partial class SimpleFilterCtrl : LMBaseUserControl
{
    #region Design Properties
    /// <summary>
    /// Get session unique key for preserving search values
    /// </summary>
    public string SessionKey { get { return this.UniqueID + _instanceUniqueKey; } }


    private string _instanceUniqueKey = string.Empty;
    /// <summary>
    /// Set application unique key when RestoreOnRedirect is true.
    /// </summary>
    /// <remarks>You can ignore this value if you have unique IDs for all your controls and pages through your application.</remarks>
    [Description("Set application unique key when RestoreOnRedirect is true"),
    Category("Data")]
    public string UniqueKey
    {
        get { return _instanceUniqueKey; }
        set { _instanceUniqueKey = value; }
    }

    private bool _restoreOnRedirect = false;
    /// <summary>
    /// Set true if you want to restore last used filter value when user returns to the page. Value is stored in the Session collection
    /// </summary>
    /// <remarks>False by default.</remarks>
    [Description("Set true if you want to restore last used filter value when user returns to the page. Value is stored in the Session collection"),
   Category("Data")]
    public bool RestoreOnRedirect
    {
        get { return _restoreOnRedirect; }
        set { _restoreOnRedirect = value; }
    }


    #endregion

    #region Filter Properties

    public string _filterByDefaultValue = string.Empty;

    /// <summary>
    /// Default filter value
    /// </summary>
    [Browsable(true)]
    public string FilterByDefaultValue
    {
        get { return _filterByDefaultValue; }
        set
        {
            _filterByDefaultValue = value;
        }
    }

    [Browsable(true)]
    public ICollection<string> FilterByOptions { get; set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
    public string FilterByValues
    {
        get
        {
            if (FilterByOptions != null)
            {
                return string.Join(",", FilterByOptions.ToArray());
            }
            else return null;
        }
        set
        {
            FilterByOptions = new List<string>(value.Split(new char[] { ',', ';' }));
        }
    }



    private string _filterByLabel = "Filter";
    /// <summary>
    /// Default filter label if FilterBy property is not set
    /// </summary>
    [Browsable(true)]
    [DefaultValue("Filter")]
    public string FilterByLabel { get { return _filterByLabel; } set { _filterByLabel = value; } }


    public string Value { get; set; }

    public string FilterBy { get; set; }


    #endregion

    #region Public Controls

    public Button SearchButton { get { return this.btnSearch; } }

    public WebControl DefaultFocusField
    {
        get
        {

            //TODO: return drop-down or textbox depending on filter value
            return null;

        }
    }


    #endregion

    #region Binding

    private bool isBound = false;

    private void EnsureDataBound()
    {
        if (!isBound)
        {
            BindFilterByList();
        }
    }
    /// <summary>
    /// Bind Filter By list or show Filter by label
    /// </summary>
    protected void BindFilterByList()
    {
        if (FilterByOptions != null && FilterByOptions.Count > 0)
        {
            lblFilter.Visible = false;
            ddlFilter.Visible = true;

            //show options for filtering
            ddlFilter.DataSource = FilterByOptions;
            ddlFilter.DataBind();
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(_filterByDefaultValue))
                {
                    var selectedItem = ddlFilter.Items.FindByText(_filterByDefaultValue);
                    if (selectedItem != null)
                    {
                        ddlFilter.SelectedValue = selectedItem.Value;
                    }
                }
            }
            else
            {
                var selectedValue = Request.Form[ddlFilter.UniqueID];
                var selectedItem = ddlFilter.Items.FindByText(selectedValue);
                if (selectedItem != null)
                {
                    ddlFilter.SelectedValue = selectedItem.Value;
                }

            }
        }
        else
        {
            lblFilter.Visible = true;
            ddlFilter.Visible = false;
            lblFilter.Text = FilterByLabel;
        }
    }

    #endregion

    #region Events

    public event EventHandler<FilterSearchingEventArgs> Searching;

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        txtValue.Attributes.Add("onkeydown", string.Format("return onSearchFieldKeyPressed(this, event, '{0}');", btnSearch.ClientID));
        if (!IsPostBack)
        {

            SetControlValue();

        }
        else
        {
            GetControlValue();
        }
        if (RestoreOnRedirect)
        {
            Search_Click(sender, e);
        }
        if (ScriptManager.GetCurrent(this.Page) != null)
        {
            ScriptManager.GetCurrent(this.Page).RegisterAsyncPostBackControl(btnSearch);
            ScriptManager.GetCurrent(this.Page).RegisterAsyncPostBackControl(btnClear);

        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        EnsureDataBound();

        //GetControlValue();
        if (string.IsNullOrEmpty(this.Value))
        {
            //hide clear button
            Page.CallAjaxStartupScript(string.Format("$get('{0}').style.visibility='hidden';", btnClear.ClientID), "hide_clear_button");
        }
        else
        {
            Page.CallAjaxScript(string.Format("$get('{0}').style.visibility='visible';", btnClear.ClientID), "show_clear_button");
        }

        base.OnPreRender(e);
    }


    protected void Search_Click(object sender, EventArgs e)
    {
        //GetControlValue();

        OnSearching(this, new FilterSearchingEventArgs(this.FilterBy, this.Value));

        SetControlValue();

    }

    protected void Clear_Click(object sender, EventArgs e)
    {
        this.Value = string.Empty;
        this.FilterBy = this.FilterByDefaultValue;

        SetControlValue();
        Page.AddAjaxScriptStatement(Page.GetControlClearStateScript(txtValue.ClientID, FrontDesk.Server.LicenseManagerWeb.HTMLControlType.Textbox, ""));

        OnSearching(this, new FilterSearchingEventArgs(this.FilterBy, this.Value));
    }

    [Obfuscation(Feature = "renaming", Exclude = true)]
    protected virtual void OnSearching(object sender, FilterSearchingEventArgs args)
    {
        if (Searching != null)
        {
            Searching(this, args);
        }
    }

    #endregion


    private void GetControlValue()
    {
        EnsureDataBound();
        if (ddlFilter.Visible)
        {
            if (IsPostBack)
            {
                this.FilterBy = Request.Form[ddlFilter.UniqueID];
                this.Value = Request.Form[txtValue.UniqueID];
            }
            else if (RestoreOnRedirect)
            {
                this.FilterBy = Convert.ToString(Session[SessionKey + "_filterBy"]);
                this.Value = Convert.ToString(Session[SessionKey + "_value"]);
            }
        }
        else
        {
            this.FilterBy = string.Empty;
        }

        if (IsPostBack)
        {
            this.Value = Request.Form[txtValue.UniqueID];
        }
        else if (RestoreOnRedirect)
        {
            this.Value = Convert.ToString(Session[SessionKey + "_value"]);
        }
    }

    private void SetControlValue()
    {
        if (RestoreOnRedirect)
        {
            Session[SessionKey + "_filterBy"] = this.FilterBy;
            Session[SessionKey + "_value"] = this.Value;
        }

        EnsureDataBound();
        txtValue.Text = this.Value;
        if (ddlFilter.Visible && !string.IsNullOrEmpty(FilterBy))
        {
            var item = ddlFilter.Items.FindByValue(FilterBy);
            if (item != null) ddlFilter.SelectedValue = item.Value;
        }
    }

    public override void ApplyTabIndexToControl(ref short startTabIndex)
    {
        EnsureDataBound();
        if (ddlFilter.Visible) ddlFilter.TabIndex = startTabIndex++;
        txtValue.TabIndex = startTabIndex++;
        btnSearch.TabIndex = startTabIndex++;
        btnClear.TabIndex = startTabIndex++;
    }
}
    //[ParseChildren(true, "FilterByValues")]
    //[PersistChildren(false)]
    //public partial class SimpleFilterCtrl : LMBaseUserControl
    //{
    //    #region Design Properties
    //    /// <summary>
    //    /// Get session unique key for preserving search values
    //    /// </summary>
    //    public string SessionKey { get { return this.UniqueID + _instanceUniqueKey; } }


    //    private string _instanceUniqueKey = string.Empty;
    //    /// <summary>
    //    /// Set application unique key when RestoreOnRedirect is true.
    //    /// </summary>
    //    /// <remarks>You can ignore this value if you have unique IDs for all your controls and pages through your application.</remarks>
    //    [Description("Set application unique key when RestoreOnRedirect is true"),
    //    Category("Data")]
    //    public string UniqueKey
    //    {
    //        get { return _instanceUniqueKey; }
    //        set { _instanceUniqueKey = value; }
    //    }

    //    private bool _restoreOnRedirect = false;
    //    /// <summary>
    //    /// Set true if you want to restore last used filter value when user returns to the page. Value is stored in the Session collection
    //    /// </summary>
    //    /// <remarks>False by default.</remarks>
    //    [Description("Set true if you want to restore last used filter value when user returns to the page. Value is stored in the Session collection"),
    //   Category("Data")]
    //    public bool RestoreOnRedirect
    //    {
    //        get { return _restoreOnRedirect; }
    //        set { _restoreOnRedirect = value; }
    //    }


    //    #endregion

    //    #region Filter Properties

    //    public string _filterByDefaultValue = string.Empty;

    //    /// <summary>
    //    /// Default filter value
    //    /// </summary>
    //    [Browsable(true)]
    //    public string FilterByDefaultValue
    //    {
    //        get { return _filterByDefaultValue; }
    //        set
    //        {
    //            _filterByDefaultValue = value;
    //        }
    //    }

    //    [Browsable(true)]
    //    public ICollection<string> FilterByOptions { get; set; }

    //    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    //    [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
    //    public string FilterByValues
    //    {
    //        get
    //        {
    //            if (FilterByOptions != null)
    //            {
    //                return string.Join(",", FilterByOptions.ToArray());
    //            }
    //            else return null;
    //        }
    //        set
    //        {
    //            FilterByOptions = new List<string>(value.Split(new char[] { ',', ';' }));
    //        }
    //    }



    //    private string _filterByLabel = "Filter";
    //    /// <summary>
    //    /// Default filter label if FilterBy property is not set
    //    /// </summary>
    //    [Browsable(true)]
    //    [DefaultValue("Filter")]
    //    public string FilterByLabel { get { return _filterByLabel; } set { _filterByLabel = value; } }


    //    public string Value { get; set; }

    //    public string FilterBy { get; set; }


    //    #endregion

    //    #region Public Controls

    //    public Button SearchButton { get { return this.btnSearch; } }

    //    public WebControl DefaultFocusField
    //    {
    //        get
    //        {

    //            //TODO: return drop-down or textbox depending on filter value
    //            return null;

    //        }
    //    }


    //    #endregion

    //    #region Binding

    //    private bool isBound = false;

    //    private void EnsureDataBound()
    //    {
    //        if (!isBound)
    //        {
    //            BindFilterByList();
    //        }
    //    }
    //    /// <summary>
    //    /// Bind Filter By list or show Filter by label
    //    /// </summary>
    //    protected void BindFilterByList()
    //    {
    //        if (FilterByOptions != null && FilterByOptions.Count > 0)
    //        {
    //            lblFilter.Visible = false;
    //            ddlFilter.Visible = true;

    //            //show options for filtering
    //            ddlFilter.DataSource = FilterByOptions;
    //            ddlFilter.DataBind();
    //            if (!IsPostBack)
    //            {
    //                if (!string.IsNullOrEmpty(_filterByDefaultValue))
    //                {
    //                    var selectedItem = ddlFilter.Items.FindByText(_filterByDefaultValue);
    //                    if (selectedItem != null)
    //                    {
    //                        ddlFilter.SelectedValue = selectedItem.Value;
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                var selectedValue = Request.Form[ddlFilter.UniqueID];
    //                var selectedItem = ddlFilter.Items.FindByText(selectedValue);
    //                if (selectedItem != null)
    //                {
    //                    ddlFilter.SelectedValue = selectedItem.Value;
    //                }

    //            }
    //        }
    //        else
    //        {
    //            lblFilter.Visible = true;
    //            ddlFilter.Visible = false;
    //            lblFilter.Text = FilterByLabel;
    //        }
    //    }

    //    #endregion

    //    #region Events

    //    public event EventHandler<FilterSearchingEventArgs> Searching;

    //    #endregion

    //    #region Page Events

    //    protected void Page_Load(object sender, EventArgs e)
    //    {
    //        txtValue.Attributes.Add("onkeydown", string.Format("return onSearchFieldKeyPressed(this, event, '{0}');", btnSearch.ClientID));
    //        if (!IsPostBack)
    //        {

    //            SetControlValue();

    //        }
    //        if (RestoreOnRedirect)
    //        {
    //            Search_Click(sender, e);
    //        }
    //        if (ScriptManager.GetCurrent(this.Page) != null)
    //        {
    //            ScriptManager.GetCurrent(this.Page).RegisterAsyncPostBackControl(btnSearch);
    //            ScriptManager.GetCurrent(this.Page).RegisterAsyncPostBackControl(btnClear);

    //        }
    //    }

    //    protected override void OnPreRender(EventArgs e)
    //    {
    //        EnsureDataBound();

    //        //GetControlValue();
    //        if (string.IsNullOrEmpty(this.Value))
    //        {
    //            //hide clear button
    //            Page.CallAjaxStartupScript(string.Format("$get('{0}').style.visibility='hidden';", btnClear.ClientID), "hide_clear_button");
    //        }
    //        else
    //        {
    //            Page.CallAjaxScript(string.Format("$get('{0}').style.visibility='visible';", btnClear.ClientID), "show_clear_button");
    //        }

    //        base.OnPreRender(e);
    //    }


    //    protected void Search_Click(object sender, EventArgs e)
    //    {
    //        GetControlValue();

    //        OnSearching(this, new FilterSearchingEventArgs(this.FilterBy, this.Value));

    //        SetControlValue();

    //    }

    //    protected void Clear_Click(object sender, EventArgs e)
    //    {
    //        this.Value = string.Empty;
    //        this.FilterBy = this.FilterByDefaultValue;

    //        SetControlValue();
    //        Page.AddAjaxScriptStatement(Page.GetControlClearStateScript(txtValue.ClientID, FrontDesk.Server.LicenseManagerWeb.HTMLControlType.Textbox, ""));

    //        OnSearching(this, new FilterSearchingEventArgs(this.FilterBy, this.Value));
    //    }

    //    [Obfuscation(Feature = "renaming", Exclude = true)]
    //    protected virtual void OnSearching(object sender, FilterSearchingEventArgs args)
    //    {
    //        if (Searching != null)
    //        {
    //            Searching(this, args);
    //        }
    //    }

    //    #endregion


    //    private void GetControlValue()
    //    {
    //        EnsureDataBound();
    //        if (ddlFilter.Visible)
    //        {
    //            if (IsPostBack)
    //            {
    //                this.FilterBy = Request.Form[ddlFilter.UniqueID];
    //                this.Value = Request.Form[txtValue.UniqueID];
    //            }
    //            else if (RestoreOnRedirect)
    //            {
    //                this.FilterBy = Convert.ToString(Session[SessionKey + "_filterBy"]);
    //                this.Value = Convert.ToString(Session[SessionKey + "_value"]);
    //            }
    //        }
    //        else
    //        {
    //            this.FilterBy = string.Empty;
    //        }

    //        if (IsPostBack)
    //        {
    //            this.Value = Request.Form[txtValue.UniqueID];
    //        }
    //        else if (RestoreOnRedirect)
    //        {
    //            this.Value = Convert.ToString(Session[SessionKey + "_value"]);
    //        }
    //    }

    //    private void SetControlValue()
    //    {
    //        if (RestoreOnRedirect)
    //        {
    //            Session[SessionKey + "_filterBy"] = this.FilterBy;
    //            Session[SessionKey + "_value"] = this.Value;
    //        }

    //        EnsureDataBound();
    //        txtValue.Text = this.Value;
    //        if (ddlFilter.Visible && !string.IsNullOrEmpty(FilterBy))
    //        {
    //            var item = ddlFilter.Items.FindByValue(FilterBy);
    //            if (item != null) ddlFilter.SelectedValue = item.Value;
    //        }
    //    }

    //    public override void ApplyTabIndexToControl(ref short startTabIndex)
    //    {
    //        EnsureDataBound();
    //        if (ddlFilter.Visible) ddlFilter.TabIndex = startTabIndex++;
    //        txtValue.TabIndex = startTabIndex++;
    //        btnSearch.TabIndex = startTabIndex++;
    //        btnClear.TabIndex = startTabIndex++;
    //    }
        
    //}
