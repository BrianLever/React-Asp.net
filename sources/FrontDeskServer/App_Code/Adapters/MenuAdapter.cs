using FrontDeskServer;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace CSSFriendly
{
    public class MenuAdapter : System.Web.UI.WebControls.Adapters.MenuAdapter
    {
        private WebControlAdapterExtender _extender = null;
        private WebControlAdapterExtender Extender
        {
            get
            {
                if (((_extender == null) && (Control != null)) ||
                        ((_extender != null) && (Control != _extender.AdaptedControl)))
                {
                    _extender = new WebControlAdapterExtender(Control);
                }

                return _extender;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (Extender.AdapterEnabled)
            {
                RegisterScripts();
            }
        }

        private void RegisterScripts()
        {
            Extender.RegisterScripts();

            /* 
             * Modified for support of compiled CSSFriendly assembly
             * 
             * We will first search for embedded JavaScript files. If they are not
             * found, we default to the standard approach.
             */

            Type type = this.GetType();


        }

        protected override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (Extender.AdapterEnabled)
            {
                Extender.RenderBeginTag(writer, "AspNet-Menu-" + Control.Orientation.ToString());
            }
            else
            {
                base.RenderBeginTag(writer);
            }
        }

        protected override void RenderEndTag(HtmlTextWriter writer)
        {
            if (Extender.AdapterEnabled)
            {
                Extender.RenderEndTag(writer);
            }
            else
            {
                base.RenderEndTag(writer);
            }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (Extender.AdapterEnabled)
            {
                writer.Indent++;
                BuildItems(Control.Items, true, writer);
                writer.Indent--;
                writer.WriteLine();
            }
            else
            {
                base.RenderContents(writer);
            }
        }

        private void BuildItems(MenuItemCollection items, bool isRoot, HtmlTextWriter writer)
        {
            if (items.Count > 0)
            {
                writer.WriteLine();

                writer.WriteBeginTag("ul");

                MenuItem selectedParentItem = null;

                if (isRoot)
                {
                    writer.WriteAttribute("class", "AspNet-Menu");
                    selectedParentItem = DetectParentSelectedItems(items);
                }
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Indent++;



                foreach (MenuItem item in items)
                {
                    BuildItem(item, writer, selectedParentItem);
                }

                writer.Indent--;
                writer.WriteLine();
                writer.WriteEndTag("ul");
            }
        }

        private MenuItem DetectParentSelectedItems(MenuItemCollection items)
        {
            var selectedItems = new List<MenuItem>();

            foreach (MenuItem item in items)
            {
                if (item.Depth > 0)
                {
                    continue;
                }

                if (item.Selected)
                {
                    selectedItems.Add(item);
                }
                else if (IsParentItemSelected(item))
                {
                    selectedItems.Add(item);
                }
                else if (IsChildItemSelected(item))
                {
                    selectedItems.Add(item);
                }
            }

            if (selectedItems.Count == 0)
            {
                var urlPath = Page.Request.Path;
                var urlDirectoryName = Path.GetDirectoryName(urlPath);
                var urlFileName = Path.GetFileName(urlPath);

                foreach (MenuItem item in items)
                {
                    //non links selected on the top level, means we need to find the parent item
                    if (item.NavigateUrl.EndsWith("Default.aspx") && string.Compare(item.Text, "Screen", StringComparison.OrdinalIgnoreCase) != 0)
                    {
                        //check by nested folder in the URL
                        var path = Path.GetDirectoryName(item.NavigateUrl);

                        if (path == urlDirectoryName)
                        {
                            return item;
                        }
                    }
                    else
                    {
                        var menuTitle = item.Text.ToLowerInvariant();

                        string[] menuUrls;

                        if (MainMenuItemsDescriptor.MainMenuItems.TryGetValue(menuTitle, out menuUrls))
                        {
                            if (menuUrls.Any(x =>
                            {
                                return String.Compare(urlFileName, x, StringComparison.OrdinalIgnoreCase) == 0;
                            }))
                            {
                                return item;
                            }
                        }

                    }
                }
            }
            return null;
        }

        private void BuildItem(MenuItem item, HtmlTextWriter writer, MenuItem selectedParentItem)
        {
            Menu menu = Control as Menu;
            if ((menu != null) && (item != null) && (writer != null))
            {
                writer.WriteLine();
                writer.WriteBeginTag("li");

                string theClass = (item.ChildItems.Count > 0) ? "AspNet-Menu-WithChildren" : "AspNet-Menu-Leaf";
                string selectedStatusClass = GetSelectStatusClass(item, selectedParentItem);
                if (!String.IsNullOrEmpty(selectedStatusClass))
                {
                    theClass += " " + selectedStatusClass;
                }
                writer.WriteAttribute("class", theClass);

                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Indent++;
                writer.WriteLine();

                var itemCssClass = GetItemClass(menu, item, selectedParentItem);


                if (((item.Depth < menu.StaticDisplayLevels) && (menu.StaticItemTemplate != null)) ||
                        ((item.Depth >= menu.StaticDisplayLevels) && (menu.DynamicItemTemplate != null)))
                {
                    writer.WriteBeginTag("div");
                    writer.WriteAttribute("class", itemCssClass);
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Indent++;
                    writer.WriteLine();

                    MenuItemTemplateContainer container = new MenuItemTemplateContainer(menu.Items.IndexOf(item), item);
                    if ((item.Depth < menu.StaticDisplayLevels) && (menu.StaticItemTemplate != null))
                    {
                        menu.StaticItemTemplate.InstantiateIn(container);
                    }
                    else
                    {
                        menu.DynamicItemTemplate.InstantiateIn(container);
                    }
                    container.DataBind();

                    //3SI2 - Modified to put POSTBACK url on Hyperlinks with Templates
                    //bind url
                    foreach (var ctrl in container.Controls)
                    {
                        if (ctrl is HyperLink && IsLink(item) && String.IsNullOrEmpty(item.NavigateUrl))
                        {
                            var link = ctrl as HyperLink;
                            link.NavigateUrl = Page.ClientScript.GetPostBackClientHyperlink(menu, "b" + item.ValuePath.Replace(menu.PathSeparator.ToString(), "\\"), true);

                        }
                        if (ctrl is HtmlGenericControl)
                        {
                            var img = ctrl as HtmlGenericControl;
                            if (img.TagName == "div")
                            {
                                img.Attributes["class"] = img.Attributes["class"] + " " + item.Value.ToLower().Replace(' ', '_');
                            }
                        }
                    }

                    container.RenderControl(writer);

                    writer.Indent--;
                    writer.WriteLine();
                    writer.WriteEndTag("div");
                }
                else
                {
                    if (IsLink(item))
                    {


                        writer.WriteBeginTag("a");
                        if (!String.IsNullOrEmpty(item.NavigateUrl))
                        {
                            writer.WriteAttribute("href", Page.Server.HtmlEncode(menu.ResolveClientUrl(item.NavigateUrl)));
                        }
                        else
                        {
                            writer.WriteAttribute("href", Page.ClientScript.GetPostBackClientHyperlink(menu, "b" + item.ValuePath.Replace(menu.PathSeparator.ToString(), "\\"), true));
                        }

                        writer.WriteAttribute("class", itemCssClass);
                        WebControlAdapterExtender.WriteTargetAttribute(writer, item.Target);

                        if (!String.IsNullOrEmpty(item.ToolTip))
                        {
                            writer.WriteAttribute("title", item.ToolTip);
                        }
                        else if (!String.IsNullOrEmpty(menu.ToolTip))
                        {
                            writer.WriteAttribute("title", menu.ToolTip);
                        }
                        writer.Write(HtmlTextWriter.TagRightChar);
                        writer.Indent++;
                        writer.WriteLine();
                    }
                    else
                    {
                        writer.WriteBeginTag("span");
                        writer.WriteAttribute("class", itemCssClass);
                        writer.Write(HtmlTextWriter.TagRightChar);
                        writer.Indent++;
                        writer.WriteLine();
                    }

                    if (!String.IsNullOrEmpty(item.ImageUrl))
                    {
                        writer.WriteBeginTag("img");
                        writer.WriteAttribute("src", menu.ResolveClientUrl(item.ImageUrl));
                        writer.WriteAttribute("alt", !String.IsNullOrEmpty(item.ToolTip) ? item.ToolTip : (!String.IsNullOrEmpty(menu.ToolTip) ? menu.ToolTip : item.Text));
                        writer.Write(HtmlTextWriter.SelfClosingTagEnd);
                    }

                    writer.Write(item.Text);

                    if (IsLink(item))
                    {
                        writer.Indent--;
                        writer.WriteEndTag("a");
                    }
                    else
                    {
                        writer.Indent--;
                        writer.WriteEndTag("span");
                    }

                }

                if ((item.ChildItems != null) && (item.ChildItems.Count > 0))
                {
                    BuildItems(item.ChildItems, false, writer);
                }

                writer.Indent--;
                writer.WriteLine();
                writer.WriteEndTag("li");
            }
        }

        private bool IsLink(MenuItem item)
        {
            return (item != null) && item.Enabled && ((!String.IsNullOrEmpty(item.NavigateUrl)) || item.Selectable);
        }

        private string GetItemClass(Menu menu, MenuItem item, MenuItem selectedParentItem)
        {
            string value = "AspNet-Menu-NonLink";
            if (item != null)
            {
                if (((item.Depth < menu.StaticDisplayLevels) && (menu.StaticItemTemplate != null)) ||
                        ((item.Depth >= menu.StaticDisplayLevels) && (menu.DynamicItemTemplate != null)))
                {
                    value = "AspNet-Menu-Template";
                }
                else if (IsLink(item))
                {
                    value = "AspNet-Menu-Link";
                }
                string selectedStatusClass = GetSelectStatusClass(item, selectedParentItem);
                if (!String.IsNullOrEmpty(selectedStatusClass))
                {
                    value += " " + selectedStatusClass;
                }
            }
            return value;
        }

        private string GetSelectStatusClass(MenuItem item, MenuItem selectedParentItem)
        {
            string value = "";
            if (item.Selected)
            {
                value += " AspNet-Menu-Selected";
            }
            else if (IsParentItemSelected(item))
            {
                value += " AspNet-Menu-ParentSelected";
            }
            else if (IsChildItemSelected(item))
            {
                value += " AspNet-Menu-ChildSelected";
            }
            else if (item == selectedParentItem)
            {
                value += " AspNet-Menu-ChildSelected";
            }
            return value;
        }

        private bool IsChildItemSelected(MenuItem item)
        {
            bool bRet = false;

            if ((item != null) && (item.ChildItems != null))
            {
                bRet = IsChildItemSelected(item.ChildItems);
            }

            return bRet;
        }

        private bool IsChildItemSelected(MenuItemCollection items)
        {
            bool bRet = false;

            if (items != null)
            {
                foreach (MenuItem item in items)
                {
                    if (item.Selected || IsChildItemSelected(item.ChildItems))
                    {
                        bRet = true;
                        break;
                    }
                }
            }

            return bRet;
        }

        private bool IsParentItemSelected(MenuItem item)
        {
            bool bRet = false;

            if ((item != null) && (item.Parent != null))
            {
                if (item.Parent.Selected)
                {
                    bRet = true;
                }
                else
                {
                    bRet = IsParentItemSelected(item.Parent);
                }
            }

            return bRet;
        }
    }
}
