using BlogSite.Models;
using Microsoft.AspNetCore.Identity;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var context = serviceProvider.GetRequiredService<AppDbContext>();

        // Create a sample user
        var user = new ApplicationUser
        {
            UserName = "testuser@example.com",
            Email = "testuser@example.com",
            FirstName = "Test",
            LastName = "Test",
            PhoneNumber = "1234567890",
        };

        if (await userManager.FindByEmailAsync(user.Email) == null)
        {
            await userManager.CreateAsync(user, "Test@123");
        }

        // Add a sample post
        if (!context.Posts.Any())
        {
            context.Posts.Add(new Post
            {
                Title = "Sample Post",
                Content = "This is a sample post.",
                ImageUrl = "https://example.com/sample.jpg",
                CreatedAt = DateTime.UtcNow,
                AuthorId = user.Id
            });

            await context.SaveChangesAsync();
        }
    }
}