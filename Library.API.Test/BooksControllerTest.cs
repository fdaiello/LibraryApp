using Library.API.Controllers;
using Library.API.Data.Models;
using Library.API.Data.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Xunit;

namespace Library.API.Test
{
    public class BooksControllerTest
    {
        readonly BooksController _controller;
        readonly IBookService _service;
        public BooksControllerTest()
        {
            _service = new BookService();
            _controller = new BooksController(_service);
        }
        [Fact]
        public void GetAll_Test()
        {
            // Arrange

            // Act
            var get = _controller.Get();

            // Assert
            Assert.IsType<OkObjectResult>(get.Result);

            var okResult = get.Result as OkObjectResult;
            Assert.IsType<List<Book>>(okResult.Value);

            var bookList = okResult.Value as List<Book>;

            Assert.Equal(5, bookList.Count);
        }
        [Theory]
        [InlineData("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200", "ab2bd817-98cd-4cf3-a80a-53ea0cd9c288")]
        public void GetById_Test(string id, string id2) 
        {
            // Arrange
            var guid1 = new Guid(id);
            var guid2 = new Guid(id2);

            // Act
            var actionResult = _controller.Get(guid1);

            // Assert
            Assert.IsType<OkObjectResult>(actionResult.Result);

            var okResult = actionResult.Result as OkObjectResult;
            Assert.IsType<Book>(okResult.Value);

            var book = okResult.Value as Book;
            Assert.Equal(book.Id.ToString(), id);

            // Act
            actionResult = _controller.Get(guid2);

            // Assert
            Assert.IsType<NotFoundResult>(actionResult.Result);

        }
        [Fact]
        public void AddBook_Test()
        {
            // Arrange
            var completeBook = new Book() { 
                Author="Felipe",
                Description = "Felipe's Book",
                Title = "Daiellos's Book"
            };

            var incompleteBook = new Book()
            {
                Description = "Felipe's Book"
            };

            // Act
            var actionResult1 = _controller.Post(completeBook);

            // Assert
            Assert.IsType<CreatedAtActionResult>(actionResult1);

            var item = actionResult1 as CreatedAtActionResult;
            Assert.IsType<Book>(item.Value);

            var book = item.Value as Book;
            Assert.Equal(completeBook.Author, book.Author);
            Assert.Equal(completeBook.Title, book.Title);
            Assert.Equal(completeBook.Description, book.Description);


            // Act
            _controller.ModelState.AddModelError("Title", "Title is required.");
            var actionResult2 = _controller.Post(incompleteBook);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actionResult2);
        }
    }
}
