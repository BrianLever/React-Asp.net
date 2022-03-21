using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;
using System.Reflection;

namespace FrontDesk.Server.Web.Controls
{
    [Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)]  // class name, method names AND method arguments are not renamed
    public class Pager : WebControl, IPostBackEventHandler
    {
        #region public property

        [Bindable(true),
        Category("Data"),
        DefaultValue(0),
        Description("Count of rows in Data Source")
        ]
        public int RowsCount
        {
            get;
            set;
        }

        [Bindable(true),
        Category("Data"),
        DefaultValue(0),
        Description("Current Page Index")
        ]
        public int CurrentPage
        {
            get
            {
                try
                {
                    return (int)ViewState["CurrentPage"];
                }
                catch
                {
                    return 0;
                }
            }

            set
            {
                ViewState["CurrentPage"] = value;
            }
        }
        [Bindable(true),
        Category("Data"),
        DefaultValue(10),
        Description("Page Size")
        ]
        public int PageSize
        {
            get;
            set;
        }


        public int PageButtonCount
        {
            get { return 5; }
            set { }
        }

        #endregion

        #region event

        public delegate void PagerEventHandler(object sender, PagerEventArgs e);
        public event PagerEventHandler Navigate;

        #endregion

        protected override void Render(HtmlTextWriter writer)
        {
            int startPage, endPage;
            int pagesTotal = (RowsCount % PageSize == 0) ? (RowsCount / PageSize) : (RowsCount / PageSize + 1);

            if (pagesTotal > 1) // do not show pager
            {
                int currentPageRange = CurrentPage / PageButtonCount;
                startPage = currentPageRange * PageButtonCount;
                //endPage = pagesTotal - CurrentPage >= PageButtonCount ? startPage + PageButtonCount : startPage + (pagesTotal - CurrentPage+1);
                endPage = startPage + PageButtonCount;
                /*div pager*/
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "gridView");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "pager");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                /*div current page */
                if (RowsCount > PageButtonCount)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "cur_page");
                    writer.RenderBeginTag(HtmlTextWriterTag.Span);
                    writer.Write(String.Format("Page {0} of {1} ", CurrentPage + 1, pagesTotal));
                    writer.RenderEndTag();
                }
                //render Previous and ... link before page navigation if needed
                if (currentPageRange > 0 && RowsCount > PageButtonCount)
                {
                    //render First link
                    RenderPageLink(writer, "first", "First", 0);
                }

                if (CurrentPage > 0 && RowsCount > PageButtonCount)
                {
                    //render Previous link
                    RenderPageLink(writer, "previous", "Previous", CurrentPage - 1);
                }
                if (currentPageRange > 0 && RowsCount > PageButtonCount)
                {
                    //render ... link
                    RenderPageLink(writer, "previous_range", "...", startPage - PageButtonCount);
                }
                /* page link */
                for (int pageIndex = startPage; pageIndex < endPage; pageIndex++)
                {
                    if (pageIndex != pagesTotal)
                    {
                        string CssClass = "page_link";
                        if (pageIndex == CurrentPage)
                        {
                            CssClass += " selected";
                        }
                        RenderPageLink(writer, CssClass, (pageIndex + 1).ToString(), pageIndex);
                    }
                    else
                    {
                        break;
                    }
                }
                //show "..." and "Next" link if we have more pages
                if (pagesTotal > endPage && RowsCount > PageButtonCount)
                {
                    //render ... button
                    RenderPageLink(writer, "next_range", "...", endPage);
                }
                if (CurrentPage < pagesTotal - 1 && RowsCount > PageButtonCount)
                {
                    RenderPageLink(writer, "next", "Next", CurrentPage + 1);
                }

                if (CurrentPage != pagesTotal - 1 && RowsCount > PageButtonCount) //render last page
                {
                    RenderPageLink(writer, "last", "Last", pagesTotal - 1);
                }

                writer.RenderEndTag();
                writer.RenderEndTag();
            }
        }
        
        protected virtual void OnNavigate(PagerEventArgs e)
        {
            if (Navigate != null)
            {
                Navigate(this, e);
            }
        }

        private void RenderPageLink(HtmlTextWriter writer, string cssClass, string text, int index)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, cssClass);
            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            /* render link */
            writer.AddAttribute(HtmlTextWriterAttribute.Href, Page.ClientScript.GetPostBackClientHyperlink(this, index.ToString()));
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.Write(text);
            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        #region IPostBackEventHandler Members

        public void RaisePostBackEvent(string eventArgument)
        {
            int selectedPage = Int32.Parse(eventArgument);
            PagerEventArgs e = new PagerEventArgs(selectedPage);
            CurrentPage = selectedPage;
            OnNavigate(e);
        }

        #endregion
    }

    public class PagerEventArgs : EventArgs
    {
        public int SelectedPage;
        public PagerEventArgs(int selectedPage)
        {
            this.SelectedPage = selectedPage;
        }
    }
}
