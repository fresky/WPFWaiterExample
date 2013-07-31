using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFWaiterExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ParallelWithDataBinding m_ParallelWithDataBinding = new ParallelWithDataBinding();

        private readonly Parallel m_Parallel;
        private readonly Sync m_Sync;
        private readonly Async m_Async;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = m_ParallelWithDataBinding;
            Sync_SpinControl.Visibility = Visibility.Collapsed;
            CodeBehind_SpinControl.Visibility = Visibility.Collapsed;
            Await_SpinControl.Visibility = Visibility.Collapsed;

            m_Parallel = new Parallel(CodeBehind_SpinControl, CodeBehind_Progressbar,
                                          CodeBehind_ElaspeTimeTextBox, CodeBehind_Start, CodeBehind_Cancel);

            m_Sync = new Sync(Sync_SpinControl, Sync_Progressbar, Sync_ElaspeTimeTextBox, Sync_Start, Sync_Canel);

            m_Async = new Async(Await_SpinControl, Await_Progressbar, Await_ElaspeTimeTextBox, Await_Start, Await_Cancel);
        }

        private void Sync_Start_Click(object sender, RoutedEventArgs e)
        {
            m_Sync.Start();
        }

        private void Sync_Cancel_Click(object sender, RoutedEventArgs e)
        {
            m_Sync.Cancel();
        }


        private void DataBinding_Start_Click(object sender, RoutedEventArgs e)
        {
            m_ParallelWithDataBinding.Start();
        }


        private void DataBinding_Cancel_Click(object sender, RoutedEventArgs e)
        {
            m_ParallelWithDataBinding.Cancel();
        }


        private void CodeBehind_Start_Click(object sender, RoutedEventArgs e)
        {
            m_Parallel.Start();
        }


        private void CodeBehind_Cancel_Click(object sender, RoutedEventArgs e)
        {
            m_Parallel.Cancel();
        }



        private void Await_Start_Click(object sender, RoutedEventArgs e)
        {
            m_Async.Start();
        }

        private void Await_Cancel_Click(object sender, RoutedEventArgs e)
        {
            m_Async.Cancel();
        }
    }
}
