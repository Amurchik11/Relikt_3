using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Relikt_2_DataAccess;
using Relikt_2_DataAccess.Repository.IRepository;
using Relikt_2_Models;
using Relikt_2_Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Relikt_2.Controllers
{
    [Authorize(Roles = WC.AdminRole)]

    public class RightLeftController : Controller
    {
        private readonly IRightLeftRepository _rightLeftRepo; 
        public RightLeftController(IRightLeftRepository rightLeftRepo) 
        {
            _rightLeftRepo = rightLeftRepo;
        }
        public IActionResult Index()
        {
            IEnumerable<RightLeft> objList = _rightLeftRepo.GetAll();
            return View(objList);
        }


        //GET - CREATE
        public IActionResult Create()
        {

            return View();
        }

        //POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RightLeft obj)
        {
            if (ModelState.IsValid)
            {
                _rightLeftRepo.Add(obj);
                _rightLeftRepo.Save();
                TempData[WC.Success] = "Действие выполнено успешно";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //GET - EDIT
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _rightLeftRepo.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(RightLeft obj)
        {
            if (ModelState.IsValid)
            {
                _rightLeftRepo.Update(obj);
                _rightLeftRepo.Save();
                TempData[WC.Success] = "Действие выполнено успешно";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _rightLeftRepo.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        //POST - DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _rightLeftRepo.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            _rightLeftRepo.Remove(obj);
            _rightLeftRepo.Save();
            TempData[WC.Success] = "Действие выполнено успешно";
            return RedirectToAction("Index");


        }
    }
}
