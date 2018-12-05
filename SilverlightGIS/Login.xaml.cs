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
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
            //SR.DBServiceClient client = new SR.DBServiceClient();
            //client.GetConnectionStringCompleted += Client_GetConnectionStringCompleted;
            //client.GetConnectionStringAsync();
        }

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void btReg_Click(object sender, RoutedEventArgs e)
        {
            //App.Current.RootVisual = new Reg();

            //App.Current.RedirectTo(new Reg());

            this.Content = new Reg();
        }

        private void btLogin_Click(object sender, RoutedEventArgs e)
        {
            //App.Current.RootVisual = new MainPage();
           
            if (string.IsNullOrEmpty(tbUserName.Text.Trim()))
            {
                MessageBox.Show("请输入用户名！");
                return;
            }
            if (string.IsNullOrEmpty(tbPassword.Password.Trim()))
            {
                MessageBox.Show("请输入密码！");
                return;
            }
            MyService.DBServiceClient client = new MyService.DBServiceClient();

            client.CheckUserCompleted += Client_CheckUserCompleted;
            client.CheckUserAsync(tbUserName.Text, tbPassword.Password);
            
        }
        private void Client_CheckUserCompleted(object sender, MyService.CheckUserCompletedEventArgs e)
        {
            if (e.Result)
            {
                this.Content = new MainPage();
                MainPage.UserName = tbUserName.Text;
            }
            else
            {
                MessageBox.Show("用户名或者密码不正确！");
                return;
            }
        }

        //private void Client_GetConnectionStringCompleted(object sender, SR.GetConnectionStringCompletedEventArgs e)
        //{
        //    MessageBox.Show(e.Result);
        //}
    }
}
