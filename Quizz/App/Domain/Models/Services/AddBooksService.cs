using Microsoft.EntityFrameworkCore;
using Quizz.App.Domain.Models.User;
using Quizz.App.Infrastructure.Context;

namespace Quizz.App.Domain.Models.Services;

public class AddBooksService : IAddBooksService
{
    private readonly ApplicationContext _context;
    public AddBooksService(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<bool> AddBook(Book book)
    {
        var name = await _context.Books.FirstOrDefaultAsync(u => u.NameBook == book.NameBook);
        if (name != null) return false;
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteBook(int id)
    {
        var exist = await _context.Books.FirstOrDefaultAsync(u => u.Id == id);
        if (exist != null)
        {
            _context.Books.Remove(exist);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }


}