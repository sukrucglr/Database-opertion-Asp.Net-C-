using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
//using System.Linq;
using VeriTabani;
using VeriTabaniİslemleri.Models;

namespace VeriTabaniİslemleri.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        NorthwindContext context = new NorthwindContext();


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        //personel sayfası
        public IActionResult Index()
        {
            List<Employee> result = context.Employees.ToList();
            return View(result);
        }

        //sipariş sayfası
        public IActionResult OrdersPage(int? id)
        {
            List<Order> orders = new List<Order>();
            if (id != null)
            {
                orders = context.Orders.Include(x => x.OrderDetails).ThenInclude(x => x.Product).Where(x => x.EmployeeId == id).ToList();
            }
            else
            {
                orders = context.Orders.Include(x => x.OrderDetails).ThenInclude(x => x.Product).ToList();
            }

            return View(orders);
        }


        // Sorumlu olduğu ülke 
        public IActionResult CountryPage(int id)
        {
            // select distinct r.RegionDescription from[dbo].[EmployeeTerritories] et
            //inner join Territories t on t.TerritoryID = et.TerritoryID
            //inner join Employees e on e.EmployeeID = et.EmployeeID
            //inner join region r on r.regionId = t.regionId
            //where e.EmployeeID = 1

            List<Region> regions = new List<Region>();
            regions = context.Territories
                .Include(t=>t.Region)
                 .Where(t=>t.Employee.Select(te=>te.EmployeeId).Contains(id))
                 .Select(e => e.Region).Distinct().ToList();

            return View(regions);
        }

        // Sipariş detay 
        public IActionResult ProductPage(int orderId)
        {

            List<Product> products = new List<Product>();
            products = context.OrderDetails.Where(x => x.OrderId == orderId).Include(x => x.Product)
                .Select(x => new Product { ProductId = x.ProductId, ProductName = x.Product.ProductName, UnitsInStock = x.Product.UnitsInStock }).ToList();
            return View(products);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}