using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FrontDesk.Server.Printouts;
using FluentAssertions;
using System.Linq;

namespace FrontDesk.Server.Tests.Printouts
{
    [TestClass]
    public class PdfReachTextFormatterTests
    {
        protected string GetFormattedText()
        {
            return @"{
	""ops"": [{
			""attributes"": {
				""background"": ""#ff9900"",
				""bold"": true
			},
			""insert"": ""Lorem ipsum dolor sit amet, volutpat dolor, lobortis leo mollis eget pellentesque. Nisl hendrerit interdum volutpat sed sapien est, ipsum vitae, id et.""
		}, {
			""insert"": "" Nec cras mauris tristique sed, maecenas neque eu pellentesque lacinia et duis. Amet in malesuada et bibendum sunt tincidunt, lectus nibh venenatis lorem cursus a lectus, diam euismod ac sed. Nunc consectetuer posuere enim suspendisse, convallis cras sed. Lacus mauris augue, lacinia ac leo proin, purus pulvinar, urna vestibulum ut ut morbi, faucibus aliquam. Accusantium egestas nulla non, imperdiet accumsan wisi massa fermentum, cras at, ut eget. ""
		}, {
			""attributes"": {
				""color"": ""#f06666"",
				""bold"": true
			},
			""insert"": ""Adipiscing suspendisse velit maecenas nec felis, nec nullam ut arcu mus ante, volutpat ad interdum magna. Dui accumsan nulla leo deleniti in integer, consequat sint facere.""
		}, {
			""insert"": "" Quam placeat neque dictum, lorem orci donec augue id rhoncus. ""
		}, {
			""attributes"": {
				""underline"": true
			},
			""insert"": ""Magna et quis, aenean tortor ut vitae sagittis feugiat orci, quis nulla malesuada sed donec ""
		}, {
			""insert"": ""id, vitae maecenas dui, erat per vitae nullam tempus imperdiet ullamcorper.\none""
		}, {
			""attributes"": {
				""list"": ""ordered""
			},
			""insert"": ""\n""
		}, {
			""insert"": ""two""
		}, {
			""attributes"": {
				""list"": ""ordered""
			},
			""insert"": ""\n""
		}, {
			""attributes"": {
				""italic"": true,
				""bold"": true
			},
			""insert"": ""three""
		}, {
			""attributes"": {
				""list"": ""ordered""
			},
			""insert"": ""\n""
		}, {
			""insert"": ""line 1\nline 2\n\nline 3\n\nSed sed, sapiente ultrices adipiscing risus ut at, mi nullam magna ipsum sed dolor varius, mattis augue risus, sint et phasellus massa amet ut consectetuer. Justo tincidunt proin mi erat, morbi ante risus nec faucibus massa. Ipsum tincidunt tempus congue in mattis, ac sit id deserunt, lacus magna neque sed magna, eu placerat. Vitae tellus iaculis nihil, ut dolor lectus faucibus imperdiet sagittis quisque. \n""
		}, {
			""attributes"": {
				""link"": ""http://microsoft.com""
			},
			""insert"": ""CENTER LINE""
		}, {
			""insert"": ""\nFirst""
		}, {
			""attributes"": {
				""list"": ""ordered""
			},
			""insert"": ""\n""
		}, {
			""insert"": ""Second""
		}, {
			""attributes"": {
				""list"": ""ordered""
			},
			""insert"": ""\n""
		}, {
			""insert"": ""Third""
		}, {
			""attributes"": {
				""list"": ""ordered""
			},
			""insert"": ""\n""
		}, {
			""insert"": ""\n""
		}, {
			""attributes"": {
				""underline"": true,
				""strike"": true,
				""italic"": true,
				""bold"": true
			},
			""insert"": ""Duis a ut sodales orci, bibendum cursus integer. Mauris pellentesque ut risus, ut nibh ac, vel adipisicing proin. Ut lectus interdum, elit eros. Lacus duis vivamus et ac, amet dui sed duis. Suspendisse praesent vitae bibendum eget. Wisi leo pede hendrerit, velit lectus augue sit vivamus, nec magna eget tempor, duis luctus suscipit.""
		}, {
			""insert"": ""\n""
		}
	]
}

";
        }

        protected string GetTextWithLastList()
        {
            return @"{
	""ops"": [{
			""attributes"": {
				""background"": ""#ff9900"",
				""bold"": true
			},
			""insert"": ""Lorem ipsum dolor sit amet, volutpat dolor, lobortis leo mollis eget pellentesque. Nisl hendrerit interdum volutpat sed sapien est, ipsum vitae, id et.""
		}, {
			""insert"": "" Nec cras mauris tristique sed, maecenas neque eu pellentesque lacinia et duis. Amet in malesuada et bibendum sunt tincidunt, lectus nibh venenatis lorem cursus a lectus, diam euismod ac sed. Nunc consectetuer posuere enim suspendisse, convallis cras sed. Lacus mauris augue, lacinia ac leo proin, purus pulvinar, urna vestibulum ut ut morbi, faucibus aliquam. Accusantium egestas nulla non, imperdiet accumsan wisi massa fermentum, cras at, ut eget. ""
		}, {
			""attributes"": {
				""color"": ""#f06666"",
				""bold"": true
			},
			""insert"": ""Adipiscing suspendisse velit maecenas nec felis, nec nullam ut arcu mus ante, volutpat ad interdum magna. Dui accumsan nulla leo deleniti in integer, consequat sint facere.""
		}, {
			""insert"": "" Quam placeat neque dictum, lorem orci donec augue id rhoncus. ""
		}, {
			""attributes"": {
				""underline"": true
			},
			""insert"": ""Magna et quis, aenean tortor ut vitae sagittis feugiat orci, quis nulla malesuada sed donec ""
		}, {
			""insert"": ""id, vitae maecenas dui, erat per vitae nullam tempus imperdiet ullamcorper.\none""
		}, {
			""attributes"": {
				""list"": ""bullet""
			},
			""insert"": ""\n""
		}, {
			""insert"": ""two""
		}, {
			""attributes"": {
				""list"": ""bullet""
			},
			""insert"": ""\n""
		}, {
			""attributes"": {
				""italic"": true,
				""background"": ""#b26b00"",
				""bold"": true
			},
			""insert"": ""three""
		}, {
			""attributes"": {
				""list"": ""bullet""
			},
			""insert"": ""\n""
		}, {
			""insert"": ""line 1""
		}, {
			""attributes"": {
				""list"": ""ordered""
			},
			""insert"": ""\n""
		}, {
			""insert"": ""line 2""
		}, {
			""attributes"": {
				""list"": ""ordered""
			},
			""insert"": ""\n""
		}, {
			""insert"": ""line 3""
		}, {
			""attributes"": {
				""list"": ""ordered""
			},
			""insert"": ""\n""
		}, {
			""insert"": ""\nSed sed, sapiente ultrices adipiscing risus ut at, mi nullam magna ipsum sed dolor varius, mattis augue risus, sint et phasellus massa amet ut consectetuer. Justo tincidunt proin mi erat, morbi ante risus nec faucibus massa. Ipsum tincidunt tempus congue in mattis, ac sit id deserunt, lacus magna neque sed magna, eu placerat. Vitae tellus iaculis nihil, ut dolor lectus faucibus imperdiet sagittis quisque. \n""
		}, {
			""attributes"": {
				""link"": ""http://microsoft.com/""
			},
			""insert"": ""CENTER LINE""
		}, {
			""insert"": ""\nFirst""
		}, {
			""attributes"": {
				""list"": ""ordered""
			},
			""insert"": ""\n""
		}, {
			""insert"": ""Second""
		}, {
			""attributes"": {
				""list"": ""ordered""
			},
			""insert"": ""\n""
		}, {
			""insert"": ""Third""
		}, {
			""attributes"": {
				""list"": ""ordered""
			},
			""insert"": ""\n""
		}
	]
}


";
        }

        protected PdfReachTextFormatter Sut()
        {
            return Sut(GetFormattedText);
        }
        protected PdfReachTextFormatter Sut(Func<string> jsonTextContent)
        {
            return new PdfReachTextFormatter(jsonTextContent(), new iTextSharp.text.pdf.PdfPCell());
        }

        [TestMethod]
        public void PdfReachTextFormatter_ShouldParseFormattedText()
        {
            var sut = Sut();

            sut.SourceFormattedText.Should().NotBeEmpty();
        }

        [TestMethod]
        public void PdfReachTextFormatter_ShouldResetListFormattingForFirtSecmentInChunk()
        {
            var sut = Sut();

            sut.SourceFormattedText[5].Attributes.List.Should().BeNull();
        }

        [TestMethod]
        public void PdfReachTextFormatter_ShouldRemoveNewLineForFirstListItem()
        {
            var sut = Sut();

            sut.SourceFormattedText[6].Insert.Should().Be("one");
        }
        [TestMethod]
        public void PdfReachTextFormatter_ShouldApplyListFormattingToTheLastRowInChunk()
        {
            var sut = Sut();

            sut.SourceFormattedText[6].Attributes.List.Should().Be("ordered");
        }
        [TestMethod]
        public void PdfReachTextFormatter_ShouldSplitLinesWithNewLine()
        {
            var sut = Sut();

            sut.SourceFormattedText[10].Insert.Should().Be("\nline 2");
        }
        [TestMethod]
        public void PdfReachTextFormatter_ShouldCopyAttributesFromEmptyLineToPreviousLine()
        {
            var sut = Sut();

            sut.SourceFormattedText[6].Attributes.List.Should().Be(@"ordered");
        }
        [TestMethod]
        public void PdfReachTextFormatter_ShouldRemoveEmptyLines()
        {
            var sut = Sut();

            sut.SourceFormattedText[7].Insert.Should().Be(@"two");
        }

        [TestMethod]
        public void PdfReachTextFormatter_ShouldRemoveFirstNewLineCharacter()
        {
            var sut = Sut();

            sut.SourceFormattedText[17].Insert.Should().Be(@"First");
        }

        [TestMethod]
        public void PdfReachTextFormatter_ShouldCreateChunks()
        {
            var sut = Sut();

            sut.Process();

            sut.ContainerCell.Should().NotBeNull() ;
        }

        [TestMethod]
        public void PdfReachTextFormatter_ShouldParseStrikeAttribute()
        {
            var sut = Sut();

            sut.SourceFormattedText.Last().Attributes.Strike.Should().BeTrue();
        }

        [TestMethod]
        public void PdfReachTextFormatter_WhenListTheLast_ShouldRenderLastList()
        {
            var sut = Sut(GetTextWithLastList);

            sut.SourceFormattedText.Last().Insert.Should().Be("Third");
        }

        [TestMethod]
        public void PdfReachTextFormatter_WhenListTheLast_ShouldKeepListAttribute()
        {
            var sut = Sut(GetTextWithLastList);

            sut.SourceFormattedText.Last().Attributes.List.Should().Be("ordered");
        }


    }
}
