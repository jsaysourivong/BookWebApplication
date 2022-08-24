
using BookWeb.DataAccess;
using BookWeb.DataAccess.Repository;
using BookWeb.DataAccess.Repository.IRepository;
using BookWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookWebApp.Areas.Admin.Controllers;
[Area("Admin")]
public class CoverTypeController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CoverTypeController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
        IEnumerable<CoverType> objCoverTypeList = _unitOfWork.CoverType.GetAll(); ;
        return View(objCoverTypeList);
    }

    // GET
    public IActionResult Create()
    {
        return View();
    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CoverType obj)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.CoverType.Add(obj);
            _unitOfWork.Save();
            TempData["success"] = "CoverType created sucessfully";
            return RedirectToAction("Index");
        }
        return View(obj);
    }

    // GET
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        // The three lines of code following this comments are three ways you can retreive a category from the database using the primary key
        //var categoryFromDb = _db.CoverType.Find(id);
        var coverTypeFromDbFirst = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
        //var categoryFromDbSingle = _db.CoverType.SingleOrDefault(u => u.Id == id);

        if (coverTypeFromDbFirst == null)
        {
            return NotFound();
        }

        return View(coverTypeFromDbFirst);
    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(CoverType obj)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.CoverType.Update(obj);
            _unitOfWork.Save();
            TempData["success"] = "CoverType updated sucessfully";
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
        var coverTypeFromDbFirst = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
        //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);

        if (coverTypeFromDbFirst == null)
        {
            return NotFound();
        }

        return View(coverTypeFromDbFirst);
    }

    // POST
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePOST(int? id)
    {
        var obj = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
        if (obj == null)
        {
            return NotFound();
        }

        _unitOfWork.CoverType.Remove(obj);
        _unitOfWork.Save();
        TempData["success"] = "CoverType deleted sucessfuly";
        return RedirectToAction("Index");
    }
}

