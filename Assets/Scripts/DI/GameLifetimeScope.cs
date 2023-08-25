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
            builder.Register<ProgressService>(Lifetime.Singleton);
            builder.RegisterInstance(_GameConfig);
            builder.RegisterComponentInHierarchy<NextNumberPanel>()
                .AsImplementedInterfaces()
                .AsSelf();
            builder.RegisterEntryPoint<GameController>();
            builder.RegisterEntryPoint<AudioController>()
                .AsSelf();
            builder.RegisterComponentInHierarchy<HelpScreen>();
            builder.RegisterComponentInHierarchy<SettingsScreen>();
            builder.RegisterComponentInHierarchy<LoseScreen>();
            builder.RegisterComponentInHierarchy<WinScreen>();
            builder.RegisterComponentInHierarchy<MainControls>();
            builder.RegisterComponentInHierarchy<HealthCounter>();
            builder.RegisterComponentInHierarchy<ConfettiController>();
            builder.RegisterComponentInHierarchy<SceneData>();
            builder.RegisterInstance(YandexGamesManager.Instance);
            builder.RegisterInstance(YandexMetrikaManager.Instance);
        }
    }
}
