using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("customers")]
    public class CustomerController : Controller
    {
        private static ConcurrentDictionary<long, Customer> customers = new ConcurrentDictionary<long, Customer>();

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetCustomerAsync([FromRoute] long id)
        {
            if (customers.TryGetValue(id, out var customer))
            {
                return Ok(customer);
            }
            return NotFound();
        }

        private static long currentId = 1;

        [HttpPost("")]
        public async Task<IActionResult> CreateCustomerAsync([FromBody] Customer customer)
        {
            long newId = currentId++; 
            var newCustomer = new Customer
            {
                Id = newId,
                Firstname = customer.Firstname,
                Lastname = customer.Lastname
            };

            if (customers.ContainsKey(newCustomer.Id))
            {
                return Conflict();
            }

            customers[newCustomer.Id] = newCustomer;
            return Ok(newCustomer.Id);
        }
    }
}