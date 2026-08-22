using System.Diagnostics;
using System.Reflection.Emit;

namespace BloodShadow.Core.Logger
{
    public class LoggerLabel
    {
        private string _label;
        private string _combine;
        private LoggerLabel? _parent;

        public string Label
        {
            get
            {
                Stack<string> labels = new();
                LoggerLabel? parent = this;
                do
                {
                    labels.Push(parent._label);
                    parent = parent._parent;
                }
                while (parent != null);
                return string.Join(_combine, labels);
            }
        }
        public LoggerLabel(string label, string? combine, LoggerLabel? parent = null)
        {
            if (string.IsNullOrEmpty(label) || string.IsNullOrWhiteSpace(label)) { throw new NullReferenceException(nameof(label)); }
            _label = label;
            _combine = " => ";
            if (!string.IsNullOrEmpty(combine) &&
                !string.IsNullOrWhiteSpace(combine)) { _combine = combine; }
            if (parent != null) { _parent = parent; }
        }
        public LoggerLabel(string label) : this(label, string.Empty) { }
        public void WriteLineException(string message, StackTrace stackTrace, Exception exception) { Logger.WriteLineException(this, message, stackTrace, exception); }
        public void WriteLineException(string message, Exception exception) { WriteLineException(message, new(1), exception); }
        public void WriteLineWarning(string message, StackTrace stackTrace) { Logger.WriteLineWarning(this, message, stackTrace); }
        public void WriteLineWarning(string message) { WriteLineWarning(message, new(1)); }
        public void WriteLineInfo(string message) { Logger.WriteLineInfo(this, message); }
        public void WriteException(string message, StackTrace stackTrace, Exception exception) { Logger.WriteException(this, message, stackTrace, exception); }
        public void WriteException(string message, Exception exception) { WriteException(message, new(1), exception); }
        public void WriteWarning(string message, StackTrace stackTrace) { Logger.WriteWarning(this, message, stackTrace); }
        public void WriteWarning(string message) { WriteWarning(message, new(1)); }
        public void WriteInfo(string message) { Logger.WriteInfo(this, message); }
    }
}