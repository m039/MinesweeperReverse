using VContainer;
using VContainer.Unity;
using UnityEngine;

namespace MR
{
    public class GameLifetimeScope : LifetimeScope
    {
        #region Inspector

        [SerializeField] GameConfig _GameConfig;

        #endregion

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<MineField>();
            builder.RegisterInstance(_GameConfig);
            builder.RegisterComponentInHierarchy<NextNumberPanel>()
                .AsImplementedInterfaces()
                .AsSelf();
            builder.RegisterEntryPoint<GameController>();
            builder.RegisterComponentInHierarchy<HelpScreen>();
            builder.RegisterComponentInHierarchy<SettingsScreen>();
            builder.RegisterComponentInHierarchy<LoseScreen>();
            builder.RegisterComponentInHierarchy<WinScreen>();
            builder.RegisterComponentInHierarchy<GameTopPanel>();
            builder.RegisterComponentInHierarchy<HealthCounter>();
            builder.RegisterComponentInHierarchy<ConfettiController>();
        }
    }
}
