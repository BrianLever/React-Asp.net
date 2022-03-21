using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using FrontDesk.Server.Web;
using System.Reflection;

namespace FrontDesk.Server.Web
{
    /// <summary>
    /// Base class for web forms that manage some business entity
    /// I.E. Client Management, User Management etc,
    /// </summary>
    public abstract class BaseManagementWebForm<TFormObject, TFormObjectID> : BaseWebForm
        where TFormObject : new()
        where TFormObjectID : IEquatable<TFormObjectID>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public BaseManagementWebForm()
        {
        }



        #region Accessors

        private TFormObjectID _formObjectID;

        /// <summary>
        /// Form object ID
        /// </summary>
        public virtual TFormObjectID formObjectID
        {
            get { return _formObjectID; }
            protected set
            {
                _formObjectID = value;
            }
        }

        /// <summary>
        /// Business entity object
        /// </summary>
        private TFormObject curFormObject;

        /// <summary>
        /// Get or set business form entity object
        /// </summary>
        public TFormObject CurFormObject
        {
            get
            {
                if (curFormObject == null && !formObjectID.Equals(default(TFormObjectID)))
                {
                    //read 
                    curFormObject = GetFormObjectByID(formObjectID);
                }

                //if new candidate - create new empty object
                if (curFormObject == null)
                {
                    OnFormObjectIDNotFound(ref this.curFormObject);
                }
                return curFormObject;
            }
            protected set { curFormObject = value; }
        }

        /// <summary>
        /// Method that retrive business object from database
        /// This method should be implemented
        /// </summary>
        /// <param name="objectID">Object ID</param>
        /// <returns></returns>
        protected abstract TFormObject GetFormObjectByID(TFormObjectID objectID);

        /// <summary>
        /// Fired if curFormObject wasn't found during GetFormObjectByID or when form ID not found
        /// This method invoked inside CurFormObject_get method
        /// </summary>
        /// <param name="formObject">Reference to the form's form object that could be initialized</param>
        [Obfuscation(Feature = "renaming", Exclude = true)] // used in data binding by name
        protected virtual void OnFormObjectIDNotFound(ref TFormObject formObject) { }

        /// <summary>
        /// Virtual property and should be overrided in the derived page class
        /// </summary>
        public override bool IsNewInstance
        {
            get
            {
                return (this.formObjectID == null || this.formObjectID.Equals(default(TFormObjectID)) || this.curFormObject == null);
            }
        }
        /// <summary>
        /// Return true if object info was retreived successfully from the database using FormObjectID value
        /// </summary>
        /// <remarks>OnFormObjectIDNotFound method not invoked</remarks>
        public override bool IsFormObjectInitialized
        {
            get
            {
                if (curFormObject == null)
                {
                    //read 
                    curFormObject = GetFormObjectByID(formObjectID);
                }
                return !IsNewInstance;
            }

        }

        /// <summary>
        /// Init form business object if object id was evaluated
        /// </summary>
        public override void EnsureFormObjectCreated()
        {
            if (this.formObjectID != null && !this.formObjectID.Equals(default(TFormObjectID)) && curFormObject == null)
            {

                this.curFormObject = GetFormObjectByID(formObjectID);

            }

        }

        #endregion

        #region Page Processing flow

        /// <summary>
        /// Prepare and bind business data to the page controls in the EDIT/READ modes
        /// </summary>
        protected abstract void EditModeDataPrepare(TFormObjectID objectID);

        /// <summary>
        /// Collect Web Form Business NEtity data before changes commit
        /// </summary>
        protected abstract TFormObject GetFormData();


        #endregion

        #region Form Object ID


        /// <summary>
        /// Get Web Form business entity ID value from Request.QueryString or Session or Request or 
        /// </summary>
        /// <param name="sKey">Key value</param>
        /// <param name="IsFound">True if ID</param>
        /// <returns></returns>
        public TFormObjectID GetPageIDValue(string sKey, out bool IsFound)
        {
            return GetPageIDValue<TFormObjectID>(sKey, out IsFound);

        }

        /// <summary>
        /// Get Web Form business entity ID value from Request.QueryString or Session or Request, ViewState
        /// </summary>
        /// <param name="sKey">Key value</param>
        /// <returns>Returns true if key was found and formObjectID has been evaluated with a new value.</returns>
        public bool TryGetPageIDValue(string sKey)
        {
            return TryGetPageIDValue<TFormObjectID>(sKey, out this._formObjectID);

        }

        #endregion
    }
}
