using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using TerminalEngine.Components;

namespace TerminalEngine.Systems {

    public class FileSpriteRenderSystem : IUpdatable {

        private readonly IReadOnlyList<Transform> _transforms;
        private readonly IReadOnlyList<FileSprite> _sprites;
        private readonly IReadOnlyList<GameEntity> _entities;

        private readonly ITerminalRenderer _renderer;

        public FileSpriteRenderSystem(IScene scene, ITerminalRenderer renderer) {
            _transforms = scene.GetComponents<Transform>();
            _sprites = scene.GetComponents<FileSprite>();
            _entities = scene.GetEntities<Transform, FileSprite>();

            _renderer = renderer;
        }

        public bool Update(TimeSpan deltaTime) {
            for (int e = 0; e < _entities.Count; ++e) {
                Transform trans = _transforms[_entities[e].ComponentIndices[0]];
                FileSprite sprite = _sprites[_entities[e].ComponentIndices[1]];
                _renderer.AddSprite(sprite.CharacterBuffer, sprite.ForeColor, sprite.BackColor, trans.LocalLeft, trans.LocalTop);
            }
            return true;
        }
    }

}
