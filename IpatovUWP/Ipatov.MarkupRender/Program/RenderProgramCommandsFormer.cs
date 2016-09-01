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
        /// <summary>
        /// Push program element.
        /// </summary>
        /// <param name="element">Element.</param>
        public void Push(IRenderProgramElement element)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Clear program state.
        /// </summary>
        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Flush current state. Call this method at program end.
        /// </summary>
        public void Flush()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Get render commands.
        /// </summary>
        /// <returns>Render commands.</returns>
        public IReadOnlyList<IRenderCommand> GetCommands()
        {
            throw new System.NotImplementedException();
        }

        private sealed class CommandFormerInternal
        {
            private readonly ITextAttributeState _attributeState = new TextAttributeState();

            private readonly StringBuilder _text = new StringBuilder();

            private bool _lineBreak;

            /// <summary>
            /// Есть не текстовый контент.
            /// </summary>
            protected virtual bool HasNonTextContent
            {
                get { return LineBreak; }
            }

            /// <summary>
            /// Добавить элемент.
            /// </summary>
            /// <param name="element">Элемент.</param>
            /// <returns>true, если добавлено успешно. false, если нужно вызвать GetCommand</returns>
            public bool AddElement(IRenderProgramElement element)
            {
                if (element == null || element.Id == null)
                {
                    return true;
                }
                if (HasNonTextContent)
                {
                    return false;
                }
                if (CommonRenderProgramElements.PrintText.Equals(element.Id, StringComparison.OrdinalIgnoreCase) &&
                    element is ITextRenderProgramElement)
                {
                    var txt = (ITextRenderProgramElement)element;
                    Text.Append(txt.Text ?? "");
                    return true;
                }
                if (Text.Length > 0)
                {
                    return false;
                }
                ExecuteElement(element);
                return true;
            }

        }
    }
}