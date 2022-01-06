using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace HaspSessionKiller
{
    internal class Program
    {
        private const int DefaultPort = 1947;

        private static async Task Main(string[] args)
        {
            var port = DefaultPort;
            if (args.Length > 0 && int.TryParse(args[0], out var customPort))
            {
                port = customPort;
            }
            var address = $"http://localhost:{port}";
            using var webClient = new WebClient();
            webClient.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.3; Win64; x64; rv:72.0) Gecko/20100101 Firefox/72.0");
            webClient.Headers.Add("Accept", "*/*");
            webClient.Headers.Add("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.5,en;q=0.3");
            webClient.Headers.Add("Accept-Encoding", "gzip, deflate");
            webClient.Headers.Add("Content-Type", "text/plain");
            webClient.Headers.Add("Origin", address);
            webClient.Headers.Add("Referer", $"{address}/_int_/sessions.html");
            webClient.Headers.Add("Cookie", "hasplmlang=_int_");

            var sessionsResponse = await webClient.DownloadStringTaskAsync($"{address}/_int_/tab_sessions.html");

            using var reader = new JsonTextReader(new StringReader(sessionsResponse))
            {
                SupportMultipleContent = true
            }; 
            var serializer = new JsonSerializer();
            while (await reader.ReadAsync())
            {
                try
                {
                    var session = serializer.Deserialize<HaspSession>(reader);
                    if (session is null)
                        continue;
                    await webClient.UploadStringTaskAsync($"{address}/_int_/action.html", "POST", $"deletelogin={session.sid}");
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}
