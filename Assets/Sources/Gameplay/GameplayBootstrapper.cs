using Assets.Sources.Infrastructure.Factories.UiFactory;
using System;
using Zenject;

namespace Assets.Sources.Gameplay
{
    public class GameplayBootstrapper : IInitializable
    {
        private readonly IUiFactory _uiFactory;

        public GameplayBootstrapper(IUiFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public void Initialize()
        {
            _uiFactory.CreateWindow();
        }
    }
}
