using FrontDesk.Common;
using ScreenDox.Server.Common.Data;
using ScreenDox.Server.Common.Models;
using ScreenDox.Server.Models;

using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace ScreenDox.Server.Common.Services
{
    public class KioskService : IKioskService
    {
        private readonly IKioskRepository _repository;


        public KioskService(IKioskRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public KioskService() : this(new KioskDatabase())
        {

        }

        /// <summary>
        /// Get FrontDesk Kiosk By KioskID (guid)
        /// </summary>
        public Kiosk GetByID(short kioskID)
        {
            return _repository.GetByID(kioskID);
        }

        /// <summary>
        /// Search kiosks for kiosk list
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="filterForUserId">If current user is not Administrator, filter only kiosks from user's location.</param>
        /// <returns></returns>
        public SearchResponse<Kiosk> GetAll(KioskSearchFilter filter,
                                            int? filterForUserId)
        {
            var result = new SearchResponse<Kiosk>();


            if (string.IsNullOrEmpty(filter.OrderBy))
            {
                filter.OrderBy = "KioskID ASC";
            }

            // recognize if name filter has kiosk id or name entered
            var nameFilter = filter.NameOrKey;

            Regex re = new Regex(@"^[A-Z0-9]{4}$", RegexOptions.ExplicitCapture);

            short kioskIdFilter = re.IsMatch(nameFilter) ? TextFormatHelper.UnpackStringInt16(nameFilter) : (short)0;
            if(kioskIdFilter > 0)
            {
                nameFilter = string.Empty;
            }

            result.TotalCount = _repository.GetKioskCount(kioskIdFilter,
                nameFilter,
                filter.BranchLocationId,
                filter.ScreeningProfileId,
                filter.ShowDisabled,
                filterForUserId);

            result.Items = result.TotalCount > 0 ? _repository.GetAll(
                kioskIdFilter,
                nameFilter,
                filter.BranchLocationId,
                filter.ScreeningProfileId,
                filter.ShowDisabled,
                filterForUserId,
                filter.StartRowIndex,
                filter.MaximumRows,
                filter.OrderBy
                ) : new List<Kiosk>();

            return result;
        }

        /// <summary>
        /// Get all FronDesk kiosks with filter
        /// </summary>
        public DataSet GetAllWithFiltering(short? kioskID, string filterByName, int? branchLocationID, int? screeningProfileID, bool showDisabled, int? userID, int startRowIndex, int maximumRows, string orderBy)
        {
            return _repository.GetAllWithFiltering(kioskID, filterByName, branchLocationID, screeningProfileID, showDisabled, userID, startRowIndex, maximumRows, orderBy);
        }


        /// <summary>
        /// Get count of kiosks
        /// </summary>
        public int GetKioskCount(short? kioskID, string filterByName, int? branchLocationID, int? screeningProfileID, bool showDisabled, int? userID)
        {
            return _repository.GetKioskCount(kioskID, filterByName, branchLocationID, screeningProfileID, showDisabled, userID);
        }

        /// <summary>
        /// Validates that kiosk id and secret matches and kiosk is not disabled
        /// </summary>
        /// <param name="kioskID"></param>
        /// <returns></returns>
        public bool ValidateKiosk(short kioskID, string secret)
        {
            var kiosk = _repository.GetByID(kioskID);
            if (kiosk == null) return false;

            if (kiosk.Disabled)
            {
                return false;
            }

            if (string.Compare(secret, kiosk.SecretKey) != 0)
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// Validates that kiosk id and secret matches and kiosk is not disabled
        /// </summary>
        /// <param name="kioskID"></param>
        /// <returns></returns>
        public bool ValidateKiosk(string kioskKey, string secret)
        {
            return ValidateKiosk(Kiosk.GetKioskIDFromString(kioskKey), secret);
        }


        /// <summary>
        /// Change last activity date of FrontDesk kiosk
        /// </summary>
        public bool ChangeLastActivityDate(KioskPingMessage kioskPingMessage, DateTimeOffset lastActivityDate)
        {
            return _repository.ChangeLastActivityDate(kioskPingMessage, lastActivityDate);
        }

        /// <summary>
        /// Updated only activity date and remains kiosk app properties unchanged
        /// </summary>
        /// <param name="kioskID"></param>
        /// <param name="lastActivityDate"></param>
        public bool ChangeLastActivityDate(short kioskID, DateTimeOffset lastActivityDate)
        {
            KioskPingMessage message = new KioskPingMessage
            {
                KioskID = kioskID
            };

            return _repository.ChangeLastActivityDate(message, lastActivityDate);
        }

        /// <summary>
        /// Get time of the last kiosk activity
        /// </summary>
        /// <param name="kioskID"></param>
        public DateTimeOffset? GetLastActivityDate(short kioskID)
        {
            return _repository.GetLastActivityDate(kioskID);
        }

        public int GetNotDisabledCount()
        {
            return _repository.GetNotDisabledCount();
        }

        /// <summary>
        /// Check if kiosk is exisits and enabled
        /// </summary>
        /// <param name="kioskID"></param>
        /// <returns>Returns true if kiosk with KiskID is exists and is not disabled</returns>
        public bool CheckKioskIsExistsAndNotDisabled(short kioskID)
        {
            var kiosk = _repository.GetByID(kioskID);
            if (kiosk == null) return false;
            return !kiosk.Disabled;
        }


        /// <summary>
        /// True if name of kiosk already is used in system 
        /// </summary>
        public bool KioskNameIsAlreadyUsed(string kioskName)
        {
            return _repository.KioskNameIsAlreadyUsed(kioskName);
        }

        public short Add(Kiosk kiosk, int maxKioskCountAllowed)
        {

            try
            {
                _repository.BeginTransaction();
                _repository.StartConnectionSharing();

                if (_repository.GetNotDisabledCount() < maxKioskCountAllowed)
                {
                    if (!KioskNameIsAlreadyUsed(kiosk.Name))
                    {

                        kiosk.KioskID = _repository.Add(kiosk);
                    }
                    //create application exception if kiosk name is already used 
                    else
                    {
                        throw new ApplicationException(Resources.TextMessages.KioskAlreadyUsed);
                    }
                }
                //create application exeption if maximum kiosk count has been exceeded
                else
                {
                    throw new ApplicationException(Resources.TextMessages.KioskCountExceeded);
                }

                _repository.StopConnectionSharing();
                _repository.CommitTransaction();
            }
            catch (Exception)
            {
                _repository.StopConnectionSharing();
                _repository.RollbackTransaction();
                throw;
            }
            finally
            {
                _repository.Disconnect();
            }
            return kiosk.KioskID;

        }

        /// <summary>
        /// Delete FrontDesk Kiosk 
        /// </summary>
        public void Delete(short kioskID)
        {
            _repository.Delete(kioskID);
        }

        /// <summary>
        /// Update FrontDesk Kiosk 
        /// </summary>
        public void Update(Kiosk kiosk)
        {
            _repository.Update(kiosk);
        }


        /// <summary>
        /// Enabled/Disabled kiosk
        /// </summary>
        public void SetDisabledStatus(Int16 kioskID, bool isDisabled, int maxKioskCountAllowed)
        {
            try
            {
                _repository.BeginTransaction();
                _repository.StartConnectionSharing();

                if (!isDisabled) // enable
                {
                    if (_repository.GetNotDisabledCount() < maxKioskCountAllowed)
                    {
                        _repository.SetKioskEnabledStatus(kioskID, isDisabled);
                    }
                    else
                    {
                        throw new ApplicationException(Resources.TextMessages.Kiosk_Disabled_Failed);
                    }
                }
                else
                {
                    _repository.SetKioskEnabledStatus(kioskID, isDisabled);
                }

                _repository.StopConnectionSharing();
                _repository.CommitTransaction();
            }
            catch (Exception)
            {
                _repository.StopConnectionSharing();
                _repository.RollbackTransaction();
                throw;
            }
            finally
            {
                _repository.Disconnect();
            }
        }

        /// <summary>
        /// Test if kiosk with specified id has been registered
        /// </summary>
        /// <param name="kioskID"></param>
        /// <returns></returns>
        public  bool TestKioskInstallation(short kioskID)
        {
            var kiosk = _repository.GetByID(kioskID);
            if (kiosk != null && !kiosk.Disabled) return true;
            return false;
        }

        public bool TestKioskInstallation(string kioskKey)
        {
            short kioskId = TextFormatHelper.UnpackStringInt16(kioskKey);

            return TestKioskInstallation(kioskId);
        }

       
    }
}
