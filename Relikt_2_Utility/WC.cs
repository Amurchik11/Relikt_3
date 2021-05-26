using Relikt_2_Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Relikt_2_Utility
{
    public static class WC
    {
        public const string ImagePath = @"\images\product\";
        public const string SessionCart = "ShoppingCartSession";
        public const string SessionInquiryId = "InquirySession";

        public const string AdminRole = "Admin";
        public const string CustomerRole = "Customer";

        public const string EmailAdmin = "dolmatov1406@gmail.com";

        public const string CategoryName = "Category";
        public const string ApplicationTypeName = "ApplicationType";
        public const string SizeTypeName = "SizeType";
        public const string RightLeftName = "RightLeft";
        public const string ColourOfGlassName = "ColourOfGlass";

        public const string Success = "Success";
        public const string Error = "Error";

        public const string StatusPending = "В Ожидании";
        public const string StatusApproved = "Подтвержден";
        public const string StatusInProcess = "В Работе";
        //public const string StatusReady = "Готов";
        public const string StatusShipped = "Готов";
        public const string StatusCancelled = "Отгружен";
        public const string StatusRefunded = "Возврат";

        public const string ssCouponCode = "ssCouponCode";


        public static double DiscountedPrice(Coupon couponFromDb, double OriginalOrderTotal)
        {
            if (couponFromDb == null)
            {
                return OriginalOrderTotal;
            }
            else
            {
                if (couponFromDb.MinimumAmount > OriginalOrderTotal)
                {
                    return OriginalOrderTotal;
                }
                else
                {
                    // все действительно
                    if (Convert.ToInt32(couponFromDb.CouponType) == (int)Coupon.ECouponType.Гривна)
                    {
                        // 10 гривень со 100
                        return Math.Round(OriginalOrderTotal - couponFromDb.Discount, 2);
                    }
                    if (Convert.ToInt32(couponFromDb.CouponType) == (int)Coupon.ECouponType.Процент)
                    {
                        //10% со $100
                        return Math.Round(OriginalOrderTotal - (OriginalOrderTotal * couponFromDb.Discount / 100), 2);
                    }
                }
            }
            return OriginalOrderTotal;
        }

        public static readonly IEnumerable<string> listStatus = new ReadOnlyCollection<string>(
            new List<string>
            {
                StatusPending,StatusInProcess,StatusShipped,StatusCancelled
            });


        public static readonly string XLSX = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    }

}

