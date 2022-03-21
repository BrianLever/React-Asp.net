using System;
using System.Data;
using System.Linq;

namespace FrontDesk.Server.Reports.ExcelReports
{
    public class ScreeningResultsExcelReportPart : ExcelReportPartDataTableBase
    {
        public ScreeningResultsExcelReportPart(BhsReportExcelReport parentReport) : base(parentReport, "screenings")
        {
        }

        protected override void InitTableRowDefinition(DataTable model)
        {
            var rowDefinition = Row;

            var excludedColumns = new[] { "Primary Drug", "Secondary Drug", "Tertiary Drug", "Drug Use - ScreeningDate", "DemographicsId" };
            foreach (DataColumn column in model.Columns)
            {

                if (excludedColumns.Contains(column.ColumnName)) continue;

                DataType dataType = DataType.Text;

                if(column.DataType == typeof(int) 
                    || column.DataType == typeof(long)
                    || column.DataType == typeof(decimal)
                    || column.DataType == typeof(double)
                    )
                {
                    dataType = DataType.Number;
                }
                else if (column.DataType == typeof(DateTimeOffset)
                    || column.DataType == typeof(DateTime)
                    )
                {
                    dataType = DataType.Date;
                }

                rowDefinition = rowDefinition
                    .Add(column.ColumnName, x => x[column], dataType);
            }
        }
    }
}
