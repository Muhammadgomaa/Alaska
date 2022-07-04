namespace Alaska.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    public partial class Product
    {
        [Key]
        public long Prod_ID { get; set; }

        [Required(ErrorMessage = "Product Name is Required")]
        [StringLength(250, MinimumLength = 2)]
        [Remote("CheckProduct", "Admin", ErrorMessage = "This Product Name is Already Exist", AdditionalFields = "Prod_ID")]
        public string Prod_Name { get; set; }

        [Required(ErrorMessage = "Product Image is Required")]
        [StringLength(250)]
        public string Prod_Img { get; set; }


        [Required(ErrorMessage = "Product Quantity is Required")]
        [Range(0, 2000000, ErrorMessage = "Enter Valid Quantity")]
        public long Quantity { get; set; }

        [Required(ErrorMessage = "Product Price is Required")]
        [Range(0, 2000000, ErrorMessage = "Enter Valid Price")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Product Category is Required")]
        [Remote("CheckProduct", "Admin", ErrorMessage = "This Product Name is Already Exist", AdditionalFields = "Prod_ID")]
        public long Cat_ID { get; set; }

        public virtual Category Category { get; set; }
    }
}