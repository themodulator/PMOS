using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Pmos
{
    public static class EnumExtensions
    {
        public static List<SelectListItem> EnumDropDown<TEnum>()
        {
            EnumNameCollection<TEnum> ec = new EnumNameCollection<TEnum>();

            return ec.Select(x => new SelectListItem() { Text = x.Title, Value = x.Name }).ToList();
        }
    }
}
