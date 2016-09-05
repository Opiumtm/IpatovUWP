using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Text;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;

namespace Ipatov.MarkupRender.Direct2D
{
    /// <summary>
    /// Direct2D text renderer.
    /// </summary>
    public sealed class Direct2DMapRenderer : IMeasureMapRenderer, IDisposable
    {
        private readonly CanvasDrawingSession _drawingSession;

        private readonly Rect? _invalidatedRect;

        /// <summary>
        /// Dispose drawing session.
        /// </summary>
        public bool DisposeSession { get; set; } = true;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="drawingSession">Drawing session.</param>
        public Direct2DMapRenderer(CanvasDrawingSession drawingSession)
        {
            if (drawingSession == null) throw new ArgumentNullException(nameof(drawingSession));
            _drawingSession = drawingSession;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="drawingSession">Drawing session.</param>
        /// <param name="invalidatedRect">Invalidated rectangle.</param>
        public Direct2DMapRenderer(CanvasDrawingSession drawingSession, Rect invalidatedRect)
        {
            if (drawingSession == null) throw new ArgumentNullException(nameof(drawingSession));
            _drawingSession = drawingSession;
            _invalidatedRect = invalidatedRect;
        }

        /// <summary>
        /// Render measured map.
        /// </summary>
        /// <param name="renderMap">Map.</param>
        public void Render(IRenderMeasureMap renderMap)
        {
            _drawingSession.Clear(Colors.Transparent);
            if (renderMap != null)
            {
                DoRender(renderMap);
            }
        }

        private void DoRender(IRenderMeasureMap map)
        {
            var formatCache = new Dictionary<TextAttributeFlags, CanvasTextFormat>();
            foreach (var line in map.GetMeasureMapLines())
            {
                if (map.MaxLines.HasValue)
                {
                    if (line.LineNumber >= map.MaxLines.Value)
                    {
                        break;
                    }
                }
                foreach (var el in line.GetMeasureMap())
                {
                    if (_invalidatedRect != null)
                    {
                        var invalidatedRect = _invalidatedRect.Value;
                        var bounds = new Rect(el.Placement, el.Size);
                        bounds.Intersect(invalidatedRect);
                        if (bounds.IsEmpty)
                        {
                            continue;
                        }
                    }
                    RenderElement(el, formatCache, map.Style, line.Height);
                }
            }
            foreach (var v in formatCache.Values)
            {
                v.Dispose();
            }
        }

        private void RenderElement(RenderMeasureMapElement element, Dictionary<TextAttributeFlags, CanvasTextFormat> formatCache, ITextRenderStyle style, double lineHeight)
        {
            var text = (element.Command?.Content as IRenderContentText)?.Text;
            if (text != null)
            {
                var flags = GetAttributeFlags(element.Command);
                var tf = GetTextFormat(formatCache, flags, style);
                Rect textRect = new Rect(element.Placement, element.Size);
                if (flags.HasFlag(TextAttributeFlags.Superscript) || flags.HasFlag(TextAttributeFlags.Subscript))
                {
                    textRect.Height = textRect.Height*2.0/3.0;
                    if (flags.HasFlag(TextAttributeFlags.Superscript) && flags.HasFlag(TextAttributeFlags.Subscript))
                    {
                        var d = (lineHeight - textRect.Height)/2;
                        textRect = new Rect(new Point(textRect.Left, textRect.Top + d), new Size(textRect.Width, textRect.Height));
                    } else if (flags.HasFlag(TextAttributeFlags.Subscript))
                    {
                        var d = lineHeight - textRect.Height;
                        textRect = new Rect(new Point(textRect.Left, textRect.Top + d), new Size(textRect.Width, textRect.Height));
                    }
                }
                if (flags.HasFlag(TextAttributeFlags.Spoiler))
                {
                    _drawingSession.FillRectangle(textRect, style?.SpoilerBackground ?? Colors.Transparent);
                }
                Color color = style?.NormalColor ?? Colors.Transparent;
                if (flags.HasFlag(TextAttributeFlags.Quote))
                {
                    color = style?.QuoteColor ?? Colors.Transparent;
                }
                if (flags.HasFlag(TextAttributeFlags.Spoiler))
                {
                    color = style?.SpoilerColor ?? Colors.Transparent;
                }
                if (flags.HasFlag(TextAttributeFlags.Link))
                {
                    color = style?.LinkColor ?? Colors.Transparent;
                }
                _drawingSession.DrawText(text, (float)textRect.Left, (float)textRect.Top, color, tf);
                if (flags.HasFlag(TextAttributeFlags.Link) || flags.HasFlag(TextAttributeFlags.Undeline))
                {
                    _drawingSession.DrawLine((float)textRect.Left, (float)textRect.Bottom, (float)textRect.Right, (float)textRect.Bottom, color, 1.2f);
                }
                if (flags.HasFlag(TextAttributeFlags.Overline))
                {
                    _drawingSession.DrawLine((float)textRect.Left, (float)textRect.Top, (float)textRect.Right, (float)textRect.Top, color, 1.2f);
                }
                if (flags.HasFlag(TextAttributeFlags.Strikethrough))
                {
                    var y = textRect.Top + textRect.Height*0.6;
                    _drawingSession.DrawLine((float)textRect.Left, (float)y, (float)textRect.Right, (float)y, color, 1.2f);
                }
            }
        }

        private TextAttributeFlags GetAttributeFlags(IRenderCommand command)
        {
            TextAttributeFlags flags = 0;
            var attr = command?.Attributes;
            if (attr != null)
            {
                if (attr.ContainsKey(CommonTextAttributes.Subscript))
                {
                    flags |= TextAttributeFlags.Subscript;
                }
                if (attr.ContainsKey(CommonTextAttributes.Bold))
                {
                    flags |= TextAttributeFlags.Bold;
                }
                if (attr.ContainsKey(CommonTextAttributes.Fixed))
                {
                    flags |= TextAttributeFlags.Fixed;
                }
                if (attr.ContainsKey(CommonTextAttributes.Italic))
                {
                    flags |= TextAttributeFlags.Italic;
                }
                if (attr.ContainsKey(CommonTextAttributes.Link))
                {
                    flags |= TextAttributeFlags.Link;
                }
                if (attr.ContainsKey(CommonTextAttributes.Overline))
                {
                    flags |= TextAttributeFlags.Overline;
                }
                if (attr.ContainsKey(CommonTextAttributes.Undeline))
                {
                    flags |= TextAttributeFlags.Overline;
                }
                if (attr.ContainsKey(CommonTextAttributes.Quote))
                {
                    flags |= TextAttributeFlags.Quote;
                }
                if (attr.ContainsKey(CommonTextAttributes.Spoiler))
                {
                    flags |= TextAttributeFlags.Spoiler;
                }
                if (attr.ContainsKey(CommonTextAttributes.Strikethrough))
                {
                    flags |= TextAttributeFlags.Strikethrough;
                }
                if (attr.ContainsKey(CommonTextAttributes.Superscript))
                {
                    flags |= TextAttributeFlags.Superscript;
                }
            }
            return flags;
        }

        private CanvasTextFormat GetTextFormat(Dictionary<TextAttributeFlags, CanvasTextFormat> formatCache, TextAttributeFlags flags, ITextRenderStyle style)
        {
            if (!formatCache.ContainsKey(flags))
            {
                var tf = new CanvasTextFormat
                {
                    FontSize = style?.FontSize ?? 8,
                    WordWrapping = CanvasWordWrapping.NoWrap,
                    Direction = CanvasTextDirection.LeftToRightThenTopToBottom,
                    Options = CanvasDrawTextOptions.Default,
                };
                if (flags.HasFlag(TextAttributeFlags.Fixed))
                {
                    tf.FontFamily = style?.FixedFontFace ?? "Courier New";
                }
                else
                {
                    tf.FontFamily = style?.FontFace ?? "Segoe UI";
                }
                if (flags.HasFlag(TextAttributeFlags.Bold))
                {
                    tf.FontWeight = FontWeights.Bold;
                }
                if (flags.HasFlag(TextAttributeFlags.Italic))
                {
                    tf.FontStyle = FontStyle.Italic;
                }
                if (flags.HasFlag(TextAttributeFlags.Superscript) || flags.HasFlag(TextAttributeFlags.Subscript))
                {
                    tf.FontSize = tf.FontSize*2.0f/3.0f;
                }
                formatCache[flags] = tf;
            }
            return formatCache[flags];
        }

        /// <summary>
        /// Dispose object.
        /// </summary>
        public void Dispose()
        {
            if (DisposeSession)
            {
                _drawingSession.Dispose();
            }
        }
    }
}