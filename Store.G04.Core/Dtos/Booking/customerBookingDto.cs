using Store.G04.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G04.Core.Dtos.Booking
{
    public class customerBookingDto
    {
        public string Id { get; set; }
        public List<BookingItemEnitiey> Items { get; set; }
    }

    public class BookingItemEnitiey
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
