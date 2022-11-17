using API_DI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;

namespace API_DI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

   
    public class MoviesController : ControllerBase
    {
        private readonly MovieContext _dbContext;
        public MoviesController(MovieContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovie()
        {
            if (_dbContext.Movie == null) 
            {
                return NotFound();
            }
            return await _dbContext.Movie.ToListAsync();
        }

        [HttpGet("id")]
        public async Task<ActionResult<Movie>> GetMovieById(int id)
        {
            if (_dbContext == null)
            {
                return NotFound();
            }
            var movie = await _dbContext.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(movie);
        }

        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            _dbContext.Movie.Add(movie);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMovie), new {id = movie.Id}, movie);
        }
        [HttpPut]
        public async Task<IActionResult> PutMovie(int id, Movie movie)
        {
            if(id != movie.Id)
            {
                return BadRequest();
            }
            _dbContext.Entry(movie).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                if(!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
                
            }
            return NoContent();
        }

       
        [HttpDelete("id")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            if( _dbContext.Movie == null)
            {
                return NotFound();
            }
            var movie = await _dbContext.Movie.FindAsync(id);
            if(movie == null) 
            {
                return NotFound();
            }
            _dbContext.Movie.Remove(movie);
            await _dbContext.SaveChangesAsync();

            return NotFound();
        }

        private bool MovieExists(long id)
        {
            return (_dbContext.Movie?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}
