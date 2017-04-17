using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmos.Poco
{
    public interface IRecordAuthorship
    {
        string CreatedBy { get; set; }  
         
        DateTime CreatedOn { get; set; }  

        string ModifiedBy { get; set; }     
     
        DateTime ModifiedOn { get; set; } 

    }
}
