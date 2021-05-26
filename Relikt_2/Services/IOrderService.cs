using Relikt_2_Models.ViewModels;

namespace Relikt_2.Services
{
    public interface IOrderService
    {
        OrderVM GetOrderById(int id);
    }
}