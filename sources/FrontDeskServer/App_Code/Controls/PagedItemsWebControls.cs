using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FrontDesk.Server.Web.Controls;
using System.Web.UI.WebControls;
using FrontDesk.Server.Web;
using RPMS.Common.Models;
using Newtonsoft.Json;
using FrontDesk.Common.Extensions;

namespace Controls
{
    public abstract class PagedItemsUserControl : BaseUserControl
    {
        #region Properties
        public bool AllowDeselect
        {
            get
            {
                if (ViewState["AllowDeselect"] == null)
                {
                    ViewState["AllowDeselect"] = true;
                }
                return (bool)ViewState["AllowDeselect"];
            }
            set
            {
                ViewState["AllowDeselect"] = value;
            }
        }

        public int? SelectedItemID
        {
            get
            {
                return GetNullableIDValue<int>("SelectedItemID", PageIDSource.ViewState);
            }
            set
            {
                ViewState["SelectedItemID"] = value;
            }
        }

       

        public virtual string SelectItemButtonText
        {
            get { return "Select"; }

        }

        protected int StartRow
        {
            get
            {
                return GetNullableIDValue<int>("Page", FrontDesk.Server.Web.PageIDSource.ViewState) ?? 0;
            }
            set
            {
                ViewState["Page"] = value;
            }
        }
        protected int RowsCount
        {
            get
            {
                return GetNullableIDValue<int>("RowsCount", FrontDesk.Server.Web.PageIDSource.ViewState) ?? 0;
            }
            set
            {
                ViewState["RowsCount"] = value;
            }
        }


        #endregion

        #region Controls

        protected abstract Repeater ItemsContainer { get; }
        protected abstract Pager PagetControl { get; }
        #endregion

        protected abstract void BindListData();

        public override void ApplyTabIndexToControl(ref short startTabIndex)
        {

        }

        #region Paging


        protected void ccPaging_Navigate(object sender, FrontDesk.Server.Web.Controls.PagerEventArgs e)
        {
            StartRow = e.SelectedPage == 0 ? e.SelectedPage : e.SelectedPage * PagetControl.PageSize;
            BindListData();
        }

        private string[] ParseCommandArgs(string commandArgument)
        {
            return commandArgument.Split(new[] { '|' });
        }


        protected virtual void OnItemSelected(string[] commandArgs) { }

        protected void btnSelect_click(object sender, EventArgs e)
        {
            var btnSelect = sender as Button;

            var commandArgs = ParseCommandArgs(btnSelect.CommandArgument);


            SelectedItemID = Convert.ToInt32(commandArgs[0]);
            OnItemSelected(commandArgs);
            foreach (RepeaterItem item in ItemsContainer.Items)
            {
                Button btn = item.FindControl("btnSelect") as Button;
                Image imgApplyIcon = (Image)item.FindControl("imgApplyIcon");

                var id =  ParseCommandArgs(btn.CommandArgument)[0].As<int>();

                if (id == SelectedItemID)
                {
                    imgApplyIcon.Visible = !imgApplyIcon.Visible;

                    if (AllowDeselect)
                    {
                        btn.Text = imgApplyIcon.Visible ? "Deselect" : SelectItemButtonText;
                    }
                }
                else
                {
                    imgApplyIcon.Visible = false;
                    btn.Text = SelectItemButtonText;
                }
            }

            PagetControl.RowsCount = RowsCount;
        }

        #endregion

        protected void RenderErrorMessage()
        {
            RenderErrorMessage(null);
        }

        protected void RenderErrorMessage(string message)
        {
            this.Controls.Add(new Label
            {
                Text = message ?? "Cannot render the list because of internal server issue",
                CssClass = "error"
            });
        }

    }
}