using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Dtos;
using Core.Models;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/todos")]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[Produces("application/json")]
	public class TodoController : ControllerBase
	{
		/// <summary>
		///     Returns all the todos, can be paged using the page and pageSize query
		/// </summary>
		/// <response code="200">Returns all the todos</response>
		/// <response code="400">If there is no todo on the database</response>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Todo>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Get([FromServices] ITodoRepository repository, [FromQuery] int page = 0,
			[FromQuery] int pageSize = 0)
		{
			if (page <= 0 || pageSize <= 0)
			{
				var result = await repository.GetAll();
				if (result?.Count == 0)
					return BadRequest("No TODOs were found");
				
				return Ok(result);
			}

			var pagedResult = await repository.GetPaged(page, pageSize);
			if (pagedResult.TotalItems == 0)
				return BadRequest("No TODOs were found");
				
			return Ok(pagedResult);
		}
		
		/// <summary>
		///     Returns a single todo by their id
		/// </summary>
		/// <response code="200">Returns the todo</response>
		/// <response code="400">If there is not todo with this ID</response>
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Todo))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetFromId([FromServices] ITodoRepository repository, [FromRoute] int id)
		{
			var result = await repository.GetFromId(id);

			if (result == null)
				return BadRequest($"The TODO {id} was not found.");

			return Ok(result);
		}
		
		/// <summary>
		///     Creates a new todo
		/// </summary>
		/// <response code="200">Returns the created todo</response>
		/// <response code="400">If there is an error</response>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Todo))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Create([FromServices] ITodoRepository repository,
			[FromBody] TodoCreateDto todo)
		{
			try
			{
				var result = await repository.Create(todo);
				return Ok(result);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		/// <summary>
		///     Updates a todo
		/// </summary>
		/// <response code="200">Returns the updated todo</response>
		/// <response code="400">If there is an error</response>
		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Todo))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Update([FromServices] ITodoRepository repository, [FromRoute] int id,
			[FromBody] TodoUpdateDto todo)
		{
			try
			{
				var result = await repository.Update(id, todo);
				if (result == null)
					return BadRequest($"The id {id} was not found.");
				
				return Ok(result);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		/// <summary>
		///     Deletes a todo
		/// </summary>
		/// <response code="200">If the todo was deleted</response>
		/// <response code="400">If there is an error</response>
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Delete([FromServices] ITodoRepository repository, [FromRoute] int id)
		{
			try
			{
				await repository.Delete(id);
				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}