using CCWin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace www._52bang.site.tk.yinliu
{
    public partial class PromptForm : CCSkinMain
    {
        public PromptForm()
        {
            InitializeComponent();
        }

        private void accountButton_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        public void DisplayInfo(string content)
        {
            skinTextBox1.Text = content;
            this.Show();
        }
    }
}
