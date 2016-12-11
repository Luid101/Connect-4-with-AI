using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edmond_Connect4
{
    public partial class Form2 : Form
    {
        string p1;
        string p2;
        string mode;

        public Form2()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;//removes the possibility of empty
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //pass values
            p1 = textBox1.Text;
            p2 = textBox2.Text;
            mode = comboBox1.Text;

            //helps with empty possibilities
            if ((p1 != "") && (p2 != "") && (comboBox1.SelectedIndex == 1))
            {
                Form form1 = new Form1(p1,p2,mode);//create the connect 4 form
                                                   //pass the needed values 
                form1.Show();//hide this one 
                this.Hide();//show the new form
            }
            else if ((p1 != "") && (comboBox1.SelectedIndex == 0))
            {
                Form form1 = new Form1(p1, p2, mode);//create the connect 4 form
                //pass the needed values 
                form1.Show();//hide this one 
                this.Hide();//show the new form
            }
            else 
            {
                MessageBox.Show("One or More fields are empty!");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex == 0)//if changed
            {
                textBox2.Visible = false;
                label2.Visible = false;
            }
            else 
            {
                textBox2.Visible = true;
                label2.Visible = true;
            }

        }
    }
}
