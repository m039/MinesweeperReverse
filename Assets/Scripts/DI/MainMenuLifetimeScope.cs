using VContainer;
using VContainer.Unity;

namespace MR
{
    public class MainMenuLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<MainMenuController>();
            builder.Register<ProgressService>(Lifetime.Singleton);
            builder.RegisterComponentInHierarchy<MainMenuView>();
        }
    }
}
