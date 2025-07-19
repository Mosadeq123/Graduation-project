using AutoMapper;
using Store.G04.Core.Dtos.Booking;
using Store.G04.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G04.Core.Mapping.Book
{
    public class BookProfile :Profile
    {
        public BookProfile()
        {
            CreateMap<CustomerBooking,customerBookingDto>().ReverseMap();
            CreateMap<bookingItemEnitiey,customerBookingDto>().ReverseMap();
        }
    }
}
