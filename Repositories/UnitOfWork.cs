using TodoApi.Models;

namespace TodoApi.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TodoContext _context;

        public IRepository<Architect> Architects { get; }
        public IRepository<Category> Categories { get; }
        public IRepository<Project> Projects { get; }
        public IRepository<Review> Reviews { get; }
        public IRepository<User> Users { get; }

        public UnitOfWork(TodoContext context)
        {
            _context = context;
            Architects = new GenericRepository<Architect>(context);
            Categories = new GenericRepository<Category>(context);
            Projects = new GenericRepository<Project>(context);
            Reviews = new GenericRepository<Review>(context);
            Users = new GenericRepository<User>(context);
        }

        public async Task<int> CompleteAsync()
            => await _context.SaveChangesAsync();

        public void Dispose()
            => _context.Dispose();
    }
}