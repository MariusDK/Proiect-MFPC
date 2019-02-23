using ClientMfpc2.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientMfpc2
{
    public partial class ClientUI : Form
    {
        private TransactionService transactionService;
        public ClientUI()
        {
            InitializeComponent();
        }
        private string Case1()
        {
            try
            {
                transactionService = new TransactionService(new RpcClient("127.0.0.1", 2016));
                return transactionService.TransactionCase1();
            }
            catch (ServiceException se)
            {
                MessageBox.Show("Connectivity issue " + se.Message, "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                this.Close();
            }
            return null;
        }
        private string Case2()
        {
            try
            {
                transactionService = new TransactionService(new RpcClient("127.0.0.1", 2016));
                return transactionService.TransactionCase2();
            }
            catch (ServiceException se)
            {
                MessageBox.Show("Connectivity issue " + se.Message, "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                this.Close();
            }
            return null;
        }
        private string Case3()
        {
            try
            {
                transactionService = new TransactionService(new RpcClient("127.0.0.1", 2016));
                return transactionService.TransactionCase3();
            }
            catch (ServiceException se)
            {
                MessageBox.Show("Connectivity issue " + se.Message, "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                this.Close();
            }
            return null;
        }
        private string Case4()
        {
            try
            {
                transactionService = new TransactionService(new RpcClient("127.0.0.1", 2016));
                return transactionService.TransactionCase4();
            }
            catch (ServiceException se)
            {
                MessageBox.Show("Connectivity issue " + se.Message, "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                this.Close();
            }
            return null;
        }
        private string Case5()
        {
            try
            {
                transactionService = new TransactionService(new RpcClient("127.0.0.1", 2016));
                return transactionService.TransactionCase5();
            }
            catch (ServiceException se)
            {
                MessageBox.Show("Connectivity issue " + se.Message, "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                this.Close();
            }
            return null;
        }
        private string Case6()
        {
            try
            {
                transactionService = new TransactionService(new RpcClient("127.0.0.1", 2016));
                return transactionService.TransactionCase6();
            }
            catch (ServiceException se)
            {
                MessageBox.Show("Connectivity issue " + se.Message, "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                this.Close();
            }
            return null;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string response = Case1();
            textBox1.Text = textBox1.Text+response;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string response = Case2();
            textBox1.Text = textBox1.Text+response;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            string response = Case3();
            textBox1.Text = textBox1.Text + response;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string response = Case4();
            textBox1.Text = textBox1.Text + response;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            string response = Case5();
            textBox1.Text = textBox1.Text + response;
        }
        private void button6_Click(object sender, EventArgs e)
        {
            string response = Case6();
            textBox1.Text = textBox1.Text + response;
        }
    }
}
