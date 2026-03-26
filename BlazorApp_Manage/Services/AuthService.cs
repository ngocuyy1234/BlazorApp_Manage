

using BlazorApp_Manage.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp_Manage.Services
{
    public class AuthService
    {
        private readonly WebAppManageContext _context;

        // Constructor: Nơi khởi tạo _context để dùng cho các hàm bên dưới
        public AuthService(WebAppManageContext context)
        {
            _context = context;
        }

        public async Task<Account?> LoginAsync(string username, string password)
        {
            Console.WriteLine($"[LOG - AuthService]: Đang kiểm tra login cho User: {username}");

            try
            {
                // Kiểm tra xem _context có bị null không
                if (_context == null)
                {
                    Console.WriteLine("[LOG - AuthService ERROR]: _context đang bị null!");
                    return null;
                }

                //var user = await _context.Accounts
                //    .Include(a => a.Role)
                //    .FirstOrDefaultAsync(a => a.Username == username && a.PasswordHash == password);

                var user = await _context.Accounts
            .Include(a => a.Role)
            .FirstOrDefaultAsync(a => a.Username == username
                                   && a.PasswordHash == password
                                   && a.IsActive == true); // THÊM DÒNG NÀY: Chỉ cho phép tài khoản đang hoạt động

                if (user != null)
                {
                    Console.WriteLine($"[LOG - AuthService]: Thành công! Tìm thấy User ID: {user.AccountId} với Role: {user.Role?.RoleName}");
                }
                else
                {
                    Console.WriteLine("[LOG - AuthService]: Thất bại! Không tìm thấy tài khoản hoặc sai mật khẩu.");
                }

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LOG - AuthService ERROR]: Lỗi truy vấn SQL: {ex.Message}");
                return null;
            }
        }
    }
}