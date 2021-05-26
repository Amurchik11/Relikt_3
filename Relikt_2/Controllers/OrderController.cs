using Microsoft.AspNetCore.Mvc;
using Relikt_2_DataAccess.Repository.IRepository;
using Relikt_2_Models.ViewModels;
using Relikt_2_Utility;
using Relikt_2_Utility.BrainTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Relikt_2_Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Relikt_2.Services;
using Relikt_2.Mapper;

namespace Relikt_2.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICategoryRepository _catRepo;
        private readonly IApplicationTypeRepository _appTypeRepo;
        private readonly ISizeTypeRepository _sizeTypeRepo;
        private readonly IRightLeftRepository _rightLeftRepo;
        private readonly IColourOfGlassRepository _colourOfGlassRepo;
        private readonly IProductRepository _prodRepo;
        private readonly IOrderHeaderRepository _orderHRepo;
        private readonly IOrderDetailRepository _orderDRepo;
        private readonly ICouponRepository _coupRepo;
        private readonly IExcelDownload _excelDownload;
        //private readonly IBrainTreeGate _brain;

        [BindProperty]
        public OrderVM OrderVM { get; set; }
        public OrderController(ICategoryRepository catRepo, IApplicationTypeRepository appTypeRepo,
            ISizeTypeRepository sizeTypeRepo, IRightLeftRepository rightLeftRepo,
            IColourOfGlassRepository colourOfGlassRepo, IProductRepository prodRepo,
            IOrderHeaderRepository orderHRepo, IOrderDetailRepository orderDRepo,
            ICouponRepository coupRepo, IExcelDownload excelDownload
, IOrderService orderService
            //IBrainTreeGate brain
            )
        {
            _catRepo = catRepo;
            _appTypeRepo = appTypeRepo;
            _sizeTypeRepo = sizeTypeRepo;
            _rightLeftRepo = rightLeftRepo;
            _colourOfGlassRepo = colourOfGlassRepo;
            _prodRepo = prodRepo;
            _orderDRepo = orderDRepo;
            _orderHRepo = orderHRepo;
            _coupRepo = coupRepo;
            _excelDownload = excelDownload;
            _orderService = orderService;
            //_brain = brain;
        }
        public IActionResult Index(string searchName = null, string searchEmail = null, string searchPhone = null, string Status = null)
        {
            OrderListVM orderListVM = new OrderListVM()
            {
                OrderHList = _orderHRepo.GetAll(),


                StatusList = WC.listStatus.ToList().Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = i,
                    Value = i
                })
            };

            if (!string.IsNullOrEmpty(searchName))
            {
                orderListVM.OrderHList = orderListVM.OrderHList.Where(u => u.FullName.ToLower().Contains(searchName.ToLower()));
            }
            if (!string.IsNullOrEmpty(searchEmail))
            {
                orderListVM.OrderHList = orderListVM.OrderHList.Where(u => u.Email.ToLower().Contains(searchEmail.ToLower()));
            }
            if (!string.IsNullOrEmpty(searchPhone))
            {
                orderListVM.OrderHList = orderListVM.OrderHList.Where(u => u.PhoneNumber.ToLower().Contains(searchPhone.ToLower()));
            }
            if (!string.IsNullOrEmpty(Status) && Status != "--Статус Заказа--")
            {
                orderListVM.OrderHList = orderListVM.OrderHList.Where(u => u.OrderStatus.ToLower().Contains(Status.ToLower()));
            }

            return View(orderListVM);
        }

        public IActionResult Details(int id)
        {
            OrderVM = _orderService.GetOrderById(id);   

            return View(OrderVM);
        }
        [HttpPost]
        public IActionResult StartProcessing()
        {
            OrderHeader orderHeader = _orderHRepo.FirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);
            orderHeader.OrderStatus = WC.StatusInProcess;
            _orderHRepo.Save();
            TempData[WC.Success] = "Заказ В Работе";
            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        public IActionResult ShipOrder()
        {
            OrderHeader orderHeader = _orderHRepo.FirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);
            orderHeader.OrderStatus = WC.StatusShipped;
            orderHeader.ShippingDate = DateTime.Now;
            _orderHRepo.Save();
            TempData[WC.Success] = "Готов";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult CancelOrder()
        {
            OrderHeader orderHeader = _orderHRepo.FirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);
            orderHeader.OrderStatus = WC.StatusCancelled;
            orderHeader.CancellDate = DateTime.Now;
            _orderHRepo.Save();
            TempData[WC.Success] = "Отгружен";
            return RedirectToAction(nameof(Index));
        }
        //[HttpPost]
        //public IActionResult CancelOrder()
        //{
        //    OrderHeader orderHeader = _orderHRepo.FirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);

        //    var gateway = _brain.GetGateway();
        //    Transaction transaction = gateway.Transaction.Find(orderHeader.TransactionId);

        //    if (transaction.Status == TransactionStatus.AUTHORIZED || transaction.Status == TransactionStatus.SUBMITTED_FOR_SETTLEMENT)
        //    {
        //        //no refund
        //        Result<Transaction> resultvoid = gateway.Transaction.Void(orderHeader.TransactionId);
        //    }
        //    else
        //    {
        //        //refund
        //        Result<Transaction> resultRefund = gateway.Transaction.Refund(orderHeader.TransactionId);
        //    }
        //    orderHeader.OrderStatus = WC.StatusRefunded;
        //    _orderHRepo.Save();
        //    TempData[WC.Success] = "Заказ Закрыт Успешно";
        //    return RedirectToAction(nameof(Index));
        //}

        [HttpPost]
        public IActionResult UpdateOrderDetails()
        {
            OrderHeader orderHeaderFromDb = _orderHRepo.FirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);
            orderHeaderFromDb.FullName = OrderVM.OrderHeader.FullName;
            orderHeaderFromDb.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
            orderHeaderFromDb.StreetAddress = OrderVM.OrderHeader.StreetAddress;
            orderHeaderFromDb.City = OrderVM.OrderHeader.City;
            orderHeaderFromDb.Region = OrderVM.OrderHeader.Region;
            orderHeaderFromDb.PostalCode = OrderVM.OrderHeader.PostalCode;
            orderHeaderFromDb.Email = OrderVM.OrderHeader.Email;
            orderHeaderFromDb.Comments = OrderVM.OrderHeader.Comments;

            _orderHRepo.Save();
            TempData[WC.Success] = "Детали заказа успешно обновлены";

            return RedirectToAction("Details", "Order", new { id = orderHeaderFromDb.Id });
        }

        [HttpGet]
        public async Task<IActionResult> ExcelDownload(int id)
        {
            OrderVM order = _orderService.GetOrderById(id);

            var productUserVm = new OrderExportCommand
            {
                OrderHeader = order.OrderHeader,
                ProductList = order.OrderDetail.Select(od =>
                {
                    var product =  od.Product;
                    product.TempCount = od.UnitCount;
                    return product;
                }).ToList(),
                Discount = order.OrderHeader.CouponCodeDiscount,
                ApplicationUser = order.OrderHeader.ToApplicationUser(),
                OrderDate = order.OrderHeader.OrderDate,
            };

            var result = await _excelDownload.GenerateAsync(productUserVm);
            return File(result, WC.XLSX, $"Реликт_Арте_Счет-Заказ_№_{productUserVm.OrderHeader.Id}.xlsx");
        }

    }
}
