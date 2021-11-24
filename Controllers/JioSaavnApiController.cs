using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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



        [HttpPost]
        public async Task<IActionResult> SearchSong(string songName)
        {

            // in the below url p=1 means page 1, u can get much more results by incrementing it
            string url = "https://www.jiosaavn.com/api.php?p=1&q=" + songName + "&_format=json&_marker=0&api_version=4&ctx=wap6dot0&n=20&__call=search.getResults";
            string referer = "https://www.jiosaavn.com/search/" + songName;
            var httpClient = new HttpClient();
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

            System.IO.File.WriteAllText("output.json", result);

            var searchedsongs = GetSongs(result);


            return Ok(searchedsongs);

        }


        public static List<SearchResult> GetSongs(string result)
        {
            List<SearchResult> searchedsongs = new List<SearchResult>();

            string splitStrStart = "{\"id\":\"";
            string[] ids = result.Split(splitStrStart);

            for (int i = 1; i < ids.Length; i++)
            {
                string text = ids[i];
                if (!text.Contains("name"))
                {
                    SearchResult song = new SearchResult();
                    int Startindex = 0;
                    int Endindex = text.IndexOf("\",\"title\"");
                    song.Id = text.Substring(Startindex, Endindex);

                    Startindex = text.IndexOf("title\":\"");
                    Endindex = text.IndexOf(",\"subtitle\":\"");
                    song.Title = text.Substring(Startindex + 8, Endindex - Startindex - 9);

                    Startindex = text.IndexOf(",\"subtitle\":\"");
                    Endindex = text.IndexOf("\",\"header_desc\"");
                    song.Artist = text.Substring(Startindex + 13, Endindex - Startindex - 13);

                    Startindex = text.IndexOf("\"image\":\"");
                    Endindex = text.IndexOf("\",\"language\"");
                    song.Image = text.Substring(Startindex + 9, Endindex - Startindex - 9).Replace("\\", "").Replace("http:", "https:");


                    searchedsongs.Add(song);
                }
            }
            return searchedsongs;

        }
    }
}