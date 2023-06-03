using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammersBlog.Shared.Entities.Concrete
{
    public class MvcErrorModel
    {
        public string Type { get; set; }
        public string Detail { get; set; }
        public string Message { get; set; }
    }
}
