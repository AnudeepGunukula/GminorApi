using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GminorApi.Models
{
    public class SongDetails
    {
        private string id;
        private string sourceUrl;
        private string lyrics;
        private string albumId;

        public string Id { get => id; set => id = value; }
        public string SourceUrl { get => sourceUrl; set => sourceUrl = value; }
        public string Lyrics { get => lyrics; set => lyrics = value; }
        public string AlbumId { get => albumId; set => albumId = value; }
    }
}