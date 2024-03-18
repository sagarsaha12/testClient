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
    /// Controller responsible for managing roleentitlement-related operations in the API.
    /// </summary>
    /// <remarks>
    /// This controller provides endpoints for adding, retrieving, updating, and deleting roleentitlement information.
    /// </remarks>
    [Route("api/roleentitlement")]
    [Authorize]
    public class RoleEntitlementController : ControllerBase
    {
        private readonly testClientContext _context;

        public RoleEntitlementController(testClientContext context)
        {
            _context = context;
        }

        /// <summary>Adds a new roleentitlement to the database</summary>
        /// <param name="model">The roleentitlement data to be added</param>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [UserAuthorize("RoleEntitlement",Entitlements.Create)]
        public IActionResult Post([FromBody] RoleEntitlement model)
        {
            _context.RoleEntitlement.Add(model);
            this._context.SaveChanges();
            return Ok(model.Id);
        }

        /// <summary>Retrieves a list of roleentitlements based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>The filtered list of roleentitlements</returns>
        [HttpGet]
        [UserAuthorize("RoleEntitlement",Entitlements.Read)]
        public IActionResult Get([FromQuery] string filters, int pageNumber = 1, int pageSize = 10)
        {
            List<FilterCriteria> filterCriteria = null;
            if (!string.IsNullOrEmpty(filters))
            {
                filterCriteria = JsonHelper.Deserialize<List<FilterCriteria>>(filters);
            }

            var query = _context.RoleEntitlement.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<RoleEntitlement>.ApplyFilter(query, filterCriteria);
            var paginatedResult = result.Skip(skip).Take(pageSize).ToList();
            return Ok(paginatedResult);
        }

        /// <summary>Retrieves a specific roleentitlement by its primary key</summary>
        /// <param name="entityId">The primary key of the roleentitlement</param>
        /// <returns>The roleentitlement data</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [UserAuthorize("RoleEntitlement",Entitlements.Read)]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var entityData = _context.RoleEntitlement.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return Ok(entityData);
        }

        /// <summary>Deletes a specific roleentitlement by its primary key</summary>
        /// <param name="entityId">The primary key of the roleentitlement</param>
        /// <returns>The result of the operation</returns>
        [HttpDelete]
        [UserAuthorize("RoleEntitlement",Entitlements.Delete)]
        [Route("{id:Guid}")]
        public IActionResult DeleteById([FromRoute] Guid id)
        {
            var entityData = _context.RoleEntitlement.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                return NotFound();
            }

            _context.RoleEntitlement.Remove(entityData);
            var returnData = this._context.SaveChanges();
            return Ok(returnData);
        }

        /// <summary>Updates a specific roleentitlement by its primary key</summary>
        /// <param name="entityId">The primary key of the roleentitlement</param>
        /// <param name="updatedEntity">The roleentitlement data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPut]
        [UserAuthorize("RoleEntitlement",Entitlements.Update)]
        [Route("{id:Guid}")]
        public IActionResult UpdateById(Guid id, [FromBody] RoleEntitlement updatedEntity)
        {
            if (id != updatedEntity.Id)
            {
                return BadRequest("Mismatched Id");
            }

            this._context.RoleEntitlement.Update(updatedEntity);
            var returnData = this._context.SaveChanges();
            return Ok(returnData);
        }
    }
}