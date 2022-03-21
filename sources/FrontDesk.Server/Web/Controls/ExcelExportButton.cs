using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;


namespace FrontDesk.Server.Web.Controls
{
    /// <summary>
    /// Export dataview to excel
    /// </summary>
    /// <remarks>
    /// Usage:
    /// <![CDATA[
    /// .cs file
    ///     btnExportErrors.Visible = true;
    ///     btnExportErrors.ExportType = ExcelExportType.XML;
    ///    btnExportErrors.BoundedView = new DataView(dsData.Tables[0]);
    /// 
    /// .aspx
    /// <cc:ExcelExportButton ID="btnExportErrors" runat="server" Width="250px" Text="Export Records with errors to Excel"
    //                Visible="False" CausesValidation="False" UseSubmitBehavior="False" CssClass="button" />
    /// ]]>
    /// 
    /// </remarks>
    public class ExcelExportButton : Button
    {

        public ExcelExportButton()
        {
            _ExportType = ExcelExportType.XML;
            Text = "Export to Excel";
            Width = Unit.Pixel(150);
            lIgnoredColumnsList = new List<string>();
            HideWhenEmpty = true;
            FileName = "ExportData";
        }
        /// <summary>
        /// DataView
        /// </summary>
        private DataView _oView;
        public DataView BoundedView
        {
            get { return _oView; }
            set { _oView = value; }
        }

        public event EventHandler BeforeExport; // event for data binding before export

        private ExcelExportType _ExportType;
        /// <summary>
        /// Export type (CSV, XML)
        /// </summary>
        [Category("Data")]
        public ExcelExportType ExportType
        {
            get { return _ExportType; }
            set { _ExportType = value; }
        }

        List<String> lIgnoredColumnsList;
        public string[] IgnoredColumnsList
        {
            set
            {
                lIgnoredColumnsList = new List<string>(value);
            }
        }

        public bool HideWhenEmpty { get; set; }

        public string FileName { get; set; }

        protected override void OnClick(EventArgs e)
        {

            BeforeExport?.Invoke(this, new EventArgs());

            base.OnClick(e);

            if (_oView == null)
            {
                return;
            }

            System.Web.HttpResponse oResponse = System.Web.HttpContext.Current.Response;
            oResponse.Clear();

            MemoryStream memStream = new MemoryStream();
            ExcelExport exporter = new ExcelExport(this.BoundedView, this.lIgnoredColumnsList);

            //switch (this.ExportType)
            //{
            //    case ExcelExportType.XML:
            //        oResponse.AddHeader("Content-Disposition", "attachment;filename=ExportData.xml");
                    
            //        break;
            //    case ExcelExportType.CSV:
            //        oResponse.AddHeader("Content-Disposition", "attachment;filename=ExportData.csv");
            //        break;
            //}
            
            exporter.Export(memStream, this.ExportType);

            oResponse.ContentType = "application/ms-excel";
            oResponse.AddHeader("Content-Disposition", string.Format(
                "attachment;filename=\"{0}.{1}\"", FileName, this.ExportType.ToString().ToLower()));
            oResponse.AddHeader("Content-Length", memStream.Length.ToString());
            oResponse.BinaryWrite(memStream.ToArray());
            memStream.Close();
            oResponse.End();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if ((_oView == null || _oView.Count == 0) && HideWhenEmpty &&  BeforeExport == null)
            {
                this.Visible = false;
            }
        }
    }
}
