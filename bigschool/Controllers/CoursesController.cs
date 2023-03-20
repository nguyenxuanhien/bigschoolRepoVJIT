using bigschool.Models;
using bigschool.ViewModels;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace bigschool.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public CoursesController()
        {
            _dbContext = new ApplicationDbContext();
        }
        // GET: Courses
        public ActionResult Create()
        {
            var viewModel = new CourseViewModels
            {
                Categories = _dbContext.Categories.ToList(),
                Heading = "Add Course"
            };
            return View(viewModel);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CourseViewModels viewModels)
        {
            if (!ModelState.IsValid)
            {
               viewModels.Categories = _dbContext.Categories.ToList();
                return View("Create", viewModels);
            }
            var course = new Course
            {
                LecturerId = User.Identity.GetUserId(),
                DateTime = viewModels.GetDateTime(),
                CategoryId = viewModels.Category,
                Place = viewModels.Place,
            };
            _dbContext.Courses.Add(course);
            _dbContext.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();
            var courses = _dbContext.Attendances
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Course)
                .Include(l => l.Lecturer)
                .Include(l => l.Category)
                .ToList();
            var viewModel = new CourseViewModels
            {
                UpcommingCourses = courses ,
                ShowAction = User.Identity.IsAuthenticated
            };
            return View(viewModel);
                
        }
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();
            var courses = _dbContext.Courses
                .Where(c => c.LecturerId == userId && c.DateTime > DateTime.Now)
                .Include(l => l.Lecturer)
                .Include(c => c.Category)
                .ToList();
                return View(courses);
        }
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId() ;
            var courses = _dbContext.Courses.Single(c => c.Id == id && c.LecturerId == userId);
            var viewModel = new CourseViewModels
            {
                Categories = _dbContext.Categories.ToList(),
                Date = courses.DateTime.ToString("dd/MM/yyyy"),
                Time = courses.DateTime.ToString("HH:mm"),
                Category = courses.CategoryId,
                Place = courses.Place,
                Heading ="Edit Course",
                Id = courses.Id
            };
            return View("Create", viewModel);
        }
        public ActionResult Update (CourseViewModels viewModels)
        {
            if (!ModelState.IsValid)
            {
                viewModels.Categories = _dbContext.Categories.ToList();
                return View("Create", viewModels);
            }
            var userId = User.Identity.GetUserId();
            var courses = _dbContext.Courses.Single(c => c.Id == viewModels.Id && c.LecturerId == userId);
            courses.Place= viewModels.Place;
            courses.DateTime= viewModels.GetDateTime();
            courses.CategoryId = viewModels.Category;
            _dbContext.SaveChanges();
            return RedirectToAction("Index","Home");
        }

    }
}