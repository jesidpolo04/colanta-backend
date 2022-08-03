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

        public CustomConsole endPharagraph()
        {
            Console.Out.Write("\n");
            Console.Out.Write("\n");
            return this;
        }

        public CustomConsole successColor()
        {
            this.color(ConsoleColor.DarkGreen);
            return this;
        }

        public CustomConsole errorColor()
        {
            this.color(ConsoleColor.DarkRed);
            return this;
        }

        public CustomConsole infoColor()
        {
            this.color(ConsoleColor.DarkCyan);
            return this;
        }

        public CustomConsole warningColor()
        {
            this.color(ConsoleColor.Yellow);
            return this;
        }

        public CustomConsole whiteColor()
        {
            this.color(ConsoleColor.White);
            return this;
        }

        public CustomConsole grayColor()
        {
            this.color(ConsoleColor.DarkGray);
            return this;
        }

        public CustomConsole magentaColor()
        {
            this.color(ConsoleColor.Magenta);
            return this;
        }

        public void processStartsAt(string processName, DateTime at)
        {
            this.warningColor().write("Iniciando proceso:")
                .infoColor().write(processName)
                .grayColor().write("Fecha Inicio:")
                .whiteColor().write(DateTime.Now.ToString()).dot();
        }

        public void processEndstAt(string processName, DateTime at)
        {
            this.warningColor().write("Finalizando proceso:")
                .infoColor().write(processName)
                .grayColor().write("Fecha Finalización:")
                .whiteColor().write(DateTime.Now.ToString()).dot();
        }

        public void throwException(string message)
        {
            this.errorColor().write("Error: ")
                .whiteColor().write(message).dot();
        }
    }
}
