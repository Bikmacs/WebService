using Quizz.App.Domain.Models.User;

namespace Quizz.App.Domain.Models.Services.BookService
{
    public interface IAddBooksService
    {
        Task<bool> AddBook(Book book);
        Task<bool> DeleteBook(int book);
        Task<List<Book>> AllBooks();        
    }
}