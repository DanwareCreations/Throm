using System;
using System.Collections.Generic;

namespace TerminalEngine {

    public interface IConsoleInput {
        IReadOnlyList<ConsoleKeyInfo> GetLatestKeys { get; }
    }

    public class ConsoleInput : IConsoleInput, IUpdatable {

        private readonly List<ConsoleKeyInfo> _keys = new List<ConsoleKeyInfo>();

        public bool Update(TimeSpan deltaTime) {
            _keys.Clear();
            while (Console.KeyAvailable)
                _keys.Add(Console.ReadKey(intercept: true));
            return true;
        }

        public IReadOnlyList<ConsoleKeyInfo> GetLatestKeys => _keys;

    }

}
