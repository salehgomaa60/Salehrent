using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vidly.Dtos;
using Vidly.Models;
using AutoMapper;
namespace Vidly.Controllers.Api
{
    public class MoviesController : ApiController
    {
         private ApplicationDbContext _context;
        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }
        [HttpGet]
        public IEnumerable<MoviesDto> GetMovies(string query = null)
        {
            var MoviesQuery = _context.Movies.Include("Genre").Where(m => m.NumberAvailable > 0);
            if (!String.IsNullOrWhiteSpace(query))
            {
                MoviesQuery = MoviesQuery.Where(c => c.Name.Contains(query));
            }
            var MoviesDto = MoviesQuery.ToList().Select(Mapper.Map<Movie, MoviesDto>);
            return MoviesDto;

        }

        [HttpGet]
        public IHttpActionResult GetMovies(int id)
        {
            var movie = _context.Movies.FirstOrDefault(x => x.Id == id);
            if (movie.Id == 0)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<Movie, MoviesDto>(movie));
        }
        [HttpPost]
        public IHttpActionResult CreateMovie(MoviesDto moviesDto)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            var movie = Mapper.Map<MoviesDto, Movie>(moviesDto);
            _context.Movies.Add(movie);
            _context.SaveChanges();
            moviesDto.Id = movie.Id;
            return Created(new Uri(Request.RequestUri + "/" + movie.Id), moviesDto);
        }
        [HttpPut]
        public void UpdateMovie(int id, MoviesDto moviesDto)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
           
            var MovieInDb = _context.Movies.SingleOrDefault(x => x.Id == id);
            if (MovieInDb==null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            Mapper.Map(moviesDto,MovieInDb);
            _context.SaveChanges();
            

        }
        [HttpDelete]
        public IHttpActionResult DeleteMovie(int id)
        {
            var MovieInDb = _context.Movies.SingleOrDefault(x => x.Id == id);
            if (MovieInDb==null)
            {
                return NotFound();
            }
            _context.Movies.Remove(MovieInDb);
            _context.SaveChanges();
            return Ok();
        }
        
    }
}
