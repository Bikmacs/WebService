using Microsoft.AspNetCore.Mvc;
using Quizz.App.Domain.Models.Services;
using Quizz.App.Domain.Models.Services.BookService;
using Quizz.App.Domain.Models.User;

namespace Quizz.App.Web.Controllers.BookController;


[ApiController]
[Route("api/[controller]")]
public class AddBooksController(IAddBooksService addBooksService) : ControllerBase
{
    [HttpPost("Add")]
    public async Task<IActionResult> Add([FromBody] Book book)
    {
        if (string.IsNullOrEmpty(book.NameBook) || string.IsNullOrEmpty(book.Izdatel))
        {
            return BadRequest("Некорректные данные книги.");
        }
        
        var result = await addBooksService.AddBook(book);
        if (!result)
        {
            return Conflict("Книга с таким названием уже существует.");
        }
        return Ok("Книга успешно добавлена.");
    }
    [HttpPost("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Некорректные Id книги.");
        }
        var result = await addBooksService.DeleteBook(id);
        if (!result)
        {
            return Conflict("Книга не удалена");
        }
        return Ok("Книга успешно удалена");
    }
    [HttpGet("AllBooks")]
    public async Task<IActionResult> AllBooks()
    {
        var viewBooks = await addBooksService.AllBooks();
        if (!viewBooks.Any())
        {
            return NotFound("Нет данных о книгах.");
        }
        return Ok(viewBooks);
    }
    
}