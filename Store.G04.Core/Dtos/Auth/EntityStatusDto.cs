using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G04.Core.Dtos.Auth
{
    public class EntityStatusDto
    {
        public required string Status { get; set; }
        public required string Message { get; set; }
    }
}
