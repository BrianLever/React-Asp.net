using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace FrontDesk.Server.Services
{
    class CustomHostFactory : ServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            CustomHost customServiceHost =
              new CustomHost(serviceType, baseAddresses[0]);
            return customServiceHost;
        }
    }

    class CustomHost : ServiceHost
    {
        public CustomHost(Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        { }
        protected override void ApplyConfiguration()
        {
            base.ApplyConfiguration();
        }
    }
}
