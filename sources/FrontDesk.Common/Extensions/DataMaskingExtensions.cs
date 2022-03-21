using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Common.Extensions
{
	public static class DataMaskingExtensions
	{
		public static ScreeningResult AsMasked(this ScreeningResult result)
		{
			if(result == null) return result;

			var maskedResult = new ScreeningResult
			{
				ScreeningID = result.ScreeningID,
				CreatedDate = result.CreatedDate,
				ID = result.ID,

			};
			foreach (var screeningSectionResult in result.SectionAnswers)
			{
				maskedResult.AppendSectionAnswer(screeningSectionResult);
			}
			return maskedResult;
		}
	}
}
