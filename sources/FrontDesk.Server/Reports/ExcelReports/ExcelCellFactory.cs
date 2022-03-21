using DocumentFormat.OpenXml.Spreadsheet;

using FrontDesk.Common.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontDesk.Server.Reports.ExcelReports
{
    public static class ExcelCellFactory
    {
        public static Cell Create<TModel>(TableColumnDefinition<TModel, object> colDef, TModel model, string cellReference = null)
        {
            switch (colDef.Type)
            {
                case DataType.Text:
                    return CreateTextCell(colDef, model);
                case DataType.Date:

                    return CreateDateCell(colDef, model);
                case DataType.Number:
                    return CreateNumberCell(colDef, model);
            }

            throw new ArgumentException(nameof(colDef));
        }

        public static Cell CreateTextCell<TModel>(TableColumnDefinition<TModel, object> colDef, TModel model, string cellReference = null)
        {
            return CreateTextCell(colDef.Getter(model).As<string>() ?? string.Empty, cellReference);
        }

        private static Cell CreateTextCell(string value, string cellReference)
        {
            InlineString inlineString = new InlineString();

            inlineString.AppendChild(new Text
            {
                Text = value ?? string.Empty
            });

            var cell = new Cell
            {
                CellReference = cellReference,
                DataType = CellValues.InlineString,
            };

            cell.AppendChild(inlineString);

            return cell;
        }

        public static Cell CreateDateCell<TModel>(TableColumnDefinition<TModel, object> colDef, TModel model, string cellReference = null)
        {
            return new Cell
            {
                CellReference = cellReference,
                DataType = CellValues.Date,
                CellValue = new CellValue(colDef.Getter(model).As<DateTimeOffset>())
            };

        }

        public static Cell CreateNumberCell<TModel>(TableColumnDefinition<TModel, object> colDef, TModel model, string cellReference = null)
        {
            var cell = new Cell
            {
                CellReference = cellReference,
                DataType = CellValues.Number,
                CellValue = new CellValue(colDef.Getter(model).As<string>())
            };

            return cell;
        }

    }
}
