using Infrastructure;

namespace API.Services
{
    public interface IPostQueryService
    {

    }

    public class PostQueryService : IPostQueryService
    {
        private readonly DataContext _context;

        public PostQueryService(DataContext context)
        {
            _context = context;
        }
    }
}
