using Common.Logging;

using iTextSharp.text;
using iTextSharp.text.pdf;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace FrontDesk.Server.Printouts
{
    public class PdfReachTextFormatter
    {
        private readonly ILog _logger = LogManager.GetLogger<PdfReachTextFormatter>();
        public PdfPCell ContainerCell { get; private set; }
        public List<FormattedTextChunk> SourceFormattedText { get; private set; }

        protected Font GetFont(FormattedTextAttributes attr)
        {
            var result = FontFactory.GetFont(BaseFont.HELVETICA, 8, Font.NORMAL);

            if (attr.Bold)
            {
                result.SetStyle(Font.BOLD);
            }

            if (attr.Italic)
            {
                result.SetStyle(Font.ITALIC);
            }
            if (attr.Underline)
            {
                result.SetStyle(Font.UNDERLINE);
            }
            if (attr.Strike)
            {
                result.SetStyle(Font.STRIKETHRU);
            }

            if (!string.IsNullOrEmpty(attr.Color))
            {
                var color = System.Drawing.ColorTranslator.FromHtml(attr.Color);
                result.SetColor(color.R, color.G, color.B);
            }


            return result;
        }

        public PdfReachTextFormatter(string jsonFormattedString, PdfPCell container)
        {
            if (string.IsNullOrEmpty(jsonFormattedString))
            {
                SourceFormattedText = new List<FormattedTextChunk>();
            }
            else
            {
                try
                {
                    SourceFormattedText = Normalize(JsonConvert.DeserializeObject<FormattedText>(jsonFormattedString).Ops);
                }
                catch (Exception ex)
                {
                    _logger.ErrorFormat("Failed to parse reach text json value. string: {0}", ex, jsonFormattedString);
                    SourceFormattedText = new List<FormattedTextChunk>();
                }

            }
            ContainerCell = container ?? throw new ArgumentNullException(nameof(container));
        }

        protected const string NEW_LINE = "\n";

        protected List<FormattedTextChunk> Normalize(List<FormattedTextChunk> chunks)
        {

            //normilize alignment
            //each \n should break lines
            //each insert=\n apply styles to the previous line

            Stack<int> itemsToRemove = new Stack<int>();

            for (var index = 0; index < chunks.Count; index++)
            {
                var item = chunks[index];

                if (item.Insert == NEW_LINE)
                {
                    //copy attribute to previous line
                    chunks[index - 1].Attributes.List = item.Attributes.List;
                    itemsToRemove.Push(index);

                    continue;
                }

                var lines = item.Insert.Split(new string[] { NEW_LINE }, StringSplitOptions.None);
                if (lines.Length > 1)
                {
                    bool isListStyle = !String.IsNullOrEmpty(item.Attributes.List);
                    //split chunks
                    var lineStartIndex = index;

                    for (var lineIndex = 0; lineIndex < lines.Length; lineIndex++)
                    {
                        var text = lines[lineIndex];

                        if (string.IsNullOrEmpty(text))
                        {
                            if (lineIndex == 0)   //skip first new line
                            {
                                continue;
                            }

                            text = NEW_LINE;
                        }

                        if (lineStartIndex == index) //update the value
                        {
                            chunks[lineStartIndex].Insert = text;
                        }
                        else //insert new item, 
                        {
                            var appendItem = new FormattedTextChunk
                            {
                                Insert = NEW_LINE + lines[lineIndex],
                                Attributes = item.Attributes
                            };

                            if (lineStartIndex < chunks.Count)
                            {
                                chunks.Insert(lineStartIndex, appendItem);
                            }
                            else
                            {
                                chunks.Add(appendItem);
                            }
                        }

                        if (isListStyle && lineIndex < lines.Length - 1)
                        {
                            //remove list from all items but not the last
                            chunks[lineStartIndex].Attributes.List = null;
                        }

                        lineStartIndex++;

                    }
                    if (lineStartIndex > index)
                    {
                        index = lineStartIndex - 1; //move pointer to the last inserted value
                    }
                }


            }

            foreach (var index in itemsToRemove)
            {
                chunks.RemoveAt(index);
            }

            //remove trailing new lines from list items
            foreach (var item in chunks)
            {
                if (!string.IsNullOrEmpty(item.Attributes.List))
                {
                    item.Insert = item.Insert.Trim(new[] { '\n' });
                }
            }


            return chunks;
        }

        public void Process()
        {
            Phrase textContainer = null;
            List listContainer = null;

            foreach (var item in SourceFormattedText)
            {
                var attributes = item.Attributes;

                if (textContainer == null)
                {
                    textContainer = new Phrase
                    {

                    };
                }

                var chunk = new Chunk(item.Insert, GetFont(attributes));

                if (!string.IsNullOrEmpty(attributes.Background))
                {
                    var color = System.Drawing.ColorTranslator.FromHtml(attributes.Background);
                    chunk.SetBackground(new Color(color), 1f, 1f, 1f, 1f);
                }

                if (!string.IsNullOrEmpty(attributes.List))
                {
                    if (listContainer == null)
                    {
                        //first line in the list, create the list item
                        listContainer = new List(attributes.List == "ordered")
                        {
                            IndentationLeft = 15f,
                            First = 1
                        };

                        if (textContainer != null)
                        {
                            //started tracking List, writing down the content of Phrase
                            ContainerCell.AddElement(textContainer);
                            textContainer = null;
                        }
                    }

                    listContainer.Add(new ListItem(chunk));

                    continue;

                }

                if (listContainer != null)
                {
                    ContainerCell.AddElement(listContainer);
                    listContainer = null;
                }

                textContainer.Add(chunk);
            }

            if (textContainer != null)
            {
                ContainerCell.AddElement(textContainer);
            }
            if (listContainer != null)
            {
                ContainerCell.AddElement(listContainer);
            }


        }

        public static PdfReachTextFormatter Init(string jsonFormattedString, PdfPCell container)
        {
            return new PdfReachTextFormatter(jsonFormattedString, container);
        }
    }


    public class FormattedText
    {
        public List<FormattedTextChunk> Ops = new List<FormattedTextChunk>();

    }

    public class FormattedTextChunk
    {
        public FormattedTextAttributes Attributes;
        public string Insert { get; set; }
    }

    public struct FormattedTextAttributes
    {
        public string Color { get; set; }

        public string Background { get; set; }

        public bool Bold { get; set; }

        public bool Underline { get; set; }

        public bool Italic { get; set; }

        public bool Strike { get; set; }

        public string Align { get; set; }

        public string List { get; set; }
    }
}
