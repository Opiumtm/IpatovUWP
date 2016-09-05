using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Ipatov.MarkupRender.Test
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, ITextRenderStyle
    {
        private const string Ipsum =
            "Sed ut perspiciatis, unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam eaque ipsa, quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt, explicabo. Nemo enim ipsam voluptatem, quia voluptas sit, aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos, qui ratione voluptatem sequi nesciunt, neque porro quisquam est, qui dolorem ipsum, quia dolor sit, amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt, ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit, qui in ea voluptate velit esse, quam nihil molestiae consequatur, vel illum, qui dolorem eum fugiat, quo voluptas nulla pariatur? At vero eos et accusamus et iusto odio dignissimos ducimus, qui blanditiis praesentium voluptatum deleniti atque corrupti, quos dolores et quas molestias excepturi sint, obcaecati cupiditate non provident, similique sunt in culpa, qui officia deserunt mollitia animi, id est laborum et dolorum fuga. Et harum quidem rerum facilis est et expedita distinctio. Nam libero tempore, cum soluta nobis est eligendi optio, cumque nihil impedit, quo minus id, quod maxime placeat, facere possimus, omnis voluptas assumenda est, omnis dolor repellendus. Temporibus autem quibusdam et aut officiis debitis aut rerum necessitatibus saepe eveniet, ut et voluptates repudiandae sint et molestiae non recusandae. Itaque earum rerum hic tenetur a sapiente delectus, ut aut reiciendis voluptatibus maiores alias consequatur aut perferendis doloribus asperiores repellat.";

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void MainPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            var p = new RenderProgramCommandsFormer();
            p.Push(CommonProgramElements.PrintTextElement(Ipsum));
            p.Push(CommonProgramElements.LinkeBreakElement);
            p.Push(CommonProgramElements.LinkeBreakElement);
            p.Push(CommonProgramElements.AddAttributeElement(CommonTextAttributes.BoldAttribute));
            p.Push(CommonProgramElements.PrintTextElement(Ipsum));
            p.Push(CommonProgramElements.RemoveAttributeElement(CommonTextAttributes.BoldAttribute));
            p.Push(CommonProgramElements.LinkeBreakElement);
            p.Push(CommonProgramElements.LinkeBreakElement);
            var link = CommonTextAttributes.CreateLink(new Uri("http://yandex.ru/"));
            p.Push(CommonProgramElements.AddAttributeElement(link));
            p.Push(CommonProgramElements.PrintTextElement("Перейти по ссылке"));
            p.Push(CommonProgramElements.RemoveAttributeElement(link));
            p.Push(CommonProgramElements.LinkeBreakElement);
            p.Push(CommonProgramElements.LinkeBreakElement);
            p.Push(CommonProgramElements.AddAttributeElement(CommonTextAttributes.StrikethroughAttribute));
            p.Push(CommonProgramElements.PrintTextElement("Зачёркнутый текст"));
            p.Push(CommonProgramElements.RemoveAttributeElement(CommonTextAttributes.StrikethroughAttribute));
            p.Push(CommonProgramElements.PrintTextElement(" "));
            p.Push(CommonProgramElements.AddAttributeElement(CommonTextAttributes.OverlineAttribute));
            p.Push(CommonProgramElements.PrintTextElement("Линия сверху"));
            p.Push(CommonProgramElements.RemoveAttributeElement(CommonTextAttributes.OverlineAttribute));
            p.Push(CommonProgramElements.PrintTextElement(" "));
            p.Push(CommonProgramElements.AddAttributeElement(CommonTextAttributes.SpoilerAttribute));
            p.Push(CommonProgramElements.PrintTextElement("Спойлер"));
            p.Push(CommonProgramElements.RemoveAttributeElement(CommonTextAttributes.SpoilerAttribute));
            p.Push(CommonProgramElements.PrintTextElement(" x"));
            p.Push(CommonProgramElements.AddAttributeElement(CommonTextAttributes.SuperscriptAttribute));
            p.Push(CommonProgramElements.PrintTextElement("2"));
            p.Push(CommonProgramElements.RemoveAttributeElement(CommonTextAttributes.SuperscriptAttribute));
            p.Push(CommonProgramElements.AddAttributeElement(CommonTextAttributes.SubscriptAttribute));
            p.Push(CommonProgramElements.PrintTextElement("1"));
            p.Push(CommonProgramElements.RemoveAttributeElement(CommonTextAttributes.SubscriptAttribute));
            p.Push(CommonProgramElements.PrintTextElement(" "));
            p.Push(CommonProgramElements.AddAttributeElement(CommonTextAttributes.StrikethroughAttribute));
            p.Push(CommonProgramElements.PrintTextElement(" x"));
            p.Push(CommonProgramElements.AddAttributeElement(CommonTextAttributes.SuperscriptAttribute));
            p.Push(CommonProgramElements.PrintTextElement("2"));
            p.Push(CommonProgramElements.RemoveAttributeElement(CommonTextAttributes.SuperscriptAttribute));
            p.Push(CommonProgramElements.AddAttributeElement(CommonTextAttributes.SubscriptAttribute));
            p.Push(CommonProgramElements.PrintTextElement("1"));
            p.Push(CommonProgramElements.RemoveAttributeElement(CommonTextAttributes.SubscriptAttribute));
            p.Push(CommonProgramElements.RemoveAttributeElement(CommonTextAttributes.StrikethroughAttribute));
            p.Push(CommonProgramElements.LinkeBreakElement);
            p.Push(CommonProgramElements.LinkeBreakElement);
            p.Push(CommonProgramElements.AddAttributeElement(CommonTextAttributes.FixedAttribute));
            p.Push(CommonProgramElements.PrintTextElement(Ipsum));
            p.Push(CommonProgramElements.RemoveAttributeElement(CommonTextAttributes.FixedAttribute));
            p.Flush();
            MarkupRenderer.RenderData = new MarkupRenderData { Commands = p, Style = this };
        }

        public string FontFace => "Segoe UI";

        public string FixedFontFace => "Consolas";

        float ITextRenderStyle.FontSize => 14f;

        public int? MaxLines => null;

        public Color NormalColor => Colors.Black;

        public Color QuoteColor => Colors.Green;

        public Color SpoilerBackground => Colors.Gray;

        public Color SpoilerColor => Colors.Black;

        public Color LinkColor => Colors.Blue;

        public event EventHandler StyleChanged;

        private async void MarkupRenderer_OnTextTapped(object sender, IRenderCommand e)
        {
            if (e.Attributes.ContainsKey(CommonTextAttributes.Link))
            {
                var l = e.Attributes[CommonTextAttributes.Link] as ITextAttributeData<Uri>;
                if (l?.Value != null)
                {
                    await Launcher.LaunchUriAsync(l.Value);
                }
            }
        }
    }
}
