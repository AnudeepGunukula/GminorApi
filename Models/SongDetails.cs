using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GminorApi.Models
{
    public class SongDetails
    {
        private string id = string.Empty;
        private string sourceUrl = string.Empty;
        private string lyrics = string.Empty;
        private string albumId = string.Empty;

        public string Id { get => id; set => id = value; }
        public string SourceUrl { get => sourceUrl; set => sourceUrl = value; }
        public string Lyrics { get => lyrics; set => lyrics = value; }
        public string AlbumId { get => albumId; set => albumId = value; }
    }
}