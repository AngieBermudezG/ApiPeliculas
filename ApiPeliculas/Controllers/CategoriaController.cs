using ApiPeliculas.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiPeliculas.Models;

namespace ApiPeliculas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly AplicationDbContext _dbContext;
        public CategoriaController(AplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<CategoriaModel>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCategoria()
        {
            var list = await _dbContext.categoriaModels.OrderBy(c => c.Name).ToListAsync();

            return Ok(list);
        }

        [HttpGet("{id}", Name = "GetCategoriaId")]
        [ProducesResponseType(200, Type = typeof(CategoriaModel))]
        [ProducesResponseType(400)] //Bad Request
        [ProducesResponseType(404)] //Not Found
        public async Task<IActionResult> GetCategoriaId(int id)
        {
            var obj = await _dbContext.categoriaModels.FirstOrDefaultAsync(c => c.Id == id);
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
        public async Task<IActionResult> CreateCategoria([FromBody] CategoriaModel categoria)
        {
            if (categoria == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _dbContext.AddAsync(categoria);
            await _dbContext.SaveChangesAsync();
            
            return CreatedAtRoute("GetCategoriaId", new {id = categoria.Id}, categoria);
        }

    }
}
