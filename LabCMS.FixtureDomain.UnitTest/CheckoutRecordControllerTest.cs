using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LabCMS.FixtureDomain.UnitTest
{
    public class CheckoutRecordControllerTest
    {
        [Fact]
        public async Task InitTest()
        {
            using HttpClient client = TestEnviroment.Instance.CreateClient();
            var res1 = await client.PostAsync("/api/Roles?userId=liha52&passwordMD5=password",null);
            string cookie = res1.Headers.GetValues("Set-Cookie").First();
            HttpRequestMessage httpRequestMessage = new(HttpMethod.Post, "/api/CheckoutRecords/Init");
            httpRequestMessage.Headers.Add("cookie", cookie);
            httpRequestMessage.Content = JsonContent.Create(TestEnviroment.SeedCheckoutRecordInClient);
            var res2 = await client.SendAsync(httpRequestMessage);
            Assert.Equal(HttpStatusCode.OK, res2.StatusCode);
        }
    }
}
