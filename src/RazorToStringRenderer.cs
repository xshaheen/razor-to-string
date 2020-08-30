using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

namespace Sharp.RazorToString
{
    public class RazorToStringRenderer
    {
        private readonly IRazorViewEngine     _viewEngine;
        private readonly ITempDataProvider    _tempData;
        private readonly IServiceProvider     _services;
        private readonly RazorToStringOptions _options;

        public RazorToStringRenderer(
            IOptions<RazorToStringOptions> options,
            IRazorViewEngine viewEngine,
            ITempDataProvider tempData,
            IServiceProvider services)
        {
            _options    = options.Value;
            _viewEngine = viewEngine;
            _tempData   = tempData;
            _services   = services;
        }

        public async Task<string> RenderAsync<TModel>(TModel viewModel) where TModel : RazorViewModel
        {
            var actionContext = _GetActionContext();
            var view          = _FindView(actionContext, viewModel.Path);

            await using var output = new StringWriter();

            var viewDictionary = new ViewDataDictionary<TModel>(viewModel.ViewData) { Model = viewModel };

            var viewContext = new ViewContext(
                actionContext,
                view,
                viewDictionary,
                new TempDataDictionary(actionContext.HttpContext, _tempData),
                output,
                new HtmlHelperOptions());

            await view.RenderAsync(viewContext);

            return output.ToString();
        }

        public async Task<string> RenderAsync<TModel>(string viewName, TModel model)
        {
            var actionContext = _GetActionContext();
            var view          = _FindView(actionContext, viewName);

            await using var output = new StringWriter();

            var viewContext = new ViewContext(
                actionContext,
                view,
                new ViewDataDictionary<TModel>(new EmptyModelMetadataProvider(),
                    new ModelStateDictionary()) { Model = model },
                new TempDataDictionary(actionContext.HttpContext, _tempData),
                output,
                new HtmlHelperOptions());

            await view.RenderAsync(viewContext);

            return output.ToString();
        }

        private IView _FindView(ActionContext actionContext, string viewName)
        {
            var getViewResult = viewName.StartsWith("/")
                ? _viewEngine.GetView(null, viewName, true)
                : _viewEngine.GetView(null, Path.Combine(_options.ViewsPath, viewName), true);

            if (getViewResult.Success)
                return getViewResult.View;

            var findViewResult = _viewEngine.FindView(actionContext, viewName, true);

            if (findViewResult.Success)
                return findViewResult.View;

            var searchedLocations = getViewResult.SearchedLocations
                .Concat(findViewResult.SearchedLocations);

            var errorMessage = string.Join(
                Environment.NewLine,
                new[] { $"Unable to find view '{viewName}'. The following locations were searched:" }
                    .Concat(searchedLocations));

            throw new InvalidOperationException(errorMessage);
        }

        private ActionContext _GetActionContext()
        {
            var httpContext = new DefaultHttpContext { RequestServices = _services };

            return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        }
    }
}