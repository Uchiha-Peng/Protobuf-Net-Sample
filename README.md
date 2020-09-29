# Protobuf-Net-Sample
Use Protobuf in Asp.Net Core WebApi



### 1.Inport Nuget

```
protobuf-net

WebApiContrib.Core.Formatter.Protobuf
```



### 2.Edit Startup.cs

```
services.AddControllers()
                .AddProtobufFormatters();
```



### 3.Model

```
[ProtoContract]
    public class Article
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public string Title { get; set; }

        [ProtoMember(3)]
        public string Content { get; set; }
    }
    
    
[ProtoContract]
    public class Book
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; }

        [ProtoMember(3)]
        public Article Article { get; set; }
    }
```



### 4.Controller Action Return Protobuf

```
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
```



### 5.Client Call Rest API And Deserialize Form Protobuf

```
HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-protobuf"));
            var response = await client.GetAsync("http://"+HttpContext.Request.Host.Value+"/book/getBook");
            MemoryStream stream = new MemoryStream();
            await response.Content.CopyToAsync(stream);
            stream.Position = 0;
            var book = Serializer.Deserialize<Book>(stream);
```

<!--  -->