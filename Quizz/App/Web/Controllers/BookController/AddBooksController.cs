using Microsoft.AspNetCore.Mvc;
using Quizz.App.Domain.Models.Services;
using Quizz.App.Domain.Models.User;

namespace Quizz.App.Web.Controllers.BookController;


[ApiController]
[Route("api/[controller]")]
public class AddBooksController : ControllerBase
{
    private readonly IAddBooksService _addBooksService;

    public AddBooksController(IAddBooksService addBooksService)
    {
        _addBooksService = addBooksService;
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] Book book)
    {
        if (string.IsNullOrEmpty(book.NameBook) || string.IsNullOrEmpty(book.Izdatel))
        {
            return BadRequest("Некорректные данные книги.");
        }
        
        var result = await _addBooksService.AddBook(book);
        if (!result)
        {
            return Conflict("Книга с таким названием уже существует.");
        }
        return Ok("Книга успешно добавлена.");
    }
}