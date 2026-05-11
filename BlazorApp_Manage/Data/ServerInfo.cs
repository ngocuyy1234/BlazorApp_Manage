//using System.Text.Json.Serialization;

//public class AnsibleRoot
//{
//    [JsonPropertyName("ansible_facts")]
//    public Facts AnsibleFacts { get; set; }
//}

//public class Facts
//{
//    [JsonPropertyName("ansible_distribution")]
//    public string AnsibleDistribution { get; set; }

//    [JsonPropertyName("ansible_distribution_version")]
//    public string AnsibleDistributionVersion { get; set; }

//    [JsonPropertyName("ansible_memfree_mb")]
//    public int AnsibleMemfreeMb { get; set; }

//    [JsonPropertyName("ansible_default_ipv4")]
//    public DefaultIpv4 AnsibleDefaultIpv4 { get; set; }

//    [JsonPropertyName("ansible_date_time")]
//    public DateTimeInfo AnsibleDateTime { get; set; }
//}

//public class DefaultIpv4
//{
//    [JsonPropertyName("address")]
//    public string Address { get; set; }
//}

//public class DateTimeInfo
//{
//    [JsonPropertyName("time")]
//    public string Time { get; set; }
//}

using System.Text.Json.Serialization;
using System.Diagnostics;
using System.Text.Json;
public class AnsibleRoot
{
    [JsonPropertyName("ansible_facts")]
    public Facts AnsibleFacts { get; set; }
}

public class Facts
{
    [JsonPropertyName("ansible_distribution")]
    public string AnsibleDistribution { get; set; }

    [JsonPropertyName("ansible_distribution_version")]
    public string AnsibleDistributionVersion { get; set; }

    [JsonPropertyName("ansible_memfree_mb")]
    public int AnsibleMemfreeMb { get; set; }

    [JsonPropertyName("ansible_default_ipv4")]
    public DefaultIpv4 AnsibleDefaultIpv4 { get; set; }

    [JsonPropertyName("ansible_date_time")]
    public DateTimeInfo AnsibleDateTime { get; set; }
}

public class DefaultIpv4
{
    [JsonPropertyName("address")]
    public string Address { get; set; }
}

public class DateTimeInfo
{
    [JsonPropertyName("time")]
    public string Time { get; set; }
}

public class AnsibleService
{
    public async Task<AnsibleRoot> GetSystemInfoAsync()
    {
        try
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                // CHỈNH TẠI ĐÂY: Thay "ansible" bằng đường dẫn tuyệt đối trên máy bạn
                // Bạn dùng lệnh 'where ansible' trong CMD để lấy đường dẫn này
                FileName = @"C:\Python39\Scripts\ansible.exe",

                // Lệnh lấy thông tin hệ thống (facts) của máy localhost
                Arguments = "localhost -m setup",

                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(startInfo))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = await reader.ReadToEndAsync();

                    // Xử lý chuỗi JSON trả về từ Ansible để khớp với Model của bạn
                    // Ansible thường trả về dạng: localhost | SUCCESS => { "ansible_facts": ... }
                    // Cần cắt bỏ phần "localhost | SUCCESS =>" trước khi Parse
                    string jsonOnly = result.Substring(result.IndexOf("{"));

                    return JsonSerializer.Deserialize<AnsibleRoot>(jsonOnly);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi quét Ansible: {ex.Message}");
            return null;
        }
    }
}