using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GminorApi.Models
{
    public class AlbumSongs
    {
        private string id = string.Empty;
        private string title = string.Empty;
        private string image = string.Empty;
        private string artist = string.Empty;


        public string Id { get => id; set => id = value; }
        public string Title { get => title; set => title = value; }
        public string Image { get => image; set => image = value; }
        public string Artist { get => artist; set => artist = value; }

    }
}