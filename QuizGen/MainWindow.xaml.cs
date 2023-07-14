using System.Windows;



namespace quiz_generator
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new QuizGen.ViewModel.MainWindow();
        }

    }
}
