using GK_CNNET.Models;
using GK_CNNET.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using GK_CNNET.DTOs;

namespace GK_CNNET.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IMongoCollection<Book> _booksCollection;
        private readonly ICloudinaryService _cloudinaryService;

        public BooksController(IMongoCollection<Book> booksCollection, ICloudinaryService cloudinaryService)
        {
            _booksCollection = booksCollection;
            _cloudinaryService = cloudinaryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookReadDto>>> GetAll()
        {
            var books = await _booksCollection.Find(_ => true).ToListAsync();

            var readDtos = books.Select(b => new BookReadDto
            {
                Id = b.Id!,
                Title = b.Title,
                Author = b.Author,
                Price = b.Price,
                ImageUrl = b.ImageUrl
            });

            return Ok(readDtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookCreateDto request)
        {
            var newBook = new Book
            {
                Title = request.Title,
                Author = request.Author,
                Price = request.Price,
                ImageUrl = request.ImageUrl
            };

            await _booksCollection.InsertOneAsync(newBook);
            return CreatedAtAction(nameof(GetById), new { id = newBook.Id }, newBook);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetById(string id)
        {
            var book = await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (book == null) return NotFound();
            return Ok(book);
        }

        [HttpPost("upload-only")]
        public async Task<IActionResult> UploadOnly(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest(new { message = "Không nhận được file ảnh!" });
            }

            var uploadResult = await _cloudinaryService.UploadImageAsync(image, "Covers");

            if (uploadResult.Error != null)
            {
                return BadRequest(new { message = "Lỗi upload ảnh lên Cloudinary", error = uploadResult.Error.Message });
            }

            // Trả về URL để Frontend dùng cho GraphQL hoặc gRPC
            return Ok(new { imageUrl = uploadResult.SecureUrl.ToString() });
        }

        // Thêm luôn hàm Delete để nút Xóa ở giao diện hoạt động được
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _booksCollection.DeleteOneAsync(x => x.Id == id);
            if (result.DeletedCount == 0) return NotFound();
            return NoContent();
        }
    }
}