using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GminorApi.Models
{
    public class SearchResult
    {
        private string id;
        private string title;
        private string artist;
        private string image;

        public string Id { get => id; set => id = value; }
        public string Title { get => title; set => title = value; }
        public string Artist { get => artist; set => artist = value; }
        public string Image { get => image; set => image = value; }
    }
}