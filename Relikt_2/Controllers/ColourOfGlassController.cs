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
    public class ColourOfGlassController : Controller
    {
        private readonly IColourOfGlassRepository _colourOfGlassRepo;
        public ColourOfGlassController(IColourOfGlassRepository colourOfGlassRepo)
        {
            _colourOfGlassRepo = colourOfGlassRepo;
        }
        public IActionResult Index()
        {
            IEnumerable<ColourOfGlass> objList = _colourOfGlassRepo.GetAll();
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
        public IActionResult Create(ColourOfGlass obj)
        {
            if (ModelState.IsValid)
            {
                _colourOfGlassRepo.Add(obj);
                _colourOfGlassRepo.Save();
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

            var obj = _colourOfGlassRepo.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ColourOfGlass obj)
        {
            if (ModelState.IsValid)
            {
                _colourOfGlassRepo.Update(obj);
                _colourOfGlassRepo.Save();
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

            var obj = _colourOfGlassRepo.Find(id.GetValueOrDefault());
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
            var obj = _colourOfGlassRepo.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            _colourOfGlassRepo.Remove(obj);
            _colourOfGlassRepo.Save();
            return RedirectToAction("Index");


        }
    }
}
