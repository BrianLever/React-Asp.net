using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;

namespace FrontDesk.Server.Web.Controls
{
    public enum ExcelExportType
    {
        CSV,
        XML
    }

    /// <summary>
    /// Exports DataView to Office Excel XML format, or CSV format.
    /// </summary>
    public class ExcelExport
    {
        private DataView dataView;
        private List<String> ignoredColumns = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelExport"/> class.
        /// </summary>
        /// <param name="dataView">The data view.</param>
        /// <param name="ignoredColumns">The ignored columns.</param>
        public ExcelExport(DataView dataView, List<String> ignoredColumns)
        {
            this.dataView = dataView;
            if (ignoredColumns != null)
            {
                this.ignoredColumns = ignoredColumns;
            }
        }

        /// <summary>
        /// Exports data to the specified output stream, in desired format.
        /// </summary>
        /// <param name="outputStream">The output stream.</param>
        /// <param name="exportType">Type of the export.</param>
        public void Export(Stream outputStream, ExcelExportType exportType)
        {
            switch (exportType)
            {
                //case ExcelExportType.CSV:
                //    StringBuilder sb = new StringBuilder();
                //    this.ExportToCsv(sb);
                //    byte[] csvBytes = Encoding.UTF8.GetBytes(sb.ToString());
                //    // TODO: do we need 3-byte unicode mark here?
                //    outputStream.Write(csvBytes, 0, csvBytes.Length);
                //    break;
                case ExcelExportType.XML:
                    // do not embrace writer in "using": will get closed stream
                    XmlTextWriter wrXml = new XmlTextWriter(outputStream, Encoding.Unicode);
                    this.ExportToXml(wrXml);
                    wrXml.Flush();
                    break;
            }
        }

        /// <summary>
        /// Exports data to XML.
        /// </summary>
        /// <param name="wrXml">The XML text writer instance.</param>
        private void ExportToXml(XmlTextWriter wrXml)
        {
            wrXml.WriteProcessingInstruction("xml", "version='1.0'");
            wrXml.WriteProcessingInstruction("mso-application", "progid=\"Excel.Sheet\"");
            wrXml.WriteStartElement("Workbook");
            wrXml.WriteAttributeString("xmlns", "urn:schemas-microsoft-com:office:spreadsheet");
            wrXml.WriteAttributeString("xmlns:o", "urn:schemas-microsoft-com:office:office");
            wrXml.WriteAttributeString("xmlns:x", "urn:schemas-microsoft-com:office:excel");
            wrXml.WriteAttributeString("xmlns:ss", "urn:schemas-microsoft-com:office:spreadsheet");
            wrXml.WriteAttributeString("xmlns:html", "http://www.w3.org/TR/REC-html40");
            wrXml.WriteAttributeString("xmlns:x2", "http://schemas.microsoft.com/office/excel/2003/xml");

            wrXml.WriteStartElement("ExcelWorkbook", "urn:schemas-microsoft-com:office:excel");
            wrXml.WriteEndElement();

            wrXml.WriteStartElement("Worksheet");
            wrXml.WriteAttributeString("ss:Name", "Page1");

            wrXml.WriteStartElement("Table");
            ///Write datatable data
            if (this.dataView.Count > 0)
            {
                wrXml.WriteStartElement("Row");
                foreach (DataColumn oCol in this.dataView.Table.Columns)
                {
                    if (!this.ignoredColumns.Contains(oCol.ColumnName))
                    {
                        wrXml.WriteStartElement("Cell");
                        wrXml.WriteStartElement("Data");
                        wrXml.WriteAttributeString("ss:Type", "String");
                        //wrXml.WriteEndAttribute();
                        wrXml.WriteString(oCol.Caption);
                        wrXml.WriteFullEndElement(); //Data
                        wrXml.WriteEndElement(); //Cell
                    }
                }

                wrXml.WriteEndElement(); //Row

                foreach (DataRowView oRow in this.dataView)
                {
                    wrXml.WriteStartElement("Row");
                    for (int i = 0; i < this.dataView.Table.Columns.Count; i++)
                    {
                        if (!this.ignoredColumns.Contains(this.dataView.Table.Columns[i].ColumnName))
                        {
                            wrXml.WriteStartElement("Cell");
                            wrXml.WriteStartElement("Data");
                            wrXml.WriteAttributeString("ss:Type", "String");
                            //wrXml.WriteEndAttribute();

                            wrXml.WriteString(Convert.ToString(oRow[i]));
                            wrXml.WriteEndElement(); //Data
                            wrXml.WriteEndElement(); //Cell
                        }
                    }

                    wrXml.WriteEndElement();//Row
                }
            }

            wrXml.WriteEndElement();///Table
            wrXml.WriteEndElement();//Worksheet
            wrXml.WriteEndElement();///Workbook
        }

        ///// <summary>
        ///// Exports data to CSV.
        ///// </summary>
        ///// <param name="sbOutput">The output.</param>
        //[Obsolete("Totally broken, fix it or rewrite")]
        //private void ExportToCsv(StringBuilder sbOutput)
        //{
        //    throw new Exception("ExportToCsv: Totally broken, fix it or rewrite");

        //    string sDelimiter = ",";
        //    string sLineSeparator = "\r\n";
        //    //excelData = new StringBuilder();
        //    if (this.dataView.Count > 0)
        //    {
        //        bool firstColumn = true;
        //        foreach (DataColumn oCol in this.dataView.Table.Columns)
        //        {
        //            if (!this.ignoredColumns.Contains(oCol.ColumnName))
        //            {
        //                if (!firstColumn)
        //                {
        //                    sbOutput.Append(sDelimiter);
        //                }
        //                sbOutput.Append("\"");
        //                sbOutput.Append(oCol.Caption);
        //                sbOutput.Append("\"");
        //                firstColumn = false;
        //            }
        //        }

        //        sbOutput.Append(sLineSeparator);

        //        foreach (DataRowView oRow in this.dataView)
        //        {
        //            firstColumn = true;
        //            for (int i = 0; i < this.dataView.Table.Columns.Count; i++)
        //            {
        //                if (!this.ignoredColumns.Contains(this.dataView.Table.Columns[i].ColumnName))
        //                {
        //                    if (!sDelimiter.Equals(String.Empty))
        //                    {
        //                        sbOutput.Append(sDelimiter);
        //                    }
        //                    sbOutput.Append(oRow[i]);
        //                    if (!sDelimiter.Equals(String.Empty))
        //                    {
        //                        sbOutput.Append(sDelimiter);
        //                    }
        //                    sbOutput.Append(sLineSeparator);
        //                }
        //            }

        //            sbOutput.Length = sbOutput.Length - sLineSeparator.Length;
        //            sbOutput.Append(Environment.NewLine);
        //        }
        //    }
        //}
    }
}
