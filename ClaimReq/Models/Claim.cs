namespace ClaimReq.Models
{
    // tag::class[]
    public class Claim
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Type => "Profile";

        //Video Info
        public string VideoTitle { get; set; }
        public string VideoUrl { get; set; }
        public string VideoId { get; set; }
        public string VideoCategory { get; set; }
        public string VideoDescription { get; set; }
        public string VideoTags { get; set; }
        public string VideoDatePublished { get; set; }
        public string VideoChannelTitle { get; set; }
        public string altUrlUpload { get; set; }


        //Claim info
        public string Description { get; set; }
        public string Date { get; set; }
        public bool IsResolved { get; set; }

    }
    // end::class[]

}
