using System.Windows;

namespace ScreenLocker.Views
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : Window
    {
        public Shell()
        {
            InitializeComponent();
            Closing += (s, e) => { e.Cancel = true; };
        }
    }
}
