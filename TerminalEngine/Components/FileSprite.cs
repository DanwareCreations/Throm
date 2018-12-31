using System;

namespace TerminalEngine.Components {

    public struct FileSprite : IGameComponent {

        public char[][] CharacterBuffer;
        public ConsoleColor BackColor;
        public ConsoleColor ForeColor;
        public int PivotLeft;
        public int PivotTop;

        public FileSprite(char[][] characters) : this(characters, ConsoleColor.White, ConsoleColor.Black, 0, 0) { }
        public FileSprite(char[][] characters, ConsoleColor foreColor, ConsoleColor backColor) : this(characters, foreColor, backColor, 0, 0) { }
        public FileSprite(char[][] characters, int pivotLeft, int pivotTop) : this(characters, ConsoleColor.White, ConsoleColor.Black, pivotLeft, pivotTop) { }
        public FileSprite(char[][] characters, ConsoleColor foreColor, ConsoleColor backColor, int pivotLeft, int pivotTop) {
            CharacterBuffer = characters;
            ForeColor = foreColor;
            BackColor = backColor;
            PivotLeft = pivotLeft;
            PivotTop = pivotTop;
        }

    }

}
