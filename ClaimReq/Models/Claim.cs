namespace ClaimReq.Models
{
    // tag::class[]
    public class Claim
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Type => "Profile";
    }
    // end::class[]
}
