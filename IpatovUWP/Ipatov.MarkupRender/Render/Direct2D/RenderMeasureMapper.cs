using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Foundation;
using Microsoft.Graphics.Canvas.Text;

namespace Ipatov.MarkupRender.Direct2D
{
    /// <summary>
    /// Measure mapper.
    /// </summary>
    public sealed class RenderMeasureMapper : IRenderMeasureMapper
    {
        /// <summary>
        /// Map text layout.
        /// </summary>
        /// <param name="layout">Text layout.</param>
        /// <returns>Measure mapping.</returns>
        public IRenderMeasureMap MapLayout(IRenderTextLayoutResult layout)
        {
            if (layout == null) throw new ArgumentNullException(nameof(layout));
            var map = AnalyzeMap(layout.Value, layout.PlainText).ToArray();
            var result = new MeasureMap(map, layout.Width, layout.Style.MaxLines, layout.Style);
            return result;
        }

        private IEnumerable<IRenderMeasureMapLine> AnalyzeMap(CanvasTextLayout tl, string allText)
        {
            var lines = tl.LineMetrics;
            int idx = 0;
            var formatChanges = Range.ChangeIndicesToRanges(tl.GetFormatChangeIndices(), allText.Length).ToArray();
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                var regions = GetRegions(tl, idx, line, formatChanges).ToArray();

                var resMap = new List<RenderMeasureMapElement>();

                foreach (var region in regions)
                {
                    var attr = tl.GetCustomBrush(region.CharacterIndex) as IRenderAttributesSnapshot;
                    var subStr = allText.Substring(region.CharacterIndex, region.CharacterCount).Replace("\n", "");
                    if (subStr.Length > 0)
                    {
                        resMap.Add(new RenderMeasureMapElement()
                        {
                            Command = new RenderCommand(attr?.Attributes ?? new Dictionary<string, ITextAttribute>(), new RenderTextContent(subStr)),
                            Placement = new Point(region.LayoutBounds.X, region.LayoutBounds.Y),
                            Size = new Size(region.LayoutBounds.Width, region.LayoutBounds.Height)
                        });
                    }
                }

                yield return new MeasureMapLine(resMap.ToArray(), i, line.Height);
                idx += line.CharacterCount;
            }
        }

        private IEnumerable<CanvasTextLayoutRegion> GetRegions(CanvasTextLayout tl, int idx, CanvasLineMetrics line, Range[] formatChanges)
        {
            var lineRange = new Range(idx, line.CharacterCount);
            var inLineChanges = formatChanges.Where(i => lineRange.InterlappedWith(i)).ToArray();
            if (inLineChanges.Length == 0)
            {
                var regs = tl.GetCharacterRegions(idx, line.CharacterCount);
                foreach (var r in regs)
                {
                    yield return r;
                }
            }
            else
            {
                foreach (var p in inLineChanges)
                {
                    var b = Math.Max(p.Begin, lineRange.Begin);
                    var e = Math.Min(p.End, lineRange.End);
                    var c = e - b + 1;
                    if (c > 0)
                    {
                        var regs = tl.GetCharacterRegions(b, c);
                        foreach (var r in regs)
                        {
                            yield return r;
                        }
                    }
                }
            }
        }

        private sealed class MeasureMapLine : IRenderMeasureMapLine
        {
            private readonly RenderMeasureMapElement[] _elements;

            public MeasureMapLine(RenderMeasureMapElement[] elements, int lineNumber, double height)
            {
                if (elements == null) throw new ArgumentNullException(nameof(elements));
                this._elements = elements;
                LineNumber = lineNumber;
                Height = height;
            }

            public IReadOnlyList<RenderMeasureMapElement> GetMeasureMap()
            {
                return _elements;
            }

            public int LineNumber { get; }

            public double Height { get; }
        }

        private sealed class MeasureMap : IRenderMeasureMap
        {
            public MeasureMap(IRenderMeasureMapLine[] lines, double width, int? maxLines, ITextRenderStyle style)
            {
                if (lines == null) throw new ArgumentNullException(nameof(lines));
                this._lines = lines;
                MaxLines = maxLines;
                var h = 0.0;
                foreach (var l in lines)
                {
                    if (maxLines.HasValue)
                    {
                        if (l.LineNumber >= maxLines.Value)
                        {
                            ExceedLines = true;
                            break;
                        }
                    }
                    h += l.Height;
                }
                Bounds = new Size(width, h);
                Style = style;
            }

            private readonly IRenderMeasureMapLine[] _lines;

            public IReadOnlyList<IRenderMeasureMapLine> GetMeasureMapLines()
            {
                return _lines;
            }

            public int? MaxLines { get; }

            public bool ExceedLines { get; }

            public Size Bounds { get; }

            public ITextRenderStyle Style { get; }
        }
    }
}