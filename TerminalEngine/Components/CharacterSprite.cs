using System;

namespace TerminalEngine.Components {

    public struct CharacterSprite : IGameComponent {

        public char Character;
        public ConsoleColor BackColor;
        public ConsoleColor ForeColor;

        public CharacterSprite(char character, ConsoleColor foreColor = ConsoleColor.White, ConsoleColor backColor = ConsoleColor.Black) {
            Character = character;
            ForeColor = foreColor;
            BackColor = backColor;
        }

    }

}
