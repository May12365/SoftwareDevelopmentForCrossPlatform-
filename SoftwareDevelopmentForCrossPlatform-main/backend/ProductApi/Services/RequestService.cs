using ProductApi.Models;

namespace ProductApi.Services
{
    public interface IRequestService
    {
        IEnumerable<RepairRequest> GetAll();
        RepairRequest? GetById(int id);
        RepairRequest Create(RepairRequest request);
        bool Update(int id, RepairRequest request);
        bool Delete(int id);
    }
    public class RequestService : IRequestService
    {
        // เปลี่ยนมาเก็บข้อมูลเป็น List ของ RepairRequest และ Mock ข้อมูลให้ตรงกับตารางฐานข้อมูล
        private readonly List<RepairRequest> _requests = new()
        {
            new RepairRequest
            {
                request_id = 1,
                request_no = "REQ-001",
                title = "หน้าจอเปิดไม่ติด",
                decription = "เปิดเครื่องแล้วไฟเข้า แต่หน้าจอดับมืด",
                location = "อาคาร A ชั้น 3",
                category = "Hardware",
                status = "Pending",
                requester_id = 101,
                approver_id = 201,
                technician_id = 301
            },
            new RepairRequest
            {
                request_id = 2,
                request_no = "REQ-002",
                title = "เข้าใช้งานระบบไม่ได้",
                decription = "ลืมรหัสผ่านระบบภายใน แจ้งขอรีเซ็ตรหัสผ่าน",
                location = "อาคาร B ชั้น 1",
                category = "Software",
                status = "In Progress",
                requester_id = 102,
                approver_id = 201,
                technician_id = 302
            }
        };

        private int _nextId = 3;

        public IEnumerable<RepairRequest> GetAll() => _requests;

        public RepairRequest? GetById(int id) => _requests.FirstOrDefault(r => r.request_id == id);

        public RepairRequest Create(RepairRequest request)
        {
            request.request_id = _nextId++;
            _requests.Add(request);
            return request;
        }

        public bool Update(int id, RepairRequest request)
        {
            var existing = GetById(id);
            if (existing is null) return false;

            existing.title = request.title;
            existing.decription = request.decription;
            existing.location = request.location;
            existing.category = request.category;
            existing.status = request.status;
            existing.requester_id = request.requester_id;
            existing.approver_id = request.approver_id;
            existing.technician_id = request.technician_id;

            return true;
        }

        public bool Delete(int id)
        {
            var existing = GetById(id);
            if (existing is null) return false;

            _requests.Remove(existing);
            return true;
        }
    }
}