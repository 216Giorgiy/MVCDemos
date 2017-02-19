﻿using MVCDemos.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using MVCDemos.Interfaces;

namespace MVCDemos.Controllers
{
    [Route("api/[controller]")]
    public class AuthorsController : Controller
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorsController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        // GET: api/authors
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _authorRepository.ListAsync());
        }

        // GET api/authors/5
        [HttpGet("{id}", Name = "GetAuthor")]
        public async Task<IActionResult> Get(int id)
        {
            if ((await _authorRepository.ListAsync()).All(a => a.Id != id))
            {
                return NotFound(id);
            }
            return Ok(await _authorRepository.GetByIdAsync(id));
        }

        // POST api/authors
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _authorRepository.AddAsync(author);
            //return CreatedAtAction("Get", new { Id = author.Id }, author);
            //return CreatedAtRoute("GetAuthor", new { Id = author.Id }, author);
            return CreatedAtRoute(new { Id = author.Id }, author);
           // return Ok(author);
        }

        // PUT api/authors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]Author author)
        {
            if ((await _authorRepository.ListAsync()).All(a => a.Id != id))
            {
                return NotFound(id);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            author.Id = id;
            await _authorRepository.UpdateAsync(author);
            return Ok();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if ((await _authorRepository.ListAsync()).All(a => a.Id != id))
            {
                return NotFound(id);
            }
            await _authorRepository.DeleteAsync(id);
            return Ok();
        }

        // GET: api/authors/populate
        [HttpGet("Populate")]
        public async Task<IActionResult> Populate()
        {
            if (!(await _authorRepository.ListAsync()).Any())
            {
                await _authorRepository.AddAsync(new Author()
                {
                    Id = 1,
                    FullName = "Steve Smith",
                    TwitterAlias = "ardalis"
                });
                await _authorRepository.AddAsync(new Author()
                {
                    Id = 2,
                    FullName = "Mark Miller",
                    TwitterAlias = "millermark"
                });
                await _authorRepository.AddAsync(new Author()
                {
                    Id = 3,
                    FullName = "Brendan Enrick",
                    TwitterAlias = "brendoneus"
                });
            }
            return Ok();
        }
    }
}