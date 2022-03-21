using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Common.Logging;
using FrontDesk.Common;
using FrontDesk.Common.Debugging;
using FrontDesk.Kiosk.Services;
using FrontDesk.Kiosk.Settings;
using Newtonsoft.Json;

namespace FrontDesk.Kiosk.Controllers
{
    internal class LookupTableDataSyncController : IDataSyncController
    {
        private readonly ILookupValuesService _lookupValuesService;
        private short _kioskId;
        private readonly ILog _log = LogManager.GetLogger<LookupTableDataSyncController>();

        public LookupTableDataSyncController(ILookupValuesService lookupValuesService, short kioskId)
        {
            _lookupValuesService = lookupValuesService ?? throw new ArgumentNullException(nameof(lookupValuesService));
            _kioskId = kioskId;

            _timer = new Timer
            {
                Interval = TimeSpan.FromSeconds(Settings.AppSettings.LookupValuesDataUpdateIntervalInSeconds).TotalMilliseconds
            };
            _timer.Elapsed += new ElapsedEventHandler(_timer_Tick);
            _timer.Stop();
        }


        public LookupTableDataSyncController(): this(new LookupValuesService(), AppSettings.KioskID)
        {

        }

        #region Timer and Parameters

        private Timer _timer;

        void _timer_Tick(object sender, EventArgs e)
        {
            ReadDataFromServer();
        }
        /// <summary>
        /// start monitoring server connection
        /// </summary>
        public void Start()
        {
            ReadDataFromServer(); //run immidiatelly
            _timer.Start();
        }
        /// <summary>
        /// stop monitoring server connection
        /// </summary>
        public void Stop()
        {
            _timer.Stop();
        }


        #endregion

        #region Ping Server Connection

        bool _isRequestInProgress = false;

        /// <summary>
        /// Call Ping method on server
        /// </summary>
        protected void ReadDataFromServer()
        {
            if (!_isRequestInProgress)
            {
                _isRequestInProgress = true;
                var client = new KioskEndpointService.KioskEndpointClient();
                try
                {
                    System.Net.ServicePointManager.ServerCertificateValidationCallback =
                            ((sender, certificate, chain, sslPolicyErrors) => true);

                    var lastDate = _lookupValuesService.GetLastestDataModifiedDateUTC() ?? new DateTime(2000, 1, 1);
                    var changes = client.GetModifiedLookupValues(lastDate, _kioskId);
                    var items2Delete = client.GetLookupValuesDeleteLog(lastDate, _kioskId);

                    UpdateLookupChanges(changes);

                    DeleteLookupChanges(items2Delete);

                    client.Close();

                }
                catch (Exception ex)
                {
                    _log.Error("Failed to update Lookup Value changes.", ex);

                    client.Abort();
                }
                finally
                {

                    _isRequestInProgress = false;
                }
            }

        }

        private void UpdateLookupChanges(Dictionary<string, LookupValue[]> values)
        {
            if (values != null && values.Any())
            {
                _lookupValuesService.UpdateValues(values);

                var newValues = JsonConvert.SerializeObject(values);

                if (_log.IsInfoEnabled)
                {
                    _log.InfoFormat("Lookup values have been updated. New values: {0}", newValues);
                }
            }
        }

        private void DeleteLookupChanges(Dictionary<string, LookupValue[]> values)
        {
            if (values != null && values.Any())
            {
                _lookupValuesService.DeleteValues(values);

                var jsonObject = JsonConvert.SerializeObject(values);

                if (_log.IsInfoEnabled)
                {
                    _log.InfoFormat("Lookup values have been deleted. Items removed: {0}", jsonObject);
                }
            }
        }



        #endregion
    }
}
