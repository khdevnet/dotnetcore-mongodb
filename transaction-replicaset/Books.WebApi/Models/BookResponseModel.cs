using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.WebApi.Models
{
    public class BookResponseModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string DownloadUrl { get; set; }

        public string Url { get; set; }
    }
}
