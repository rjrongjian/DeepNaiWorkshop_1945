using CCWin;
using CCWin.SkinControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using www._52bang.site.tk.yinliu.MyTool;
using www._52bang.site.yinliu.MyModel;
using www_52bang_site_enjoy.MyModel;
using www_52bang_site_enjoy.MyTool;

namespace TestPlugin
{
    public partial class Form1 : CCSkinMain
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void UpdateDataGridView(SkinDataGridView skinDataGridView1, List<www_52bang_site_enjoy.MyModel.GroupInfo> groupList, Dictionary<long, int> checkedQQQun)
        {
            MyDictionaryUtil<long, int> myDictionaryUtil = new MyDictionaryUtil<long, int>();
            for (int i = 0;i< groupList.Count;i++)
            {
                www_52bang_site_enjoy.MyModel.GroupInfo groupInfo = groupList[i];
                int result = myDictionaryUtil.GetValue(checkedQQQun, groupInfo.GroupId);
                int index = skinDataGridView1.Rows.Add();
                if (result == 1)//选中了
                {
                    skinDataGridView1.Rows[index].Cells[0].Value = true;
                }
                else//未选中
                {
                    skinDataGridView1.Rows[index].Cells[0].Value = false;
                }

                skinDataGridView1.Rows[index].Cells[1].Value = groupInfo.GroupName;
                skinDataGridView1.Rows[index].Cells[2].Value = groupInfo.GroupId;
                
                
                
            }
        }

        private void UpdateDataGridView2(SkinDataGridView skinDataGridView1, List<www_52bang_site_enjoy.MyModel.GroupInfo> groupList, Dictionary<long, int> checkedQQQun)
        {
            MyDictionaryUtil<long, int> myDictionaryUtil = new MyDictionaryUtil<long, int>();
            for (int i = 0; i < groupList.Count; i++)
            {
                www_52bang_site_enjoy.MyModel.GroupInfo groupInfo = groupList[i];
                int result = myDictionaryUtil.GetValue(checkedQQQun, groupInfo.GroupId);
                int index = skinDataGridView1.Rows.Count - 1;
                if (result == 1)//选中了
                {
                    skinDataGridView1.Rows[index].Cells[0].Value = true;
                }
                else//未选中
                {
                    skinDataGridView1.Rows[index].Cells[0].Value = false;
                }

                skinDataGridView1.Rows[index].Cells[1].Value = groupInfo.GroupName;
                skinDataGridView1.Rows[index].Cells[2].Value = groupInfo.GroupId;
                if (!(i == groupList.Count - 1))
                {
                    skinDataGridView1.Rows.Add();
                }


            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            List<www_52bang_site_enjoy.MyModel.GroupInfo> groupList = new List<www_52bang_site_enjoy.MyModel.GroupInfo>();
            groupList.Add(new www_52bang_site_enjoy.MyModel.GroupInfo("老无",32131232144,0));
            groupList.Add(new www_52bang_site_enjoy.MyModel.GroupInfo("双方都是", 123131,0));
            Dictionary<long, int> dic = new Dictionary<long, int>();
            dic.Add(123131, 1);
            UpdateDataGridView(skinDataGridView1, groupList, dic);

            checkBox1.Checked = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String key = "哈哈-##-1-##-1";
            string[] keys = key.Split(new char[4] { '-', '#', '#', '-' });
            Console.WriteLine();
        }

        private void button2_Click(object sender, EventArgs e)
        {
             KuYunSearch.Search("寂静之地");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<KunyunInfo> list = KuYunSearch.Search(textBox1.Text);
            Console.WriteLine("search接口");
            foreach(KunyunInfo kunyunInfo in list)
            {
                Console.WriteLine("电影名：" + kunyunInfo.name);
                if (kunyunInfo.url != null)
                {
                    foreach (MovieInfo2 urlOne in kunyunInfo.url)
                    {
                        Console.WriteLine("链接名：" + urlOne.MovieName+"\r\n"+"，链接："+urlOne.Url);
                    }
                }
                
            }
            Console.WriteLine("search接口结束.........");
            List<KunyunInfo> list2 = KuYunSearch.Search2(textBox1.Text);
            Console.WriteLine("search接口");
            foreach (KunyunInfo kunyunInfo in list2)
            {
                Console.WriteLine("电影名："+kunyunInfo.name  );
                if (kunyunInfo.url != null)
                {
                    foreach (MovieInfo2 urlOne in kunyunInfo.url)
                    {
                        Console.WriteLine("链接名：" + urlOne.MovieName + "\r\n" + "，链接：" + urlOne.Url);
                    }
                }
                
            }
            Console.WriteLine("search2接口结束.........");

        }
    }
}
