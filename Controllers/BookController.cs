using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProtoBuf;
using Protobuf_Net_Sample.Models;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Protobuf_Net_Sample.Controllers
{
    [ApiController]
    [Route("{controller}/{action}")]
    public class BookController : ControllerBase
    {

        private readonly ILogger<BookController> _logger;

        public BookController(ILogger<BookController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        //添加此配置项，仅返回application/x-protobuf格式的数据,若未添加默认返回json，或客户端请求时指定Accept=application/x-protobuf
        //[Produces("application/x-protobuf")]
        public Book GetBook()
        {
            var request = HttpContext.Request;
            _logger.LogInformation(request.Host.Value);

            var book = new Book
            {
                Id = 12345,
                Name = "Fred",
                Article = new Article
                {
                    Id = 1,
                    Title = ".Net Core Protobuf",
                    Content = "Protobuf is great"
                }
            };
            return book;

        }

        [HttpGet]
        public async Task<Book> CallProtoBufRequest()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-protobuf"));
            var response = await client.GetAsync("http://"+HttpContext.Request.Host.Value+"/book/getBook");
            MemoryStream stream = new MemoryStream();
            await response.Content.CopyToAsync(stream);
            stream.Position = 0;
            var book = Serializer.Deserialize<Book>(stream);
            return book;

        }
    }
}
