using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.WebApi.Models
{
    public class CreateBookRequestModel
    {
        public string Title { get; set; }

        public string Author { get; set; }

        public IFormFile File { get; set; }
    }
}
