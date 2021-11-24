using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GminorApi.Models
{
    public class AlbumSongs
    {
         private string title;
        private string image;
        private string artist;

       
        public string Title { get => title; set => title = value; }
        public string Image { get => image; set => image = value; }
        public string Artist { get => artist; set => artist = value; }
    }
}