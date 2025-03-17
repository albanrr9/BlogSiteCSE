using BlogSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Controllers
{
    public class CommentsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public CommentsController(AppDbContext context,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(int postId,string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized();
                }
                var comment = new Comment
                {
                    Content = content,
                    UserId = user.Id,
                    PostId = postId,
                    CreatedAt = DateTime.Now
                };
                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Details","Posts",new {id =postId});
        }
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var comment = await _context.Comments.FindAsync(id);
            if(comment == null || comment.UserId != _userManager.GetUserId(User))
            {
                return NotFound();
            }
            return View(comment);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if(comment != null && comment.UserId == _userManager.GetUserId(User))
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Details","Posts",new {id = comment?.PostId});
        }
    }
}
