using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Reflection;

namespace FrontDesk.Server.Web.Controls
{
    /// <summary>
    /// The HierarColumn is derived from the DataGridColumn and contains an image with a plus/minus icon 
    /// and a DynamicControlsPlaceholder that takes the dynamically loaded templates
    /// </summary>
    [Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)]
    public class HierarColumn : DataControlField
    {

        Grid.HierarDynamicGridExtender _extender;

        /// <summary>
        /// Initializes a new instance of HierarColumn class.
        /// </summary>
        public HierarColumn()
            : base()
        {
        }

        /// <summary>
        /// On initialization the HierarGridColumn adds a plus image and a DynamicControlsPlaceholder 
        /// that is later filled with the templates
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="cellType"></param>
        /// <param name="rowState"></param>
        /// <param name="rowIndex"></param>
        public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
        {
            string divCssClass = String.Empty;
            base.InitializeCell(cell, cellType, rowState, rowIndex);

            cell.DataBinding += new EventHandler(OnDataBinding);
            if (cellType == DataControlCellType.DataCell)
            {
                switch (rowState)
                {
                    case DataControlRowState.Normal:
                    case DataControlRowState.Alternate:
                    case DataControlRowState.Selected:
                        {
                           
                            AddControls(cell, rowState);
                            break;
                        }
                    case DataControlRowState.Edit:
                        break;
                }
            }
        }

        void OnDataBinding(object sender, EventArgs e)
        {
            if (_extender != null)
            {
                Control control = sender as Control;
                var row = control.NamingContainer as GridViewRow;


                if (row != null)
                {
                    //var gv = row.NamingContainer as GridView;
                    //if (gv != null)
                    //{
                    DataRowView rowView = (DataRowView)row.DataItem;
                    if (rowView != null)
                    {
                        if (!string.IsNullOrEmpty(DataKeyName))
                        {
                            _extender.DataKey = Convert.ToString(rowView[DataKeyName]);
                        }
                    }
                    //}}
                }
            }
        }

        /// <summary>
        /// Adds a plus image and a DynamicControlsPlaceholder to the child collection
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="itemType"></param>
        protected virtual void AddControls(TableCell cell, DataControlRowState itemType)
        {
            Image image = new Image();
            image.ID = "Icon";
            image.CssClass = "datagrid expand icon";
            this._extender = new Grid.HierarDynamicGridExtender();
            _extender.ID = "hdgExt";
            _extender.TargetControlID = image.ID;

         

            var grid = this.Control as HierarDynamicGrid;
            var mainPage = grid.Page as BasePage;
            _extender.CollapsedImageUrl = mainPage.GetVirtualPath(grid.CollapsedImageUrl);
            _extender.ExpandedImageUrl = mainPage.GetVirtualPath(grid.ExpandedImageUrl);


            image.ImageUrl = grid.CollapsedImageUrl;



            //image.Attributes.Add("onclick", "javascript:HierarGrid_toggleRow(this);");
            cell.Controls.Add(image);
            

            DynamicControlsPlaceholder dcp = new DynamicControlsPlaceholder();
            dcp.ID = "DCP";
            cell.Controls.Add(dcp);

            cell.Controls.Add(_extender);
        }





        public string DataKeyName { 
            get { return (string)ViewState["DataKeyName"]; } 
            set { ViewState["DataKeyName"] = value; } }

        protected override DataControlField CreateField()
        {
            return new HierarColumn();
        }




    }
}
