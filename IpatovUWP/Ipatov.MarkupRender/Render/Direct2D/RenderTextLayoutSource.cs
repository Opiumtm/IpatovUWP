using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;

namespace Ipatov.MarkupRender.Direct2D
{
    /// <summary>
    /// Render text layout source.
    /// </summary>
    public sealed class RenderTextLayoutSource : IRenderTextLayoutSource
    {
        private readonly MappingHelper _mappingHelper = new MappingHelper();

        /// <summary>
        /// Create D2D text canvas layout.
        /// </summary>
        /// <param name="commandsSource">Text rendering commands source.</param>
        /// <param name="resourceCreator">Canvas resource creator.</param>
        /// <param name="style">Render style.</param>
        /// <returns>Cavvas text layout.</returns>
        public IRenderTextLayoutResult CreateLayout(IRenderCommandsSource commandsSource, ICanvasResourceCreator resourceCreator, ITextRenderStyle style)
        {
            if (commandsSource == null) throw new ArgumentNullException(nameof(commandsSource));
            if (resourceCreator == null) throw new ArgumentNullException(nameof(resourceCreator));
            if (style == null) throw new ArgumentNullException(nameof(style));
            var interProgram = CreateIntermediateProgram(commandsSource, style).ToArray();
            var allText = interProgram.Aggregate(new StringBuilder(), (sb, s) => sb.Append(s.RenderString)).ToString();
            var tf = new CanvasTextFormat
            {
                FontFamily = style.FontFace ?? "Segoe UI",
                FontSize = style.FontSize,
                WordWrapping = CanvasWordWrapping.Wrap,
                Direction = CanvasTextDirection.LeftToRightThenTopToBottom,
                Options = CanvasDrawTextOptions.Default
            };
            var tl = new CanvasTextLayout(resourceCreator, allText, tf, style.Width, 10f);
            var result = new CanvasTextLayoutWrapper(tl, tf, style, allText);
            try
            {
                var helperArgs = interProgram.OfType<IMappingHelperArg>().ToList();
                _mappingHelper.ApplyAttributes(tl, helperArgs);
                return result;
            }
            catch
            {
                result.Dispose();
                throw;
            }
        }

        private sealed class CanvasTextLayoutWrapper : IRenderTextLayoutResult
        {
            public CanvasTextLayoutWrapper(CanvasTextLayout value, CanvasTextFormat textFormat, ITextRenderStyle style, string plainText)
            {
                Value = value;
                TextFormat = textFormat;
                Style = style;
                PlainText = plainText;
            }

            public void Dispose()
            {
                TextFormat.Dispose();
                Value.Dispose();
            }

            public CanvasTextLayout Value { get; }

            public CanvasTextFormat TextFormat { get; }

            public ITextRenderStyle Style { get; }
            public string PlainText { get; }
        }

        private IEnumerable<IntermediateElement> CreateIntermediateProgram(IRenderCommandsSource program, ITextRenderStyle style)
        {
            var idx = 0;
            foreach (var p in program.GetCommands())
            {
                if (p?.Content != null)
                {
                    if (CommonRenderContentTypes.LineBreak.Equals(p.Content.Type, StringComparison.OrdinalIgnoreCase))
                    {
                        yield return new IntermediateElement() { RenderString = "\n", Command = p, Index = idx, Style = style };
                        idx++;
                    }
                    if (CommonRenderContentTypes.Text.Equals(p.Content.Type, StringComparison.OrdinalIgnoreCase))
                    {
                        var t = p.Content as IRenderContentText;
                        var s = t?.Text ?? "";
                        if (s.Length > 0)
                        {
                            yield return new IntermediateElement() { RenderString = t?.Text ?? "", Command = p, Index = idx, Style = style };
                            idx += s.Length;
                        }
                    }
                }
            }
        }

        private sealed class IntermediateElement : IMappingHelperArg
        {
            private IRenderCommand _command;

            public int Index { get; set; }

            public string RenderString { get; set; }

            public ITextRenderStyle Style { get; set; }

            public IRenderCommand Command
            {
                get { return _command; }
                set
                {
                    _command = value;
                    TextAttributeFlags flags = 0;
                    var attr = _command?.Attributes;
                    if (attr != null)
                    {
                        if (attr.ContainsKey(CommonTextAttributes.Bold))
                        {
                            flags = flags | TextAttributeFlags.Bold;
                        }
                        if (attr.ContainsKey(CommonTextAttributes.Italic))
                        {
                            flags = flags | TextAttributeFlags.Italic;
                        }
                        if (attr.ContainsKey(CommonTextAttributes.Fixed))
                        {
                            flags = flags | TextAttributeFlags.Fixed;
                        }
                        if (attr.ContainsKey(CommonTextAttributes.Subscript))
                        {
                            flags = flags | TextAttributeFlags.Subscript;
                        }
                        if (attr.ContainsKey(CommonTextAttributes.Superscript))
                        {
                            flags = flags | TextAttributeFlags.Superscript;
                        }
                    }
                    Flags = flags;
                    _attributes = new RenderAttributeSnapshot(_command?.Attributes);
                }
            }

            public string FontFace => Style?.FontFace;

            public string FixedFontFace => Style?.FixedFontFace;

            public float FontSize => Style?.FontSize ?? 8.0f;

            private IRenderAttributesSnapshot _attributes;

            object IMappingHelperArg.Command => _attributes;

            public TextAttributeFlags Flags { get; private set; }
        }
    }
}