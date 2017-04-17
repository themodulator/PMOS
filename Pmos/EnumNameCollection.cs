using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmos
{
    public class EnumNameCollection<TEnum> : List<EnumName>
    {
        public EnumNameCollection()
        {
            foreach (var e in Enum.GetValues(typeof(TEnum)))
            {
                EnumName n = new EnumName()
                {
                    Name = e.ToString(),
                    Value = e,
                };

                this.Add(n);
            }
        }

        public int GetInteger(string name)
        {
            EnumName n = this.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (n == null)
                return -1;
            else
                return (int)n.Value;
        }
    }
}
