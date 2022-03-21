using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Web.Controls
{
    public abstract class BaseFormUserControl<TFormObject, TFormObjectId> : BaseUserControl
       where TFormObject : class, new()
       where TFormObjectId : struct, IEquatable<TFormObjectId>
    {

        /// <summary>
        /// Collect Web Form Business NEtity data before changes commit
        /// </summary>
        public abstract TFormObject GetModel();


        public abstract void SetModel(TFormObject model);

    }
}
