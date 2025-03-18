using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogSite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using Microsoft.Extensions.Hosting;

namespace BlogSite.Controllers
{
    public class PostsController : Controller
    {
        private readonly AppDbContext _context;
        public readonly UserManager<ApplicationUser> _userManager;
        public PostsController(AppDbContext context,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Posts
        public async Task<IActionResult> Index(string searchQuery,int page=1,int pageSize = 10)
        {
            var postsQuery = _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreatedAt)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                postsQuery = postsQuery.Where(p =>
                p.Title.Contains(searchQuery) ||
                p.Content.Contains(searchQuery) ||
                p.Category.Name.Contains(searchQuery));
            }

            var totalPosts = await postsQuery.CountAsync();
            var totalPages = (int)Math.Ceiling(totalPosts / (double)pageSize);
            var posts = await postsQuery.Skip((page-1) * pageSize).Take(pageSize).ToListAsync();

            ViewData["Currentfilter"] = searchQuery;
            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = totalPages;
            ViewData["PageSize"] = pageSize;

            return View(posts);
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Author)
                .Include(x => x.Category)
                .Include(c => c.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        [Authorize(Roles ="Admin")]
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Post post)
        {
            if (!ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                post.AuthorId = user.Id;
                post.CreatedAt = DateTime.Now;
                Debug.WriteLine($"Post details: Title={post.Title}, Content={post.Content}, ImageUrl={post.ImageUrl},Category = {post.CategoryId}");
                try
                {
                    _context.Add(post);
                    await _context.SaveChangesAsync();
                    Debug.WriteLine("Post saved successfully.");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error saving post: {ex.Message}");
                }
                return RedirectToAction(nameof(Index));
            }
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Debug.WriteLine($"Validation error: {error.ErrorMessage}");
            }
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", post.CategoryId);
            return View(post);
        }

        // GET: Posts/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name");
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int id, Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", post.CategoryId);
                return View(post);
            }

            try
            {
                var existingPost = await _context.Posts.FindAsync(id);
                if (existingPost == null)
                {
                    return NotFound();
                }

                // Preserve the AuthorId
                post.AuthorId = existingPost.AuthorId;
                post.CreatedAt = existingPost.CreatedAt; // Preserve the original creation time

                _context.Entry(existingPost).CurrentValues.SetValues(post);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating post: {ex.Message}");
                ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", post.CategoryId);
                return View(post);
            }
        }


        // GET: Posts/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Author)
                .Include(x => x.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
