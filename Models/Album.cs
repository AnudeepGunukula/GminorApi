using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GminorApi.Models
{
    public class Album
    {
        private string albumName;

        private ICollection<AlbumSongs> albumSongs;
        public string AlbumName { get => albumName; set => albumName = value; }
        public ICollection<AlbumSongs> AlbumSongs { get => albumSongs; set => albumSongs = value; }
    }
}