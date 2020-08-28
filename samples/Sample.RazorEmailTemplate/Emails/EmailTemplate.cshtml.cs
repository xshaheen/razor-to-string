using Sharp.RazorToString;

namespace Sample.RazorEmailTemplate.Emails
{
    public class EmailTemplate : RazorViewModel
    {
        public string Content { get; set; }

        public string Url { get; set; }
    }
}