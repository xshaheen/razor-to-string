using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Sharp.RazorToString {
    public abstract class RazorViewModel {
        private ViewDataDictionary? _viewData;
        private string?             _viewPath;

        protected RazorViewModel() { }

        protected RazorViewModel(string path) => Path = path;

        /// <summary>
        /// Gets or sets <see cref="ViewDataDictionary"/> can accessed in the view by using ViewBag or
        /// ViewData.
        /// </summary>
        public ViewDataDictionary ViewData {
            get
                => _viewData ??= new ViewDataDictionary(
                    new EmptyModelMetadataProvider(),
                    new ModelStateDictionary());
            set => _viewData = value ?? throw new ArgumentNullException(nameof(ViewData));
        }

        /// <summary>
        /// Gets the view path associated with this model.
        /// </summary>
        public string Path {
            get {
                if (_viewPath is not null)
                    return _viewPath;

                var viewName = GetType().Name;

                _viewPath = viewName + ".cshtml";

                return _viewPath;
            }
            private init => _viewPath = value;
        }
    }
}