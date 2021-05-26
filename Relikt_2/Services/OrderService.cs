using Relikt_2_DataAccess.Repository.IRepository;
using Relikt_2_Models.ViewModels;

namespace Relikt_2.Services
{
    public class OrderService : IOrderService
    {
        private readonly ICategoryRepository _catRepo;
        private readonly IApplicationTypeRepository _appTypeRepo;
        private readonly ISizeTypeRepository _sizeTypeRepo;
        private readonly IRightLeftRepository _rightLeftRepo;
        private readonly IColourOfGlassRepository _colourOfGlassRepo;
        private readonly IOrderHeaderRepository _orderHRepo;
        private readonly IOrderDetailRepository _orderDRepo;
        private readonly ICouponRepository _coupRepo;

        public OrderService(ICategoryRepository catRepo, IApplicationTypeRepository appTypeRepo, ISizeTypeRepository sizeTypeRepo, IRightLeftRepository rightLeftRepo, IColourOfGlassRepository colourOfGlassRepo, IOrderHeaderRepository orderHRepo, IOrderDetailRepository orderDRepo, ICouponRepository coupRepo)
        {
            _catRepo = catRepo;
            _appTypeRepo = appTypeRepo;
            _sizeTypeRepo = sizeTypeRepo;
            _rightLeftRepo = rightLeftRepo;
            _colourOfGlassRepo = colourOfGlassRepo;
            _orderHRepo = orderHRepo;
            _orderDRepo = orderDRepo;
            _coupRepo = coupRepo;
        }

        public OrderVM GetOrderById(int id)
        {
            var orderVM = new OrderVM()
            {

                OrderHeader = _orderHRepo.FirstOrDefault(u => u.Id == id),
                OrderDetail = _orderDRepo.GetAll(o => o.OrderHeaderId == id, includeProperties: "Product,Category,ApplicationType,SizeType,RightLeft,ColourOfGlass"),

                Categories = _catRepo.GetAll(),
                ApplicationTypes = _appTypeRepo.GetAll(),
                SizeTypes = _sizeTypeRepo.GetAll(),
                RightLefts = _rightLeftRepo.GetAll(),
                ColourOfGlasses = _colourOfGlassRepo.GetAll(),
                //Coupon = _coupRepo.GetCoupons(orderID) 
                Coupons = _coupRepo.GetAll()
            };

            return orderVM;
        }
    }
}
