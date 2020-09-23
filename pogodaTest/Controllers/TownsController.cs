using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using pogodaTest.Data;
using pogodaTest.Models;

namespace pogodaTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TownsController : ControllerBase
    {
        private readonly TownWeatherContext _context;
        private readonly ILogger<TownsController> _logger;

        public TownsController(ILogger<TownsController> logger)
        {
            _logger = logger;
            _context = new TownWeatherContext();
        }

        // GET: api/Towns
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Town>>> GetTowns()
        {
            _logger.LogInformation("get все города");
            return await _context.Towns.ToListAsync();
        }

        // GET: api/Towns/5
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Town>> GetTown(int id)
        {
            _logger.LogInformation($"Get {id} город с погодой");
            var town = await _context.Towns.Include(t => t.Weathers)
                .FirstOrDefaultAsync(t => t.TownId == id);

            if (town == null)
            {
                return NotFound();
            }

            return town;
        }
    }
}
