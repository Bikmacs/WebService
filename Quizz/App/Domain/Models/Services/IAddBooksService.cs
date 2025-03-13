using Quizz.App.Domain.Models.User;
using System.Threading.Tasks;

namespace Quizz.App.Domain.Models.Services
{
    public interface IAddBooksService
    {
        Task<bool> AddBook(Book book);
        Task<bool> DeleteBook(Book book);
    }
}