using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vidly.Models;
using Vidly.Dtos;
using AutoMapper;
namespace Vidly.Controllers.Api
{
  
    public class CustomersController : ApiController
    {
        private ApplicationDbContext _context;
        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }
        //Get/Api/Customers
        public IHttpActionResult GetCustomers(string query = null)
        {
            var CustomersQuery = _context.Customers.Include(c => c.MemberShipType);
            if (!string.IsNullOrWhiteSpace(query))
            {
                CustomersQuery = CustomersQuery.Where(c => c.Name.Contains(query));
            }
            var CustomerDtos = CustomersQuery.ToList().Select(Mapper.Map<Customer,CustomerDto>);
            return Ok(CustomerDtos);

        }
        //Get/Api/Customers/1
        public IHttpActionResult GetCustomers(int id)
        {
            var customer = _context.Customers.SingleOrDefault(x => x.Id == id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<Customer, CustomerDto>(customer));
        }
        //post/Api/Customers
        [HttpPost]
        [Authorize(Roles = RoleName.CanManageMovies)]
        public IHttpActionResult CreateCustomers(CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var customer = Mapper.Map<CustomerDto, Customer>(customerDto);
            _context.Customers.Add(customer);
            _context.SaveChanges();
            customerDto.Id = customer.Id;
            return Created(new Uri(Request.RequestUri + "/" + customer.Id), customerDto);
        }
        //put/Customers/Id
        [HttpPut]
        [Authorize(Roles = RoleName.CanManageMovies)]
        public void UpdateCustomer(int id, CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            var CustomerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);
            if (CustomerInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            Mapper.Map(customerDto, CustomerInDb);
           
            _context.SaveChanges();
            

        }
        //Delete/api/Customer
        [HttpDelete]
        [Authorize(Roles = RoleName.CanManageMovies)]
        public void DeleteCustomer(int id, Customer customer)
        {
            var CustomerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);
            if (CustomerInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            _context.Customers.Remove(CustomerInDb);
            _context.SaveChanges();
            
        }

    }
}
