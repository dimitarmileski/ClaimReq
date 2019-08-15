using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimReq.Models
{
    public class VideoSearchInfo
    {
        public string Title { get; set; }
        public string VideoId { get; set; }
        public string Description { get; set; }
        public string PublishedAt { get; set; }
        public string ThumbnailUrl { get; set; }
        public string ChannelTitle { get; set; } 

        public bool isChannel { get; set; }
        public string ChannelId { get; set; }

        public bool isPlaylist { get; set; }
        public string PlaylistId { get; set; }

    }
}
