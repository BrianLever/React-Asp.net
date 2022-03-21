using System;

namespace FrontDesk.Server.Web.Controls
{
    public abstract class BaseWebFormUserControl<TFormObject, TFormObjectId> : BaseFormUserControl<TFormObject, TFormObjectId>
         where TFormObject : class, new()
         where TFormObjectId : struct, IEquatable<TFormObjectId>
    {

        public new BaseManagementWebForm<TFormObject, TFormObjectId> Page
        {
            get { return (BaseManagementWebForm<TFormObject, TFormObjectId>)(base.Page); }
        }

        /// <summary>
        /// Prepare and bind business data to the page controls in the EDIT/READ modes
        /// </summary>
        protected abstract void EditModeDataPrepare(TFormObject model);

        protected virtual void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                EditModeDataPrepare(Page.CurFormObject);
            }
            SetControlsEnabledState(Page.IsNewInstance);
        }

        protected virtual void SetControlsEnabledState(bool isNewInstance) { }

        private bool _isReadOnly;

        public bool ReadOnly
        {
            get
            {
                return _isReadOnly;
            }
            set
            {
                if (_isReadOnly != value)
                {
                    _isReadOnly = value;
                    OnReadOnlyValueChanged(_isReadOnly);
                }
            }
        }

        protected virtual void OnReadOnlyValueChanged(bool readOnlyValue)
        {

        }
    }



}
