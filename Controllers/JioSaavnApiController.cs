using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Json.Net;
using System.Text.RegularExpressions;
using GminorApi.Models;
namespace GminorApi.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class JioSaavnApiController : ControllerBase
    {
        private static readonly string song_details_base_url = "https://www.jiosaavn.com/api.php?__call=song.getDetails&cc=in&_marker=0%3F_marker%3D0&_format=json&pids=";

        private static readonly string lyrics_base_url = "https://www.jiosaavn.com/api.php?__call=lyrics.getLyrics&ctx=web6dot0&api_version=4&_format=json&_marker=0%3F_marker%3D0&lyrics_id=";

        private static readonly string album_details_base_url = "https://www.jiosaavn.com/api.php?__call=content.getAlbumDetails&_format=json&cc=in&_marker=0%3F_marker%3D0&albumid=";


        private static readonly List<string> user_agent = new List<String>(){

                @"Opera/9.80 (X11; Linux i686; Ubuntu/14.10) Presto/2.12.388 Version/12.16",
                @"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_6; ko-kr) AppleWebKit/533.20.25 (KHTML, like Gecko) Version/5.0.4 Safari/533.20.27",
                @"Mozilla/5.0 (Linux; U; Android 2.3.3; zh-tw; HTC Pyramid Build/GRI40) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1",
                @"Mozilla/5.0 (Windows; U; MSIE 9.0; Windows NT 9.0; en-US)",
                @"Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 7.1; Trident/5.0)",
                @"Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:94.0) Gecko/20100101 Firefox/94.0",
                @"Mozilla/5.0 (X11; OpenBSD i386) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.125 Safari/537.36",
                @"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/534.9 (KHTML, like Gecko) Chrome/7.0.531.0 Safari/534.9"

            };

        public static Random random = new Random();

        [HttpGet]
        [Route("GetTrending")]
        public async Task<IActionResult> GetTrending()
        {


            List<SearchResult> songList = new List<SearchResult>();
            List<string> tokens = new List<string>(){
               "8MT-LQlP35c_","C3TvSMCoP2A_","eM6m7c9EezYwkg5tVhI3fw__","j2,VLFcNmrA_","qI50eijQECA_","m9Qkal5S733ufxkxMEIbIw__","tsJahdem34A_","I3kvhipIy73uCJW60TJk1Q__"
            };

            for (int i = 0; i < tokens.Count; i++)
            {
                string url = $"https://www.jiosaavn.com:443/api.php?__call=webapi.get&token={tokens.ElementAt(i)}&type=playlist&p=1&n=50&includeMetaTags=0&ctx=wap6dot0&api_version=4&_format=json&_marker=0";
                string referer = "https://www.jiosaavn.com/";


                var httpClient = new HttpClient(new HttpClientHandler());
                var request = new HttpRequestMessage(new HttpMethod("GET"), url);
                var user_ag = user_agent[random.Next(user_agent.Count)];

                request.Headers.TryAddWithoutValidation("Host", "www.jiosaavn.com");
                request.Headers.TryAddWithoutValidation("User-Agent", user_ag);
                request.Headers.TryAddWithoutValidation("Accept", "application/json, text/plain, */*");
                request.Headers.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.5");
                request.Headers.TryAddWithoutValidation("DNT", "1");
                request.Headers.TryAddWithoutValidation("Connection", "keep-alive");
                request.Headers.TryAddWithoutValidation("Referer", referer);
                request.Headers.TryAddWithoutValidation("Sec-Fetch-Dest", "empty");
                request.Headers.TryAddWithoutValidation("Sec-Fetch-Mode", "cors");
                request.Headers.TryAddWithoutValidation("Sec-Fetch-Site", "same-origin");
                request.Headers.TryAddWithoutValidation("Cache-Control", "max-age=0");
                request.Headers.TryAddWithoutValidation("TE", "trailers");

                var response = await httpClient.SendAsync(request);


                var result = response.Content.ReadAsStringAsync().Result;

                var songIds = GetSongs(result);


                songIds.RemoveAt(0);

                Console.WriteLine(songIds.Count);

                songList.AddRange(songIds);

            }
            songList = songList.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            var shuffledsongs = songList.OrderBy(a => Guid.NewGuid()).ToList();

            Console.WriteLine(shuffledsongs.Count);

            GetEnglish();

            return Ok(shuffledsongs);
        }


        [HttpGet]
        [Route("GetEnglish")]
        public async Task<IActionResult> GetEnglish()
        {

            // "I3kvhipIy73uCJW60TJk1Q__","LdbVc1Z5i9E_", "jVmOAc1aK2OO0eMLZZxqsA__" ,
            string url = "https://www.jiosaavn.com/api.php?__call=webapi.get&token=LdbVc1Z5i9E_&type=playlist&p=1&n=50&includeMetaTags=0&ctx=web6dot0&api_version=4&_format=json&_marker=0";

            var httpClient = new HttpClient(new HttpClientHandler());
            var request = new HttpRequestMessage(new HttpMethod("GET"), url);
            request.Headers.TryAddWithoutValidation("Host", "www.jiosaavn.com");
            request.Headers.TryAddWithoutValidation("Cookie", "L:english");
            request.Headers.TryAddWithoutValidation("Accept", "application/json, text/plain, */*");
            request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 95.0.4638.69 Safari / 537.36");
            request.Headers.TryAddWithoutValidation("Sec-Fetch-Site", "same-origin");
            request.Headers.TryAddWithoutValidation("Sec-Fetch-Mode", "cors");
            request.Headers.TryAddWithoutValidation("Sec-Fetch-Dest", "empty");
            request.Headers.TryAddWithoutValidation("Origin","www.jiosaavn.com");
            request.Headers.TryAddWithoutValidation("Referer", "https://www.jiosaavn.com/featured/trending_today/I3kvhipIy73uCJW60TJk1Q__");
            request.Headers.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.9");

            var response = await httpClient.SendAsync(request);

            System.IO.File.WriteAllText("output.txt", response.Content.ReadAsStringAsync().Result);

            return Ok();

        }



        [HttpPost]
        [Route("GetAlbumDetails")]
        public async Task<IActionResult> GetAlbumDetails(string id)
        {

            string url = album_details_base_url + id;
            var httpClient = new HttpClient(new HttpClientHandler());
            var request = new HttpRequestMessage(new HttpMethod("GET"), url);

            var user_ag = user_agent[random.Next(user_agent.Count)];
            request.Headers.TryAddWithoutValidation("Host", "www.jiosaavn.com");
            request.Headers.TryAddWithoutValidation("User-Agent", user_ag);
            request.Headers.TryAddWithoutValidation("Accept", "application/json, text/plain, */*");
            request.Headers.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.5");
            request.Headers.TryAddWithoutValidation("DNT", "1");
            request.Headers.TryAddWithoutValidation("Connection", "keep-alive");
            //request.Headers.TryAddWithoutValidation("Referer", referer);
            request.Headers.TryAddWithoutValidation("Sec-Fetch-Dest", "empty");
            request.Headers.TryAddWithoutValidation("Sec-Fetch-Mode", "cors");
            request.Headers.TryAddWithoutValidation("Sec-Fetch-Site", "same-origin");
            request.Headers.TryAddWithoutValidation("Cache-Control", "max-age=0");
            request.Headers.TryAddWithoutValidation("TE", "trailers");

            var response = await httpClient.SendAsync(request);

            var jsonData = response.Content.ReadAsStringAsync();

            string result = jsonData.Result;

            Album album = GetAlbumMeta(result, id);
            return Ok(album);

        }

        public static Album GetAlbumMeta(string result, string id)
        {
            Album album = new Album();

            int Startindex = result.IndexOf("\"title\":\"");
            int Endindex = result.IndexOf("\",\"name\":\"");
            album.AlbumName = filter(result.Substring(Startindex + 9, Endindex - Startindex - 9));

            album.AlbumId = id;

            List<AlbumSongs> albumsongs = new List<AlbumSongs>();

            string splitStrStart = "{\"id\":\"";
            string[] ids = result.Split(splitStrStart);

            for (int i = 1; i < ids.Length; i++)
            {

                AlbumSongs albumsong = new AlbumSongs();
                string text = ids[i];

                Startindex = 0;
                Endindex = text.IndexOf(",\"type\"");

                albumsong.Id = text.Substring(Startindex, Endindex - 1);

                Startindex = text.IndexOf("\"song\":\"");
                Endindex = text.IndexOf(",\"album\"");
                albumsong.Title = filter(text.Substring(Startindex + 8, Endindex - Startindex - 9));

                Startindex = text.IndexOf("\"primary_artists\":\"");
                Endindex = text.IndexOf("\",\"primary_artists_id\"");

                albumsong.Artist = filter(text.Substring(Startindex + 19, Endindex - Startindex - 19));


                Startindex = text.IndexOf("\"image\":\"");
                Endindex = text.IndexOf("\",\"label\"");
                albumsong.Image = text.Substring(Startindex + 9, Endindex - Startindex - 9).Replace("\\", "").Replace("http:", "https:");

                albumsongs.Add(albumsong);

            }

            album.AlbumSongs = albumsongs;

            return album;

        }


        [HttpPost]
        [Route("SearchSong")]
        public async Task<IActionResult> SearchSong(string songName)
        {

            // in the below url p=1 means page 1, u can get much more results by incrementing it
            string url = "https://www.jiosaavn.com/api.php?p=1&q=" + songName + "&_format=json&_marker=0&api_version=4&ctx=wap6dot0&n=20&__call=search.getResults";
            string referer = "https://www.jiosaavn.com/search/" + songName;
            var httpClient = new HttpClient(new HttpClientHandler());
            var request = new HttpRequestMessage(new HttpMethod("GET"), url);
            request.Headers.TryAddWithoutValidation("Host", "www.jiosaavn.com");
            request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:94.0) Gecko/20100101 Firefox/94.0");
            request.Headers.TryAddWithoutValidation("Accept", "application/json, text/plain, */*");
            request.Headers.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.5");
            request.Headers.TryAddWithoutValidation("DNT", "1");
            request.Headers.TryAddWithoutValidation("Connection", "keep-alive");
            request.Headers.TryAddWithoutValidation("Referer", referer);
            request.Headers.TryAddWithoutValidation("Sec-Fetch-Dest", "empty");
            request.Headers.TryAddWithoutValidation("Sec-Fetch-Mode", "cors");
            request.Headers.TryAddWithoutValidation("Sec-Fetch-Site", "same-origin");
            request.Headers.TryAddWithoutValidation("Cache-Control", "max-age=0");
            request.Headers.TryAddWithoutValidation("TE", "trailers");

            var response = await httpClient.SendAsync(request);

            var jsonData = response.Content.ReadAsStringAsync();

            string result = jsonData.Result;

            var searchedsongs = GetSongs(result);


            return Ok(searchedsongs);

        }

        [HttpPost]
        [Route("SongDetails")]
        public async Task<IActionResult> SongDetails(string id)
        {
            string url = song_details_base_url + id;
            var httpClient = new HttpClient(new HttpClientHandler());
            var request = new HttpRequestMessage(new HttpMethod("GET"), url);

            var user_ag = user_agent[random.Next(user_agent.Count)];
            request.Headers.TryAddWithoutValidation("Host", "www.jiosaavn.com");
            request.Headers.TryAddWithoutValidation("User-Agent", user_ag);
            request.Headers.TryAddWithoutValidation("Accept", "application/json, text/plain, */*");
            request.Headers.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.5");
            request.Headers.TryAddWithoutValidation("DNT", "1");
            request.Headers.TryAddWithoutValidation("Connection", "keep-alive");
            //request.Headers.TryAddWithoutValidation("Referer", referer);
            request.Headers.TryAddWithoutValidation("Sec-Fetch-Dest", "empty");
            request.Headers.TryAddWithoutValidation("Sec-Fetch-Mode", "cors");
            request.Headers.TryAddWithoutValidation("Sec-Fetch-Site", "same-origin");
            request.Headers.TryAddWithoutValidation("Cache-Control", "max-age=0");
            request.Headers.TryAddWithoutValidation("TE", "trailers");

            var response = await httpClient.SendAsync(request);

            var jsonData = response.Content.ReadAsStringAsync();

            string result = jsonData.Result;

            var song = GetSongDetails(result, id);
            return Ok(song);
        }


        public static SongDetails GetSongDetails(string result, string id)
        {
            SongDetails song = new SongDetails();
            int Startindex = result.IndexOf("\"media_preview_url\":\"");
            int Endindex = result.IndexOf(".mp4\",");
            string sourceUrl = result.Substring(Startindex + 21, Endindex - Startindex - 17).Replace("\\", "").Replace("preview", "aac");
            if (result.Contains("320kbps\":\"true"))
            {
                sourceUrl = sourceUrl.Replace("_96_p.mp4", "_320.mp4");
            }
            else
            {
                sourceUrl = sourceUrl.Replace("_96_p.mp4", "_160.mp4");
            }

            Startindex = result.IndexOf("\"albumid\":\"");
            Endindex = result.IndexOf("\",\"language\":");

            string albumId = result.Substring(Startindex + 11, Endindex - Startindex - 11);



            song.Id = id;
            song.SourceUrl = sourceUrl;
            song.AlbumId = albumId;
            song.Lyrics = GetLyrics(id).Result;

            return song;

        }

        public static async Task<string> GetLyrics(string id)
        {

            string url = lyrics_base_url + id;
            var httpClient = new HttpClient(new HttpClientHandler());
            var request = new HttpRequestMessage(new HttpMethod("GET"), url);

            var user_ag = user_agent[random.Next(user_agent.Count)];
            request.Headers.TryAddWithoutValidation("Host", "www.jiosaavn.com");
            request.Headers.TryAddWithoutValidation("User-Agent", user_ag);
            request.Headers.TryAddWithoutValidation("Accept", "application/json, text/plain, */*");
            request.Headers.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.5");
            request.Headers.TryAddWithoutValidation("DNT", "1");
            request.Headers.TryAddWithoutValidation("Connection", "keep-alive");
            //request.Headers.TryAddWithoutValidation("Referer", referer);
            request.Headers.TryAddWithoutValidation("Sec-Fetch-Dest", "empty");
            request.Headers.TryAddWithoutValidation("Sec-Fetch-Mode", "cors");
            request.Headers.TryAddWithoutValidation("Sec-Fetch-Site", "same-origin");
            request.Headers.TryAddWithoutValidation("Cache-Control", "max-age=0");
            request.Headers.TryAddWithoutValidation("TE", "trailers");

            var response = await httpClient.SendAsync(request);

            var jsonData = response.Content.ReadAsStringAsync();

            string result = jsonData.Result;

            LyricsMeta test = JsonConvert.DeserializeObject<LyricsMeta>(result);

            return test.Lyrics;

        }

        public static List<SearchResult> GetSongs(string result)
        {
            List<SearchResult> searchedsongs = new List<SearchResult>();

            string splitStrStart = "{\"id\":\"";
            string[] ids = result.Split(splitStrStart);

            for (int i = 1; i < ids.Length; i++)
            {
                string text = ids[i];
                if (!text.Contains(",\"name\":\""))
                {
                    SearchResult song = new SearchResult();
                    int Startindex = 0;
                    int Endindex = text.IndexOf("\",\"title\"");
                    song.Id = text.Substring(Startindex, Endindex);

                    Startindex = text.IndexOf("title\":\"");
                    Endindex = text.IndexOf(",\"subtitle\":\"");
                    song.Title = filter(text.Substring(Startindex + 8, Endindex - Startindex - 9));

                    Startindex = text.IndexOf(",\"subtitle\":\"");
                    Endindex = text.IndexOf("\",\"header_desc\"");
                    song.Artist = filter(text.Substring(Startindex + 13, Endindex - Startindex - 13));

                    Startindex = text.IndexOf("\"image\":\"");
                    Endindex = text.IndexOf("\",\"language\"");
                    song.Image = text.Substring(Startindex + 9, Endindex - Startindex - 9).Replace("\\", "").Replace("http:", "https:");


                    searchedsongs.Add(song);
                }
            }
            return searchedsongs;

        }

        public static string filter(string st)
        {
            return st.Replace("&quot;", "'").Replace("&amp;", "&").Replace("&#039;", "'");
        }
    }
}