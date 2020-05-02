using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;
using System.Data.Entity;
namespace Vidly.Controllers
{
    public class CustomersController : Controller
    {
 //       List<Customer> GetCustomers = new List<Customer>
 //           {
                
 //new Customer{Name="salehgomaa",Id=1},
 //               new Customer{Name="moazGomaa",Id=2},
 //               new Customer{Name="ssssssss",Id=3}
 //           };
        private ApplicationDbContext _context;
        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        
        // GET: Customers
        public ActionResult Customers()
        {
          
            return View();
        }
        public ActionResult Details(int id)
        {
            var customers =_context.Customers.Include(c => c.MemberShipType).SingleOrDefault(c => c.Id == id);
            if (customers==null)
            {
                return HttpNotFound(); 
            }
            return View(customers);
        }
        public ActionResult CreateNew()
        {
            var MembershipTypes = _context.MembershipTypes.ToList();
            var viewmodel = new NewCustomerViewModel
            {
                MembershipTypes = MembershipTypes,
                Customer=new Customer()
            };
            return View(viewmodel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNew(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                var viewmodel=new NewCustomerViewModel{
                    Customer=customer,
                    MembershipTypes=_context.MembershipTypes.ToList()
                };
                return View("CreateNew", viewmodel);
            }
            if (customer.Id==0)
            {
                _context.Customers.Add(customer);
            }
            else
            {
                var CustInDb = _context.Customers.Single(m => m.Id == customer.Id);
                CustInDb.Name = customer.Name;
                CustInDb.BirthDate = customer.BirthDate;
                CustInDb.MemberShipTypeId = customer.MemberShipTypeId;
                CustInDb.IsSubscribedToNewsLetter=customer.IsSubscribedToNewsLetter;
            }
            
            _context.SaveChanges();
            return RedirectToAction("Customers", "Customers");
        }
        public ActionResult Edit(int id)
        {
            var customer = _context.Customers.SingleOrDefault(x => x.Id == id);
            var viewmodel = new NewCustomerViewModel
            {
                Customer=customer,
                MembershipTypes=_context.MembershipTypes.ToList()
            };
            return View("CreateNew",viewmodel);
        }
    }
}