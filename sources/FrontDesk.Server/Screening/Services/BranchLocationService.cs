using Common.Logging;

using FrontDesk.Common.InfrastructureServices;
using FrontDesk.Server.Data;
using FrontDesk.Server.Data.ScreeningProfile;
using FrontDesk.Server.Licensing.Services;
using FrontDesk.Server.Membership;

using ScreenDox.Server.Models;

using System;
using System.Collections.Generic;
using System.Web.Security;

namespace FrontDesk.Server.Screening.Services
{
    public class BranchLocationService : IBranchLocationService
    {
        protected readonly IBranchLocationDb _repository;
        protected readonly IScreeningProfileRepository _screeiningProfileRepository;
        protected readonly ITimeService _timeService;

        private readonly ILog _logger = LogManager.GetLogger<BranchLocationService>();

        public BranchLocationService(IBranchLocationDb repository, IScreeningProfileRepository screeiningProfileRepository, ITimeService timeService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _screeiningProfileRepository = screeiningProfileRepository ?? throw new ArgumentNullException(nameof(screeiningProfileRepository));
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
        }


        public BranchLocationService(): this(new BranchLocationDb(), new ScreeningProfileDb(), new TimeService())
        {

        }

        /// <summary>
        /// Creates new Branch Location in database.
        /// </summary>
        /// <returns>ID of new branch location. Also, new ID is assigned to this instance.</returns>
        /// <remarks>
        /// When Screening Profile changed, new profile get fresh Last Modified Date values to trigger connected kiosk settings reload
        /// </remarks>
        public int Add(BranchLocation model)
        {
            int id = 0;
            var db = _repository;
            var cert = LicenseService.Current.GetActivatedLicense();

            if (cert == null) throw new ApplicationException(Resources.TextMessages.LicenseValidation_NeedToActivateMessage);
            try
            {
                db.BeginTransaction();
                db.StartConnectionSharing();

                if (db.GetNotDisabledCount() < cert.License.MaxBranchLocations)
                {
                    id = db.Add(model);
                }
                else
                {
                    throw new ApplicationException(Resources.TextMessages.LocationCountExceeded);
                }

                db.StopConnectionSharing();
                db.CommitTransaction();
            }
            catch (Exception)
            {
                db.StopConnectionSharing();
                db.RollbackTransaction();
                throw;
            }
            finally
            {
                db.Disconnect();
            }
            return id;
        }

        public bool Update(BranchLocation model)
        {
            bool result = false;
            try
            {
                _repository.BeginTransaction();
                _repository.StartConnectionSharing();

                //get value from db
                var dbValue = _repository.Get(model.BranchLocationID);
                // update with new data
                result = _repository.Update(model);

                // if profile has changed
                if (result && dbValue != null && dbValue.ScreeningProfileID != model.ScreeningProfileID)
                {
                    // trigger kiosk updates for certain branch location
                    var now = _timeService.GetUtcNow();
                    _screeiningProfileRepository.RefreshKioskSettings(model.ScreeningProfileID, now);
                }

                _repository.StopConnectionSharing();
                _repository.CommitTransaction();
            }
            catch
            {
                _repository.StopConnectionSharing();
                _repository.RollbackTransaction();
                throw;
            }
            return result;
        }

        public bool Delete(int id)
        {
            return _repository.Delete(id);
        }

        public BranchLocation Get(int id)
        {
            return _repository.Get(id);
        }

        public List<BranchLocation> GetAll()
        {
            return _repository.GetAll();
        }


        public SearchResponse<BranchLocation> GetAll(BranchLocationSearchFilter filter)
        {
            var result = new SearchResponse<BranchLocation>();


            if (string.IsNullOrEmpty(filter.OrderBy))
            {
                filter.OrderBy = "Name ASC";
            }

            result.TotalCount = _repository.CountAll(filter.FilterByName,
                filter.ShowDisabled,
                filter.ScreeningProfileId);

            result.Items = result.TotalCount > 0 ? _repository.GetAll(
                filter.FilterByName,
                filter.ShowDisabled,
                filter.ScreeningProfileId,
                filter.StartRowIndex,
                filter.MaximumRows,
                filter.OrderBy
                ) : new List<BranchLocation>();

            return result;
        }

        [Obsolete("migrated to GetAll(filter)")]
        public List<BranchLocation> GetAll(int startRowIndex, int maximumRows, string orderBy)
        {
            return GetAll(string.Empty, false, null, startRowIndex, maximumRows, orderBy);
        }

        [Obsolete("migrated to GetAll(filter)")]
        public List<BranchLocation> GetAll(string filterByName, bool showDisabled, int? screeningProfileId, int startRowIndex, int maximumRows, string orderBy)
        {
            if (string.IsNullOrEmpty(orderBy)) orderBy = "Name ASC";
            return _repository.GetAll(filterByName, showDisabled, screeningProfileId, startRowIndex, maximumRows, orderBy);
        }
        [Obsolete("migrated to GetAll(filter)")]
        /// <summary>
        /// get number of records that meet filterByName expression
        /// </summary>
        public int CountAll(string filterByName, bool showDisabled, int? screeningProfileId)
        {
            return _repository.CountAll(filterByName, showDisabled, screeningProfileId);
        }

        /// <summary>
        /// get number of records
        /// </summary>
        [Obsolete("migrated to GetAll(filter)")]
        public int CountAll()
        {
            return _repository.CountAll(string.Empty, false, null);
        }

        public int GetNotDisabledCount()
        {
            return _repository.GetNotDisabledCount();
        }

        [Obsolete("deprecated")]
        public List<BranchLocation> GetAllForDisplay()
        {
            List<BranchLocation> locations = GetAll();
            locations.Insert(0,
                new BranchLocation(0)
                {
                    Name = "<<All locations>>"
                });
            return locations;
        }

        public List<BranchLocation> GetForUserID(int userID)
        {
            return _repository.GetForUserID(userID);
        }

       
        /// <summary>
        /// get all locations that user can filter to see the Patient Check-In data
        /// </summary>
        public List<BranchLocation> GetAccesibleCheckInLocationsForCurrentUser()
        {
            if (Roles.IsUserInRole(UserRoles.BranchAdministrator) || Roles.IsUserInRole(UserRoles.SuperAdministrator))
            {
                return _repository.GetAll();
            }
            else
            {
                return _repository.GetForUserID(FDUser.CurrentUserID);
            }
        }

        /// <summary>
        /// Enabled/Disabled Branch Location
        /// </summary>
        public void SetDisabledStatus(int branchLocationID, bool makeDisabled)
        {
            var db = _repository;
            var cert = LicenseService.Current.GetActivatedLicense();

            if (cert == null) throw new ApplicationException(Resources.TextMessages.LicenseValidation_NeedToActivateMessage);
            try
            {
                db.BeginTransaction();
                db.StartConnectionSharing();


                if (!makeDisabled) //enable
                {
                    if (db.GetNotDisabledCount() < cert.License.MaxBranchLocations)
                    {
                        db.SetBranchLocationDisabledStatus(branchLocationID, makeDisabled);
                    }
                    else
                    {
                        throw new ApplicationException(Resources.TextMessages.Location_UnableToEnableBecauseMaxExceeded);
                    }
                }
                else //disable
                {
                    if (!HasActiveKiosk(branchLocationID))
                    {
                        db.SetBranchLocationDisabledStatus(branchLocationID, makeDisabled);
                    }
                }

                db.StopConnectionSharing();
                db.CommitTransaction();
            }
            catch (Exception)
            {
                db.StopConnectionSharing();
                db.RollbackTransaction();
                throw;
            }
            finally
            {
                db.Disconnect();
            }
        }

        /// <summary>
        /// True if branch location has a kiosk and kiosk is not disabled
        /// </summary>
        public bool HasActiveKiosk(int branchLocationID)
        {
            return _repository.HasActiveKiosk(branchLocationID);
        }

        public int GetScreeningProfileByKioskID(short kioskID)
        {
            if (kioskID <= 0) return ScreeningProfile.DefaultProfileID;

            return _repository.GetScreeningProfileByKioskID(kioskID)?? ScreeningProfile.DefaultProfileID;
        }
    }
}
