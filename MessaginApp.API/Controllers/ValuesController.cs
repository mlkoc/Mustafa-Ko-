using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessaginApp.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MessaginApp.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize] //kimlik doğrulama yapılmadıysa aşağıdaki actionları
    public class ValuesController : ControllerBase
    {
        private readonly DataContext _context;

        public ValuesController(DataContext context)
        {
            _context = context;


        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var values = await _context.Values.ToListAsync();
            return Ok(values);
    
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [AllowAnonymous] // bu action kimlik doğrulamadan çalışır
        public async Task<IActionResult> Get(int id)
        {
            var value = await _context.Values.FirstOrDefaultAsync(x=>x.Id==id);
            return Ok(value);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
