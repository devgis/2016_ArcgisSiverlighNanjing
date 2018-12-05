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
    public partial class ViewInfo : ChildWindow
    {
        public ViewInfo(string ID)
        {
            InitializeComponent();
            MyService.DBServiceClient client = new DBServiceClient();
            client.GetInfoListCompleted += Client_GetInfoListCompleted;
            client.GetInfoListAsync(string.Format("ID='{0}'", ID));
        }

        private void Client_GetInfoListCompleted(object sender, GetInfoListCompletedEventArgs e)
        {
            if (e.Result != null && e.Result.Count > 0)
            {
                tbTime.Text = e.Result[0].Time.ToString("yyyy-MM-dd HH:mm:ss");
                tbAuthor.Text = e.Result[0].Author;
                tbTitle.Text = e.Result[0].Title;
                tbContent.Text = e.Result[0].Content;
            }
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
