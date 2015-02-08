using System;
using System.IO;
using System.Net;

namespace EnergyTechAudit.PowerAccounting.WebApi.HttpClient
{
    class Program
    {
        public static void Main()
        {
            
            using (var client = new System.Net.Http.HttpClient())
            {
                var startDateTime = DateTime.Parse("2015-01-01");
                var endDateTime = DateTime.Parse("2015-01-31");

                // для возможности использования самоподписанного сертификата сервера 
                // после установки на сервер заверенного сертификата вызов данного обработчика будет не нужен 
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) =>
                {
                    return true;
                };

                client.BaseAddress = new Uri("https://192.168.1.2");

                client.DefaultRequestHeaders.Add("login", "Archive.Downloader");
                client.DefaultRequestHeaders.Add("password", "xxxxx");

                var requestUri = string.Format
                (
                    "/api/package/archive?" +
                    "ar.withDictionaries=true&" +
                    "ar.startDateTime={0:yyyy-MM-dd}&" +
                    "ar.endDateTime={1:yyyy-MM-dd}&" +
                    "responseToFile=true",

                    startDateTime,
                    endDateTime
                );

                var response = client
                    .GetAsync(requestUri)
                    .Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    byte[] content = response.Content.ReadAsByteArrayAsync().Result;
                    if (response != null)
                    {
                        string path = @"d:\ArchivePackageCs.xml";
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }
                        File.WriteAllBytes(@"d:\ArchivePackageCs.xml", content);
                    }
                }
            }
        }

    }
}
