namespace Alaska.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Invoice_Details
    {
        [Key]
        public long Order_ID { get; set; }

        public long Invo_Num { get; set; }

        [Required]
        [StringLength(250)]
        public string Prod_Name { get; set; }

        public long Quantity { get; set; }
    }
}
