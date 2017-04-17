using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmos.Poco
{
    public class PocoEntity : IIdentifiablePoco, IRecordAuthorship
    {
        [Key]
        [MaxLength(50)]
        public string Id { get; set; }

        [Display(Name = "Author")]
        [MaxLength(128)]
        public string CreatedBy { get; set; }

        [Display(Name = "Created")]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Revised By")]
        [MaxLength(128)]
        public string ModifiedBy { get; set; }

        [Display(Name = "Modified")]
        public DateTime ModifiedOn { get; set; } 
    }
}
