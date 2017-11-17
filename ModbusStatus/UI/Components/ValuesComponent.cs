using System;
using System.Collections.Generic;
using System.Text;
using ModbusStatus.UI.Shared;

namespace ModbusStatus.UI.Components
{
    public class ValuesComponent : IValuesComponent
    {
        private readonly IConsoleExtensions _consoleExtensions;

        private readonly ConsoleColor _valueColor;
        private readonly ConsoleColor _valueBackgroundColor;

        private FormPosition _stateTextForm;

        public ValuesComponent(IConsoleExtensions consoleExtensions,
            ConsoleColor valueColor, ConsoleColor valueBackgroundColor)
        {
            _consoleExtensions = consoleExtensions;
            _valueColor = valueColor;
            _valueBackgroundColor = valueBackgroundColor;
        }

        public void Initialize(FormPosition formPosition)
        {
            _stateTextForm = formPosition;
        }

        public void SetValues(bool[] state)
        {
            Console.BackgroundColor = _valueColor;
            Console.ForegroundColor = _valueBackgroundColor;

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
