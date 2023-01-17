using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiPeliculas.Data;
using ApiPeliculas.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        private readonly AplicationDbContext _dbContext;

        public PeliculasController(AplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<PeliculasModels>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetMovies()
        {
            //AQUI USAMOS EL INCLUDE PARA QUE ME INCLUYA LOS DATOS TAMBIEN DEL MODELO O LA TABLA DE CATEGORIAS
            var list = await _dbContext.peliculasModels.OrderBy(p => p.NameMovie).Include(p=> p.CategoriaModel).ToListAsync();
            return Ok(list);
        }

        [HttpGet("{id}", Name = "GetMoviesById")]
        [ProducesResponseType(200, Type = typeof(PeliculasModels))]
        [ProducesResponseType(400)] //Bad Request
        [ProducesResponseType(404)] //Not Found)]
        public async Task<IActionResult> GetMoviesById(int id)
        {
            var obj = await _dbContext.peliculasModels.Include(p => p.CategoriaModel).FirstOrDefaultAsync(p=> p.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            return Ok(obj);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)] //ERROR INTERNO
        public async Task<IActionResult> CreateMovies([FromBody] PeliculasModels movies)
        {
            if (movies == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _dbContext.AddAsync(movies);
            await _dbContext.SaveChangesAsync();
            
            return CreatedAtRoute("GetMoviesById", new {id = movies.Id}, movies);
        }
    }
}
