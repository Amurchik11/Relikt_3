
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Relikt_2_DataAccess;
using Relikt_2_DataAccess.Repository;
using Relikt_2_DataAccess.Repository.IRepository;
using Relikt_2_Models;
using Relikt_2_Models.ViewModels;
using Relikt_2_Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Threading.Tasks;

namespace Relikt_2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _prodRepo;
        private readonly ICategoryRepository _catRepo;
        private readonly IApplicationTypeRepository _appTypeRepo;
        private readonly ISizeTypeRepository _sizeTypeRepo;
        private readonly IRightLeftRepository _rightLeftRepo;
        private readonly IColourOfGlassRepository _colourOfGlassRepo;
        private readonly ICouponRepository _coupRepo;
        public HomeController(ILogger<HomeController> logger, IProductRepository prodRepo,
            ICategoryRepository catRepo, IApplicationTypeRepository appTypeRepo,
            ISizeTypeRepository sizeTypeRepo, IRightLeftRepository rightLeftRepo,
            IColourOfGlassRepository colourOfGlassRepo, ICouponRepository coupRepo)
        {
            _logger = logger;
            _prodRepo = prodRepo;
            _catRepo = catRepo;
            _appTypeRepo = appTypeRepo;
            _sizeTypeRepo = sizeTypeRepo;
            _rightLeftRepo = rightLeftRepo;
            _colourOfGlassRepo = colourOfGlassRepo;
            _coupRepo = coupRepo;
        }

        [HttpGet]
        public IActionResult Index([FromQuery] int page = 1, [FromQuery] int size = 12, [FromQuery]string filters = null)
        {
            List<FilterModel> filterModels = new List<FilterModel>();

            if (filters != null)
            {

                filterModels = JsonSerializer.Deserialize<List<FilterModel>>(filters);

            }
            
            Expression<Func<Product, bool>> filterExpression = null;
            if (filterModels.Any())
            {
                var productParameter = Expression.Parameter(typeof(Product));

                var firstFilter = filterModels.First();
                Expression expression = BuildFilterExpression(productParameter, firstFilter);
                foreach (var filter in filterModels.Skip(1))
                {
                    expression = Expression.AndAlso(expression, BuildFilterExpression(productParameter, filter));
                }

                filterExpression = Expression.Lambda<Func<Product, bool>>(expression, productParameter);
            }


            var pageInfo = new PageInfo { PageSize = size, PageNumber = page };
            HomeVM homeVM = new HomeVM()
            {
                ProductsPage = _prodRepo.GetPage(pageInfo, includeProperties: "Category,ApplicationType,SizeType,RightLeft,ColourOfGlass", filter: filterExpression),
                Categories = _catRepo.GetAll(),
                ApplicationTypes = _appTypeRepo.GetAll(),
                SizeTypes = _sizeTypeRepo.GetAll(),
                RightLefts = _rightLeftRepo.GetAll(),
                ColourOfGlasses = _colourOfGlassRepo.GetAll(),
                Coupons = _coupRepo.GetAll(c => c.IsActive == true).ToList(),
            };
            return View(homeVM);
            
        }

        [HttpPost]
        [ActionName("Index")]
        public IActionResult ApplyFilter([FromQuery] int page = 1, [FromQuery] int size = 12, [FromBody] List<FilterModel> filters = null)
        {
            filters = filters ?? new List<FilterModel>();
            Expression<Func<Product, bool>> filterExpression = null;
            if (filters.Any())
            {
                var productParameter = Expression.Parameter(typeof(Product));

                var firstFilter = filters.First();
                Expression expression = BuildFilterExpression(productParameter, firstFilter);
                foreach (var filter in filters.Skip(1))
                {
                    expression = Expression.AndAlso(expression, BuildFilterExpression(productParameter, filter));
                }

                filterExpression = Expression.Lambda<Func<Product, bool>>(expression, productParameter);
            }


            var pageInfo = new PageInfo { PageSize = size, PageNumber = page };
            HomeVM homeVM = new HomeVM()
            {
                ProductsPage = _prodRepo.GetPage(pageInfo, includeProperties: "Category,ApplicationType,SizeType,RightLeft,ColourOfGlass", filter: filterExpression),
                Categories = _catRepo.GetAll(),
                ApplicationTypes = _appTypeRepo.GetAll(),
                SizeTypes = _sizeTypeRepo.GetAll(),
                RightLefts = _rightLeftRepo.GetAll(),
                ColourOfGlasses = _colourOfGlassRepo.GetAll(),
                Coupons = _coupRepo.GetAll(c => c.IsActive == true).ToList(),

            };
            return Ok(homeVM);

        }

        private static BinaryExpression BuildFilterExpression(ParameterExpression productParameter, FilterModel firstFilter)
        {
            var property = Expression.Property(productParameter, firstFilter.PropertyName);
            var expression = Expression.Equal(property, Expression.Constant(firstFilter.Value));
            return expression;
        }

        public IActionResult Details(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }


            DetailsVM DetailsVM = new DetailsVM()
            {
                Product = _prodRepo.FirstOrDefault(u => u.Id == id, includeProperties: "Category,ApplicationType,SizeType,RightLeft,ColourOfGlass"),

                ExistsInCart = false
            };

            foreach (var item in shoppingCartList)
            {
                if (item.ProductId == id)
                {
                    DetailsVM.ExistsInCart = true;
                }
            }

            
            return View(DetailsVM);
        }

        [HttpPost, ActionName("Details")]
        public IActionResult DetailsPost(int id, DetailsVM detailsVM)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }
            shoppingCartList.Add(new ShoppingCart { ProductId = id, CountFt = detailsVM.Product.TempCount });
            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);
            TempData[WC.Success] = "Товар успешно добавлен в корзину";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Test(Object obj)
        {
            return Redirect("Index");
        }
        public IActionResult RemoveFromCart(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            var itemToRemove = shoppingCartList.SingleOrDefault(r => r.ProductId == id);
            if (itemToRemove != null)
            {
                shoppingCartList.Remove(itemToRemove);
            }


            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);
            TempData[WC.Success] = "Товар успешно удален из корзины";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
