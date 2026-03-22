using Grpc.Core;
using MongoDB.Driver;
using GK_CNNET.Models;
using GK_CNNET;
namespace GK_CNNET.Services
{
    public class BookGrpcService : BookGrpc.BookGrpcBase
    {
        private readonly IMongoCollection<Book> _books;

        public BookGrpcService(IMongoCollection<Book> books)
        {
            _books = books;
        }

        public override async Task<BookList> GetAllBooks(EmptyRequest request, ServerCallContext context)
        {
            var mongoBooks = await _books.Find(_ => true).ToListAsync();
            var response = new BookList();

            foreach (var b in mongoBooks)
            {
                response.Items.Add(new BookModel
                {
                    Id = b.Id ?? string.Empty,
                    Title = b.Title ?? "Unknown",
                    Author = b.Author ?? "Unknown",
                    Price = b.Price,
                    ImageUrl = b.ImageUrl ?? string.Empty
                });
            }

            return response;
        }

        public override async Task<BookModel> CreateBook(CreateBookRequest request, ServerCallContext context)
        {
            try
            {
                var newBook = new Book
                {
                    Title = request.Title,
                    Author = request.Author,
                    Price = request.Price, // Không cần ép kiểu (int) nếu request.Price đã là int32
                    ImageUrl = request.ImageUrl
                };

                await _books.InsertOneAsync(newBook);

                // Log này cực kỳ quan trọng để check xem code có chạy đến đây không
                Console.WriteLine($"===> gRPC Insert Success: {newBook.Title} (ID: {newBook.Id})");

                return new BookModel
                {
                    Id = newBook.Id ?? string.Empty,
                    Title = newBook.Title,
                    Author = newBook.Author,
                    Price = newBook.Price,
                    ImageUrl = newBook.ImageUrl
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"!!! gRPC Insert Error: {ex.Message}");
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }
        public override async Task<BookModel> UpdateBook(UpdateBookRequest request, ServerCallContext context)
        {
            var filter = Builders<Book>.Filter.Eq(b => b.Id, request.Id);
            var update = Builders<Book>.Update
                .Set(b => b.Title, request.Title)
                .Set(b => b.Author, request.Author)
                .Set(b => b.Price, request.Price)
                .Set(b => b.ImageUrl, request.ImageUrl);

            await _books.UpdateOneAsync(filter, update);

            return new BookModel
            {
                Id = request.Id,
                Title = request.Title,
                Author = request.Author,
                Price = request.Price,
                ImageUrl = request.ImageUrl
            };
        }

        public override async Task<DeleteResponse> DeleteBook(DeleteRequest request, ServerCallContext context)
        {
            var result = await _books.DeleteOneAsync(b => b.Id == request.Id);
            return new DeleteResponse { Success = result.DeletedCount > 0 };
        }

    }
}