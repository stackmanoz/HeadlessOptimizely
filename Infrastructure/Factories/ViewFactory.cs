using Concrete.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Factories
{
    public class ViewFactory : ModelBuilder
    {
        public IActionResult RenderView(Func<IModel> model)
        {
            var result = base.BuildNavigation();
            result.Content = model();

            return new JsonResult(result);
        }
    }
}
