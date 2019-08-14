using System.ComponentModel.DataAnnotations;

namespace ClaimReq.Models
{
    // tag::class[]
    public class Claim
    {

        public string Type => "Claim";

        //Claim info
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Date")]
        public string Date { get; set; }
        [Display(Name = "Resolved")]
        public bool IsResolved { get; set; }
        [Display(Name = "Alt Url Upload")]
        public string altUrlUpload { get; set; }

        public VideoInfo VideoInfo { get; set; }

    }
    // end::class[]

}
