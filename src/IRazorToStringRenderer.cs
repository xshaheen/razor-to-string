using System.Threading.Tasks;

namespace Sharp.RazorToString {
    public interface IRazorToStringRenderer {
        Task<string> RenderAsync<TModel>(TModel viewModel) where TModel : RazorViewModel;

        Task<string> RenderAsync<TModel>(string viewName, TModel model);
    }
}