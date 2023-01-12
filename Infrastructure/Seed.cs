using Core.Models;

namespace Infrastructure
{
  public class Seed
    {
        public static async Task SeedData(DataContext context)
        {
            if (
                context.Posts.Any() 
                || context.Users.Any() 
                || context.Categories.Any()
            ) return;

            List<string> userNames = new List<string>
            {
                "Eric", "Max", "Monica", "Rick", "Joe"
            };
            List<User> users = new();
            List<Post> posts = new();
            List<Post> replies = new();

            foreach (string user in userNames)
            {
                users.Add(
                    new User
                    {
                        UserName=user, CreatedDate=DateTime.UtcNow
                    }
                );
            }

            var categories = new List<Category>
            {
                new Category
                {
                    CreatedDate = DateTime.UtcNow,
                    Name = "General",
                    Description = "General Stuff"
                },
            };

            var rand = new Random();
            for (int i = 1; i <= 15; i++)
            {
                int index = rand.Next(userNames.Count);
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

            await context.Users.AddRangeAsync(users);
            await context.Categories.AddRangeAsync(categories);
            await context.Posts.AddRangeAsync(posts);
            await context.Posts.AddRangeAsync(replies);
            await context.SaveChangesAsync();
        }
    }
}