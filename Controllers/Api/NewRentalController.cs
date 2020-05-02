using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vidly.Dtos;
using Vidly.Models;
namespace Vidly.Controllers.Api
{
    public class NewRentalController : ApiController
    {
        private ApplicationDbContext _context;
        public NewRentalController()
        {
            _context = new ApplicationDbContext();
        }
        [HttpPost]
        public IHttpActionResult CreateRental(NewRentalDto dto)
        {
            var customer = _context.Customers.Single(x => x.Id == dto.CustomerId);
            var movies = _context.Movies.Where(m => dto.MovieIds.Contains(m.Id)).ToList();
            foreach ( var  movie in movies)
            {
                if (movie.NumberAvailable == 0)
                    return BadRequest("Movie Is Not Available ");
                movie.NumberAvailable--;
                var rental = new Rental
                {
                    Customer = customer,
                    Movie = movie,
                    DateRented = DateTime.Now
                };
                
             
                _context.Rentals.Add(rental);

            }
            _context.SaveChanges();
            return Ok();
        }
      
    }
}
