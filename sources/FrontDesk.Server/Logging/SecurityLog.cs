using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Reflection;
using System.Web;

using FrontDesk.Common.Data;
using FrontDesk.Server.Data.Logging;

namespace FrontDesk.Server.Logging
{
    [Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)]  // class name, method names AND method arguments are not renamed
    public class SecurityLog
    {
        #region Properties

        public int UserID { get; set; }

        public DateTimeOffset LogDate { get; set; }

        private SecurityEvent _event;
        public SecurityEvent LoggedEvent { get { return _event; } }

        public int? RelatedBranchLocationID { get; set; }

        #endregion

        #region Constructor

        public SecurityLog(SecurityEvents securityEvent, object metadata)
        {
            _event = new SecurityEvent(securityEvent, metadata);
        }

        public SecurityLog(SecurityEvents securityEvent, object metadata, int? relatedBranchLocationID) :
            this(securityEvent, metadata)
        {
            RelatedBranchLocationID = relatedBranchLocationID;
        }

        #endregion

        #region API

        private static SecurityLogDb DbObject
        {
            get
            {
                return new SecurityLogDb();
            }
        }

        /// <summary>
        /// Add new security log record
        /// </summary>
        public static void Add(SecurityLog logItem, int userIdentityID)
        {
            if (DbObject.IsEventEnabled(logItem.LoggedEvent.Event))
            {
                logItem.UserID = userIdentityID;
                logItem.LogDate = DateTimeOffset.Now;

                DbObject.Add(logItem);
            }
        }

        public static void Add(SecurityLog logItem)
        {
            Add(logItem, (int)System.Web.Security.Membership.GetUser().ProviderUserKey);
        }

        /// <summary>
        /// Add new security log record
        /// </summary>
        public static void Add(int userID, DateTimeOffset logDate, SecurityEvents loggedEvent, object metadata)
        {
            if (DbObject.IsEventEnabled(loggedEvent))
            {
                SecurityLog sl = new SecurityLog(loggedEvent, metadata);
                sl.UserID = userID;
                sl.LogDate = DateTimeOffset.Now;
                DbObject.Add(sl);
            }
        }


        /// <summary>
        /// Get items count for paging
        /// </summary>
        public static int GetReportItemsCount(int categoryID, int eventID,
             bool isSA, DateTime? startDate, DateTime? endDate)
        {
            return DbObject.GetReportItemsCount(
                categoryID == 0 ? (int?)null : categoryID,
                eventID == 0 ? (int?)null : eventID,
                startDate, endDate, isSA,
                (int)System.Web.Security.Membership.GetUser().ProviderUserKey);
        }

        /// <summary>
        /// Get paged report
        /// </summary>
        public static DataSet GetReport(int categoryID, int eventID, DateTime? startDate, DateTime? endDate,
            bool isSA, int startRowIndex, int maximumRows, string orderBy)
        {

            Debug.WriteLine(String.Format("Pager settings start:{0}, max: {1}", startRowIndex, maximumRows));

            DataSet ds = DbObject.GetReport(categoryID == 0 ? (int?)null : categoryID,
                eventID == 0 ? (int?)null : eventID,
                startDate, endDate, isSA,
                (int)System.Web.Security.Membership.GetUser().ProviderUserKey,
                startRowIndex, maximumRows, orderBy);

            if (DBDatabase.IsHasOneRow(ds))
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    SecurityEvents securityEvent = (SecurityEvents)Convert.ToInt32(r["SecurityEventID"]);
                    object metadata = r["Metadata"];

                    r["HTMLDescription"] = SecurityEvent.GetEventText(securityEvent, metadata);
                }
            }

            return ds;
        }

        public static DataSet GetReport(int categoryID, int eventID, DateTime? startDate, DateTime? endDate,
            bool isSA, string orderBy)
        {
            DataSet ds = DbObject.GetReport(categoryID == 0 ? (int?)null : categoryID,
                eventID == 0 ? (int?)null : eventID,
                startDate, endDate, isSA,
                (int)System.Web.Security.Membership.GetUser().ProviderUserKey,
                orderBy);

            if (DBDatabase.IsHasOneRow(ds))
            {
                ds.Tables[0].Columns["LogDate"].Caption = Resources.TextMessages.SecurityLogExcelCaption_LogDate;
                ds.Tables[0].Columns["FullName"].Caption = Resources.TextMessages.SecurityLogExcelCaption_UserName;
                ds.Tables[0].Columns["LocalDescription"].Caption = Resources.TextMessages.SecurityLogExcelCaption_Description;


                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    SecurityEvents securityEvent = (SecurityEvents)Convert.ToInt32(r["SecurityEventID"]);
                    object metadata = r["Metadata"];

                    r["LocalDescription"] = SecurityEvent.GetLocalDescription(securityEvent, metadata);
                }
            }

            return ds;
        }

        /// <summary>
        /// Get list of available security event categories
        /// </summary>
        public static List<SecurityEventCategory> GetCategories(bool isSA)
        {
            return DbObject.GetCategories(isSA);
        }

        /// <summary>
        /// Get the list of events by category
        /// </summary>
        public static List<SecurityEventView> GetEvents(int categoryID)
        {
            return DbObject.GetEvents(categoryID);
        }


        /// <summary>
        /// Get DataSet with two tables: EventCatefories and Events
        /// </summary>
        public static DataSet GetCategoriesWithEvents()
        {
            return DbObject.GetCategoriesWithEvents();
        }

        #endregion

        /// <summary>
        /// Set the status of security log event
        /// </summary>
        public static void SetEventEnabledStatus(int eventID, bool enabled)
        {
            DbObject.SetEventEnabledStatus(eventID, enabled);
        }
    }



    /// <summary>
    /// Security Events
    /// </summary>
    public enum SecurityEvents
    {
        LogIn = 1, // user logged in
        PasswordChanged = 2, // user password has been changed
        SecurityQuestionAndAnswerChanged = 3, // sec. question and / or answer have been changed
        NewUserCreated = 4, // new user has been created
        AccountActivated = 5, // new account has been activated
        ViewBHR = 6,  // view Behavioral Health Screening Report
        PrintBHR = 7, // print Behavioral Health Screening Report
        NewBranchLocationAdded = 8,
        BranchLocationDeleted = 9,
        NewKioskRegistered = 10,
        KioskRemoved = 11,
        BHRDeleted = 12,
        EditPatientContactInformation = 13,
        Export = 14,
        UpdateBhsVisitInformation = 15,
        ManuallyCreateBhsVisitInformation = 16,
        UpdateBhsDemographicsInformation = 18,
        UpdateBhsThirtyDayFollowUpInformation = 19,
        PatientAddressHasBeenAddedFromBhsVisit = 20,
        PrintBhsVisit = 21,
        PrintBhsFollowUp = 22,
        PrintBhsDemographics = 23,
        ExportBhsReports = 24,
        UpdateDrugOfChoice = 25,
        NewScreeningProfileCreated = 30,
        ScreeningProfileDeleted = 31,
    }

    [Serializable]
    [Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)]  // class name, method names AND method arguments are not renamed
    public class SecurityEventView
    {
        public int ID { get; set; }
        public string Description { get; set; }

        public SecurityEventView() { }
    }

    public class SecurityEvent
    {
        public SecurityEvents Event { get; set; }
        public int EventCategoryID { get; set; }
        public string Description { get; set; }
        public object Metadata { get; set; }


        public SecurityEvent(SecurityEvents securityEvent, object metadata)
        {
            Event = securityEvent;
            Metadata = metadata;
        }

        /// <summary>
        /// Prepare event text
        /// </summary>
        public static string GetEventText(SecurityEvents securityEvent, object metadata)
        {
            switch (securityEvent)
            {
                case SecurityEvents.LogIn: return Resources.TextMessages.SecurityEvent_LoggedIn;
                case SecurityEvents.PasswordChanged:
                    return Resources.TextMessages.SecurityEvent_PasswordChanged;
                case SecurityEvents.SecurityQuestionAndAnswerChanged: return Resources.TextMessages.SecurityEvent_SecQuestionChanged;
                case SecurityEvents.NewUserCreated:
                    string[] data = Convert.ToString(metadata).Split(new char[] { '~' });
                    string userDetailsUrl = VirtualPathUtility.ToAbsolute("~/UserDetails.aspx");
                    userDetailsUrl += "?id=" + data[0];
                    return String.Format(Resources.TextMessages.SecurityEvent_NewUser, userDetailsUrl, data[1]);
                case SecurityEvents.AccountActivated: return Resources.TextMessages.SecurityEvent_AcountActivated;
                case SecurityEvents.ViewBHR:
                    string screeningResURL = VirtualPathUtility.ToAbsolute("~/PatientCheckIn.aspx");
                    screeningResURL += "?id=" + metadata;
                    return String.Format(Resources.TextMessages.SecurityEvent_BHRViewed, screeningResURL, metadata);
                case SecurityEvents.PrintBHR:
                    screeningResURL = VirtualPathUtility.ToAbsolute("~/PatientCheckIn.aspx");
                    screeningResURL += "?id=" + metadata;
                    return String.Format(Resources.TextMessages.SecurityEvent_BHRPrinted, screeningResURL, metadata);

                case SecurityEvents.NewBranchLocationAdded:
                    data = Convert.ToString(metadata).Split(new char[] { '~' });
                    string branchDetailsURL = VirtualPathUtility.ToAbsolute("~/management/BranchLocationDetails.aspx");
                    branchDetailsURL += "?id=" + data[0];
                    return String.Format(Resources.TextMessages.SecurityEvent_NewBranch, branchDetailsURL, data[1]);
                case SecurityEvents.BranchLocationDeleted: return String.Format(Resources.TextMessages.SecurityEvent_BranchRemoved, metadata);

                case SecurityEvents.NewKioskRegistered:
                    data = Convert.ToString(metadata).Split(new char[] { '~' });
                    string kioskDetailsUrl = VirtualPathUtility.ToAbsolute("~/management/KioskDetails.aspx");
                    kioskDetailsUrl += "?id=" + data[0];
                    return String.Format(Resources.TextMessages.SecurityEvent_NewKiosk, kioskDetailsUrl, data[1]);

                case SecurityEvents.KioskRemoved: return String.Format(Resources.TextMessages.SecurityEvent_KioskRemoved, metadata);

                case SecurityEvents.BHRDeleted:
                    data = Convert.ToString(metadata).Split(new char[] { '~' });
                    return String.Format(Resources.TextMessages.SecurityEvent_BHRRemoved, data[0], Convert.ToDateTime(data[1]).ToShortDateString());


                case SecurityEvents.Export:
                    return String.Format(Resources.TextMessages.SecurityEvent_ExportedToHealthManagementSystem, metadata);

                case SecurityEvents.EditPatientContactInformation:
                    return String.Format(Resources.TextMessages.SecurityEvent_EditPatient, metadata);

                case SecurityEvents.UpdateBhsVisitInformation:
                    data = Convert.ToString(metadata).Split(new char[] { '~' });
                    return String.Format(Resources.TextMessages.SecurityEvent_UpdateBhsVisitInformation, data);


                case SecurityEvents.UpdateBhsDemographicsInformation:
                    data = Convert.ToString(metadata).Split(new char[] { '~' });
                    return String.Format(Resources.TextMessages.SecurityEvent_UpdateBhsDemographicsInformation, data);

                case SecurityEvents.UpdateBhsThirtyDayFollowUpInformation:
                    data = Convert.ToString(metadata).Split(new char[] { '~' });
                    return String.Format(Resources.TextMessages.SecurityEvent_UpdateBhsThirtyDayFollowUpInformation, data);

                case SecurityEvents.ManuallyCreateBhsVisitInformation:
                    data = Convert.ToString(metadata).Split(new char[] { '~' });
                    return String.Format(Resources.TextMessages.SecurityEvent_ManuallyCreateBhsVisitInformation, data);

                case SecurityEvents.PatientAddressHasBeenAddedFromBhsVisit:
                    data = Convert.ToString(metadata).Split(new char[] { '~' });
                    return String.Format(Resources.TextMessages.SecurityEvent_PatientAddressHasBeenAddedFromBhsVisit, data);

                case SecurityEvents.PrintBhsVisit:
                    return String.Format(Resources.TextMessages.SecurityEvent_PrintBhsVisit, metadata);
                case SecurityEvents.PrintBhsDemographics:
                    return String.Format(Resources.TextMessages.SecurityEvent_PrintBhsDemographics, metadata);
                case SecurityEvents.PrintBhsFollowUp:
                    return String.Format(Resources.TextMessages.SecurityEvent_PrintBhsFollowUp, metadata);

                case SecurityEvents.ExportBhsReports:
                    data = Convert.ToString(metadata).Split(new char[] { '~' });

                    var dataItems = new List<string>(data);
                    if (dataItems.Count < 4)
                    {
                        // adding report type
                        dataItems.Add("Total Reports");
                    }
                    return String.Format(Resources.TextMessages.SecurityEvent_ExportBhsReports, dataItems.ToArray());

                case SecurityEvents.UpdateDrugOfChoice:
                    data = Convert.ToString(metadata).Split(new char[] { '~' });
                    return String.Format(Resources.TextMessages.SecurityEvent_UpdateDrugOfChoice, data);


                case SecurityEvents.NewScreeningProfileCreated:
                    data = Convert.ToString(metadata).Split(new char[] { '~' });
                    string detailsURL = VirtualPathUtility.ToAbsolute("~/management/ScreeningProfile.aspx");
                    detailsURL += "?id=" + data[0];
                    return String.Format(Resources.TextMessages.SecurityEvent_NewBranch, detailsURL, data[1]);

                case SecurityEvents.ScreeningProfileDeleted: return String.Format(Resources.TextMessages.SecurityEvent_ScreeningProfileRemoved, metadata);

                default: return String.Empty;
            }
        }

        public static string GetLocalDescription(SecurityEvents securityEvent, object metadata)
        {
            switch (securityEvent)
            {
                case SecurityEvents.LogIn: return Resources.TextMessages.SecurityEvent_LoggedIn;
                case SecurityEvents.PasswordChanged: return Resources.TextMessages.SecurityEvent_PasswordChanged;
                case SecurityEvents.SecurityQuestionAndAnswerChanged: return Resources.TextMessages.SecurityEvent_SecQuestionChanged;
                case SecurityEvents.NewUserCreated:
                    string[] data = Convert.ToString(metadata).Split(new char[] { '~' });
                    return String.Format(Resources.TextMessages.SecurityEvent_Local_NewUser, data[1]);
                case SecurityEvents.AccountActivated: return Resources.TextMessages.SecurityEvent_AcountActivated;
                case SecurityEvents.ViewBHR: return String.Format(Resources.TextMessages.SecurityEvent_Local_BHRViewed, metadata);
                case SecurityEvents.PrintBHR: return String.Format(Resources.TextMessages.SecurityEvent_Local_BHRPrinted, metadata);
                case SecurityEvents.NewBranchLocationAdded:
                    data = Convert.ToString(metadata).Split(new char[] { '~' });
                    return String.Format(Resources.TextMessages.SecurityEvent_Local_NewBranch, data[1]);
                case SecurityEvents.BranchLocationDeleted: return String.Format(Resources.TextMessages.SecurityEvent_BranchRemoved, metadata);
                case SecurityEvents.NewKioskRegistered:
                    data = Convert.ToString(metadata).Split(new char[] { '~' });
                    return String.Format(Resources.TextMessages.SecurityEvent_Local_NewKiosk, data[1]);
                case SecurityEvents.KioskRemoved: return String.Format(Resources.TextMessages.SecurityEvent_KioskRemoved, metadata);
                case SecurityEvents.BHRDeleted:
                    data = Convert.ToString(metadata).Split(new char[] { '~' });
                    return String.Format(Resources.TextMessages.SecurityEvent_BHRRemoved, data[0], Convert.ToDateTime(data[1]).ToShortDateString());
                case SecurityEvents.Export:
                    return String.Format(Resources.TextMessages.SecurityEvent_ExportedToHealthManagementSystem, metadata);
                case SecurityEvents.EditPatientContactInformation:
                    data = Convert.ToString(metadata).Split(new char[] { '~' });
                    return String.Format(Resources.TextMessages.SecurityEvent_EditPatient, metadata);
                case SecurityEvents.UpdateBhsVisitInformation:
                    data = Convert.ToString(metadata).Split(new char[] { '~' });
                    return String.Format(Resources.TextMessages.SecurityEvent_UpdateBhsVisitInformation, data);


                case SecurityEvents.UpdateBhsDemographicsInformation:
                    data = Convert.ToString(metadata).Split(new char[] { '~' });
                    return String.Format(Resources.TextMessages.SecurityEvent_UpdateBhsDemographicsInformation, data);

                case SecurityEvents.UpdateBhsThirtyDayFollowUpInformation:
                    data = Convert.ToString(metadata).Split(new char[] { '~' });
                    return String.Format(Resources.TextMessages.SecurityEvent_UpdateBhsThirtyDayFollowUpInformation, data);

                case SecurityEvents.ManuallyCreateBhsVisitInformation:
                    data = Convert.ToString(metadata).Split(new char[] { '~' });
                    return String.Format(Resources.TextMessages.SecurityEvent_ManuallyCreateBhsVisitInformation, data);

                case SecurityEvents.PatientAddressHasBeenAddedFromBhsVisit:
                    data = Convert.ToString(metadata).Split(new char[] { '~' });
                    return String.Format(Resources.TextMessages.SecurityEvent_PatientAddressHasBeenAddedFromBhsVisit, data);

                case SecurityEvents.PrintBhsVisit:
                    return String.Format(Resources.TextMessages.SecurityEvent_PrintBhsVisit, metadata);
                case SecurityEvents.PrintBhsDemographics:
                    return String.Format(Resources.TextMessages.SecurityEvent_PrintBhsDemographics, metadata);
                case SecurityEvents.PrintBhsFollowUp:
                    return String.Format(Resources.TextMessages.SecurityEvent_PrintBhsFollowUp, metadata);

                case SecurityEvents.ExportBhsReports:
                    data = Convert.ToString(metadata).Split(new char[] { '~' });
                    var dataItems = new List<string>(data);
                    if (dataItems.Count < 4)
                    {
                        // adding report type
                        dataItems.Add("Total Reports");
                    }
                    return String.Format(Resources.TextMessages.SecurityEvent_ExportBhsReports, dataItems.ToArray());

                case SecurityEvents.UpdateDrugOfChoice:
                    data = Convert.ToString(metadata).Split(new char[] { '~' });
                    return String.Format(Resources.TextMessages.SecurityEvent_UpdateDrugOfChoice, data);


                case SecurityEvents.NewScreeningProfileCreated:
                    data = Convert.ToString(metadata).Split(new char[] { '~' });
                    return String.Format(Resources.TextMessages.SecurityEvent_NewScreeningProfile, data[1]);
                case SecurityEvents.ScreeningProfileDeleted: return String.Format(Resources.TextMessages.SecurityEvent_ScreeningProfileRemoved, metadata);


                default: return String.Empty;
            }
        }
    }

    [Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)]  // class name, method names AND method arguments are not renamed
    public class SecurityEventCategory
    {
        public int ID { get; set; }
        public string Description { get; set; }

        public SecurityEventCategory() { }

        public SecurityEventCategory(int id, string description)
        {
            ID = id;
            Description = description;
        }

        public SecurityEventCategory(IDataReader reader)
        {
            ID = Convert.ToInt32(reader["SecurityEventCategoryID"]);
            Description = Convert.ToString(reader["CategoryName"]);
        }

    }


    public class SecurityLogException : Exception { }
}
