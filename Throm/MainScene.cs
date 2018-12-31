using Microsoft.Extensions.DependencyInjection;
using System;
using TerminalEngine;
using TerminalEngine.Components;

namespace Throm {

    class MainScene : Scene {

        public MainScene() : base("Throm") {
            // Add object 0
            int entityId;
            entityId = GetNewEntityId();
            AddEntityComponent(entityId, new Transform(localLeft: 10, localTop: 10));
            AddEntityComponent(entityId, new CharacterSprite('x'));

            // Add object 1
            entityId = GetNewEntityId();
            AddEntityComponent(entityId, new Transform(localLeft: 20, localTop: 20));
            AddEntityComponent(entityId, new CharacterSprite('x'));

            // Add object 2
            int playerId = GetNewEntityId();
            AddEntityComponent(playerId, new Transform(localLeft: 15, localTop: 10));
            AddEntityComponent(playerId, new CharacterSprite('O', foreColor: ConsoleColor.White));
            AddEntityComponent(playerId, new HumanMover(speed: 5));

            // Add a FileSprite
            int fileSpriteId = GetNewEntityId();
            AddEntityComponent(fileSpriteId, new Transform(localLeft: 30, localTop: 30));
            char[][] fileSpriteMatrix = new[] {
                new[] { '/', '-', '\\' },
                new[] { '|', '+', '|' },
                new[] { '\\', '-', '/' }
            };
            AddEntityComponent(fileSpriteId, new FileSprite(fileSpriteMatrix, ConsoleColor.White, ConsoleColor.Black));
        }

    }

}
