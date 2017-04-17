using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Humanizer;

namespace Pmos
{
    public class EnumName
    {
        public object Value { get; set; }

        public string Name { get; set; }

        public string Title
        {
            get
            {
                return this.Name.Humanize(LetterCasing.Title);
            }
        }

        public int Numeric
        {
            get
            {
                return (int)this.Value;
            }
        }
    }
}
