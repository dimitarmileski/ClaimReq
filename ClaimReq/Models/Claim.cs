using System.ComponentModel.DataAnnotations;

namespace ClaimReq.Models
{
    // tag::class[]
    public class Claim
    {

        public string Type => "Claim";

        //Video Info
        [Display(Name = "Video Title")]
        public string VideoTitle { get; set; }

        [Display(Name = "Video Url")]
        public string VideoUrl { get; set; }

        [Display(Name= "Video Id")]
        public string VideoId { get; set; }

        [Display(Name = "Video Category")]
        public string VideoCategory { get; set; }

        [Display(Name = "Video Description")]
        public string VideoDescription { get; set; }

        [Display(Name = "Video Tags")]
        public string VideoTags { get; set; }

        [Display(Name = "Video Date Published")]
        public string VideoDatePublished { get; set; }

        [Display(Name = "Video Channel Title")]
        public string VideoChannelTitle { get; set; }

        [Display(Name = "Alt Url Upload")]
        public string altUrlUpload { get; set; }


        //Claim info
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Date")]
        public string Date { get; set; }
        [Display(Name = "Resolved")]
        public bool IsResolved { get; set; }

    }
    // end::class[]

}
