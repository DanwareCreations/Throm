using System;
using System.Collections.Generic;

namespace TerminalEngine.Systems {

    public class LifecycleSystem : IUpdatable {

        private readonly IConsoleInput _input;

        public LifecycleSystem(IConsoleInput input) => _input = input;


        public bool Update(TimeSpan deltaTime) {
            IReadOnlyList<ConsoleKeyInfo> keys = _input.GetLatestKeys;
            for (int k = 0; k < keys.Count; ++k) {
                ConsoleKeyInfo key = keys[k];
                if (key.Key == ConsoleKey.Escape)
                    return false;
            }

            return true;
        }
    }

}
