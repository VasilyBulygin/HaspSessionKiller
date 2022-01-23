using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
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
            using var httpClient = new HttpClient();
            var sessionsResponse = await httpClient.GetStreamAsync($"{address}/_int_/tab_sessions.html");
            using var reader = new JsonTextReader(new StreamReader(sessionsResponse))
            {
                SupportMultipleContent = true
            };
            var serializer = new JsonSerializer();
            while (await reader.ReadAsync())
            {
                try
                {
                    var session = serializer.Deserialize<HaspSession>(reader);
                    if (session?.sid is null)
                        continue;
                    await httpClient.PostAsync($"{address}/_int_/action.html", new StringContent($"deletelogin={session.sid}"));
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}
