using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontDesk.Server.Reports.ExcelReports
{

    public class TableColumnDefinition<T>
    {
        public string Title { get; set; }
        public Func<T, string> Getter { get; set; }
        public HorizontalCellAllignment Alignment { get; set; }
        public float Width { get; set; }
        public DataType Type { get; set; }


        public TableColumnDefinition(string title, Func<T, string> getter, HorizontalCellAllignment alignment, float width = 0, DataType type = DataType.Text)
        {
            Title = title;
            Getter = getter;
            Alignment = alignment;
            Width = width;
            Type = type;
        }
    }


    public class TableColumnDefinition<TSource, TData>
    {
        public string Title { get; set; }
        public Func<TSource, TData> Getter { get; set; }
        public HorizontalCellAllignment Alignment { get; set; }
        public float Width { get; set; }
        public DataType Type { get; set; }


        public TableColumnDefinition(string title, Func<TSource, TData> getter, HorizontalCellAllignment alignment, float width = 0, DataType type = DataType.Text)
        {
            Title = title;
            Getter = getter;
            Alignment = alignment;
            Width = width;
            Type = type;
        }
    }

    public enum DataType
    {
        Text,
        Number,
        //Price,
        Date
    };
}
