using API_assignment.DatabaseHelper;
using API_assignment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_assignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProduceController : ControllerBase
    {
        private readonly MyDbContext _context;

        public ProduceController(MyDbContext context)
        {
            _context = context;
        }

        // GetAll() is automatically recognized as
        // http://localhost:<port #>/api/todo
        [HttpGet]
        public IEnumerable<Produce> GetAll()
        {
            return _context.Produces.ToList();
        }

        // GetById() is automatically recognized as
        // http://localhost:<port #>/api/todo/{id}

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            var item = _context.Produces.FirstOrDefault(t => t.ProduceID == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }
        [HttpPost]
        public IActionResult Create([FromBody] Produce produce)
        {
            if (produce.Description == null || produce.Description == "")
            {
                return BadRequest();
            }
            _context.Produces.Add(produce);
            _context.SaveChanges();
            return new ObjectResult(produce);
        }
        [HttpPut]
        [Route("edit")] // Custom route
        public IActionResult GetByParams([FromBody] Produce produce)
        {
            var item = _context.Produces.Where(t => t.ProduceID == produce.ProduceID).FirstOrDefault();
            if (item == null)
            {
                return NotFound();
            }
            else
            {
                item.Description= produce.Description;
                _context.SaveChanges();
            }
            return new ObjectResult(item);
        }

        [HttpDelete("{id}")]
        public IActionResult MyDelete(int id)
        {
            var item = _context.Produces.Where(t => t.ProduceID == id).FirstOrDefault();
            var children = _context.ProduceSuppliers.Where(t => t.ProduceID == id).ToList();
            //var ps = _context.ProduceSuppliers.Where(t => t.ProduceID == id).FirstOrDefault();
            if (item == null)
            {
                return NotFound();
            }
        
            if (children != null)
            { foreach(var child in children)
                {
                    _context.ProduceSuppliers.Remove(child);
                }
                
            }
            try
            {
                _context.Produces.Remove(item);

                _context.SaveChanges();
                return new ObjectResult(item);
            }
            catch
            {
                return Conflict();
    }
}
    }
}
