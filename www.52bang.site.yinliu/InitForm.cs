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
    public partial class InitForm : CCSkinMain
    {
        public InitForm()
        {
            InitializeComponent();
        }


        public void ShowInfo(string content)
        {
            this.Show();
            this.label1.Text = content;
        }
    }
}
