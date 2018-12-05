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

namespace SilverlightGIS.Information
{
    public partial class AddInfo : ChildWindow
    {
        public AddInfo()
        {
            InitializeComponent();
        }

        private void btOK_Click(object sender, RoutedEventArgs e)
        {
            
            if (string.IsNullOrEmpty(tbTitle.Text.Trim()))
            {
                MessageBox.Show("标题不能为空！");
                return;
            }
            if (string.IsNullOrEmpty(tbAuthor.Text.Trim()))
            {
                MessageBox.Show("作者不能为空！");
                return;
            }
            if (string.IsNullOrEmpty(tbContent.Text.Trim()))
            {
                MessageBox.Show("内容不能为空！");
                return;
            }

            Info info = new Info();
            info.ID = Guid.NewGuid().ToString();
            info.Time = DateTime.Now;
            info.Title = tbTitle.Text;
            info.Content = tbContent.Text;
            info.Author = tbAuthor.Text;

            MyService.DBServiceClient client = new DBServiceClient();
            client.AddInfoCompleted += Client_AddInfoCompleted;
            client.AddInfoAsync(info);
        }

        private void Client_AddInfoCompleted(object sender, AddInfoCompletedEventArgs e)
        {
            if (e.Result)
            {
                MessageBox.Show("保存成功！");
                this.Close();
            }
            else
            {
                MessageBox.Show("保存失败！");
            }
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
