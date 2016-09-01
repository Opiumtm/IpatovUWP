using System;
using System.Collections.Generic;
using System.Text;

namespace Ipatov.MarkupRender
{
    /// <summary>
    /// Former of stream of commands from render program.
    /// </summary>
    public sealed class RenderProgramCommandsFormer : IRenderProgramConsumer, IRenderCommandsSource
    {
        private readonly CommandFormerInternal _internalFormer = new CommandFormerInternal();

        private readonly List<IRenderCommand> _result = new List<IRenderCommand>();

        /// <summary>
        /// Push program element.
        /// </summary>
        /// <param name="element">Element.</param>
        public void Push(IRenderProgramElement element)
        {
            if (!_internalFormer.AddElement(element))
            {
                Flush();
                _internalFormer.AddElement(element);
            }
        }

        /// <summary>
        /// Clear program state.
        /// </summary>
        public void Clear()
        {
            _internalFormer.Clear();
            _result.Clear();
        }

        /// <summary>
        /// Flush current state. Call this method at program end.
        /// </summary>
        public void Flush()
        {
            var command = _internalFormer.GetCommand();
            if (command != null)
            {
                _result.Add(command);
            }
            _internalFormer.Flush();
        }

        /// <summary>
        /// Get render commands.
        /// </summary>
        /// <returns>Render commands.</returns>
        public IReadOnlyList<IRenderCommand> GetCommands()
        {
            return _result.ToArray();
        }

        private sealed class CommandFormerInternal
        {
            private readonly ITextAttributeState _attributeState = new TextAttributeState();

            private readonly StringBuilder _text = new StringBuilder();

            private bool _lineBreak;

            private bool HasNonTextContent => _lineBreak;

            public bool AddElement(IRenderProgramElement element)
            {
                if (element?.Command == null)
                {
                    return true;
                }
                if (HasNonTextContent)
                {
                    return false;
                }
                if (CommonProgramElements.PrintText.Equals(element.Command, StringComparison.OrdinalIgnoreCase) && element is IRenderProgramText)
                {
                    var txt = (IRenderProgramText)element;
                    _text.Append(txt.Text ?? "");
                    return true;
                }
                if (_text.Length > 0)
                {
                    return false;
                }
                ExecuteElement(element);
                return true;
            }

            private void ExecuteElement(IRenderProgramElement element)
            {
                if (CommonProgramElements.LineBreak.Equals(element.Command, StringComparison.CurrentCultureIgnoreCase))
                {
                    _lineBreak = true;
                    return;
                }
                if (CommonProgramElements.AddAttribute.Equals(element.Command, StringComparison.CurrentCultureIgnoreCase))
                {
                    var el = element as IRenderProgramAttribute;
                    if (el?.Attribute != null)
                    {
                        _attributeState.Add(el.Attribute);
                    }
                    return;
                }
                if (CommonProgramElements.RemoveAttribute.Equals(element.Command, StringComparison.CurrentCultureIgnoreCase))
                {
                    var el = element as IRenderProgramAttribute;
                    if (el?.Attribute?.Name != null)
                    {
                        _attributeState.Remove(el.Attribute.Name);
                    }
                    return;
                }
            }

            public IRenderCommand GetCommand()
            {
                if (_lineBreak)
                {
                    return new RenderCommand(_attributeState.GetSnapshot(), CommonRenderContentTypes.LineBreakContent);
                }
                if (_text.Length > 0)
                {                    
                    return new RenderCommand(_attributeState.GetSnapshot(), CommonRenderContentTypes.TextContent(_text.ToString()));
                }
                return null;
            }

            public void Clear()
            {
                _attributeState.Clear();
                Flush();
            }

            public void Flush()
            {
                _text.Clear();
                _lineBreak = false;
            }
        }
    }
}