using Microsoft.Extensions.DependencyInjection;
using System;
using TerminalEngine;
using TerminalEngine.Components;

namespace Throm {

    class Program {

        static int Main(string[] args) {
            // Define the application environment for the game
            var env = new GameEnvironment() {
                Assembly = typeof(Program).Assembly,
                Environment = "Development",
            };

            // Define the Game instance and play!
            var game = new ThromGame(env, args);
            game.Play();

            return 0;
        }

    }

}
