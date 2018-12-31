using Microsoft.Extensions.DependencyInjection;
using TerminalEngine;

namespace Throm {

    class ThromGame : TerminalGame {

        public ThromGame(GameEnvironment env, string[] args) : base(env, args) {
            AddScene(new MainScene());
        }

        protected override void AddGameSystems() {
            base.AddGameSystems();


            ServiceCollection.AddSingleton<HumanMoverSystem>();
        }

    }

}
