using ExtCore.Data.Abstractions;
using Identity.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Posts.Data.Abstractions;
using Posts.Data.Entities;
using System;

namespace Posts.Controllers
{
    public class PostsController : Controller
    {
        private IStorage storage;
        private readonly UserManager<ApplicationUser> _userManager;
        private IPostRepository postRepo;

        public PostsController(IStorage storage, UserManager<ApplicationUser> userManager)
        {
            this.storage = storage;
            _userManager = userManager;
            postRepo = this.storage.GetRepository<IPostRepository>();
        }

        // GET: PostsController
        public ActionResult Index()
        {
            return View(postRepo.All());
        }

        // GET: PostsController/Details/5
        public ActionResult Details(Guid id)
        {
            Post post = postRepo.FindById(id);
            return View(post);
        }

        // GET: PostsController/Create
        public ActionResult Create(Guid id)
        {
            Post post = new Post();
            post.SubjectId = id;
            return View(post);
        }

        // POST: PostsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm] Post post)
        {
            if (ModelState.IsValid)
            {
                //laboratory.Id = Guid.NewGuid();
                post.DateCreated = DateTime.Now;
                postRepo.Create(post);
                this.storage.Save();
                return RedirectToAction("Index", "Courses");
            }
            return this.View();
        }

        // GET: PostsController/Edit/5
        public ActionResult Edit(Guid id)
        {
            Post post = postRepo.FindById(id);
            return View(post);
        }

        // POST: PostsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, [Bind("Id,Name,Description,SubjectId,DateCreated")] Post post)
        {
            post.Id = id;
            postRepo.Edit(post);
            this.storage.Save();
            return RedirectToAction(nameof(Index));
        }

        // GET: PostsController/Delete/5
        public ActionResult Delete(Guid id)
        {
            Post post = postRepo.FindById(id);
            return View(post);
        }

        // POST: PostsController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            postRepo.Delete(id);
            this.storage.Save();
            return RedirectToAction(nameof(Index));

        }
    }
}
