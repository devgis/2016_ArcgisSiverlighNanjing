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
    public partial class Reg : Page
    {
        public Reg()
        {
            InitializeComponent();
        }

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void btReg_Click(object sender, RoutedEventArgs e)
        {
            //App.Current.RootVisual = new MainPage();

            if (string.IsNullOrEmpty(tbUserName.Text.Trim()))
            {
                MessageBox.Show("请输入用户名！");
                return;
            }
            if (string.IsNullOrEmpty(tbPassword1.Password.Trim()))
            {
                MessageBox.Show("请输入密码！");
                return;
            }
            if (tbPassword1.Password != tbPassword2.Password)
            {
                MessageBox.Show("两次输入密码不一致！");
                return;
            }
            MyService.DBServiceClient client = new MyService.DBServiceClient();

            client.UserExistsCompleted += Client_UserExistsCompleted;
            client.UserExistsAsync(tbUserName.Text, tbPassword1.Password);

        }

        private void Client_UserExistsCompleted(object sender, MyService.UserExistsCompletedEventArgs e)
        {
            if (e.Result)
            {
                MessageBox.Show("用户已存在！");
                return;
            }
            else
            {
                MyService.DBServiceClient client = new MyService.DBServiceClient();
                client.AddUserCompleted += Client_AddUserCompleted;
                client.AddUserAsync(tbUserName.Text, tbPassword1.Password);
            }
        }

        private void Client_AddUserCompleted(object sender, MyService.AddUserCompletedEventArgs e)
        {
            if (e.Result)
            {
                MessageBox.Show("注册成功！");
                this.Content = new Login();
            }
            else
            {
                MessageBox.Show("注册失败！");
            }
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new Login();
        }
        private void Client_CheckUserCompleted(object sender, MyService.CheckUserCompletedEventArgs e)
        {
            if (e.Result)
            {
                this.Content = new MainPage();
            }
            else
            {
                MessageBox.Show("用户名或者密码不正确！");
                return;
            }
        }

    }
}
