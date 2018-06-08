using CCWin;
using CCWin.SkinControl;
using DeepNaiWorkshop_6001.MyTool;
using FileCreator.MyTool;
using Newbe.CQP.Framework;
using Newbe.CQP.Framework.Extensions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFrom_WebApi_Demo;
using www._52bang.site.tk.yinliu.MyModel;
using www._52bang.site.tk.yinliu.MyModel.index;
using www._52bang.site.tk.yinliu.MyModel.rot;
using www._52bang.site.tk.yinliu.MyTool;
using www_52bang_site_enjoy.MyModel;
using www_52bang_site_enjoy.MyTool;

namespace www._52bang.site.tk.yinliu
{
    public partial class MainForm : CCSkinMain
    //public partial class MainForm : Form
    {
        private PromptForm promptForm;
        private InitForm initForm;
        private ICoolQApi coolQApi;
        private String ret;
        private String loginUserName;

        public MainForm(ICoolQApi coolQApi,String ret,String loginUserName):this(ret,loginUserName)
        {
            this.coolQApi = coolQApi;
            promptForm = new PromptForm();
            

            InitializeComponent();
            
            //正在加载请稍后
            initForm = new InitForm();
            initForm.ShowInfo("正在加载配置信息...");
            //初始化配置
            InitConfig();
            initForm.ShowInfo("正在重新渲染UI...");
            ReloadPaint();

            //Thread.Sleep(2000);
            initForm.Hide();
            
        }

        private MainForm(String ret,String loginUserName)
        {
            this.ret = ret;
            this.loginUserName = loginUserName;
        }

        //利用数据重画界面
        private void ReloadPaint()
        {
            if (CacheData.BaseJson!=null)
            {
                if (CacheData.BaseJson.Keys != null)
                {
                    int i = 1;
                    foreach (var item in CacheData.BaseJson.Keys)
                    {
                        string key = item.Key;//关键字-##-1-##-1  关键字-##-此关键字在群聊中生效-##-此关键字在私聊中生效
                        string[] keys = new String[]{ "","0","0"};
                        if (!string.IsNullOrWhiteSpace(key))
                        {

                            keys = key.Split(new char[4] { '-', '#', '#','-' });
                        }

                        string value = item.Value;
                        if (i == 1)
                        {
                            textBox1.Text = keys[0];

                            textBox2.Text = value;
                            if ("1".Equals(keys[4]))
                            {
                                checkBox1.Checked = true;
                            }

                            if ("1".Equals(keys[8]))
                            {
                                checkBox2.Checked = true;
                            }

                        }
                        else if (i == 2)
                        {
                            textBox4.Text = keys[0];
                            textBox3.Text = value;
                            if ("1".Equals(keys[4]))
                            {
                                checkBox4.Checked = true;
                            }

                            if ("1".Equals(keys[8]))
                            {
                                checkBox3.Checked = true;
                            }
                        }
                        else if (i == 3)
                        {
                            textBox6.Text = keys[0];
                            textBox5.Text = value;
                            if ("1".Equals(keys[4]))
                            {
                                checkBox6.Checked = true;
                            }

                            if ("1".Equals(keys[8]))
                            {
                                checkBox5.Checked = true;
                            }
                        }
                        else if (i == 4)
                        {
                            textBox8.Text = keys[0];
                            textBox7.Text = value;
                            if ("1".Equals(keys[4]))
                            {
                                checkBox8.Checked = true;
                            }

                            if ("1".Equals(keys[8]))
                            {
                                checkBox7.Checked = true;
                            }
                        }
                        else if (i == 5)
                        {
                            textBox10.Text = keys[0];
                            textBox9.Text = value;
                            if ("1".Equals(keys[4]))
                            {
                                checkBox10.Checked = true;
                            }

                            if ("1".Equals(keys[8]))
                            {
                                checkBox9.Checked = true;
                            }
                        }
                        Console.WriteLine(item.Key + item.Value);
                        i++;

                    }
                }

                //是否自动加群
                if (CacheData.BaseJson.IsAutoAddQun)
                {
                    radioButton6.Checked = true;
                }
                else
                {
                    radioButton5.Checked = true;
                }

                //是否自动加好友
                if (CacheData.BaseJson.IsAutoAddFriend)
                {
                    radioButton3.Checked = true;
                }
                else
                {
                    radioButton4.Checked = true;
                }

                //新人进群欢迎词
                skinTextBox1.Text = CacheData.BaseJson.NewerEnterQun;

                //选中群列表中某些群
                UpdateDataGridView(skinDataGridView2,CacheData.GroupList, CacheData.BaseJson.CheckedQQQun);
            }

            if (CacheData.MovieConfig != null)
            {
                //监听到群消息后，是否需要 @机器人触发命令：
                if (CacheData.MovieConfig.IsNeed)
                {
                    radioButton1.Checked = true;
                }
                else
                {
                    radioButton2.Checked = true;
                }

                if (!string.IsNullOrWhiteSpace(CacheData.MovieConfig.SearchCommand))
                {
                    skinTextBox2.Text = CacheData.MovieConfig.SearchCommand;
                }

                //未找到电影，群中回复信息
                if (!string.IsNullOrWhiteSpace(CacheData.MovieConfig.NoSearchedMovieInQun))
                {
                    skinTextBox3.Text = CacheData.MovieConfig.NoSearchedMovieInQun;
                }

                //未找到电影，私聊回复信息
                if (!string.IsNullOrWhiteSpace(CacheData.MovieConfig.NoSearchedMovie))
                {
                    skinTextBox4.Text = CacheData.MovieConfig.NoSearchedMovie;
                }

                //找到电影，群中回复信息
                if (!string.IsNullOrWhiteSpace(CacheData.MovieConfig.HaveSearchedMovieInQun))
                {
                    skinTextBox6.Text = CacheData.MovieConfig.HaveSearchedMovieInQun;
                }

                //找到电影，私信回复信息
                if (!string.IsNullOrWhiteSpace(CacheData.MovieConfig.HaveSearchedMovie))
                {
                    skinTextBox5.Text = CacheData.MovieConfig.HaveSearchedMovie;
                }

                //设置资源解析方式
                this.skinComboBox1.SelectedIndex = CacheData.MovieConfig.ConvertLinkIndex;
                //选择的解析器
                this.skinComboBox2.SelectedIndex = CacheData.MovieConfig.SelectedParserIndex;

                UpdateDataGridView(skinDataGridView1,CacheData.GroupList, CacheData.MovieConfig.CheckedQQQun);
            }

            //显示公告
            promptForm.DisplayInfo(CacheData.NotifyInfo);

            
        }

        private void UpdateDataGridView(SkinDataGridView skinDataGridView1, List<www_52bang_site_enjoy.MyModel.GroupInfo> groupList, Dictionary<long, int> checkedQQQun)
        {
            skinDataGridView1.Rows.Clear();
            MyDictionaryUtil<long, int> myDictionaryUtil = new MyDictionaryUtil<long, int>();
            for (int i = 0; i < groupList.Count; i++)
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

        internal void setCoolQApi(ICoolQApi coolQApi)
        {
            this.coolQApi = coolQApi;
        }

        //初始化基础配置
        private void InitConfig()
        {
            try
            {
                //GetBulletin | 获取程序公告
                String url1 = "http://w.eydata.net/a785c7d10885be26";  //  这里改成自己的地址
                IDictionary<string, string> parameters1 = new Dictionary<string, string>();
                //  这里改成自己的参数名称
                parameters1.Add("UserName", loginUserName);
                String ret1 = WebPost.ApiPost(url1, parameters1);

                if (ret1.Length > 0)
                {
                    CacheData.NotifyInfo = ret1;
                }


                // GetExpired | 获取用户的到期时间
                String url = "http://w.eydata.net/1f6208e5b7cc9208";  //  这里改成自己的地址
                IDictionary<string, string> parameters = new Dictionary<string, string>();
                //  这里改成自己的参数名称
                parameters.Add("UserName", loginUserName);

                ret = WebPost.ApiPost(url, parameters);

                if (ret.Length > 0)
                {
                    this.Text = this.Text+"  （"+ loginUserName + "）过期时间："+ret;
                }


                this.skinComboBox1.ValueMember = "index";
                this.skinComboBox1.DisplayMember = "moduleName";
                this.skinComboBox1.DataSource = CacheData.linkList;

                this.skinComboBox2.ValueMember = "index";
                this.skinComboBox2.DisplayMember = "moduleName";
                this.skinComboBox2.DataSource = CacheData.resourceApiList;
                

                BaseJson baseJson = MySystemUtil.GetBaseJsonConfig();
                MovieConfig movieConfig = MySystemUtil.GetMovieJsonConfig();
                CacheData.BaseJson = baseJson;
                CacheData.MovieConfig = movieConfig;
                //MyLogUtil.ToLog("coolQApi:"+ coolQApi);
                //初始群列表
                if (coolQApi == null)
                {
                    CacheData.GroupList = new List<www_52bang_site_enjoy.MyModel.GroupInfo>();
                }
                else
                {
                    //由于在这调用获取群列表时酷q初始化未完成，会导致抛异常
                    //CacheData.GroupList = CoolQApiExtend.GetGroupList(coolQApi);
                    CacheData.GroupList = new List<www_52bang_site_enjoy.MyModel.GroupInfo>();
                    /*
                    CacheData.GroupList = new List<www_52bang_site_enjoy.MyModel.GroupInfo>();
                    IEnumerable<Newbe.CQP.Framework.Extensions.GroupInfo> list = ExtrasCoolApiExtensions.GetGroupList(coolQApi);
                    foreach (Newbe.CQP.Framework.Extensions.GroupInfo obj in list)
                    {
                        CacheData.GroupList.Add(new www_52bang_site_enjoy.MyModel.GroupInfo(obj.GroupName,obj.GroupNumber,obj.OwnerNumber));
                    }
                    */
                }
            }catch(Exception e)
            {
                MyLogUtil.ErrToLog("初始化出现异常，原因："+e);
            }



        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("fsdfs");
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //新用户加入，可用变量
            promptForm.DisplayInfo("{@新用户} 变量作用：用户进群后，可@新用户");
        }
        //电影助手启动监听
        private void skinButton1_Click(object sender, EventArgs e)
        {
            try { 
                skinButton1.Enabled = false;
                if (string.IsNullOrWhiteSpace(skinTextBox2.Text))
                {
                    MessageBox.Show("搜索命令不能为空");
                    return;
                }
                //保存首页配置
                UpdateAndSaveBaseJson();
                //保存机器人配置
                UpdateAndSaveMovieConfig();
                CacheData.IsRunMovieRot = true;
                skinButton3.Enabled = true;
                RecordLog("更新配置完成，且启动监听...");
            }catch(Exception e23)
            {
                MyLogUtil.ErrToLog("影音机器人启动监听失效,原因：" + e23);
            }

        }
        private void RecordLog(string str)
        {
            label33.Text = str;
        }

        private void UpdateAndSaveMovieConfig()
        {
            MovieConfig movieConfig = new MovieConfig();
            movieConfig.CheckedQQQun = new Dictionary<long, int>();
            //设置选中的QQ群
            for (int i = 0; i < skinDataGridView1.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell checkBox = (DataGridViewCheckBoxCell)this.skinDataGridView1.Rows[i].Cells[0];
                if ((bool)checkBox.Value == true)
                {
                    movieConfig.CheckedQQQun.Add((long)this.skinDataGridView1.Rows[i].Cells[2].Value, 1);
                }
            }
            //监听到群消息后，是否需要 @机器人触发命令：
            movieConfig.IsNeed = (radioButton1.Checked == true) ? true : false;
            //搜索命令
            movieConfig.SearchCommand = string.IsNullOrWhiteSpace(skinTextBox2.Text)?"": skinTextBox2.Text;
            //未找到电影时，机器人在“群中”给用户的回复:
            movieConfig.NoSearchedMovieInQun = string.IsNullOrWhiteSpace(skinTextBox3.Text) ? "" : skinTextBox3.Text;
            //找到电影时，机器人在“群中”给用户的回复:
            movieConfig.HaveSearchedMovieInQun = string.IsNullOrWhiteSpace(skinTextBox6.Text) ? "" : skinTextBox6.Text;
            //未找到电影时，机器人“私聊”给与的回复:
            movieConfig.NoSearchedMovie = string.IsNullOrWhiteSpace(skinTextBox4.Text) ? "" : skinTextBox4.Text;
            //找到电影时，机器人“私聊”给与的回复:
            movieConfig.HaveSearchedMovie = string.IsNullOrWhiteSpace(skinTextBox5.Text) ? "" : skinTextBox5.Text;
            //是否开启私聊接受信息
            movieConfig.IsCanPrivateMessage = radioButton7.Checked ? true:false;
            //选中的解析资源的方式
            movieConfig.ConvertLinkIndex = skinComboBox1.SelectedIndex;
            //选择的解析器
            movieConfig.SelectedParserIndex = skinComboBox2.SelectedIndex;

            //更新缓存数据
            CacheData.MovieConfig = movieConfig;
            //持久化
            MyJsonUtil<MovieConfig> myJsonUtil = new MyJsonUtil<MovieConfig>();
            MyFileUtil.writeToFile(MySystemUtil.GetMovieJsonPath(), myJsonUtil.parseJsonObj(movieConfig));
            
        }

        private BaseJson UpdateAndSaveBaseJson()
        {
            BaseJson baseJson = new BaseJson();
            baseJson.Keys = new Dictionary<string, string>();
            baseJson.CheckedQQQun = new Dictionary<long, int>();

            //设置关键词1
            if (!string.IsNullOrWhiteSpace(this.textBox1.Text)&& !string.IsNullOrWhiteSpace(this.textBox2.Text)&&(checkBox1.Checked|| checkBox2.Checked)){
                baseJson.Keys.Add(this.textBox1.Text + "-##-" + (checkBox1.Checked ? 1 : 0) + "-##-" + (checkBox2.Checked ? 1 : 0), this.textBox2.Text);
            }

            //设置关键词2
            if (!string.IsNullOrWhiteSpace(this.textBox4.Text) && !string.IsNullOrWhiteSpace(this.textBox3.Text) && (checkBox4.Checked || checkBox3.Checked)){
                baseJson.Keys.Add(this.textBox4.Text + "-##-" + (checkBox4.Checked ? 1 : 0) + "-##-" + (checkBox3.Checked ? 1 : 0), this.textBox3.Text);
            }

            //设置关键词3
            if (!string.IsNullOrWhiteSpace(this.textBox6.Text) && !string.IsNullOrWhiteSpace(this.textBox5.Text) && (checkBox6.Checked || checkBox5.Checked)){
                baseJson.Keys.Add(this.textBox6.Text + "-##-" + (checkBox6.Checked ? 1 : 0) + "-##-" + (checkBox5.Checked ? 1 : 0), this.textBox5.Text);
            }

            //设置关键词4
            if (!string.IsNullOrWhiteSpace(this.textBox8.Text) && !string.IsNullOrWhiteSpace(this.textBox7.Text) && (checkBox8.Checked || checkBox7.Checked)){
                baseJson.Keys.Add(this.textBox8.Text + "-##-" + (checkBox8.Checked ? 1 : 0) + "-##-" + (checkBox7.Checked ? 1 : 0), this.textBox7.Text);
            }

            //设置关键词5
            if (!string.IsNullOrWhiteSpace(this.textBox10.Text) && !string.IsNullOrWhiteSpace(this.textBox9.Text) && (checkBox10.Checked || checkBox9.Checked)){
                baseJson.Keys.Add(this.textBox10.Text + "-##-" + (checkBox10.Checked ? 1 : 0) + "-##-" + (checkBox9.Checked ? 1 : 0), this.textBox9.Text);
            }
            //设置选中的QQ群
            for(int i = 0;i< skinDataGridView2.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell checkBox = (DataGridViewCheckBoxCell)this.skinDataGridView2.Rows[i].Cells[0];
                if ((bool)checkBox.Value==true)
                {
                    baseJson.CheckedQQQun.Add((long)this.skinDataGridView2.Rows[i].Cells[2].Value, 1);
                }
            }

            //是否自动加用户入群
            baseJson.IsAutoAddQun = radioButton6.Checked ? true:false;
            //是否自动添加好友
            baseJson.IsAutoAddFriend = radioButton3.Checked ? true:false;
            //入群后欢迎词
            baseJson.NewerEnterQun = string.IsNullOrWhiteSpace(skinTextBox1.Text)?"": skinTextBox1.Text;
            //更新缓存数据
            CacheData.BaseJson = baseJson;
            //持久化
            MyJsonUtil<BaseJson> myJsonUtil = new MyJsonUtil<BaseJson>();
            MyFileUtil.writeToFile(MySystemUtil.GetBaseJsonPath(), myJsonUtil.parseJsonObj(baseJson));

            return baseJson;
        }
        //停止影音机器人
        private void skinButton3_Click(object sender, EventArgs e)
        {
            CacheData.IsRunMovieRot = false;
            skinButton3.Enabled = false;
            skinButton1.Enabled = true;
            RecordLog("影音机器人已经停止");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //新用户加入，可用变量
            promptForm.DisplayInfo("{@用户} 变量作用：可@用户");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //新用户加入，可用变量
            promptForm.DisplayInfo("{@用户} 变量作用：可@用户 "+ Environment.NewLine+"{电影名} 变量作用显示电影名（如果搜索为多个结果，会要求用户按指定关键词再次搜索）" + Environment.NewLine +"{电影链接} 变量作用：观影链接，如果链接为多个会全部显示出来");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //未找到电影，私聊提示
            promptForm.DisplayInfo("{@用户} 变量作用：可@用户");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //新用户加入，可用变量
            promptForm.DisplayInfo("{电影名} 变量作用显示电影名（如果搜索为多个结果，会要求用户按指定关键词再次搜索）" + Environment.NewLine + "{电影链接} 变量作用：观影链接，如果链接为多个会全部显示出来");
        }

        private void skinButton2_Click_1(object sender, EventArgs e)
        {
            promptForm.DisplayInfo("私聊包括用户添加好友或者通过群里找到你私聊和你聊天，私聊支持用户分享各个视频网站的vip视频，然后负责转链直接观看");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //MyLogUtil.ToLog(""+coolQApi.GetCookies());
            //CoolQApiExtend.GetGroupList2(coolQApi);
            try
            {
                CacheData.GroupList = new List<www_52bang_site_enjoy.MyModel.GroupInfo>();
                IEnumerable<Newbe.CQP.Framework.Extensions.GroupInfo> list = ExtrasCoolApiExtensions.GetGroupList(coolQApi);
                foreach (Newbe.CQP.Framework.Extensions.GroupInfo obj in list)
                {
                    CacheData.GroupList.Add(new www_52bang_site_enjoy.MyModel.GroupInfo(obj.GroupName, obj.GroupNumber, obj.OwnerNumber));
                }



            }catch(Exception e23)
            {
                MyLogUtil.ErrToLog("空指针：" + e23);
            }
            try
            {
                
            }catch(Exception eeee)
            {
                MyLogUtil.ErrToLog("hahah：" + eeee);
            }
           


        }

        private void skinButton4_Click(object sender, EventArgs e)
        {
            initQunAndPaint();
        }

        public void initQunAndPaint()
        {
            try
            {
                CacheData.GroupList = CoolQApiExtend.GetGroupList(coolQApi);
                //选中群列表中某些群
                UpdateDataGridView(skinDataGridView2, CacheData.GroupList, CacheData.BaseJson.CheckedQQQun);
                //选中群列表中某些群
                UpdateDataGridView(skinDataGridView1, CacheData.GroupList, CacheData.MovieConfig.CheckedQQQun);
            }
            catch (Exception eee)
            {
                MyLogUtil.ErrToLog("加载群信息失败，原因：" + eee);

            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 程序关闭前退出登录 
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            // 	退出登录(LogOut) url
            var url = "http://w.eydata.net/61b3173a01089775";  //  这里改成自己的地址

            //  这里改成自己的参数名称
            parameters.Add("StatusCode", ret);
            parameters.Add("UserName", loginUserName);
            var retValue = WebPost.ApiPost(url, parameters);
            if (retValue == "1")
            {
                // 退出成功,清除本地状态码
                www._52bang.site.yinliu.OperateIniFile.WriteIniData("root", "code", "", "config.ini");
            }
            Application.Exit();
        }
    }
}
