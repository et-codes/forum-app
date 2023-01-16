using Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure
{
  public class Seed
    {
        public static async Task SeedData(DataContext context, UserManager<User> userManager)
        {
            var posts = new List<Post>();
            var replies = new List<Post>();

            if (!userManager.Users.Any())
            {
                var users = new List<User>();                
                users.Add(new User{DisplayName = "Eric", UserName = "eric", Email = "eric@email.com"});
                users.Add(new User{DisplayName = "Monica", UserName = "monica", Email = "monica@email.com"});
                users.Add(new User{DisplayName = "Max", UserName = "max", Email = "max@email.com"});

                foreach (var user in users)
                {
                    // UserManager saves on creation, don't need SaveChangesAsync
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                }
            }

            if (!context.Categories.Any())
            {
                var categories = new List<Category>();
                categories.Add(new Category
                    {
                        CreatedDate = DateTime.UtcNow,
                        Name = "General",
                        Description = "General Stuff"
                    }
                );

                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }

            if (!context.Posts.Any())
            {
                var users = userManager.Users.ToList();
                var categories = context.Categories.ToList();
                var rand = new Random();
                for (int i = 1; i <= 15; i++)
                {
                    int index = rand.Next(users.Count - 1);
                    posts.Add(
                        new Post{
                            PostCategory = categories[0],
                            CreatedDate = DateTime.UtcNow,
                            Author = users[index],
                            Title = $"Test post {i}",
                            Text = "This is a test post.",
                        }
                    );

                    replies.Add(
                        new Post
                        {
                            PostCategory = categories[0],
                            CreatedDate = DateTime.UtcNow,
                            Author = users[index],
                            InReplyTo = posts[i - 1],
                            Title = $"Reply post {i}",
                            Text = "This is a reply post.",
                        }
                    );
                }

                await context.Posts.AddRangeAsync(posts);
                await context.Posts.AddRangeAsync(replies);
                await context.SaveChangesAsync();
            }
        }
    }
}