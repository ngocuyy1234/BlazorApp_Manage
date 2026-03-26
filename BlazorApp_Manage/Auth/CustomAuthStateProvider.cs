
//using Microsoft.AspNetCore.Components.Authorization;
//using System.Security.Claims;

//namespace BlazorApp_Manage.Auth
//{
//    public class CustomAuthStateProvider : AuthenticationStateProvider
//    {
//        // Biến lưu trữ người dùng hiện tại trong bộ nhớ (Memory)
//        private ClaimsPrincipal _currentUser = new ClaimsPrincipal(new ClaimsIdentity());

//        public override Task<AuthenticationState> GetAuthenticationStateAsync()
//        {
//            // Trả về trạng thái hiện tại (nếu trống thì là Anonymous - chưa đăng nhập)
//            return Task.FromResult(new AuthenticationState(_currentUser));
//        }

//        public void MarkUserAsAuthenticated(string username, string role)
//        {
//            Console.WriteLine($"[LOG - AuthProvider]: Đang tạo Identity cho {username} - Quyền: {role}");

//            var identity = new ClaimsIdentity(new[]
//            {
//        new Claim(ClaimTypes.Name, username),
//        new Claim(ClaimTypes.Role, role)
//    }, "apiauth");

//            _currentUser = new ClaimsPrincipal(identity);

//            // Dòng này cực kỳ quan trọng để MainLayout thay đổi UI
//            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

//            Console.WriteLine("[LOG - AuthProvider]: Đã gửi thông báo thay đổi trạng thái tới toàn ứng dụng.");
//        }

//        // Hàm xử lý đăng xuất
//        public void MarkUserAsLoggedOut()
//        {
//            _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
//            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
//        }
//    }
//}

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace BlazorApp_Manage.Auth
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedSessionStorage _sessionStorage;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthStateProvider(ProtectedSessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        // Hàm này giúp Blazor kiểm tra xem có ai đang đăng nhập không mỗi khi load trang
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userSessionStorageResult = await _sessionStorage.GetAsync<UserSession>("UserSession");
                var userSession = userSessionStorageResult.Success ? userSessionStorageResult.Value : null;

                if (userSession == null)
                    return await Task.FromResult(new AuthenticationState(_anonymous));

                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, userSession.Username),
                    new Claim(ClaimTypes.Role, userSession.Role)
                }, "apiauth"));

                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
            }
            catch
            {
                return await Task.FromResult(new AuthenticationState(_anonymous));
            }
        }

        // Hàm này được gọi từ Login.razor để LƯU vào Storage
        public async Task UpdateAuthenticationState(UserSession? userSession)
        {
            ClaimsPrincipal claimsPrincipal;

            if (userSession != null)
            {
                await _sessionStorage.SetAsync("UserSession", userSession);
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, userSession.Username),
                    new Claim(ClaimTypes.Role, userSession.Role)
                }, "apiauth"));
            }
            else
            {
                await _sessionStorage.DeleteAsync("UserSession");
                claimsPrincipal = _anonymous;
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }
    }

    // Lớp phụ trợ để đóng gói dữ liệu lưu xuống Storage
    public class UserSession
    {
        public string Username { get; set; } = "";
        public string Role { get; set; } = "";
    }
}