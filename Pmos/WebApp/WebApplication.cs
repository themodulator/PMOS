using Pmos.Poco;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmos.WebApp
{
    [Table("WebApplication", Schema = "Infrastructure")]
    public class WebApplication : PocoEntity, IApplicationInfo, IApplicationProfile
    {

        public virtual PasswordPolicy PasswordPolicy { get; set; }

        [Display(Name = "Title")]
        [Required]
        [MaxLength(128)]
        public string Title { get; set; }

        [Display(Name = "Guid")]
        [Required]
        [MaxLength(128)]
        [Index("IX_Application_Guid", IsUnique = true)]
        public string Guid { get; set; }

        [Display(Name = "Copyright")]
        [Required]
        [MaxLength(128)]
        public string Copyright { get; set; }

        [Display(Name = "Description")]
        [Required]
        [MaxLength(256)]
        public string Description { get; set; }

        [Display(Name = "Trademark")]
        [Required]
        [MaxLength(128)]
        public string Trademark { get; set; }

        [Display(Name = "Company")]
        [Required]
        [MaxLength(128)]
        public string Company { get; set; }

        [Display(Name = "Theme")]
        [Required]
        [MaxLength(128)]
        public string Theme { get; set; }

        [Display(Name = "Display Name")]
        [Required]
        [MaxLength(128)]
        public string DisplayName { get; set; }

    }
}
