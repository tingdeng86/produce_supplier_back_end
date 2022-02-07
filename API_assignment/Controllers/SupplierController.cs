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
    public class SupplierController : ControllerBase
    {
        private readonly MyDbContext _context;

        public SupplierController(MyDbContext context)
        {
            _context = context;
        }

        // GetAll() is automatically recognized as
        // http://localhost:<port #>/api/todo
        [HttpGet]
        public IEnumerable<Supplier> GetAll()
        {
            return _context.Suppliers.ToList();
        }

        // GetById() is automatically recognized as
        // http://localhost:<port #>/api/todo/{id}

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            var item = _context.Suppliers.FirstOrDefault(t => t.SupplierID == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }
        [HttpPost]
        public IActionResult Create([FromBody] Supplier supplier)
        {
            if (supplier.SupplierName == null || supplier.SupplierName == "")
            {
                return BadRequest();
            }
            _context.Suppliers.Add(supplier);
            _context.SaveChanges();
            return new ObjectResult(supplier);
        }
        [HttpPut]
        [Route("edit")] // Custom route
        public IActionResult GetByParams([FromBody] Supplier supplier)
        {
            var item = _context.Suppliers.Where(t => t.SupplierID==supplier.SupplierID).FirstOrDefault();
            if (item == null)
            {
                return NotFound();
            }
            else
            {
                item.SupplierName = supplier.SupplierName;
                _context.SaveChanges();
            }
            return new ObjectResult(item);
        }

        [HttpDelete("{id}")]
        public IActionResult MyDelete(int id)
        {
            var item = _context.Suppliers.Where(t => t.SupplierID == id).FirstOrDefault();
            var children = _context.ProduceSuppliers.Where(t => t.SupplierID == id).ToList();
            //var ps = _context.ProduceSuppliers.Where(t => t.ProduceID == id).FirstOrDefault();
            if (item == null)
            {
                return NotFound();
            }

            if (children != null)
            {
                foreach (var child in children)
                {
                    _context.ProduceSuppliers.Remove(child);
                }

            }


            try
            {
                _context.Suppliers.Remove(item);
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
