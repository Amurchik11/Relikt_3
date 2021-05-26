
using LiqPay.SDK;
using LiqPay.SDK.Dto;
using LiqPay.SDK.Dto.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Relikt_2.Mapper;
using Relikt_2_DataAccess;
using Relikt_2_DataAccess.Repository.IRepository;
using Relikt_2_Models;
using Relikt_2_Models.ViewModels;
using Relikt_2_Utility;
using Relikt_2_Utility.BrainTree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Relikt_2.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;
        private readonly IApplicationUserRepository _userRepo;
        private readonly ICategoryRepository _catRepo;
        private readonly IApplicationTypeRepository _appTypeRepo;
        private readonly ISizeTypeRepository _sizeTypeRepo;
        private readonly IRightLeftRepository _rightLeftRepo;
        private readonly IColourOfGlassRepository _colourOfGlassRepo;
        private readonly IProductRepository _prodRepo;
        private readonly IInquiryHeaderRepository _inqHRepo;
        private readonly IInquiryDetailRepository _inqDRepo;
        private readonly IOrderHeaderRepository _orderHRepo;
        private readonly IOrderDetailRepository _orderDRepo;
        private readonly ICouponRepository _coupRepo;
        private readonly IExcelDownload _exelDow;
        //private readonly IBrainTreeGate _brain;

        [BindProperty]
        public ProductUserVM ProductUserVM { get; set; }
        public CartController(IWebHostEnvironment webHostEnvironment, IEmailSender emailSender,
            IApplicationUserRepository userRepo, ICategoryRepository catRepo, IApplicationTypeRepository appTypeRepo,
            ISizeTypeRepository sizeTypeRepo, IRightLeftRepository rightLeftRepo,
            IColourOfGlassRepository colourOfGlassRepo, IProductRepository prodRepo,
            IInquiryHeaderRepository inqHRepo, IInquiryDetailRepository inqDRepo,
            IOrderHeaderRepository orderHRepo, IOrderDetailRepository orderDRepo,
            ICouponRepository coupRepo, IExcelDownload exelDow
            //IBrainTreeGate brain
            )
        {

            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
            _userRepo = userRepo;
            _catRepo = catRepo;
            _appTypeRepo = appTypeRepo;
            _sizeTypeRepo = sizeTypeRepo;
            _rightLeftRepo = rightLeftRepo;
            _colourOfGlassRepo = colourOfGlassRepo;
            _prodRepo = prodRepo;
            _inqDRepo = inqDRepo;
            _inqHRepo = inqHRepo;
            _orderDRepo = orderDRepo;
            _orderHRepo = orderHRepo;
            _coupRepo = coupRepo;
            _exelDow = exelDow;
            //_brain = brain;
        }
        public IActionResult Index(Coupon coupon = null)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                // Сессия существует
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            List<int> prodInCart = shoppingCartList.Select(i => i.ProductId).ToList();
            //IEnumerable<Product> prodList = _prodRepo.GetAll(u => prodInCart.Contains(u.Id), includeProperties: "Category,ApplicationType,SizeType");
            IEnumerable<Product> prodListTemp = _prodRepo.GetAll(u => prodInCart.Contains(u.Id), includeProperties: "Category,ApplicationType,SizeType,RightLeft,ColourOfGlass");
            IList<Product> prodList = new List<Product>();

            foreach (var cartObj in shoppingCartList)
            {
                Product prodTemp = prodListTemp.FirstOrDefault(u => u.Id == cartObj.ProductId);
                prodTemp.TempCount = cartObj.CountFt;
                prodList.Add(prodTemp);
            }
            var cartVM = new CartVM { Products = prodList.ToList(), Coupon = coupon };
            return View(cartVM); //тут другая модель будет
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost(IEnumerable<Product> Products)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            foreach (Product prod in Products)
            {
                shoppingCartList.Add(new ShoppingCart { ProductId = prod.Id, CountFt = prod.TempCount });
            }
            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);

            return RedirectToAction(nameof(Summary));
        }


        public IActionResult Summary()
        {
            ApplicationUser applicationUser;
            var orderNumber = 0;
            if (User.IsInRole(WC.AdminRole)|| User.IsInRole(WC.CustomerRole))
            {
                // var inquiryId = HttpContext.Session.Get<int>(WC.SessionInquiryId);
                // if (inquiryId != 0)
                // {
                    //cart has been loaded using an inquiry
                //    InquiryHeader inquiryHeader = _inqHRepo.FirstOrDefault(u => u.Id == inquiryId));
                //    applicationUser = new ApplicationUser()
                //    {
                //        Email = inquiryHeader.Email,
                //        FullName = inquiryHeader.FullName,
                //        PhoneNumber = inquiryHeader.PhoneNumber
                //    };
                //}
                //else
                //{
                    applicationUser = null;
                    orderNumber = (_orderHRepo.GetAll().LastOrDefault()?.Id ?? 0) + 1;
                //}
                //var claimsIdentity = (ClaimsIdentity)User.Identity;
                //var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                //var gateway = _brain.GetGateway();
                //var clientToken = gateway.ClientToken.Generate(
                //    new ClientTokenRequest
                //    {
                //        CustomerId = claim.Value
                //    }
                //);
                //ViewBag.ClientToken = clientToken;
                //var gateway = _brain.GetGateway();
                //var clientToken = gateway.ClientToken.Generate();
                //ViewBag.ClientToken = clientToken;
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                //var userId = User.FindFirstValue(ClaimTypes.Name);

                applicationUser = _userRepo.FirstOrDefault(u => u.Id == claim.Value);
            }

            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                // Сессия существует
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            // unique
            IReadOnlyCollection<int> prodInCart = shoppingCartList.Select(i => i.ProductId).ToHashSet();
            IReadOnlyDictionary<int, Product> productIdToProduct = _prodRepo.GetAll(u => prodInCart.Contains(u.Id), includeProperties: "Category,ApplicationType,SizeType,RightLeft,ColourOfGlass").ToDictionary(x => x.Id);

            Coupon couponFromDb = GetCoupon();
            ProductUserVM = new ProductUserVM()
            {
                ApplicationUser = applicationUser,
                Coupon = couponFromDb,
                OrderHeader =  new OrderHeader { Id = orderNumber },

            //ProductList = prodList.ToList()
            };

            foreach (var cartObj in shoppingCartList)
            {
                Product prodTemp = productIdToProduct[cartObj.ProductId];
                //Product prodTemp = _prodRepo.FirstOrDefault(u => u.Id == cartObj.ProductId);
                prodTemp.TempCount = cartObj.CountFt;
                ProductUserVM.ProductList.Add(prodTemp);
            }

            return View(ProductUserVM);
        }

        private Coupon GetCoupon()
        {
            return _coupRepo.FirstOrDefault(c => !string.IsNullOrEmpty(HttpContext.Session.GetString(WC.ssCouponCode)) ? c.Name.ToLower() == HttpContext.Session.GetString(WC.ssCouponCode).ToLower() : false);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost(IFormCollection collection, ProductUserVM ProductUserVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (User.IsInRole(WC.AdminRole) || User.IsInRole(WC.CustomerRole))
            {
                //нужно создать заказ
                //var orderTotal = 0.0;
                //foreach (Product prod in ProductUserVM.ProductList)
                //{
                //    orderTotal += prod.Price * prod.TempCount;
                //}
                OrderHeader orderHeader = new OrderHeader()
                {
                    CreatedByUserId = claim.Value,
                    FinalOrderTotal = ProductUserVM.ProductList.Sum(x => x.TempCount * x.Price),
                    City = ProductUserVM.ApplicationUser.City,
                    StreetAddress = ProductUserVM.ApplicationUser.StreetAddress,
                    Region = ProductUserVM.ApplicationUser.Region,
                    PostalCode = ProductUserVM.ApplicationUser.PostalCode,
                    FullName = ProductUserVM.ApplicationUser.FullName,
                    Email = ProductUserVM.ApplicationUser.Email,
                    PhoneNumber = ProductUserVM.ApplicationUser.PhoneNumber,
                    OrderDate = DateTime.Now,
                    OrderStatus = WC.StatusPending,
                    Comments = ProductUserVM.OrderHeader.Comments
                    

                };
                _orderHRepo.Add(orderHeader);
                _orderHRepo.Save();

                foreach (var prod in ProductUserVM.ProductList)
                {
                    OrderDetail orderDetail = new OrderDetail()
                    {
                        OrderHeaderId = orderHeader.Id,
                        PriceCount = prod.Price,
                        UnitCount = prod.TempCount,
                        ProductId = prod.Id,
                        ApplicationTypeId = prod.ApplicationTypeId,
                        ColourOfGlassId = prod.ColourOfGlassId,                       
                        CategoryId = prod.CategoryId,
                        RightLeftId = prod?.RightLeftId,
                        SizeTypeId = prod?.SizeTypeId,
                    };
                    _orderDRepo.Add(orderDetail);

                }
                _orderDRepo.Save();

                if (HttpContext.Session.GetString(WC.ssCouponCode) != null)
                {
                    orderHeader.CouponCode = HttpContext.Session.GetString(WC.ssCouponCode);
                    var couponFromDb = _coupRepo.FirstOrDefault(c => c.Name.ToLower() == orderHeader.CouponCode.ToLower());
                    orderHeader.CouponCodeDiscount = orderHeader.FinalOrderTotal - (orderHeader.FinalOrderTotal - orderHeader.FinalOrderTotal / 100 * couponFromDb.Discount);
                    orderHeader.FinalOrderTotal = WC.DiscountedPrice(couponFromDb, orderHeader.FinalOrderTotal);
                    
                }
                //_coupRepo.Add(orderHeader);

                _coupRepo.Save();


                // BRIANTREE
                ////string nonceFromTheClient = collection["payment_method_nonce"];

                ////var request = new TransactionRequest
                ////{
                ////    Amount = Convert.ToDecimal(orderHeader.FinalOrderTotal),
                ////    PaymentMethodNonce = nonceFromTheClient,
                ////    OrderId = orderHeader.Id.ToString(),
                ////    Options = new TransactionOptionsRequest
                ////    {
                ////        SubmitForSettlement = true
                ////    }
                ////};

                ////var gateway = _brain.GetGateway();
                ////Result<Transaction> result = gateway.Transaction.Sale(request);

                ////if (result.Target.ProcessorResponseText == "Подтвержден")
                ////{
                ////    orderHeader.TransactionId = result.Target.Id;
                ////    orderHeader.OrderStatus = WC.StatusApproved;
                ////}
                ////else
                ////{
                ////    orderHeader.OrderStatus = WC.StatusCancelled;
                ////}
                _orderHRepo.Save();

                //// LIQPAY
                //// send invoce by email

                //var invoiceRequest = new LiqPayRequest
                //{
                //    Email = orderHeader.Email,
                //    Amount = orderHeader.FinalOrderTotal,
                //    Currency = "UAH",
                //    OrderId = orderHeader.Id.ToString(),
                //    Action = LiqPayRequestAction.Pay,
                //    Language = LiqPayRequestLanguage.RU,
                //    Description = "Мой Заказ в Дверном Магазине",
                //    Goods = new List<LiqPayRequestGoods>
                //    {
                //         new LiqPayRequestGoods {
                //             Amount = 200,
                //             Count = 4,
                //             Unit = "штук.",
                //            Name = "Конструкции",

                //        }
                //    },
                //    ResultUrl = "/cart",
                //    ServerUrl = "/cart/test"

                //};

                //var liqPayClient = new LiqPayClient("sandbox_i27554924732", "sandbox_g1W9WZTQKFBN42vBHv0gQ632CaP3teSVJYBlCc2n");
                //liqPayClient.IsCnbSandbox = true;
                ////var form = liqPayClient.CNBForm(invoiceRequest);
                ////var response = await liqPayClient.RequestAsync("request", invoiceRequest);

                ////ProductUserVM.btn = form;


                return RedirectToAction(nameof(InquiryConfirmation), new { id = orderHeader.Id });
            }
            else
            {
                //нужно создать запрос
                var PathToTemplate = _webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                + "templates" + Path.DirectorySeparatorChar.ToString() +
                "Inquiry.html";

                var subject = "Новый Запрос";
                string HtmlBody = "";
                using (StreamReader sr = System.IO.File.OpenText(PathToTemplate))
                {
                    HtmlBody = sr.ReadToEnd();
                }
                //Имя: { 0}
                //Email: { 1}
                //Телефон: { 2}
                //Выбранные Товары: { 3}

                StringBuilder productListSB = new StringBuilder();
                //foreach (var prod in ProductUserVM.ProductList)
                //{
                //    productListSB.Append($" - Название: {prod.Name} <span style='font-size:14px;'>" +
                //        $" Тип Товара: {prod.ApplicationType.Name} <span style='font-size:14px;'>" +
                //        $" Цвет: {prod.Category.Name} <span style='font-size:14px;'>" +
                //        $" Размер: {prod.SizeType.Name} <span style='font-size:14px;'>" +
                //        $" Право-Лево: {prod.RightLeft?.Name} <span style='font-size:14px;'>" +
                //        $" Цвет Стекла: {prod.ColourOfGlass?.Name} (Номер Заказа: {prod.Id})</span><br />");
                //}
                foreach (var prod in ProductUserVM.ProductList)
                {
                    productListSB.Append($"<div style='margin-top:5px;border-width:1px; border-style: solid; border-radius:3px;'><h2>{prod.Name}</h2><br />");
                    productListSB.Append($"<span style='font-size:14px; margin-left:5px;'> Тип Товара: <b>{prod.ApplicationType.Name}</b> </span><br />");
                    productListSB.Append($"<span style='font-size:14px;margin-left:5px;'> Цвет: <b>{prod.Category.Name}</b> </span><br />");
                    productListSB.Append($"<span style='font-size:14px;margin-left:5px;'> Размер: <b>{prod.SizeType.Name}</b> </span><br />");


                    if (prod.RightLeft != null)
                        productListSB.Append($"<span style='font-size:14px;margin-left:5px;'> Право-Лево: <b>{prod.RightLeft?.Name}</b> </span><br />");

                    if (prod.RightLeft != null)
                        productListSB.Append($"<span style='font-size:14px;margin-left:5px;'> Цвет Стекла: <b>{prod.ColourOfGlass?.Name}</b> </span><br />");

                    productListSB.Append($"<span style='font-size:14px;margin-left:5px;'>(Номер Заказа: {prod.Id})</span></div>");
                    productListSB.Append($"<span style='font-size:14px;margin-left:5px;'> Количество: <b>{prod.TempCount}</b> </span><br />");
                    //productListSB.Append($"<span style='font-size:14px;margin-left:5px;'> Количество: <b>{prod.}</b> </span><br />");
                }

                string messageBody = string.Format(HtmlBody,
                    ProductUserVM.ApplicationUser.FullName,
                    ProductUserVM.ApplicationUser.Email,
                    ProductUserVM.ApplicationUser.PhoneNumber,
                    productListSB.ToString());

                await _emailSender.SendEmailAsync(WC.EmailAdmin, subject, messageBody);



                InquiryHeader inquiryHeader = new InquiryHeader()
                {
                    ApplicationUserId = claim.Value,
                    FullName = ProductUserVM.ApplicationUser.FullName,
                    Email = ProductUserVM.ApplicationUser.Email,
                    PhoneNumber = ProductUserVM.ApplicationUser.PhoneNumber,
                    InquiryDate = DateTime.Now
                };

                //_inqHRepo.Add(inquiryHeader);
                //_inqHRepo.Save();

                inquiryHeader.InquiryDetail = new List<InquiryDetail>();
                foreach (var prod in ProductUserVM.ProductList)
                {

                    InquiryDetail inquiryDetail = new InquiryDetail()
                    {
                        //InquiryHeaderId = inquiryHeader.Id,
                        ProductId = prod.Id,
                        ApplicationTypeId = prod.ApplicationType.Id,
                        CategoryId = prod.Category.Id,
                        SizeTypeId = prod.SizeType.Id,
                        RightLeftId = prod.RightLeft?.Id,
                        ColourOfGlassId = prod.ColourOfGlass?.Id

                    };
                    //_inqDRepo.Add(inquiryDetail);
                    inquiryHeader.InquiryDetail.Add(inquiryDetail);
                }
                _inqHRepo.Add(inquiryHeader);
                _inqHRepo.Save();
                TempData[WC.Success] = "Запрос успешно отправлен";
            }





            return RedirectToAction(nameof(InquiryConfirmation));


        }

        [HttpPost]
        public IActionResult AddCoupon(IEnumerable<Product> products, Coupon coupon)
        {
            //if (string.IsNullOrEmpty(coupon.Name))
            //{
            //    //coupon.Name = null; это на клиенте
            //}
            HttpContext.Session.SetString(WC.ssCouponCode, coupon.Name.ToLower());
            var couponFromDb = _coupRepo.FirstOrDefault(c => c.Name.ToLower() == coupon.Name.ToLower());
            return UpdateCart(products, couponFromDb);

        }

        public IActionResult RemoveCoupon()
        {

            HttpContext.Session.SetString(WC.ssCouponCode, string.Empty);

            return RedirectToAction(nameof(Index));
        }


        public IActionResult InquiryConfirmation(int id = 0)
        {
            OrderHeader orderHeader = _orderHRepo.FirstOrDefault(u => u.Id == id);
            HttpContext.Session.Clear();
            //// LIQPAY
            //// send invoce by email

            //var invoiceRequest = new LiqPayRequest
            //{
            //    Email = orderHeader.Email,
            //    Amount = orderHeader.FinalOrderTotal,
            //    Currency = "UAH",
            //    OrderId = orderHeader.Id.ToString(),
            //    Action = LiqPayRequestAction.Pay,
            //    Language = LiqPayRequestLanguage.RU,
            //    Description = $"Мой Заказ в Дверном Магазине №: { orderHeader.Id }",
            //    Goods = new List<LiqPayRequestGoods>
            //        {
            //             new LiqPayRequestGoods {
            //                 Amount = 200,
            //                 Count = 4,
            //                 Unit = "штук.",
            //                Name = "Конструкции",
            //            }
            //        },
            //    ResultUrl = "/cart",
            //    ServerUrl = "/cart/test"
            //};

            //var liqPayClient = new LiqPayClient("sandbox_i27554924732", "sandbox_g1W9WZTQKFBN42vBHv0gQ632CaP3teSVJYBlCc2n");
            //liqPayClient.IsCnbSandbox = true;
            //var form = liqPayClient.CNBForm(invoiceRequest);
            //orderHeader.btn = form;

            return View(orderHeader);
        }

        public IActionResult Remove(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                // Сессия существует
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            shoppingCartList.Remove(shoppingCartList.FirstOrDefault(u => u.ProductId == id));
            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);
            //List<int> prodInCart = shoppingCartList.Select(i => i.ProductId).ToList();
            //IEnumerable<Product> prodList = _db.Product.Include(u => u.Category).Include(u => u.ApplicationType).Include(u => u.SizeType).Where(u => prodInCart.Contains(u.Id));



            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateCart(IEnumerable<Product> Products, Coupon coupon = null)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            foreach (Product prod in Products)
            {
                shoppingCartList.Add(new ShoppingCart { ProductId = prod.Id, CountFt = prod.TempCount });
            }
            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);
            if (coupon != null) coupon.Picture = null;
            return RedirectToAction(nameof(Index), coupon);
        }

        public IActionResult Clear()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcelDownload(IFormCollection collection, ProductUserVM ProductUserVM)
        {
            var command = new OrderExportCommand
            {
                OrderDate = DateTime.Now,
                OrderHeader = ProductUserVM.OrderHeader,
                ProductList = ProductUserVM.ProductList,
            };


            command.Discount = command.Total * (GetCoupon()?.Discount ?? 0) / 100;
            //command.Discount = command.Total * GetCoupon().Discount / 100;


            command.ApplicationUser = ProductUserVM.ApplicationUser ?? ProductUserVM.OrderHeader.ToApplicationUser();
            
             //   command.ApplicationUser = OrderHeaderMapper.ToApplicationUser(ProductUserVM.OrderHeader);
            //if(ProductUserVM.OrderHeader == null || string.IsNullOrEmpty(ProductUserVM.OrderHeader.Id.ToString()){
            //    ProductUserVM.OrderHeader = new OrderHeader
            //    {
            //        Id = ProductUserVM.
            //    }
            //}
            var result = await _exelDow.GenerateAsync(command);

            
            
            return File(result, WC.XLSX, $"Реликт_Арте_Счет-Заказ_№_{ProductUserVM.OrderHeader.Id}.xlsx");

        }
    }
}


