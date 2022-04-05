namespace colanta_backend.App.Shared.Application
{
    using System;
    public class CustomConsole
    {
        private ConsoleColor _color;

        public CustomConsole color(ConsoleColor color)
        {
            this._color = color;
            return this;
        }

        public CustomConsole skipLine()
        {
            System.Console.Out.Write("\n");
            return this;
        }

        public CustomConsole write(string text)
        {
            System.Console.ForegroundColor = this._color;
            System.Console.Out.Write(text + " ");
            return this;
        }

        public CustomConsole writeLine(string text)
        {
            System.Console.ForegroundColor = this._color;
            System.Console.Out.WriteLine(text + " ");
            return this;
        }
        public CustomConsole reset()
        {
            this._color = ConsoleColor.White;
            this.skipLine();
            return this;
        }

        public CustomConsole dot(int lineSkips = 1)
        {
            Console.Out.Write(".");
            for (int i = 0; i < lineSkips; i++)
            {
                Console.Out.Write("\n");
            }
            return this;
        }
    }
}
