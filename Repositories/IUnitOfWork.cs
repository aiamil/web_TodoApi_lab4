using TodoApi.Models;

namespace TodoApi.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Architect> Architects { get; }
        IRepository<Category> Categories { get; }
        IRepository<Project> Projects { get; }
        IRepository<Review> Reviews { get; }
        IRepository<User> Users { get; }
        Task<int> CompleteAsync();
    }
}