using Concrete.Models;
using EPiServer.ServiceLocation;
using Infrastructure.Factories;

namespace Infrastructure
{
    public abstract class ModelBuilder
    {
        private readonly Injected<MenuFactory> _menuFactory;
        public CustomApplicationModel BuildNavigation()
        {
            return new CustomApplicationModel()
            {
                Navigation = _menuFactory.Service.GetNavigationMainMenu()
            };
        }
    }
}
