using Windows.UI.Xaml.Controls;
using WireFrame.DrawArea.Misc;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WireFrame.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            _canvas.SetCanvasProfile(new CanvasProfile(4000.0, 4000.0));
        }
    }
}
