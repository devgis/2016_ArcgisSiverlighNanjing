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

namespace SilverlightGIS.Order
{
    public partial class OrderList : ChildWindow
    {
        public OrderList()
        {
            InitializeComponent();
            //myDataList.DataContext
            //ESRI.
            //List<> listMessage = new List<Message>();
            //Message m = new Message();

            /*
            List<OrderInfo> listInfo = new List<OrderInfo>();
            OrderInfo order1 = new OrderInfo() { ID = "1", TrainNO="1323", Start="南京",End="北京", OrderTime = DateTime.Now };
            OrderInfo order2 = new OrderInfo() { ID = "2", TrainNO = "K8", Start = "南京", End = "上海", OrderTime = DateTime.Now };
            listInfo.Add(order1);
            listInfo.Add(order2);
            myDataList.ItemsSource = listInfo;
            */


            RefreshData();
        }

        public void RefreshData()
        {
            MyService.DBServiceClient client = new DBServiceClient();
            client.GetOrderListCompleted += Client_GetOrderListCompleted;
            client.GetOrderListAsync("");
        }

        private void Client_GetOrderListCompleted(object sender, GetOrderListCompletedEventArgs e)
        {
            if(e.Result!=null)
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
            AddOrder frmAdd = new AddOrder();
            frmAdd.Closed += FrmAdd_Closed;
            frmAdd.Show();

        }

        private void FrmAdd_Closed(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void btDelRow_Click(object sender, RoutedEventArgs e)
        {
            string ID = (sender as Button).Tag.ToString();
            MyService.DBServiceClient client = new DBServiceClient();
            client.DeleteOrderInfoCompleted += Client_DeleteOrderInfoCompleted;
            client.DeleteOrderInfoAsync(ID);
        }

        private void Client_DeleteOrderInfoCompleted(object sender, DeleteOrderInfoCompletedEventArgs e)
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
            ViewOrder frmViewOrder = new ViewOrder(ID);
            frmViewOrder.Show();
        }
    }
}
