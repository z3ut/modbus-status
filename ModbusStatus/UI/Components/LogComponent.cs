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

        private readonly int _storeMaxEventCount;
        private List<IStateEvent> _stateEvents = new List<IStateEvent>();

        private readonly string _dateFormat;

        private FormPosition _logTextForm;

        public LogComponent(IConsoleExtensions consoleExtensions,
            int storeMaxEventCount, string dateFormat)
        {
            _consoleExtensions = consoleExtensions;
            _storeMaxEventCount = storeMaxEventCount;
            _dateFormat = dateFormat;
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

            if (_stateEvents.Count() > _storeMaxEventCount)
            {
                _stateEvents = _stateEvents
                .Skip(Math.Max(0, _stateEvents.Count() - _storeMaxEventCount))
                .ToList();
            }
            
            ClearLog();
            WriteLog();
        }

        private void ClearLog()
        {
            _consoleExtensions.ClearBox(_logTextForm.ContentLeft, _logTextForm.ContentTop,
                _logTextForm.ContentWidth, _logTextForm.ContentHeight);
        }

        private void WriteLog()
        {
            var numberOfLogsToShow = _logTextForm.ContentHeight;
            var displayEvents = _stateEvents.TakeLast(numberOfLogsToShow).ToList();

            for (var i = 0; i < displayEvents.Count; i++)
            {
                var stateEvent = displayEvents[i];
                Console.SetCursorPosition(_logTextForm.ContentLeft, _logTextForm.ContentTop + i);
                Console.Write($"{stateEvent.Date.ToString(_dateFormat)} {stateEvent.Message}");
            }
        }
    }
}
