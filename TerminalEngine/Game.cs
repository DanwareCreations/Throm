using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace TerminalEngine {

    public interface IGame {
        IServiceCollection ServiceCollection { get; }
        void AddScene(IScene scene);
        void Play();
    }

    public class Game : IGame {

        public IServiceCollection ServiceCollection { get; } = new ServiceCollection();
        protected IServiceProvider? ServiceProvider;
        protected IScene? Scene;

        private IUpdatable[]? _updatables;

        public void AddScene(IScene scene) => ServiceCollection.AddSingleton(scene);
        public void Play() {
            // Build the service provider for dependency injection
            ServiceProvider = ServiceCollection.BuildServiceProvider();

            // Cache some specific services
            Scene = ServiceProvider.GetRequiredService<IScene>();
            _updatables = ServiceCollection.Select(s => s.ImplementationType ?? s.ServiceType)
                                           .Where(t => typeof(IUpdatable).IsAssignableFrom(t))
                                           .Select(t => (IUpdatable)ServiceProvider.GetService(t))
                                           .ToArray();
            ILogger<Game> logger = ServiceProvider.GetService<ILogger<Game>>();
            OnBuild();

            // Run game loop
            TimeSpan deltaTime;
            DateTime oldTime = DateTime.Now;
            int frameNum = 0;
            bool keepPlaying = true;
            while (keepPlaying) {
                DateTime newTime = DateTime.Now;
                deltaTime = newTime - oldTime;
                oldTime = newTime;
                ++frameNum;

                for (int u = 0; u < _updatables.Length; ++u) {
                    keepPlaying = _updatables[u].Update(deltaTime);
                    if (!keepPlaying)
                        break;
                }
            }
        }

        protected virtual void OnBuild() { }

    }


}
