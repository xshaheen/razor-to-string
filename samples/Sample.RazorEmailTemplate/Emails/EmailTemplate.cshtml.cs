using Sharp.RazorToString;

namespace Sample.RazorEmailTemplate.Emails {
    public class EmailTemplate : RazorViewModel {
        public string Content { get; init; }

        public string Url { get; init; }
    }
}