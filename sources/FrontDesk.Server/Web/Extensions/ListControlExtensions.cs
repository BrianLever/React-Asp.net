using FrontDesk.Common;
using FrontDesk.Server.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace FrontDesk.Server.Web.Extensions
{
    public static class ListCtrlExtensions
    {
        public static string SetValueOrDefault(this ListControl ctrl, int value)
        {
            return ctrl.SetValueOrDefault(value.ToString(CultureInfo.InvariantCulture));
        }

        public static string SetValueOrDefault<TModel, TProperty>(this ListControl ctrl, TModel model, Func<TModel, TProperty> propertyLocator)
        where TModel : class
        {
            return model != null ? ctrl.SetValueOrDefault<TProperty>(propertyLocator(model)) : ctrl.SetValueOrDefault(string.Empty);
        }



        public static string SetValueOrDefault(this ListControl ctrl, string value)
        {
            var item = ctrl.Items.FindByValue(value);
            if (item != null)
            {
                ctrl.SelectedIndex = ctrl.Items.IndexOf(item);
            }
            return ctrl.SelectedValue;
        }

        public static string SetValueOrDefault(this ListControl ctrl, long? value)
        {
            return ctrl.SetValueOrDefault(value.HasValue
                ? value.Value.ToString(CultureInfo.InvariantCulture)
                : String.Empty);
        }

        public static string SetValueOrDefault<T>(this ListControl ctrl, T value)
        {
            return ctrl.SetValueOrDefault(value.ToString());
        }

        public static string AppendSelectedItemIfNotExists<TModel>(this ListControl ctrl, TModel model,
            Func<TModel, string> idPropertyLocator, Func<TModel, string> textPropertyLocator)
        {
            if (model == null) return string.Empty;
            return ctrl.AppendSelectedItemIfNotExists(idPropertyLocator(model), textPropertyLocator(model));
        }

        public static string AppendSelectedItemIfNotExists(this ListControl ctrl, string id, string text)
        {

            var value = id ?? string.Empty;
            //if text is empty, do nothing, the value is not set
            if (string.IsNullOrEmpty(text)) return string.Empty;

            if (ctrl.SetValueOrDefault(id) == value) return value;



            var insertPosition = 0;
            if (ctrl.Items.Count > 1 && string.IsNullOrEmpty(ctrl.Items[0].Value))
            {
                insertPosition++;
            }
            ctrl.Items.Insert(insertPosition, new ListItem(text, value));
            ctrl.SelectedIndex = insertPosition;
            return value;
        }


        /// <summary>
        /// Get text from selected value or string.empty if not selected
        /// </summary>
        public static string GetSelectedItemText(this ListControl listCtrl)
        {
            return listCtrl.SelectedItem != null ? listCtrl.SelectedItem.Text : string.Empty;
        }


        /// <summary>
        ///     Add default item to the top of the list if not exists
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="emptyValue"></param>
        public static void AddDefaultNotSelectedItem(this ListControl ctrl, string emptyValue = "")
        {
            ctrl.AddDefaultNotSelectedItem(TextMessages.DropDown_NotSelectedText, emptyValue);
        }

        public static void AddDefaultNotSelectedItem(this ListControl ctrl, string emptyLabel, string emptyValue)
        {
            if (ctrl.Items.FindByValue(emptyValue) == null)
            {
                ctrl.Items.Insert(0, new ListItem(emptyLabel, emptyValue));
            }
        }


        /// <summary>
        /// Select the first item that has the item text starts with certain value
        /// </summary>
        public static string SetTestStartsWithOrDefault(this ListControl listCtrl, string textStartsWith)
        {
            var selectedItem = (
                from ListItem item in listCtrl.Items
                let text = item.Text ?? string.Empty
                where text.StartsWith(textStartsWith)
                select item).FirstOrDefault();

            if (selectedItem != null)
            {
                listCtrl.SelectedIndex = listCtrl.Items.IndexOf(selectedItem);
                return selectedItem.Value;
            }
            return listCtrl.SelectedValue;
        }


        public static string SetValues(this ListControl ctrl, IEnumerable<LookupValue> values)
        {
            bool found = false;
            foreach (ListItem item in ctrl.Items)
            {
                item.Selected = false;
            }

            foreach (LookupValue val in values)
            {
                var item = ctrl.Items.FindByValue(Convert.ToString(val.Id));
                if (item != null)
                {
                    item.Selected = true;
                    found = true;
                }
            }

            if (!found)
            {
                //select None

            }

            return ctrl.SelectedValue;
        }
    }
}