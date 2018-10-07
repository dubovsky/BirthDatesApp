using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BirthDates
{
    /// <summary>
    /// Interaction logic for ChangeData.xaml
    /// </summary>
    public partial class ChangeData : Window
    {
        public ChangeData(string n, string s, string p, int d, int m, int y)
        {
            InitializeComponent();
            tb_Name.Text = n;
            tb_Surename.Text = s;
            tb_Patronimic.Text = p;
            tb_Day.Text = d.ToString();
            tb_Month.Text = m.ToString();
            tb_Year.Text = y.ToString();
        }
        private void b_OK_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Day.Text != "" && tb_Month.Text != "" && tb_Year.Text != "" && tb_Surename.Text != "" && tb_Patronimic.Text != "" && tb_Name.Text != "")
            {
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("Пожалуйста заполните все поля!");
            }
        }
        public string MemberName
        {
            get { return tb_Name.Text; }
        }
        public string Surename
        {
            get { return tb_Surename.Text; }
        }
        public string Patronimic
        {
            get { return tb_Patronimic.Text; }
        }
        public string Day
        {
            get { return tb_Day.Text; }
        }
        public string Month
        {
            get { return tb_Month.Text; }
        }
        public string Year
        {
            get { return tb_Year.Text; }
        }
    }
}
