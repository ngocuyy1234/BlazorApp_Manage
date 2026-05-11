//using System.Diagnostics;

//namespace YourProjectName.Services // Đổi lại đúng namespace của bạn
//{
//    public class AnsibleService
//    {
//        private readonly IWebHostEnvironment _env;

//        public AnsibleService(IWebHostEnvironment env)
//        {
//            _env = env;
//        }

//        public async Task<string> RunAnsiblePlaybook()
//        {
//            // Lấy đường dẫn thư mục chứa file hosts.ini và playbook
//            // Giả sử bạn để file ansible trong thư mục "Ansible" của project
//            string workingDirectory = Path.Combine(_env.ContentRootPath, "AnsibleScripts");

//            ProcessStartInfo start = new ProcessStartInfo();
//            start.FileName = "bash";
//            // Lưu ý: Thêm cd vào thư mục làm việc trước khi chạy lệnh
//            start.Arguments = $"-c \"cd {workingDirectory} && ansible-playbook -i hosts.ini check_version.yml\"";
//            start.UseShellExecute = false;
//            start.RedirectStandardOutput = true;
//            start.RedirectStandardError = true; // Lấy cả lỗi nếu có
//            start.CreateNoWindow = true;

//            try
//            {
//                using (Process process = Process.Start(start))
//                {
//                    string output = await process.StandardOutput.ReadToEndAsync();
//                    string error = await process.StandardError.ReadToEndAsync();
//                    await process.WaitForExitAsync();

//                    if (process.ExitCode != 0)
//                    {
//                        return $"Lỗi Ansible: {error}";
//                    }
//                    return output;
//                }
//            }
//            catch (Exception ex)
//            {
//                return $"Lỗi hệ thống: {ex.Message}";
//            }
//        }
//    }
//}

using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;

namespace BlazorApp_Manage.Services
{
    public class AnsibleService
    {
        private readonly IWebHostEnvironment _env;

        public AnsibleService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> RunAnsiblePlaybook()
        {
            // Đường dẫn này khớp với thư mục đã mount trong docker-compose
            string workingDirectory = "/app/ansible_scripts";

            ProcessStartInfo start = new ProcessStartInfo
            {
                FileName = "bash",
                // Sử dụng lệnh cd để đảm bảo luôn ở đúng thư mục chứa file yml
                Arguments = $"-c \"cd {workingDirectory} && ansible-playbook -i inventory get_info.yml\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            try
            {
                using (Process process = Process.Start(start))
                {
                    // Đọc kết quả đồng thời để tránh đầy bộ đệm (buffer)
                    var outputTask = process.StandardOutput.ReadToEndAsync();
                    var errorTask = process.StandardError.ReadToEndAsync();

                    await process.WaitForExitAsync();

                    string output = await outputTask;
                    string error = await errorTask;

                    if (process.ExitCode != 0)
                    {
                        // Nếu lỗi, trả về cả log lỗi để Uy dễ debug
                        return $"[LỖI ANSIBLE - Mã {process.ExitCode}]:\n{error}\n{output}";
                    }

                    return string.IsNullOrWhiteSpace(output) ? "Thực thi xong nhưng không có kết quả trả về." : output;
                }
            }
            catch (Exception ex)
            {
                return $"[LỖI HỆ THỐNG]: {ex.Message}";
            }
        }
    }
}