using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Ipatov.MarkupRender
{
    /// <summary>
    /// Markup render control data.
    /// </summary>
    public sealed class MarkupRenderData : IMarkupRenderData
    {
        private IRenderCommandsSource _commands;

        /// <summary>
        /// Render commands.
        /// </summary>
        public IRenderCommandsSource Commands
        {
            get { return _commands; }
            set
            {
                _commands = value;
                OnPropertyChanged();
            }
        }

        private ITextRenderStyle _style;

        /// <summary>
        /// Render style.
        /// </summary>
        public ITextRenderStyle Style
        {
            get { return _style; }
            set
            {
                _style = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Raised when property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}