using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk
{
    /// <summary>
    /// One of allowed options for answer to the question
    /// </summary>
    public class AnswerScaleOption
    {
        #region properties

        public int AnswerScaleOptionID { get; set; }

        public int AnswerScaleID { get; set; }

        public string Text { get; set; }

        public int Value { get; set; }

        #endregion
    }
}
