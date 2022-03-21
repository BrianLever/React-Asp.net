using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Configuration;

namespace FrontDeskRpmsService.ErrorHanding
{
    public class ErrorHandlerElement : BehaviorExtensionElement
    {
        protected override object CreateBehavior()
        {
            return new ErrorHandler();
        }

        public override Type BehaviorType
        {
            get
            {
                return typeof(ErrorHandler);
            }
        }
    }
}