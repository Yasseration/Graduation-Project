//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GraduationProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Patient
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Patient()
        {
            this.PatientHistories = new HashSet<PatientHistory>();
        }
    
        public int ID { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public short Age { get; set; }
        public string Email { get; set; }
        public string PW { get; set; }
        public byte[] Img { get; set; }
        public Nullable<int> BloodGroupID { get; set; }
    
        public virtual BloodGroup BloodGroup { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PatientHistory> PatientHistories { get; set; }
    }
}
