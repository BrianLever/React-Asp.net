using System;
using System.Data;

namespace FrontDesk.Server.Reports.ExcelReports
{
    public class DrugsOfChoiceResultsExcelReportPart : ExcelReportPartDataTableBase
    {
        public DrugsOfChoiceResultsExcelReportPart(BhsReportExcelReport parentReport) : base(parentReport, "drug use")
        {
        }

        protected override void InitTableRowDefinition(DataTable model)
        {

            var rowDefinition = Row;
            foreach (DataColumn column in model.Columns)
            {
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
