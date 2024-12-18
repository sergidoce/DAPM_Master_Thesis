using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilLibrary
{
    public class UserEditDto
    {
        public int Id {  get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string NewPassword { get; set; } = "";
        public List<string> Roles { get; set; }
    }
}
