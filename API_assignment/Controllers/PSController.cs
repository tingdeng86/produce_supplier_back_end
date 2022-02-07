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
    public class PSController : ControllerBase
    {
        private readonly MyDbContext _context;

        public PSController(MyDbContext context)
        {
            _context = context;
        }

        // GetAll() is automatically recognized as
        // http://localhost:<port #>/api/todo
        [HttpGet]
        public IEnumerable<ProduceSupplier> GetAll()
        {
            return _context.ProduceSuppliers.ToList();
        }

        // GetById() is automatically recognized as
        // http://localhost:<port #>/api/todo/{id}

        [HttpGet("{produceId}/{supplierId}")]
        public IActionResult GetById(long produceId, long supplierId)
        {
            var item = _context.ProduceSuppliers.FirstOrDefault(t => t.SupplierID == supplierId && t.ProduceID==produceId);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }
        [HttpPost]
        public IActionResult Create([FromBody] ProduceSupplier produceSupplier)
        {
            //var produce = _context.Produces.FirstOrDefault(t => t.ProduceID == produceSupplier.ProduceID);
            //var supplier = _context.Suppliers.FirstOrDefault(t => t.SupplierID == produceSupplier.SupplierID);
            var produce = _context.Produces.Where(t => t.ProduceID == produceSupplier.ProduceID).FirstOrDefault();
            var supplier= _context.Suppliers.Where(t => t.SupplierID == produceSupplier.SupplierID).FirstOrDefault();
            if (produce == null || supplier == null || produceSupplier.Qty.ToString() ==null || produceSupplier.Qty.ToString() =="")
            {
                return BadRequest();
            }
            _context.ProduceSuppliers.Add(produceSupplier);
            _context.SaveChanges();
            return new ObjectResult(produceSupplier);
        }
        [HttpPut]
        [Route("edit")] // Custom route
        public IActionResult GetByParams([FromBody] ProduceSupplier produceSupplier)
        {
            var item = _context.ProduceSuppliers.Where(t => t.SupplierID == produceSupplier.SupplierID &&t.ProduceID==produceSupplier.ProduceID ).FirstOrDefault();
            if (item == null)
            {
                return NotFound();
            }
            else
            {
                item.Qty = produceSupplier.Qty;
                _context.SaveChanges();
            }
            return new ObjectResult(item);
        }

        [HttpDelete("{produceId}/{supplierId}")]
        public IActionResult MyDelete(int produceId, int supplierId)
        {
            var item = _context.ProduceSuppliers.Where(t => t.SupplierID == supplierId && t.ProduceID==produceId).FirstOrDefault();
            if (item == null)
            {
                return NotFound();
            }
            _context.ProduceSuppliers.Remove(item);
            _context.SaveChanges();
            return new ObjectResult(item);
        }
    }
}
