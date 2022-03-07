using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WireFrame.DrawArea.Controls
{
    public sealed partial class ActionTip : UserControl
    {
        public ActionTip()
        {
            this.InitializeComponent();
        }

        public void SetTip(string tip)
        {
            _text.Text = tip;
        }
    }
}
