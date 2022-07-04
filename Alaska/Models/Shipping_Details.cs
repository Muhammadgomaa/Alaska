namespace Alaska.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Shipping_Details
    {
        [Key]
        public long Shipping_ID { get; set; }

        public long User_ID { get; set; }

        [Required]
        [StringLength(250)]
        public string Address { get; set; }

        [Required]
        [StringLength(250)]
        public string Area { get; set; }

        [Required]
        [StringLength(250)]
        public string City { get; set; }

        [Required]
        [StringLength(250)]
        public string Country { get; set; }

        public double AmountPaid { get; set; }

        [Required]
        [StringLength(250)]
        public string Payment_Type { get; set; }

        [Required]
        [StringLength(250)]
        public string Date { get; set; }

        [Required]
        [StringLength(250)]
        public string Time { get; set; }

        public virtual User User { get; set; }
    }
}
