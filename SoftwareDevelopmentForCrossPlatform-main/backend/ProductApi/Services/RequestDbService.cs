using ProductApi.Data;
using ProductApi.Models;

namespace ProductApi.Services
{
    public class RequestDbService : IRequestService
    {
        private readonly AppDbContext _context;

        public RequestDbService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<RepairRequest> GetAll()
        {
            return _context.RepairRequests.ToList();
        }

        public RepairRequest? GetById(int id)
        {
            return _context.RepairRequests.FirstOrDefault(r => r.request_id == id);
        }

        public RepairRequest Create(RepairRequest request)
        {
            // 🔥 ล้างค่า ID ดักไว้ให้เป็น 0 เสมอ เพื่อบอก EF Core ว่าให้ปล่อยเป็นหน้าที่ของ SQL Server รันเลขให้อัตโนมัติ
            request.request_id = 0;

            _context.RepairRequests.Add(request);
            _context.SaveChanges();
            return request;
        }

        public bool Update(int id, RepairRequest request)
        {
            var existing = _context.RepairRequests.FirstOrDefault(r => r.request_id == id);
            if (existing is null) return false;

            existing.title = request.title;
            existing.decription = request.decription;
            existing.location = request.location;
            existing.category = request.category;
            existing.status = request.status;
            existing.requester_id = request.requester_id;
            existing.approver_id = request.approver_id;
            existing.technician_id = request.technician_id;

            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var existing = _context.RepairRequests.FirstOrDefault(r => r.request_id == id);
            if (existing is null) return false;

            _context.RepairRequests.Remove(existing);
            _context.SaveChanges();
            return true;
        }
    }
}