using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;

namespace SilverlightGIS
{
    public partial class Modify : Page
    {
        public Modify()
        {
            InitializeComponent();
        }

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void btModify_Click(object sender, RoutedEventArgs e)
        {
            //注册
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new Login();
        }
    }
}
