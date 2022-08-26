﻿
using BookWeb.DataAccess;
using BookWeb.DataAccess.Repository;
using BookWeb.DataAccess.Repository.IRepository;
using BookWeb.Models;
using BookWeb.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookWebApp.Areas.Admin.Controllers;
[Area("Admin")]
public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _hostEnvironment;

    public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _hostEnvironment = hostEnvironment;
    }
    public IActionResult Index()
    {
        return View();
        // getting rid of this to use API 
        /*        IEnumerable<Product> objProductList = _unitOfWork.Product.GetAll(); ;
                return View(objProductList);*/
    }

    // GET
    public IActionResult Upsert(int? id)
    {
         ProductViewModel productViewModel = new()
        {
            Product = new(),
            CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            }),
            CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            })
        };


        if (id == null || id == 0)
        {
            // create product
/*          ViewBag.CategoryList = CategoryList;
            ViewData["CoverTypeList"] = CoverTypeList;*/
            return View(productViewModel);
        }
        else
        {
            // update product
        }
        return View(productViewModel);   
    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(ProductViewModel obj, IFormFile file)
    {
        if (ModelState.IsValid)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            if(file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"images\products");
                var extension = Path.GetExtension(file.FileName);

                using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                }
                obj.Product.ImageUrl = @"\images\products\" + fileName + extension;

            }
            _unitOfWork.Product.Add(obj.Product);
            _unitOfWork.Save();
            TempData["success"] = "Product created sucessfully";
            return RedirectToAction("Index");
        }
        return View(obj);
    }

    // GET
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        // The three lines of code following this commenta are three ways you can retreive a category from the database using the primary key
        //var categoryFromDb = _db.Categories.Find(id);
        var productFromDbFirst = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
        //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);

        if (productFromDbFirst == null)
        {
            return NotFound();
        }

        return View(productFromDbFirst);
    }

    // POST
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePOST(int? id)
    {
        var obj = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
        if (obj == null)
        {
            return NotFound();
        }

        _unitOfWork.Product.Remove(obj);
        _unitOfWork.Save();
        TempData["success"] = "CoverType deleted sucessfuly";
        return RedirectToAction("Index");
    }

    #region API CALLS
    [HttpGet]
    public IActionResult GetAll()
    {
        var productList = _unitOfWork.Product.GetAll();
        return Json(new { data = productList });
    }
    #endregion
}

