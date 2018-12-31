using System;
using TerminalEngine.Components;

namespace Throm {

    public struct HumanMover : IGameComponent {

        public int Speed;
        public bool LimitDiagonalSpeed;

        public ConsoleKey LeftKey;
        public ConsoleKey RightKey;
        public ConsoleKey UpKey;
        public ConsoleKey DownKey;

        public HumanMover(int speed) {
            Speed = speed;
            LimitDiagonalSpeed = true;

            LeftKey = ConsoleKey.LeftArrow;
            RightKey = ConsoleKey.RightArrow;
            UpKey = ConsoleKey.UpArrow;
            DownKey = ConsoleKey.DownArrow;
        }

    }

}
