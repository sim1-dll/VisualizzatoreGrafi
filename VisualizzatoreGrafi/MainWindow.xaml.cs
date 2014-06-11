using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GraphSharp.Controls;

namespace VisualizzatoreGrafi
{

    

    public partial class MainWindow : Window
    {
        private MainWindowViewModel vm;
        public MainWindow()
        {
            vm = new MainWindowViewModel();
            this.DataContext = vm;
            InitializeComponent();
        }

        private void Layout_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            vm.CaricaFile();
        }

        private void HandlePosizioniChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox) sender;
            vm.CaricaPosizioni(tb.Text);
        }
    }
}
