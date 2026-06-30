using Microsoft.EntityFrameworkCore;
using ProductApi.Data;
using ProductApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5000);
});

// เพิ่ม services เข้า container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ต้องเพิ่มไฟล์ config ก่อน ถึงจะอ่านค่าได้
builder.Configuration.AddJsonFile("appsettings.Database.json", optional: true, reloadOnChange: true);

// อ่านค่าจาก appsettings.Database.json ว่าจะใช้ Database หรือ In-memory
var useDatabase = builder.Configuration.GetValue<bool>("UseDatabase");

if (useDatabase)
{
    // 1. ลงทะเบียน DbContext เพื่อเชื่อมต่อฐานข้อมูล SQL Server
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    // 2. ลงทะเบียน Service ฝั่ง Product (ต่อ Database)
    builder.Services.AddScoped<IProductService, ProductDbService>();

    // 3. เพิ่มการลงทะเบียน Service ฝั่ง Request (ต่อ Database ตัวใหม่ที่คุณเพิ่งสร้าง)
    builder.Services.AddScoped<IRequestService, RequestDbService>();
}
else
{
    // ลงทะเบียน Service แบบ In-Memory (กรณีไม่ได้ใช้ Database)
    builder.Services.AddSingleton<IProductService, ProductService>();

    // เพิ่มการลงทะเบียน Service ฝั่ง Request แบบ In-Memory (คลาส RequestService ที่เราปรับปรุงไปก่อนหน้านี้)
    builder.Services.AddSingleton<IRequestService, RequestService>();
}

// ตั้งค่า CORS เพื่อให้ Angular (ปกติรันที่ http://localhost:4200) เรียก API นี้ได้
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularApp");

app.UseAuthorization();

app.MapControllers();

app.Run();