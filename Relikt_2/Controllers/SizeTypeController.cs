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
    public class SizeTypeController : Controller
    {
        private readonly ISizeTypeRepository _sizeTypeRepo;
        public SizeTypeController(ISizeTypeRepository sizeTypeRepo)
        {
            _sizeTypeRepo = sizeTypeRepo;
        }
        public IActionResult Index()
        {
            IEnumerable<SizeType> objList = _sizeTypeRepo.GetAll();
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
        public IActionResult Create(SizeType obj)
        {
            if (ModelState.IsValid)
            {
                _sizeTypeRepo.Add(obj);
                _sizeTypeRepo.Save();
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

            var obj = _sizeTypeRepo.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(SizeType obj)
        {
            if (ModelState.IsValid)
            {
                _sizeTypeRepo.Update(obj);
                _sizeTypeRepo.Save();
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

            var obj = _sizeTypeRepo.Find(id.GetValueOrDefault());
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
            var obj = _sizeTypeRepo.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            _sizeTypeRepo.Remove(obj);
            _sizeTypeRepo.Save();
            return RedirectToAction("Index");


        }
    }
}
