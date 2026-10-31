using System;
using System.Collections.Generic;
using System.Text;

namespace HttpServerDemo
{
    internal class Rating
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MediaId { get; set; }
        public int Value { get; set; }
    }
}