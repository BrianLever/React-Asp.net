using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontDesk.Server.Reports.ExcelReports
{
    public class TableRowDefinition<TModel> where TModel : class
    {

        public List<TableColumnDefinition<TModel, object>> Columns { get; protected set; } = new List<TableColumnDefinition<TModel, object>>();

        public TableRowDefinition<TModel> Add(string title, Func<TModel, object> getter, HorizontalCellAllignment alignment = HorizontalCellAllignment.Left, float width = 0, DataType type = DataType.Text)
        {
            Columns.Add(new TableColumnDefinition<TModel, object>(title, getter, alignment, width, type));

            return this;
        }

        public TableRowDefinition<TModel> Add(string title, Func<TModel, object> getter, DataType type)
        {
            Columns.Add(new TableColumnDefinition<TModel, object>(title, getter, HorizontalCellAllignment.Left, 0, type));

            return this;
        }

    }
}
