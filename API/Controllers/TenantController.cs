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
    /// Controller responsible for managing tenant-related operations in the API.
    /// </summary>
    /// <remarks>
    /// This controller provides endpoints for adding, retrieving, updating, and deleting tenant information.
    /// </remarks>
    [Route("api/tenant")]
    [Authorize]
    public class TenantController : ControllerBase
    {
        private readonly testClientContext _context;

        public TenantController(testClientContext context)
        {
            _context = context;
        }

        /// <summary>Adds a new tenant to the database</summary>
        /// <param name="model">The tenant data to be added</param>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [UserAuthorize("Tenant",Entitlements.Create)]
        public IActionResult Post([FromBody] Tenant model)
        {
            _context.Tenant.Add(model);
            this._context.SaveChanges();
            return Ok(model.Id);
        }

        /// <summary>Retrieves a list of tenants based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>The filtered list of tenants</returns>
        [HttpGet]
        [UserAuthorize("Tenant",Entitlements.Read)]
        public IActionResult Get([FromQuery] string filters, int pageNumber = 1, int pageSize = 10)
        {
            List<FilterCriteria> filterCriteria = null;
            if (!string.IsNullOrEmpty(filters))
            {
                filterCriteria = JsonHelper.Deserialize<List<FilterCriteria>>(filters);
            }

            var query = _context.Tenant.AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<Tenant>.ApplyFilter(query, filterCriteria);
            var paginatedResult = result.Skip(skip).Take(pageSize).ToList();
            return Ok(paginatedResult);
        }

        /// <summary>Retrieves a specific tenant by its primary key</summary>
        /// <param name="entityId">The primary key of the tenant</param>
        /// <returns>The tenant data</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [UserAuthorize("Tenant",Entitlements.Read)]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var entityData = _context.Tenant.FirstOrDefault(entity => entity.Id == id);
            return Ok(entityData);
        }

        /// <summary>Deletes a specific tenant by its primary key</summary>
        /// <param name="entityId">The primary key of the tenant</param>
        /// <returns>The result of the operation</returns>
        [HttpDelete]
        [UserAuthorize("Tenant",Entitlements.Delete)]
        [Route("{id:Guid}")]
        public IActionResult DeleteById([FromRoute] Guid id)
        {
            var entityData = _context.Tenant.FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                return NotFound();
            }

            _context.Tenant.Remove(entityData);
            var returnData = this._context.SaveChanges();
            return Ok(returnData);
        }

        /// <summary>Updates a specific tenant by its primary key</summary>
        /// <param name="entityId">The primary key of the tenant</param>
        /// <param name="updatedEntity">The tenant data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPut]
        [UserAuthorize("Tenant",Entitlements.Update)]
        [Route("{id:Guid}")]
        public IActionResult UpdateById(Guid id, [FromBody] Tenant updatedEntity)
        {
            if (id != updatedEntity.Id)
            {
                return BadRequest("Mismatched Id");
            }

            this._context.Tenant.Update(updatedEntity);
            var returnData = this._context.SaveChanges();
            return Ok(returnData);
        }
    }
}