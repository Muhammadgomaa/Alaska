namespace Alaska.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Message
    {
        [Key]
        public long Message_ID { get; set; }

        [Required]
        public string Message_Desc { get; set; }

        public long User_ID { get; set; }

        public virtual User User { get; set; }
    }
}
