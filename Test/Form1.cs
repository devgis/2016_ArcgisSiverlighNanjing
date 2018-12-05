using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Test.MyService;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MyService.DBServiceClient client = new MyService.DBServiceClient();
            //client.CheckUser("","")
            //client.CheckUserCompleted += Client_CheckUserCompleted;
            //client.CheckUserAsync()
            client.AddUserCompleted += Client_AddUserCompleted;
            client.AddUserAsync("", "");
        }

        private void Client_AddUserCompleted(object sender, AddUserCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Client_UserExistsCompleted(object sender, UserExistsCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Client_CheckUserCompleted(object sender, CheckUserCompletedEventArgs e)
        {
            if (e.Result)
            { }
        }
    }
}
