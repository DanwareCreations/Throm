using System;
using System.Collections.Generic;
using TerminalEngine;
using TerminalEngine.Components;

namespace Throm {

    public class HumanMoverSystem : IUpdatable {

        private readonly int _transIndex = -1;
        private readonly ComponentList<Transform> _transforms;
        private readonly HumanMover? _mover = null;

        private readonly IConsoleInput _input;

        private double _continuousLeft;
        private double _continuousTop;

        public HumanMoverSystem(IScene scene, IConsoleInput input) {
            GameEntity? entity = scene.GetEntity<Transform, HumanMover>();
            if (entity.HasValue) {
                _transIndex = entity.Value.ComponentIndices[0];
                _transforms = scene.GetComponents<Transform>();
                _mover = scene.GetComponents<HumanMover>()[entity.Value.ComponentIndices[1]];

                _continuousLeft = _transforms[_transIndex].LocalLeft;
                _continuousTop = _transforms[_transIndex].LocalTop;
            }


            _input = input;
        }

        public bool Update(TimeSpan deltaTime) {
            if (!_mover.HasValue)
                return true;

            // Adjust velocity from player input
            int speedR = 0;
            int speedC = 0;

            IReadOnlyList<ConsoleKeyInfo> keys = _input.GetLatestKeys;
            for (int k = 0; k < keys.Count; ++k) {
                ConsoleKeyInfo key = keys[k];
                switch (key.Key) {
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A:
                        speedC -= _mover.Value.Speed;
                        break;

                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        speedC += _mover.Value.Speed;
                        break;

                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        speedR -= _mover.Value.Speed;
                        break;

                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        speedR += _mover.Value.Speed;
                        break;
                }
            }

            // Adjust position according to velocity
            Transform trans = _transforms[_transIndex];
            double dt = deltaTime.TotalSeconds;
            _continuousLeft += speedC * dt;
            _continuousTop  += speedR * dt;
            _transforms[_transIndex] = new Transform((int)_continuousLeft, (int)_continuousTop);

            return true;
        }
    }

}
