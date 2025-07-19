using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G04.Core.Dtos._Common
{
    public class EmailSettings
    {
        public required string SenderEmail { get; set; }
        public required string SenderDisplayName { get; set; }
        public required string SenderEmailPassword { get; set; }
        public required string Host { get; set; }
        public int Port { get; set; }
    }
}
