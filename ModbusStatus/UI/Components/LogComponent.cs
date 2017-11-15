using System;
using System.Collections.Generic;
using System.Text;
using ModbusStatus.StateEvents;
using ModbusStatus.UI.Shared;
using System.Linq;

namespace ModbusStatus.UI.Components
{
    public class LogComponent : ILogComponent
    {
        private readonly IConsoleExtensions _consoleExtensions;

        private const int MAX_EVENT_COUNT = 100;
        private List<IStateEvent> _stateEvents = new List<IStateEvent>();

        private const string LOG_DATE_FORMAT = "yyyy-dd-MM HH.mm.ss";

        private FormPosition _logTextForm;

        public LogComponent(IConsoleExtensions consoleExtensions)
        {
            _consoleExtensions = consoleExtensions;
        }

        public void Initialize(FormPosition formPosition)
        {
            _logTextForm = formPosition;
        }

        public void AddLog(IStateEvent stateEvent)
        {
            AddLog(new List<IStateEvent>() { stateEvent });
        }

        public void AddLog(IEnumerable<IStateEvent> stateEvents)
        {
            _stateEvents.AddRange(stateEvents);
            _stateEvents = _stateEvents
                .Skip(Math.Max(0, _stateEvents.Count() - MAX_EVENT_COUNT))
                .ToList();

            ClearLog();

            var numberOfLogsToShow = _logTextForm.ContentHeight;
            var displayEvents = _stateEvents.TakeLast(numberOfLogsToShow).ToList();

            for (var i = 0; i < displayEvents.Count; i++)
            {
                var stateEvent = displayEvents[i];
                Console.SetCursorPosition(_logTextForm.ContentLeft, _logTextForm.ContentTop + i);
                Console.Write($"{stateEvent.Date.ToString(LOG_DATE_FORMAT)} {stateEvent.Message}");
            }
        }

        private void ClearLog()
        {
            _consoleExtensions.ClearBox(_logTextForm.ContentLeft, _logTextForm.ContentTop,
                _logTextForm.ContentWidth, _logTextForm.ContentHeight);
        }
    }
}
