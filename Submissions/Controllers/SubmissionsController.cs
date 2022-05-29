using Assignments.Data.Abstractions;
using Assignments.Data.Entities;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Submissions.Data.Abstractions;
using Submissions.Data.Entities;
using Submissions.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Submissions.Controllers
{
    public class SubmissionsController : Controller
    {
        private IStorage storage;
        private readonly IWebHostEnvironment webHostEnvironment;
        public SubmissionsController(IStorage storage, IWebHostEnvironment hostEnvironment)
        {
            this.storage = storage;
            webHostEnvironment = hostEnvironment;
        }
        // GET: SubmissionsController
        public ActionResult Index()
        {
            IEnumerable submissions = this.storage.GetRepository<ISubmissionRepository>().All();
            List<Tuple<Submission, Assignment>> list = new List<Tuple<Submission, Assignment>>();

            foreach (Submission submission in submissions)
            {
                Assignment assignment = this.storage.GetRepository<IAssignmentRepository>().FindById(submission.AssignmentId);
                list.Add(new Tuple<Submission, Assignment>(submission, assignment));
            }
            return View(list);
        }

        // GET: SubmissionsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SubmissionsController/Create
        public ActionResult Create(Guid id)
        {
            SubmissionViewModel submission = new SubmissionViewModel();
            submission.AssignmentId = id;
            return View(submission);
        }

        // POST: SubmissionsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm] SubmissionViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);
                Submission submission = new Submission
                {
                    FilePath = uniqueFileName,
                    AssignmentId = model.AssignmentId,
                    TurnedInDate = DateTime.Now
                };


                this.storage.GetRepository<ISubmissionRepository>().Create(submission);
                this.storage.Save();
                return RedirectToAction("Index", "Default");
            }
            return this.View();
        }

        private string UploadedFile(SubmissionViewModel model)
        {
            string uniqueFileName = null;
            if (model.FilePath != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = model.FilePath.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.FilePath.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        // GET: SubmissionsController/Edit/5
        public ActionResult Edit(Guid id)
        {
            SubmissionViewModel submission = new SubmissionViewModel();
            submission.AssignmentId = id;
            return View(submission);
        }

        // POST: SubmissionsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([FromForm] SubmissionViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);
                Submission submission = new Submission
                {
                    FilePath = uniqueFileName,
                    AssignmentId = model.AssignmentId,
                    TurnedInDate = DateTime.Now
                };

                this.storage.GetRepository<ISubmissionRepository>().Edit(submission);
                this.storage.Save();
                return RedirectToAction("Index", "Default");
            }
            return this.View();
        }

        // GET: SubmissionsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SubmissionsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
