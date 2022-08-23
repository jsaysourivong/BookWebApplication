
using BookWeb.DataAccess;
using BookWeb.DataAccess.Repository;
using BookWeb.DataAccess.Repository.IRepository;
using BookWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookWebApp.Controllers;
public class CategoryController : Controller
{
    private readonly IUnitOfWork  _unitOfWork;

    public CategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
     {
        IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll(); ;
       return View(objCategoryList);
    }

    // GET
    public IActionResult Create()
    {
       return View();
    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Category obj)
    {
        if (obj.Name == obj.DisplayOrder.ToString())
        {
            ModelState.AddModelError("name ", "The DisplayOrder cannot exactly match the Name.");
        }
        if (ModelState.IsValid)
        {
            _unitOfWork.Category.Add(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category created sucessfully";   
            return RedirectToAction("Index");
        }
        return View(obj);
    }

    // GET
    public IActionResult Edit(int? id)
    {
        if(id == null || id == 0)
        {
            return NotFound();
        }

        // The three lines of code following this commenta are three ways you can retreive a category from the database using the primary key
        //var categoryFromDb = _db.Categories.Find(id);
        var categoryFromDbFirst = _unitOfWork.Category.GetFirstOrDefault(u=>u.Id == id);
        //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);

        if(categoryFromDbFirst == null)
        {
            return NotFound(); 
        }            

        return View(categoryFromDbFirst);
    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Category obj)
    {
        if (obj.Name == obj.DisplayOrder.ToString())
        {
            ModelState.AddModelError("name ", "The DisplayOrder cannot exactly match the Name.");
        }
        if (ModelState.IsValid)
        {
            _unitOfWork.Category.Update(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category updated sucessfully";
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
        var categoryFromDbFirst = _unitOfWork.Category.GetFirstOrDefault(u=>u.Id == id);
        //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);

        if (categoryFromDbFirst == null)
        {
            return NotFound();
        }

        return View(categoryFromDbFirst);
    }

    // POST
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePOST(int? id)
    {
        var obj = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
        if (obj == null)
        {
            return NotFound();
        }

        _unitOfWork.Category.Remove(obj);
        _unitOfWork.Save();
        TempData["success"] = "Category deleted sucessfuly";
        return RedirectToAction("Index");
    }
}

