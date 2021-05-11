using HtmlAgilityPack;
using R34_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace R34_API
{
    public class R34Service
    {
        public List<string> GetSources(int page = 0, params string[] tags)
        {
            try
            {
                var search = String.Join("+",tags.Select(x => x.Replace(" ", "_").ToLower().Trim()));
                int index;
                if (page == 1 || page <= 0) index = 0;
                else index = (page * 42) - 42;
                var url = String.Format("https://rule34.xxx/index.php?page=post&s=list&tags={0}&pid={1}", search, index);
                var web = new HtmlWeb();
                var doc = web.Load(url);
                var result = doc.GetElementbyId("content")
                    ?.SelectSingleNode("//div[@class='content']")
                    ?.Descendants("div")
                    ?.FirstOrDefault()
                    ?.Descendants("a")
                    .Select(x => "https://rule34.xxx/" + x.Attributes["href"].Value)
                    .ToList();
                if (result == null) throw new Exception("Not found");
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Item> GetMedia(IEnumerable<string> source)
        {
            var web = new HtmlWeb();
            var items = new List<Item>();
            foreach(var item in source)
            {
                var doc = web.Load(item);

                if (IsImage(doc))
                   items.Add(GetImage(doc));
                else
                   items.Add(GetVideo(doc));
            }
            return items;
        }
        public Item GetMedia(string source)
        {
            var web = new HtmlWeb();
            var doc = web.Load(source);
            if (IsImage(doc))
                return GetImage(doc);
                
            return GetVideo(doc);
        }
        private Item GetVideo(HtmlDocument doc)
        {
            var video = doc
                .GetElementbyId("content")
                .SelectSingleNode("//div[@id='post-view']")
                .SelectSingleNode("//div[@class='content']")
                .SelectSingleNode("//div[@class='flexi']")
                .Descendants("video")
                .FirstOrDefault()
                .Descendants("source")
                .FirstOrDefault();
            var item = new Item()
            {
                Url = video.Attributes["src"].Value,
                Width = 100,
                Height = 100,
                Tags = "",
                Type = Models.Type.Video
            };
            return item;

        }
        public int GetMaxPageId(params string[] tags)
        {
            var search = String.Join("+",tags.Select(x => x.Replace(" ", "_").ToLower().Trim()));
            var url = String.Format("https://rule34.xxx/index.php?page=post&s=list&tags={0}&pid=0", search);
            var web = new HtmlWeb();
            var doc = web.Load(url);

            var pagination = doc.GetElementbyId("content")
                ?.SelectSingleNode("//div[@id='post-list']")
                ?.SelectSingleNode("//div[@class='content']")
                ?.SelectSingleNode("//div[@id='paginator']")
                ?.SelectSingleNode("//div[@class='pagination']")
                .SelectSingleNode("//a[@alt='last page']");
            if (pagination == null) return 1;
            return (int.Parse(pagination.Attributes["href"].Value.Split("=")[4]) + 42) / 42;
        }
        private Item GetImage(HtmlDocument doc)
        {
            var image = doc
                .GetElementbyId("content")
                .SelectSingleNode("//div[@id='post-view']")
                .SelectSingleNode("//div[@class='content']")
                .SelectSingleNode("//div[@class='flexi']")
                .Descendants("img")
                .FirstOrDefault();
            var item = new Item()
            {
                Url = image.Attributes["src"].Value,
                Width = int.Parse(image.Attributes["width"].Value),
                Height = int.Parse(image.Attributes["height"].Value),
                Tags = image.Attributes["alt"].Value,
                Type = Models.Type.Image
            };
            return item;
        }
        private bool IsImage(HtmlDocument doc) =>
            doc.GetElementbyId("content")
                    .SelectSingleNode("//div[@id='post-view']").SelectSingleNode("//div[@class='content']")
                    .SelectSingleNode("//div[@class='flexi']").Descendants("div").FirstOrDefault()
                    .SelectSingleNode("//img[@id='image']") != null;
    }
}
