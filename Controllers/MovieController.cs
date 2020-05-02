using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;
using System.Data.Entity;
using System.IO;

namespace Vidly.Controllers
{
    public class MovieController : Controller
    {
        private ApplicationDbContext _context;
        public MovieController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Customers
        // GET: Movie
        public ActionResult Index()
        {

            if (User.IsInRole(RoleName.CanManageMovies))
            {
                return View("List");
            }
                return View("ReadOnlyList");
            
       
           
        }
        public ActionResult Details(int? id)
        {
            var movies = _context.Movies.Include(x => x.Genre).SingleOrDefault(m => m.Id == id);
            if (movies == null)
            {
                return HttpNotFound();
            }
            return View(movies);

        }
        [Authorize(Roles = RoleName.CanManageMovies)]
        public ActionResult NewMovie()
        {
            var genre=_context.Genres.ToList();
            var viewmodel = new SaveMovieVm
            {
                Genre=genre
                
            
            };
            return View("NewMovie",viewmodel);
        }

        [Authorize(Roles = RoleName.CanManageMovies)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewMovie(Movie movie)
        {
            if (movie.Id == 0)
            
            {
      
                movie.DateAdded = DateTime.Now;
                
                _context.Movies.Add(movie);
            }
            else
            {
                var movieIndb = _context.Movies.Single(x => x.Id == movie.Id);
                movieIndb.Name = movie.Name;
                movieIndb.GenreId = movie.GenreId;
                movieIndb.ReleaseDate = movie.ReleaseDate;
                movieIndb.NumberInStock = movie.NumberInStock;
                movieIndb.DateAdded = DateTime.Now;
                movieIndb.MovieImage = movie.MovieImage;
            }
            _context.SaveChanges();
            return RedirectToAction("Index","Movie");
        }
        [Authorize(Roles=RoleName.CanManageMovies)]
        public ActionResult Edit(int id)
        {
            var movie = _context.Movies.SingleOrDefault(x => x.Id == id);
            var viewmodel = new SaveMovieVm
            {
                Genre=_context.Genres.ToList(),
                movie=movie

            };
            return View("NewMovie", viewmodel);
        }
       
            
        }
    }
