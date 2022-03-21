using Common.Logging;
using FrontDesk.Common.Debugging;
using Ninject.Extensions.Interception;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace FrontDeskRpmsService.Infrastructure
{
	public class TimingInterceptor : SimpleInterceptor
	{
		private readonly ILog _logger = LogManager.GetLogger<TimingInterceptor>();
		readonly Stopwatch _stopwatch = new Stopwatch();
		protected override void BeforeInvoke(IInvocation invocation)
		{
			_stopwatch.Start();
		}

		protected override void AfterInvoke(IInvocation invocation)
		{
			
			_logger.InfoFormat("[Execution of {0}.{1} took {2}.]",
			invocation.Request.Method.DeclaringType,
			invocation.Request.Method.Name,
			_stopwatch.Elapsed);
			_stopwatch.Reset();


		}
	}
}