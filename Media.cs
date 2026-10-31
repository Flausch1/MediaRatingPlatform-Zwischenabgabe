using System;
using System.Collections.Generic;
using System.Text;


namespace HttpServerDemo
{
    internal class Media
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        public string MediaType { get; set; } = "";

        public int ReleaseYear { get; set; }

        public List<string> Genres { get; set; } = new List<string>();

        public int AgeRestriction { get; set; }

        public int CreatorId { get; set; }
    }
}
