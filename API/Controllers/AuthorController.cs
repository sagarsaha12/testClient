using Microsoft.AspNetCore.Mvc;
using testClient.Models;
using testClient.Data;
using testClient.Filter;
using testClient.Entities;
using testClient.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace testClient.Controllers
{
    /// <summary>
    /// Controller responsible for managing author-related operations in the API.
    /// </summary>
    /// <remarks>
    /// This controller provides endpoints for adding, retrieving, updating, and deleting author information.
    /// </remarks>
    [Route("api/author")]
    [Authorize]
    public class AuthorController : ControllerBase
    {
        private readonly testClientContext _context;

        public AuthorController(testClientContext context)
        {
            _context = context;
        }

        /// <summary>Adds a new author to the database</summary>
        /// <param name="model">The author data to be added</param>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [UserAuthorize("Author",Entitlements.Create)]
        public IActionResult Post([FromBody] Author model)
        {
            _context.Author.Add(model);
            this._context.SaveChanges();
            return Ok(model.Id);
        }

        /// <summary>Retrieves a list of authors based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>The filtered list of authors</returns>
        [HttpGet]
        [UserAuthorize("Author",Entitlements.Read)]
        public IActionResult Get([FromQuery] string filters, int pageNumber = 1, int pageSize = 10)
        {
            List<FilterCriteria> filterCriteria = null;
            if (!string.IsNullOrEmpty(filters))
            {
                filterCriteria = JsonHelper.Deserialize<List<FilterCriteria>>(filters);
            }

            var query = _context.Author.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<Author>.ApplyFilter(query, filterCriteria);
            var paginatedResult = result.Skip(skip).Take(pageSize).ToList();
            return Ok(paginatedResult);
        }

        /// <summary>Retrieves a specific author by its primary key</summary>
        /// <param name="entityId">The primary key of the author</param>
        /// <returns>The author data</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [UserAuthorize("Author",Entitlements.Read)]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var entityData = _context.Author.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return Ok(entityData);
        }

        /// <summary>Deletes a specific author by its primary key</summary>
        /// <param name="entityId">The primary key of the author</param>
        /// <returns>The result of the operation</returns>
        [HttpDelete]
        [UserAuthorize("Author",Entitlements.Delete)]
        [Route("{id:Guid}")]
        public IActionResult DeleteById([FromRoute] Guid id)
        {
            var entityData = _context.Author.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                return NotFound();
            }

            _context.Author.Remove(entityData);
            var returnData = this._context.SaveChanges();
            return Ok(returnData);
        }

        /// <summary>Updates a specific author by its primary key</summary>
        /// <param name="entityId">The primary key of the author</param>
        /// <param name="updatedEntity">The author data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPut]
        [UserAuthorize("Author",Entitlements.Update)]
        [Route("{id:Guid}")]
        public IActionResult UpdateById(Guid id, [FromBody] Author updatedEntity)
        {
            if (id != updatedEntity.Id)
            {
                return BadRequest("Mismatched Id");
            }

            this._context.Author.Update(updatedEntity);
            var returnData = this._context.SaveChanges();
            return Ok(returnData);
        }
    }
}