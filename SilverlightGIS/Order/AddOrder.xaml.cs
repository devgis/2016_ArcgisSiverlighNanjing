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
    public partial class AddOrder : ChildWindow
    {
        public AddOrder()
        {
            InitializeComponent();
            tbDate.Text = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btOrder_Click(object sender, RoutedEventArgs e)
        {
            //if(string.IsNullOrEmpty(dt))
            try
            {
                Convert.ToDateTime(tbDate.Text);
            }
            catch
            {
                MessageBox.Show("时间格式不正确");
                tbDate.Text = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                return;
            }

            if (string.IsNullOrEmpty(tbTrainNO.Text.Trim()))
            {
                MessageBox.Show("车次不能为空！");
                return;
            }
            
            if (string.IsNullOrEmpty(tbStart.Text.Trim()))
            {
                MessageBox.Show("起点不能为空！");
                return;
            }
            
            if (string.IsNullOrEmpty(tbEnd .Text.Trim()))
            {
                MessageBox.Show("终点不能为空！");
                return;
            }

            
            if (string.IsNullOrEmpty(tbPaserg.Text.Trim()))
            {
                MessageBox.Show("乘客不能为空！");
                return;
            }

            OrderInfo order = new OrderInfo();
            order.ID = Guid.NewGuid().ToString();
            order.OrderTime=Convert.ToDateTime(tbDate.Text);
            order.TrainNO = tbTrainNO.Text;
            order.Start = tbStart.Text;
            order.End = tbEnd.Text;
            order.PsgName = tbPaserg.Text;
            order.UserName = MainPage.UserName;

            MyService.DBServiceClient client = new DBServiceClient();
            client.AddOrderInfoCompleted += Client_AddOrderInfoCompleted;
            client.AddOrderInfoAsync(order);
        }

        private void Client_AddOrderInfoCompleted(object sender, AddOrderInfoCompletedEventArgs e)
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
    }
}
