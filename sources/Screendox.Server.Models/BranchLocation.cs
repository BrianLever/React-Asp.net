namespace FrontDesk.Server
{
    public class BranchLocation
    {
        #region properties

        public int BranchLocationID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public bool Disabled { get; set; }


        public int ScreeningProfileID { get; set; }

        /// <summary>
        /// Read only label of the Screening Profile Name
        /// </summary>
        public string ScreeningProfileName { get; set; }


        #endregion

        public BranchLocation()
        {
        }

        public BranchLocation(int id)
        {
            this.BranchLocationID = id;
        }
      
    }
}
