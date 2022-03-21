using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using System.ServiceModel;
using System.Collections.ObjectModel;
using System.ServiceModel.Channels;
using System.Diagnostics;
using Common.Logging;

namespace FrontDeskRpmsService.ErrorHanding
{
    public class ErrorHandler : IErrorHandler, IServiceBehavior
    {
        private readonly ILog _logger = LogManager.GetLogger<ErrorHandler>();

        public void AddBindingParameters(
            ServiceDescription serviceDescription,
            ServiceHostBase serviceHostBase,
            Collection<ServiceEndpoint> endpoints,
            BindingParameterCollection bindingParameters)
        {
        }


        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            IErrorHandler errorHandler = new ErrorHandler();

            foreach (ChannelDispatcherBase channelDispatcherBase in serviceHostBase.ChannelDispatchers)
            {
                ChannelDispatcher channelDispatcher = channelDispatcherBase as ChannelDispatcher;

                if (channelDispatcher != null)
                {
                    channelDispatcher.ErrorHandlers.Add(errorHandler);
                }
            }
        }

        public bool HandleError(Exception error)
        {
            
            _logger.Error(error.ToString());

            // Returning true indicates you performed your behavior.
            return true;
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            // Shield the unknown exception
            FaultException faultException = new FaultException(
                error.Message);
            MessageFault messageFault = faultException.CreateMessageFault();

            fault = Message.CreateMessage(version, messageFault, faultException.Action);
        }
    }
}