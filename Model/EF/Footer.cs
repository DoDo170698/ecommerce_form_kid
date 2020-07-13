namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Footer")]
    public partial class Footer
    {
        [StringLength(50)]
        [Required]
        public string ID { get; set; }

        [Column(TypeName = "ntext")]
        [Required]
        public string Content { get; set; }

        public bool? Status { get; set; }
    }
}
