
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConceptArchitect.Data;
using ConceptArchitect.Utils;
using System.Data;

namespace ConceptArchitect.BookManagement.Repositories.Ado
{


    public class AdoBookRepository : IRepository<Book, string>
    {
        DbManager db;

        public AdoBookRepository(DbManager db)
        {
            this.db = db;
        }

        public async Task<Book> Add(Book book)
        {
            var query = $"insert into books(id,title,description,author_id,cover_photo) " +
                              $"values('{book.Id}','{book.Title}','{book.Description}','{book.AuthorId}','{book.CoverPhoto}')";

            await db.ExecuteUpdateAsync(query);

            return book;
        }

        public async Task Delete(string id)
        {
            await db.ExecuteUpdateAsync($"delete from books where id='{id}'");
        }

        private Book BookExtractor(IDataReader reader)
        {
            Book book = new Book();

            book.Id = reader["id"].ToString();
              book.Title = reader["title"].ToString();
                book.Description = reader["description"].ToString();
                book.AuthorId = reader["author_id"].ToString();
                book.CoverPhoto = reader["cover_photo"].ToString();
            return book;
            
        }

        public async Task<List<Book>> GetAll()
        {
            return await db.QueryAsync("select * from books", BookExtractor);
        }

        public async Task<List<Book>> GetAll(Func<Book, bool> predicate)
        {
            var books = await GetAll();

            return (from book in books
                    where predicate(book)
                    select book
                   ).ToList();
        }

        public async Task<Book> GetById(string id)
        {
            return await db.QueryOneAsync($"Select * from book where id='{id}'",BookExtractor);
        }

        public async Task<Book> Update(Book entity, Action<Book, Book> mergeOldNew)
        {
            var oldBook = await GetById(entity.Id);
            if (oldBook != null)
            {
                mergeOldNew(oldBook, entity);
                var query = $"update books set " +
                            $"id='{oldBook.Title}', " +
                            $"description='{oldBook.Description}', " +
                            $"author_id='{oldBook.AuthorId}, " +
                            $"cover_photo='{oldBook.CoverPhoto} " +
                            $"where id='{oldBook.Id}'";

                await db.ExecuteUpdateAsync(query);
            }

            return entity;



        }
    }
}
