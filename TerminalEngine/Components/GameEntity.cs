using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TerminalEngine.Components;

namespace TerminalEngine {

    public struct GameEntity {
        public int Id;
        public int[] ComponentIndices;
        public GameEntity(int id, int numComponents) {
            Id = id;
            ComponentIndices = new int[numComponents];
        }
        public GameEntity(int numComponents, GameEntity original) {
            Id = original.Id;
            ComponentIndices = new int[numComponents];
            original.ComponentIndices.CopyTo(ComponentIndices, 0);
        }
    }

}
