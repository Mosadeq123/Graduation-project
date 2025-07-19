using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G04.Core.Dtos.Auth
{
    public class EmailDto
    {
        public required string To { get; set; } // Person I'm Gonna Send Email For
        public required string Subject { get; set; } // Email Headline (Subject)
        public required string Body { get; set; } // Email Content
    }
}
