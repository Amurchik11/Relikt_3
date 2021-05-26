using Relikt_2_Models;

namespace Relikt_2.Mapper
{
    public static class OrderHeaderMapper
    {
        public static ApplicationUser ToApplicationUser(this OrderHeader orderHeader)
        {
            return new ApplicationUser()
            {
                FullName = orderHeader?.FullName ?? "",
                PhoneNumber = orderHeader?.PhoneNumber ?? ""
            };
        }
    }
    
}
