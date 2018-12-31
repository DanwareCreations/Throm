using System;
using System.Collections;
using System.Collections.Generic;
using TerminalEngine.Components;

namespace TerminalEngine {

    public interface IScene {

        string Name { get; }

        int GetNewEntityId();
        void AddEntityComponent<C>(int entityId, C component) where C : struct, IGameComponent;

        GameEntity? GetEntity(params Type[] componentTypes);
        GameEntity? GetEntity<C>() where C : struct, IGameComponent;
        GameEntity? GetEntity<C1, C2>()
            where C1 : struct, IGameComponent
            where C2 : struct, IGameComponent;
        GameEntity? GetEntity<C1, C2, C3>()
            where C1 : struct, IGameComponent
            where C2 : struct, IGameComponent
            where C3 : struct, IGameComponent;
        GameEntity? GetEntity<C1, C2, C3, C4>()
            where C1 : struct, IGameComponent
            where C2 : struct, IGameComponent
            where C3 : struct, IGameComponent
            where C4 : struct, IGameComponent;


        IReadOnlyList<GameEntity> GetEntities(params Type[] componentTypes);
        IReadOnlyList<GameEntity> GetEntities<C>() where C : struct, IGameComponent;
        IReadOnlyList<GameEntity> GetEntities<C1, C2>()
            where C1 : struct, IGameComponent
            where C2 : struct, IGameComponent;
        IReadOnlyList<GameEntity> GetEntities<C1, C2, C3>()
            where C1 : struct, IGameComponent
            where C2 : struct, IGameComponent
            where C3 : struct, IGameComponent;
        IReadOnlyList<GameEntity> GetEntities<C1, C2, C3, C4>()
            where C1 : struct, IGameComponent
            where C2 : struct, IGameComponent
            where C3 : struct, IGameComponent
            where C4 : struct, IGameComponent;

        C GetComponent<C>() where C : struct, IGameComponent;
        ComponentList<C> GetComponents<C>() where C : struct, IGameComponent;
    }

    public class Scene : IScene {

        private class EntityComponents {
            public IDictionary<Type, int> ComponentIndices = new Dictionary<Type, int>();
            public EntityComponents(int numComponents) {
                ComponentIndices = new Dictionary<Type, int>(numComponents);
            }
        }

        private int _entityId = 0;
        private readonly IDictionary<Type, IList> _comps = new Dictionary<Type, IList>();
        private readonly IDictionary<int, EntityComponents> _entities = new Dictionary<int, EntityComponents>();

        public Scene(string name) => Name = name;

        public string Name { get; set; } = "My Scene";

        public int GetNewEntityId() => ++_entityId;
        public void AddEntityComponent<C>(int entityId, C component) where C : struct, IGameComponent {
            // Make sure this Entity has been added to the list of all Entities
            bool hasEntity = _entities.TryGetValue(entityId, out EntityComponents entityComponents);
            if (!hasEntity)
                entityComponents = new EntityComponents(1);
            _entities[entityId] = entityComponents;

            // Add this Component to the Type-specific list of Components, and store its index on the Entity
            if (!_comps.TryGetValue(typeof(C), out IList comps)) {
                comps = new List<C>(1);
                _comps.Add(typeof(C), comps);
            }
            ((IList<C>)comps).Add(component);
            entityComponents.ComponentIndices[typeof(C)] = comps.Count - 1;
        }

        public GameEntity? GetEntity(params Type[] componentTypes) {
            var entities = GetEntities(componentTypes);
            return (entities.Count > 0) ? entities[0] : (GameEntity?)null;
        }
        public GameEntity? GetEntity<C>() where C : struct, IGameComponent => GetEntity(typeof(C));
        public GameEntity? GetEntity<C1, C2>()
            where C1 : struct, IGameComponent
            where C2 : struct, IGameComponent
            => GetEntity(typeof(C1), typeof(C2));
        public GameEntity? GetEntity<C1, C2, C3>()
            where C1 : struct, IGameComponent
            where C2 : struct, IGameComponent
            where C3 : struct, IGameComponent
            => GetEntity(typeof(C1), typeof(C2), typeof(C3));
        public GameEntity? GetEntity<C1, C2, C3, C4>()
            where C1 : struct, IGameComponent
            where C2 : struct, IGameComponent
            where C3 : struct, IGameComponent
            where C4 : struct, IGameComponent
            => GetEntity(typeof(C1), typeof(C2), typeof(C3), typeof(C4));

        public IReadOnlyList<GameEntity> GetEntities(params Type[] componentTypes) {
            var entities = new List<GameEntity>();

            // Create a new list of EntityComponents with only the indices of the Components with the requested Types
            foreach (int entityId in _entities.Keys) {
                EntityComponents comps = _entities[entityId];
                var newEntity = new GameEntity(entityId, componentTypes.Length);
                int ct;
                for (ct = 0; ct < componentTypes.Length; ++ct) {
                    if (comps.ComponentIndices.TryGetValue(componentTypes[ct], out int index))
                        newEntity.ComponentIndices[ct] = index;
                    else
                        break;
                }
                if (ct == componentTypes.Length)
                    entities.Add(newEntity);
            }

            return entities;
        }
        public IReadOnlyList<GameEntity> GetEntities<C>() where C : struct, IGameComponent => GetEntities(typeof(C));
        public IReadOnlyList<GameEntity> GetEntities<C1, C2>()
            where C1 : struct, IGameComponent
            where C2 : struct, IGameComponent
            => GetEntities(typeof(C1), typeof(C2));
        public IReadOnlyList<GameEntity> GetEntities<C1, C2, C3>()
            where C1 : struct, IGameComponent
            where C2 : struct, IGameComponent
            where C3 : struct, IGameComponent
            => GetEntities(typeof(C1), typeof(C2), typeof(C3));
        public IReadOnlyList<GameEntity> GetEntities<C1, C2, C3, C4>()
            where C1 : struct, IGameComponent
            where C2 : struct, IGameComponent
            where C3 : struct, IGameComponent
            where C4 : struct, IGameComponent
            => GetEntities(typeof(C1), typeof(C2), typeof(C3), typeof(C4));

        public C GetComponent<C>() where C : struct, IGameComponent => GetComponents<C>()[0];
        public ComponentList<C> GetComponents<C>() where C : struct, IGameComponent {
            if (!_comps.TryGetValue(typeof(C), out IList comps))
                comps = new List<C>();
            return new ComponentList<C>(comps);
        }

    }

}
