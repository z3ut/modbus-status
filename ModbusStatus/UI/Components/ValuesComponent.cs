using System;
using System.Collections.Generic;
using System.Text;
using ModbusStatus.UI.Shared;

namespace ModbusStatus.UI.Components
{
    public class ValuesComponent : IValuesComponent
    {
        private readonly IConsoleExtensions _consoleExtensions;

        private const ConsoleColor STATE_COLOR = ConsoleColor.DarkMagenta;
        private const ConsoleColor STATE_BACKGROUND_COLOR = ConsoleColor.DarkYellow;

        private FormPosition _stateTextForm;

        public ValuesComponent(IConsoleExtensions consoleExtensions)
        {
            _consoleExtensions = consoleExtensions;
        }

        public void Initialize(FormPosition formPosition)
        {
            _stateTextForm = formPosition;
        }

        public void SetValues(bool[] state)
        {
            Console.BackgroundColor = STATE_COLOR;
            Console.ForegroundColor = STATE_BACKGROUND_COLOR;

            for (var i = 0; i < state.Length; i++)
            {
                Console.SetCursorPosition(_stateTextForm.ContentLeft + 1,
                    _stateTextForm.ContentTop + i);
                Console.Write($"DI-{i.ToString("00")}: {Convert.ToInt32(state[i])}");
            }

            Console.ResetColor();
        }
    }
}
