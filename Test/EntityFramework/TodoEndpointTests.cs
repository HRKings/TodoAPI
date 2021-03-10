using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers;
using API.Repositories;
using Core.Dtos;
using Core.Models;
using Infrastructure.Database.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Test.EntityFramework
{
	public class TodoEndpointTests
	{
		[Fact]
		public async Task GetAllTodos()
		{
			var options = new DbContextOptionsBuilder<PostgresContext>()
				.UseInMemoryDatabase("GetAllTodos")
				.Options;

			// Insert seed data into the database using one instance of the context
			await using (var context = new PostgresContext(options))
			{
				await context.Todos.AddAsync(new Todo {Id = 1, Name = "Milk"});
				await context.Todos.AddAsync(new Todo {Id = 2, Name = "Chocolate"});
				await context.Todos.AddAsync(new Todo {Id = 3, Name = "Gym"});
				await context.SaveChangesAsync();
			}

			// Use a clean instance of the context to run the test
			await using (var context = new PostgresContext(options))
			{
				var repository = new TodoRepository(context);
				var todos = await new TodoController().Get(repository) as OkObjectResult;

				Assert.NotNull(todos);
				Assert.Equal(3, ((List<Todo>) todos.Value).Count);
			}
		}
		
		[Fact]
		public async Task GetPagedTodos()
		{
			var options = new DbContextOptionsBuilder<PostgresContext>()
				.UseInMemoryDatabase("GetPagedTodos")
				.Options;

			// Insert seed data into the database using one instance of the context
			await using (var context = new PostgresContext(options))
			{
				await context.Todos.AddAsync(new Todo {Id = 1, Name = "Milk"});
				await context.Todos.AddAsync(new Todo {Id = 2, Name = "Chocolate"});
				await context.Todos.AddAsync(new Todo {Id = 3, Name = "Gym"});
				await context.Todos.AddAsync(new Todo {Id = 4, Name = "Pickup"});
				await context.SaveChangesAsync();
			}

			// Use a clean instance of the context to run the test
			await using (var context = new PostgresContext(options))
			{
				var repository = new TodoRepository(context);
				var todos = await new TodoController().Get(repository, 1, 2) as OkObjectResult;

				Assert.NotNull(todos);
				Assert.Equal(2, ((PagedData<Todo>) todos.Value).Value.Count());
			}
		}

		[Fact]
		public async Task GetOneTodo()
		{
			var options = new DbContextOptionsBuilder<PostgresContext>()
				.UseInMemoryDatabase("GetOneTodo")
				.Options;

			// Insert seed data into the database using one instance of the context
			await using (var context = new PostgresContext(options))
			{
				await context.Todos.AddAsync(new Todo {Id = 1, Name = "Milk"});
				await context.Todos.AddAsync(new Todo {Id = 2, Name = "Chocolate"});
				await context.Todos.AddAsync(new Todo {Id = 3, Name = "Gym"});
				await context.SaveChangesAsync();
			}

			// Use a clean instance of the context to run the test
			await using (var context = new PostgresContext(options))
			{
				var repository = new TodoRepository(context);
				var todo = await new TodoController().GetFromId(repository, 2) as OkObjectResult;

				Assert.NotNull(todo);
				Assert.Equal("Chocolate", ((Todo) todo.Value).Name);
			}
		}

		[Fact]
		public async Task CreateTodo()
		{
			var options = new DbContextOptionsBuilder<PostgresContext>()
				.UseInMemoryDatabase("CreateTodo")
				.Options;

			// Use a clean instance of the context to run the test
			await using var context = new PostgresContext(options);

			var repository = new TodoRepository(context);
			var todo =
				await new TodoController().Create(repository, new TodoCreateDto {Name = "Shopping"}) as OkObjectResult;

			Assert.NotNull(todo);
			Assert.Equal("Shopping", ((Todo) todo.Value).Name);
		}

		[Fact]
		public async Task UpdateTodo()
		{
			var options = new DbContextOptionsBuilder<PostgresContext>()
				.UseInMemoryDatabase("UpdateTodo")
				.Options;

			// Insert seed data into the database using one instance of the context
			await using (var context = new PostgresContext(options))
			{
				await context.Todos.AddAsync(new Todo {Id = 1, Name = "Milk"});
				await context.SaveChangesAsync();
			}

			// Use a clean instance of the context to run the test
			await using (var context = new PostgresContext(options))
			{
				var repository = new TodoRepository(context);
				var todo =
					await new TodoController().Update(repository, 1, new TodoUpdateDto {Name = "Skating"}) as OkObjectResult;

				Assert.NotNull(todo);
				Assert.Equal("Skating", ((Todo) todo.Value).Name);
			}
		}

		[Fact]
		public async Task DeleteTodo()
		{
			var options = new DbContextOptionsBuilder<PostgresContext>()
				.UseInMemoryDatabase("DeleteTodo")
				.Options;

			// Insert seed data into the database using one instance of the context
			await using (var context = new PostgresContext(options))
			{
				await context.Todos.AddAsync(new Todo {Id = 1, Name = "Milk"});
				await context.SaveChangesAsync();
			}

			// Use a clean instance of the context to run the test
			await using (var context = new PostgresContext(options))
			{
				var repository = new TodoRepository(context);
				_ = await new TodoController().Delete(repository, 1) as OkObjectResult;
				var todos = await context.Todos.FindAsync(1);

				Assert.Null(todos);
			}
		}
	}
}