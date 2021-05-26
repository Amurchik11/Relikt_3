using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Relikt_2_DataAccess;
using Relikt_2_Models;
using Relikt_2_Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Relikt_2.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class CouponController : Controller
    {
        private readonly ApplicationDbContext _coupRepo;

        public CouponController(ApplicationDbContext coupRepo)
        {
            _coupRepo = coupRepo;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _coupRepo.Coupon.ToListAsync());
        }

        // Создание Купона
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Coupon coupons)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] p1 = null;
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                    }
                    coupons.Picture = p1;
                }
                _coupRepo.Coupon.Add(coupons);
                await _coupRepo.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(coupons);
        }


        //GET Редактирование Купона
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var coupon = await _coupRepo.Coupon.SingleOrDefaultAsync(m => m.Id == id);
            if (coupon == null)
            {
                return NotFound();
            }
            return View(coupon);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Coupon coupons)
        {
            if (coupons.Id == 0)
            {
                return NotFound();
            }

            var couponFromDb = await _coupRepo.Coupon.Where(c => c.Id == coupons.Id).FirstOrDefaultAsync();

            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] p1 = null;
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                    }
                    couponFromDb.Picture = p1;
                }
                couponFromDb.MinimumAmount = coupons.MinimumAmount;
                couponFromDb.Name = coupons.Name;
                couponFromDb.Discount = coupons.Discount;
                couponFromDb.CouponType = coupons.CouponType;
                couponFromDb.IsActive = coupons.IsActive;

                await _coupRepo.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(coupons);
        }




        //GET Удаление Купона
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var coupon = await _coupRepo.Coupon.SingleOrDefaultAsync(m => m.Id == id);
            if (coupon == null)
            {
                return NotFound();
            }
            return View(coupon);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coupons = await _coupRepo.Coupon.SingleOrDefaultAsync(m => m.Id == id);
            _coupRepo.Coupon.Remove(coupons);
            await _coupRepo.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
