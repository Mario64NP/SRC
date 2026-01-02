using System.Windows;

namespace SpeedrunCommunity
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new SpeedrunCommunity.ViewModel.MainViewModel();
        }
    }
}
