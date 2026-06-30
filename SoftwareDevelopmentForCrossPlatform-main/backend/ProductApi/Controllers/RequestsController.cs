using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;
using ProductApi.Services;

namespace ProductApi.Controllers
{
    [ApiController]
    // จะได้ Route เป็น api/requests (ตามชื่อคอนโทรลเลอร์)
    [Route("api/[controller]")]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestService _service;

        public RequestsController(IRequestService service)
        {
            _service = service;
        }

        // GET: api/requests
        [HttpGet]
        public ActionResult<IEnumerable<RepairRequest>> GetAll()
        {
            return Ok(_service.GetAll());
        }

        // GET: api/requests/5
        [HttpGet("{id}")]
        public ActionResult<RepairRequest> GetById(int id)
        {
            var request = _service.GetById(id);
            if (request is null) return NotFound();
            return Ok(request);
        }

        // POST: api/requests
        [HttpPost]
        public ActionResult<RepairRequest> Create(RepairRequest request)
        {
            request.request_id = 0;
            var created = _service.Create(request);

            // เปลี่ยนจาก created.Id เป็น created.request_id ตาม Model RepairRequest
            return CreatedAtAction(nameof(GetById), new { id = created.request_id }, created);
        }

        // PUT: api/requests/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, RepairRequest request)
        {
            var success = _service.Update(id, request);
            if (!success) return NotFound();
            return NoContent();
        }

        // DELETE: api/requests/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var success = _service.Delete(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}