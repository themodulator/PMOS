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
    [Table("PasswordPolicy", Schema = "Security")]
    public class PasswordPolicy: IIdentifiablePoco, IRecordAuthorship
    {

        public PasswordPolicy()
        {
            this.RequireDigit = false;
            this.Lifetime = 42;
            this.RequiredLength = 6;
            this.RequireLowercase = false;
            this.RequireNonLetterOrDigit = false;
            this.RequireUppercase = false;
        }

        [Key, ForeignKey("WebApplication")]
        [Required]
        [MaxLength]
        public string Id { get; set; }
        public virtual WebApplication WebApplication { get; set; } 

        [Display(Name = "Required password length")]
        public int RequiredLength { get; set; }

        [Display(Name = "Password must contain a digit")]
        public bool RequireDigit { get; set; }

        [Display(Name = "Password must contain a lower case character")]
        public bool RequireLowercase { get; set; }

        [Display(Name = "Password must contain a nonalphanumeric character")]
        public bool RequireNonLetterOrDigit { get; set; }

        [Display(Name = "Password must contain an upper case character")]
        public bool RequireUppercase { get; set; }

        [Display(Name = "Number of days until the password must be changed")]
        public int Lifetime { get; set; }

        [Required]
        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [Required]
        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; } 
    }
}
