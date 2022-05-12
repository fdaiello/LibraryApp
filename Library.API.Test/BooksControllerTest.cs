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
            // Act
            var get = _controller.Get(new Guid(id));

            // Assert
            Assert.IsType<OkObjectResult>(get.Result);

            var okResult = get.Result as OkObjectResult;
            Assert.IsType<Book>(okResult.Value);

            var book = okResult.Value as Book;
            Assert.Equal(book.Id.ToString(), id);

            // Act
            get = _controller.Get(new Guid(id2));
            Assert.IsType<NotFoundResult>(get.Result);

        }

    }
}
