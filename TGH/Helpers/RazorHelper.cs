using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.IO;
using System.Threading.Tasks;

namespace TGH.Helpers
{
    public class RazorHelper
    {
        private IHttpContextAccessor httpContextAccessor;
        private IActionContextAccessor actionContextAccessor;
        private ICompositeViewEngine viewEngine;
        private ITempDataProvider tempDataProvider;

        public RazorHelper(
            IHttpContextAccessor httpContextAccessor,
            IActionContextAccessor actionContextAccessor,
            ICompositeViewEngine viewEngine,
            ITempDataProvider tempDataProvider)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.actionContextAccessor = actionContextAccessor;
            this.viewEngine = viewEngine;
            this.tempDataProvider = tempDataProvider;
        }

        public async Task<string> RenderView<T>(string name, T model) where T : class
        {
            var actionContext = actionContextAccessor.ActionContext;

            var viewEngineResult = viewEngine.FindView(actionContext, name, false);

            using (var output = new StringWriter())
            {
                var view = viewEngineResult.View;
                var viewContext = new ViewContext(
                    actionContext,
                    viewEngineResult.View,
                    new ViewDataDictionary<T>(
                        metadataProvider: new EmptyModelMetadataProvider(),
                        modelState: new ModelStateDictionary())
                    {
                        Model = model
                    },
                    new TempDataDictionary(
                        httpContextAccessor.HttpContext,
                        tempDataProvider),
                    output,
                    new HtmlHelperOptions());

                await view.RenderAsync(viewContext);

                return output.ToString();
            }
        }
    }
}