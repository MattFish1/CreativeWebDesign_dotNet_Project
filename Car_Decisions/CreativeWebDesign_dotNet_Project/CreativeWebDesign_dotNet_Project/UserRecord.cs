//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CreativeWebDesign_dotNet_Project
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserRecord
    {
        public UserRecord()
        {
            this.UserActivities = new HashSet<UserActivity>();
        }
    
        public string Id { get; set; }
        public Nullable<System.DateTime> TimeLoggedIn { get; set; }
        public Nullable<System.DateTime> TimeLoggedOff { get; set; }
        public string TimeOnMatrix { get; set; }
        public string TimeOnLandingPage { get; set; }
        public string UsersActivity { get; set; }
        public string Age { get; set; }
        public string Sex { get; set; }
        public string Job { get; set; }
        public string JobOther { get; set; }
        public string HighSchool { get; set; }
        public string YearGraduatedHS { get; set; }
        public string PlaceHS { get; set; }
        public string BachelorsComp { get; set; }
        public string YearGraduatedBA { get; set; }
        public string PlaceBA { get; set; }
        public string MastersComp { get; set; }
        public string YearGraduatedMST { get; set; }
        public string PlaceMST { get; set; }
        public string Employer { get; set; }
        public string EmployedSince { get; set; }
        public string CurrentCar { get; set; }
        public string FinalChoice { get; set; }
        public string CarOther { get; set; }
        public string Test { get; set; }
    
        public virtual ICollection<UserActivity> UserActivities { get; set; }
    }
}
