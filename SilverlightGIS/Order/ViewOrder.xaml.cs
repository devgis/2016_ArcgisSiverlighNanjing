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
    public partial class ViewOrder : ChildWindow
    {
        public ViewOrder(string ID)
        {
            InitializeComponent();
            MyService.DBServiceClient client = new DBServiceClient();
            client.GetOrderListCompleted += Client_GetOrderListCompleted;
            client.GetOrderListAsync(string.Format("ID='{0}'",ID));
        }
        private void Client_GetOrderListCompleted(object sender, GetOrderListCompletedEventArgs e)
        {
            if (e.Result != null&&e.Result.Count>0)
            {
                tbDate.Text = e.Result[0].OrderTime.ToString("yyyy-MM-dd hh:mm:ss");
                tbTrainNO.Text = e.Result[0].TrainNO;
                tbStart.Text = e.Result[0].Start;
                tbEnd.Text = e.Result[0].End;
                tbPaserg.Text = e.Result[0].PsgName;
            }
        }
        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
