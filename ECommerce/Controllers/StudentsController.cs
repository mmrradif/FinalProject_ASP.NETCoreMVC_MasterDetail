using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ECommerce.Models;

namespace ECommerce.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment host;

        public StudentsController(ApplicationDbContext db, IWebHostEnvironment host)
        {
            this.db = db;
            this.host = host;
        }

        public IActionResult Index()
        {
            var data = db.Students.Include(x=>x.StudentEntries).ThenInclude(x=>x.Course).ToList();
            return View(data);
        }

        public IActionResult AddNewCourse(int? id)
        {
            ViewBag.courses = new SelectList(db.Courses.ToList(),"CourseId","CourseName", id.ToString()??"");
            return PartialView("_addNewCourse");
        }

        public IActionResult Create()
        {
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StudentVM studentVM, int[] CourseId)
        {
            if (ModelState.IsValid)
            {
                Student student = new Student()
                {
                    StudentName = studentVM.StudentName,
                    Age = studentVM.Age,
                    AdmissionDate = studentVM.AdmissionDate,
                    IsRegular = studentVM.IsRegular
                };

                IFormFile pictureFile = studentVM.PictureFile;
                if (pictureFile != null)
                {
                    string webroot = host.WebRootPath;
                    string folder = "Images";
                    string imgFileName = Path.GetFileName(pictureFile.FileName);
                    string fileToWrite = Path.Combine(webroot,folder, imgFileName);

                    using (var stream = new FileStream(fileToWrite,FileMode.Create))
                    {
                        pictureFile.CopyTo(stream);
                        student.PicturePath = "/" + folder + "/" + imgFileName;
                    }
                }

                foreach (var item in CourseId)
                {
                    StudentEntry studentEntry = new StudentEntry()
                    {
                        Student = student,
                        StudentId = student.StudentId,
                        CourseId = item
                    };
                    db.StudentEntries.Add(studentEntry);
                }
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            Student student = db.Students.First(x=>x.StudentId==id);
            var courses = db.StudentEntries.Where(x=>x.StudentId == id).Select(x=>x.CourseId).ToList();
            StudentVM vm = new StudentVM()
            {
                StudentId = student.StudentId,
                StudentName = student.StudentName,
                Age = student.Age,
                AdmissionDate = student.AdmissionDate,
                IsRegular = student.IsRegular,
                PicturePath = student.PicturePath,
                CourseList = courses
            };
            return View(vm);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(StudentVM studentVM, int[] CourseId)
        {
            if (ModelState.IsValid)
            {
                Student student = new Student()
                {
                    StudentId= studentVM.StudentId,
                    StudentName = studentVM.StudentName,
                    Age = studentVM.Age,
                    AdmissionDate = studentVM.AdmissionDate,
                    IsRegular = studentVM.IsRegular,
                    PicturePath = studentVM.PicturePath
                };

                IFormFile pictureFile = studentVM.PictureFile;
                if (pictureFile != null)
                {
                    string webroot = host.WebRootPath;
                    string folder = "Images";
                    string imgFileName = Path.GetFileName(pictureFile.FileName);
                    string fileToWrite = Path.Combine(webroot, folder, imgFileName);

                    using (var stream = new FileStream(fileToWrite, FileMode.Create))
                    {
                        pictureFile.CopyTo(stream);
                        student.PicturePath = "/" + folder + "/" + imgFileName;
                    }
                }

                var courses = db.StudentEntries.Where(x=>x.StudentId==studentVM.StudentId).ToList();
                foreach (var item in courses)
                {
                    db.StudentEntries.Remove(item);
                }

                foreach (var item in CourseId)
                {
                    StudentEntry studentEntry = new StudentEntry()
                    {
                        StudentId = student.StudentId,
                        CourseId = item
                    };
                    db.StudentEntries.Add(studentEntry);
                }
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            Student student = db.Students.First(x=>x.StudentId ==id);
            var courses = db.StudentEntries.Where(x=>x.StudentId==id).ToList();
            foreach (var item in courses)
            {
                db.StudentEntries.Remove(item);
            }
            db.Entry(student).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

       
    }
}
