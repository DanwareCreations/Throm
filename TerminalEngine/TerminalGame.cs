using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using TerminalEngine.Systems;

namespace TerminalEngine {

    public class TerminalGameOptions {
        public bool MatchBufferSizeToWindow { get; set; } = true;
        public int BufferWidth { get; set; } = Console.LargestWindowWidth;
        public int BufferHeight { get; set; } = Console.LargestWindowHeight;

        public bool UseLargestWindow { get; set; } = true;
        public int WindowWidth { get; set; } = Console.LargestWindowWidth;
        public int WindowHeight { get; set; } = Console.LargestWindowHeight;
        public int WindowLeft { get; set; } = 0;
        public int WindowTop { get; set; } = 0;
    }

    public class TerminalGame : Game {

        private readonly GameEnvironment _env;

        public TerminalGame(GameEnvironment env, string[] args) {
            // Using DI in a console app like this inspired by: https://pioneercode.com/post/dependency-injection-logging-and-configuration-in-a-dot-net-core-console-

            _env = env;

            IConfigurationRoot config = buildConfiguration(args);
            configureServices(config);
        }

        protected override void OnBuild() {
            base.OnBuild();

            // Adjust configuration defaults
            TerminalGameOptions cfg = ServiceProvider.GetService<IOptions<TerminalGameOptions>>().Value;
            if (cfg.UseLargestWindow) {
                cfg.WindowWidth = Console.LargestWindowWidth;
                cfg.WindowHeight = Console.LargestWindowHeight;
            }
            if (cfg.MatchBufferSizeToWindow) {
                cfg.BufferWidth = cfg.WindowWidth;
                cfg.BufferHeight = cfg.WindowHeight;
            }

            // Set up the Console window
            Console.SetWindowSize(cfg.WindowWidth, cfg.WindowHeight);
            Console.SetWindowPosition(cfg.WindowLeft, cfg.WindowTop);
            Console.SetBufferSize(cfg.BufferWidth, cfg.BufferHeight);

            Console.Title = Scene.Name;

            Console.CursorVisible = false;
            Console.TreatControlCAsInput = true;
        }

        // HELPERS
        private IConfigurationRoot buildConfiguration(string[] args) {
            IConfigurationBuilder config = new ConfigurationBuilder();
            config.SetBasePath(Directory.GetCurrentDirectory());
            config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{_env.Environment}.json", optional: true, reloadOnChange: true);

            if (_env.IsDevelopment()) {
                if (_env.Assembly != null)
                    config.AddUserSecrets(_env.Assembly, optional: true);
            }

            config.AddEnvironmentVariables();

            if (args != null)
                config.AddCommandLine(args);

            return config.Build();
        }
        private void configureServices(IConfiguration config) {
            // Add logging
            ServiceCollection.AddLogging(logs => {
                logs.AddConfiguration(config.GetSection("Logging"));
                logs.AddDebug();
            });

            // Add input handling
            ServiceCollection.AddSingleton<ConsoleInput>();
            ServiceCollection.AddSingleton<IConsoleInput>(provider => provider.GetService<ConsoleInput>());

            // Add options from configuration
            ServiceCollection.AddOptions();
            ServiceCollection.Configure<TerminalGameOptions>(config.GetSection("Game"));
            ServiceCollection.Configure<RendererOptions>(config.GetSection("Renderer"));

            // Add game systems
            AddGameSystems();

            // Add rendering
            ServiceCollection.AddSingleton<TerminalRenderer>();
            ServiceCollection.AddSingleton<ITerminalRenderer>(provider => provider.GetService<TerminalRenderer>());
        }
        protected virtual void AddGameSystems() {
            ServiceCollection.AddSingleton<LifecycleSystem>();
            ServiceCollection.AddSingleton<CharacterSpriteRenderSystem>();
            ServiceCollection.AddSingleton<FileSpriteRenderSystem>();
        }

    }

}
