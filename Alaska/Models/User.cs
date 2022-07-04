namespace Alaska.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Refund_Details = new HashSet<Refund_Details>();
            Shipping_Details = new HashSet<Shipping_Details>();
            Messages = new HashSet<Message>();
        }

        [Key]
        public long User_ID { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [RegularExpression("^[A-Za-z0-9._%+-]*@[A-Za-z0-9.-]*\\.[A-Za-z0-9-]{2,}$",
        ErrorMessage = "Enter Valid Email")]
        [StringLength(250)]
        [Remote("CheckUser", "Home", ErrorMessage = "This Email is Already Exist", AdditionalFields = "User_ID")]
        public string User_Email { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [StringLength(250, ErrorMessage = "The Password Must be 6 Digits at Least", MinimumLength = 6)]
        public string User_Password { get; set; }

        [Required(ErrorMessage = "Phone Number is Required")]
        [RegularExpression(@"^([0-9]{11})$", ErrorMessage = "Enter Valid Phone Number")]
        [StringLength(11, ErrorMessage = "The Phone Number Must be 11 Number", MinimumLength = 11)]
        [Remote("CheckUser", "Home", ErrorMessage = "This Phone Number is Already Exist", AdditionalFields = "User_ID")]
        public string User_Phone { get; set; }

        [Required(ErrorMessage = "First Name is Required")]
        [RegularExpression("^((?!^First Name$)[a-zA-Z '])+$", ErrorMessage = "Enter Valid Name")]
        [StringLength(250, MinimumLength = 2)]
        public string First_Name { get; set; }

        [Required(ErrorMessage = "Last Name is Required")]
        [RegularExpression("^((?!^Last Name$)[a-zA-Z '])+$", ErrorMessage = "Enter Valid Name")]
        [StringLength(250, MinimumLength = 2)]
        public string Last_Name { get; set; }

        [Required]
        [StringLength(250)]
        public string User_Role { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Refund_Details> Refund_Details { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Shipping_Details> Shipping_Details { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Message> Messages { get; set; }

    }
}
