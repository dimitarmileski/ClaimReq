using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimReq.Models.ViewModels
{
    public class SearchViewModel
    {
        public string Keyword { get; set; }
        public List<VideoSearchInfo> VideoSearchInfos{ get; set; }
    }
}
