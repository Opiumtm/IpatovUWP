using System;
using System.Collections.Generic;
using Windows.UI.Text;
using Microsoft.Graphics.Canvas.Text;

namespace Ipatov.MarkupRender.Direct2D
{
    /// <summary>
    /// Mapping helper.
    /// </summary>
    internal sealed class MappingHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tl"></param>
        /// <param name="attributes"></param>
        public void ApplyAttributes(CanvasTextLayout tl, IReadOnlyList<IMappingHelperArg> attributes)
        {
            if (tl == null) throw new ArgumentNullException(nameof(tl));
            if (attributes != null)
            {
                foreach (var attribute in attributes)
                {
                    if (attribute != null)
                    {
                        ApplyAttribute(tl, attribute);
                    }
                }
            }
        }

        private void ApplyAttribute(CanvasTextLayout tl, IMappingHelperArg attribute)
        {
            var sz = attribute.RenderString?.Length ?? 0;
            var flags = attribute.Flags;
            var index = attribute.Index;
            if (flags.HasFlag(TextAttributeFlags.Bold))
            {
                tl.SetFontWeight(index, sz, FontWeights.Bold);
            }
            if (flags.HasFlag(TextAttributeFlags.Italic))
            {
                tl.SetFontStyle(index, sz, FontStyle.Italic);
            }
            if (flags.HasFlag(TextAttributeFlags.Fixed))
            {
                tl.SetFontFamily(index, sz, attribute.FixedFontFace ?? "Courier New");
            }
            if (flags.HasFlag(TextAttributeFlags.Subscript) || flags.HasFlag(TextAttributeFlags.Subscript))
            {
                var fs = attribute.FontSize;
                fs = fs * 2.0f / 3.0f;
                tl.SetFontSize(index, sz, fs);
            }
            tl.SetCustomBrush(index, sz, attribute.Command);
        }
    }
}