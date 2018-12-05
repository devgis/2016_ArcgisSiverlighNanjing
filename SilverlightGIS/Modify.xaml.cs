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
using SilverlightGIS.MyService;

namespace SilverlightGIS
{
    public partial class Modify : ChildWindow
    {
        public Modify()
        {
            InitializeComponent();
            //MyService.DBServiceClient client = new DBServiceClient();
            tbUserName.Text = MainPage.UserName;
        }


        private void btModify_Click(object sender, RoutedEventArgs e)
        {
            //注册

            if (string.IsNullOrEmpty(tbOldPassword.Password) || string.IsNullOrEmpty(tbNewPassword1.Password) || string.IsNullOrEmpty(tbNewPassword2.Password))
            {
                MessageBox.Show("密码不能为空！");
                return;
            }

            if (tbNewPassword1.Password != tbNewPassword2.Password)
            {
                MessageBox.Show("两次输入密码不一致！");
                return;
            }

            MyService.DBServiceClient client = new DBServiceClient();
            client.CheckUserCompleted += Client_CheckUserCompleted;
            client.CheckUserAsync(tbUserName.Text, tbOldPassword.Password);

        }

        private void Client_CheckUserCompleted(object sender, MyService.CheckUserCompletedEventArgs e)
        {
            if (e.Result)
            {
                MyService.DBServiceClient client = new DBServiceClient();
                client.EditUserCompleted += Client_EditUserCompleted;
                client.EditUserAsync(tbUserName.Text, tbNewPassword1.Password);
            }
            else
            {
                MessageBox.Show("旧密码输入不正确，请检查！");
                return;
            }
        }

        private void Client_EditUserCompleted(object sender, EditUserCompletedEventArgs e)
        {
            if (e.Result)
            {
                MessageBox.Show("修改成功！");
                this.Close();

            }
            else
            {
                MessageBox.Show("修改失败！");
            }
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
