using System.Diagnostics;

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
        public void WriteLine(MessageChanel chanel, string message, StackTrace? stackTrace, Exception? exception) { Logger.WriteLine(chanel, this, message, stackTrace, exception); }
        public void Write(MessageChanel chanel, string message, StackTrace? stackTrace, Exception? exception) { Logger.Write(chanel, this, message, stackTrace, exception); }
    }
}