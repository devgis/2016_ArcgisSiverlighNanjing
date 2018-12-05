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
    public partial class InfoList : ChildWindow
    {
        public InfoList()
        {
            InitializeComponent();
            //myDataList.DataContext
            //ESRI.
            //List<> listMessage = new List<Message>();
            //Message m = new Message();

            /*
            List<Info> listInfo = new List<Info>();
            Info info1 = new Info() { ID = "1", Title = "标题1", Content = "标题2", Author = "张三", Time = DateTime.Now };
            Info info2 = new Info() { ID = "2", Title = "标题2", Content = "内容3", Author = "李四", Time = DateTime.Now };
            listInfo.Add(info1);
            listInfo.Add(info2);
            myDataList.ItemsSource = listInfo;
            */
        }

        public void RefreshData()
        {
            MyService.DBServiceClient client = new DBServiceClient();
            client.GetInfoListCompleted += Client_GetInfoListCompleted;
            client.GetInfoListAsync("");
        }

        private void Client_GetInfoListCompleted(object sender, GetInfoListCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                myDataList.ItemsSource = e.Result;
            }
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            //this.Content = new Add();
            AddInfo frmAdd = new AddInfo();
            frmAdd.Closed += FrmAdd_Closed;
            frmAdd.Show();
        }

        private void FrmAdd_Closed(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void btEditRow_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show((sender as Button).Tag.ToString());
        }

        private void btDelRow_Click(object sender, RoutedEventArgs e)
        {
            string ID = (sender as Button).Tag.ToString();
            MyService.DBServiceClient client = new DBServiceClient();
            client.DeleteInfoCompleted += Client_DeleteInfoCompleted;
            client.DeleteInfoAsync(ID);
        }

        private void Client_DeleteInfoCompleted(object sender, DeleteInfoCompletedEventArgs e)
        {
            if (e.Result)
            {
                MessageBox.Show("删除成功！");
                RefreshData();
            }
            else
            {
                MessageBox.Show("删除失败！");
            }
        }

        private void btViewRow_Click(object sender, RoutedEventArgs e)
        {
            string ID = (sender as Button).Tag.ToString();
            ViewInfo frmViewInfo = new ViewInfo(ID);
            frmViewInfo.Show();
        }
    }
}
