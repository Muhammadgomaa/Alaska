namespace Alaska.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    [Table("Category")]
    public partial class Category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Category()
        {
            Products = new HashSet<Product>();
        }

        [Key]
        public long Cat_ID { get; set; }

        [Required(ErrorMessage = "Category Name is Required")]
        [RegularExpression("^((?!^Cat_Name$)[a-zA-Z '])+$", ErrorMessage = "Enter Valid Category Name")]
        [StringLength(250, MinimumLength = 2)]
        [Remote("CheckCategory", "Admin", ErrorMessage = "This Category Name is Already Exist", AdditionalFields = "Cat_ID")]
        public string Cat_Name { get; set; }

        [Required(ErrorMessage = "Category Image is Required")]
        [StringLength(250)]
        public string Cat_Image { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }
    }
}