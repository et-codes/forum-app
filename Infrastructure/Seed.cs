using Core.Models;

namespace Infrastructure
{
  public class Seed
    {
        public static async Task SeedData(DataContext context)
        {
            if (context.Posts.Any() || context.Users.Any() || context.Categories.Any()) return;

            var users = new List<User>
            {
                new User {UserName = "Eric", CreatedDate = DateTime.UtcNow},
                new User {UserName = "Max", CreatedDate = DateTime.UtcNow},
                new User {UserName = "Monica", CreatedDate = DateTime.UtcNow},
            };

            var categories = new List<Category>
            {
                new Category {
                    CreatedDate = DateTime.UtcNow,
                    Name = "General",
                    Description = "General Stuff"
                },
            };

            var posts = new List<Post>
            {
                new Post
                {
                    PostCategory = categories[0],
                    CreatedDate = DateTime.UtcNow,
                    Author = users[0],
                    Title = "Test post 1",
                    Text = "This is a test post.",
                },
                new Post
                {
                    PostCategory = categories[0],
                    CreatedDate = DateTime.UtcNow,
                    Author = users[1],
                    Title = "Test post 2",
                    Text = "This is a test post.",
                },
                new Post
                {
                    PostCategory = categories[0],
                    CreatedDate = DateTime.UtcNow,
                    Author = users[2],
                    Title = "Test post 3",
                    Text = "This is a test post.",
                },
            };

            await context.Users.AddRangeAsync(users);
            await context.Categories.AddRangeAsync(categories);
            await context.Posts.AddRangeAsync(posts);
            await context.SaveChangesAsync();
        }
    }
}