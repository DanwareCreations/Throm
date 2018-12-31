using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using TerminalEngine.Components;

namespace TerminalEngine.Systems {

    public class CharacterSpriteRenderSystem : IUpdatable {

        private readonly IReadOnlyList<Transform> _transforms;
        private readonly IReadOnlyList<CharacterSprite> _sprites;
        private readonly IReadOnlyList<GameEntity> _entities;
        private readonly ITerminalRenderer _renderer;

        public CharacterSpriteRenderSystem(IScene scene, ITerminalRenderer renderer) {
            _transforms = scene.GetComponents<Transform>();
            _sprites = scene.GetComponents<CharacterSprite>();
            _entities = scene.GetEntities<Transform, CharacterSprite>();

            _renderer = renderer;
        }

        public bool Update(TimeSpan deltaTime) {
            for (int e = 0; e < _entities.Count; ++e) {
                Transform trans = _transforms[_entities[e].ComponentIndices[0]];
                CharacterSprite sprite = _sprites[_entities[e].ComponentIndices[1]];
                _renderer.AddCharacter(sprite.Character, sprite.ForeColor, sprite.BackColor, trans.LocalLeft, trans.LocalTop);
            }
            return true;
        }
    }

}
