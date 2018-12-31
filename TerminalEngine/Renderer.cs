using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TerminalEngine {

    public class RendererOptions {
        public ConsoleColor FillBackColor { get; set; } = ConsoleColor.Black;
        public ConsoleColor FillForeColor { get; set; } = ConsoleColor.White;
        public char FillCharacter { get; set; } = ' ';
    }

    public interface ITerminalRenderer {

        void AddCharacter(char character);
        void AddCharacter(char character, ConsoleColor foreColor, ConsoleColor backColor);
        void AddCharacter(char character, int left, int top);
        void AddCharacter(char character, ConsoleColor foreColor, ConsoleColor backColor, int left, int top);

        void AddSprite(char[][] characters);
        void AddSprite(char[][] characters, ConsoleColor foreColor, ConsoleColor backColor);
        void AddSprite(char[][] characters, int left, int top);
        void AddSprite(char[][] characters, ConsoleColor foreColor, ConsoleColor backColor, int left, int top);

        void RenderCharacter(char character);
        void RenderCharacter(char character, ConsoleColor foreColor, ConsoleColor backColor);
        void RenderCharacter(char character, int left, int top);
        void RenderCharacter(char character, ConsoleColor foreColor, ConsoleColor backColor, int left, int top);

        void RenderSprite(char[][] characters);
        void RenderSprite(char[][] characters, ConsoleColor foreColor, ConsoleColor backColor);
        void RenderSprite(char[][] characters, int left, int top);
        void RenderSprite(char[][] characters, ConsoleColor foreColor, ConsoleColor backColor, int left, int top);

    }

    public class TerminalRenderer : ITerminalRenderer, IUpdatable {

        private readonly RendererOptions _rendererOpts;
        private readonly TerminalGameOptions _gameOpts;
        private readonly char[][] _blankBuffer;
        private readonly IDictionary<ValueTuple<ConsoleColor, ConsoleColor>, char[][]> _colorBuffers = new Dictionary<ValueTuple<ConsoleColor, ConsoleColor>, char[][]>();

        public TerminalRenderer(IOptions<RendererOptions> rendererOptions, IOptions<TerminalGameOptions> gameOptions) {
            _rendererOpts = rendererOptions.Value;
            _gameOpts = gameOptions.Value;

            // Create the blank buffer
            int numRows = _gameOpts.BufferHeight;
            int numCols = _gameOpts.BufferWidth;
            _blankBuffer = new char[numRows][];
            for (int r = 0; r < numRows; ++r) {
                _blankBuffer[r] = new char[numCols];
                for (int c = 0; c < numCols; ++c)
                    _blankBuffer[r][c] = _rendererOpts.FillCharacter;
            }

            // Start all color buffers off cleared
            clearBuffer((_rendererOpts.FillForeColor, _rendererOpts.FillBackColor));
        }

        public void AddCharacter(char character) =>
            AddCharacter(character, ConsoleColor.White, ConsoleColor.Black, 0, 0);
        public void AddCharacter(char character, ConsoleColor foreColor, ConsoleColor backColor) =>
            AddCharacter(character, foreColor, backColor, 0, 0);
        public void AddCharacter(char character, int left, int top) =>
            AddCharacter(character, ConsoleColor.White, ConsoleColor.Black, left, top);
        public void AddCharacter(char character, ConsoleColor foreColor, ConsoleColor backColor, int left, int top) =>
            AddSprite(new[] { new[] { character } }, foreColor, backColor, left, top);

        public void AddSprite(char[][] characters) =>
            AddSprite(characters, ConsoleColor.White, ConsoleColor.Black, 0, 0);
        public void AddSprite(char[][] characters, ConsoleColor foreColor, ConsoleColor backColor) =>
            AddSprite(characters, foreColor, backColor, 0, 0);
        public void AddSprite(char[][] characters, int left, int top) =>
            AddSprite(characters, ConsoleColor.White, ConsoleColor.Black, left, top);
        public void AddSprite(char[][] characters, ConsoleColor foreColor, ConsoleColor backColor, int left, int top) {
            // Add the color buffer for these colors, if necessary
            (ConsoleColor, ConsoleColor) colors = (foreColor, backColor);
            if (!_colorBuffers.TryGetValue(colors, out char[][] buffer))
                clearBuffer(colors);

            // Copy this sprite to the buffer
            for (int b = 0; b < characters.Length; ++b)
                characters[b].CopyTo(buffer[top + b], left);
        }

        public void RenderCharacter(char character) =>
            RenderCharacter(character, ConsoleColor.White, ConsoleColor.Black, 0, 0);
        public void RenderCharacter(char character, ConsoleColor foreColor, ConsoleColor backColor) =>
            RenderCharacter(character, foreColor, backColor, 0, 0);
        public void RenderCharacter(char character, int left, int top) =>
            RenderCharacter(character, ConsoleColor.White, ConsoleColor.Black, left, top);
        public void RenderCharacter(char character, ConsoleColor foreColor, ConsoleColor backColor, int left, int top) {
            Console.ForegroundColor = foreColor;
            Console.BackgroundColor = backColor;
            Console.SetCursorPosition(left, top);
            Console.Write(character);
        }

        public void RenderSprite(char[][] characters) =>
            RenderSprite(characters, ConsoleColor.White, ConsoleColor.Black, 0, 0);
        public void RenderSprite(char[][] characters, ConsoleColor foreColor, ConsoleColor backColor) =>
            RenderSprite(characters, foreColor, backColor, 0, 0);
        public void RenderSprite(char[][] characters, int left, int top) =>
            RenderSprite(characters, ConsoleColor.White, ConsoleColor.Black, left, top);
        public void RenderSprite(char[][] characters, ConsoleColor foreColor, ConsoleColor backColor, int left, int top) {
            Console.ForegroundColor = foreColor;
            Console.BackgroundColor = backColor;
            for (int b = 0; b < characters.Length; ++b) {
                Console.SetCursorPosition(left, top + b);
                Console.Write(characters[b]);
            }
        }

        public bool Update(TimeSpan deltaTime) {
            // Render the contents of all buffers to the Console
            foreach ((ConsoleColor foreColor, ConsoleColor backColor) in _colorBuffers.Keys) {
                Console.ForegroundColor = foreColor;
                Console.BackgroundColor = backColor;
                char[][] buffer = _colorBuffers[(foreColor, backColor)];
                for (int b = 0; b < buffer.Length; ++b) {
                    char[] chars = buffer[b];
                    Console.SetCursorPosition(0, b);
                    Console.Write(chars);
                }
            }

            // Clear all per-color buffers
            (ConsoleColor, ConsoleColor)[] keys = _colorBuffers.Keys.ToArray();
            foreach ((ConsoleColor, ConsoleColor) colors in keys)
                clearBuffer(colors);

            return true;
        }

        private void clearBuffer((ConsoleColor, ConsoleColor) colors) {
            char[][] buffer = new char[_blankBuffer.Length][];
            for (int b = 0; b < _blankBuffer.Length; ++b) {
                buffer[b] = new char[_blankBuffer[b].Length];
                _blankBuffer[b].CopyTo(buffer[b], 0);
            }
            _colorBuffers[colors] = buffer;
        }

    }

}
