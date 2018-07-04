using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WinFrom_WebApi_Demo
{
    public partial class FrmMain : Form
    {
        private string _statusCode;
        private string _userName;
        private Timer checkStatus;
        private DateTime upDateTime;


        public FrmMain(string code, string userName)
        {
            _statusCode = code;
            _userName = userName;
            checkStatus = new Timer();

            // 设置心跳间隔为 20 - 30 分钟随机最小不能低于 10分钟
            checkStatus.Interval = new Random().Next(20, 30)*60*1000;
            checkStatus.Tick += checkStatus_Tick;
            checkStatus.Start();
            upDateTime = DateTime.Now;
            InitializeComponent();

            textBox5.Text = code + "___" + userName;
        }

        private void checkStatus_Tick(object sender, EventArgs e)
        {

            //CheckUserStatus | 检测用户状态
            var url = "http://w.eydata.net/a246b5c4c26efc10";  //  这里改成自己的地址
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            try
            {
                //  这里改成自己的参数名称
                parameters.Add("StatusCode", _statusCode);
                parameters.Add("UserName", _userName);

                var ret = WebPost.ApiPost(url, parameters);

                if (ret != "1")
                {
                    // 如果返回心跳不是 1 说明异常,退出程序并且记录错误代码
                    OperateIniFile.WriteIniData("root", "errorCode", ret, "config.ini");
                    Application.Exit();
                }
                upDateTime = DateTime.Now;
            }
            catch (Exception)
            {
                // 如果异常超过一小时,清除本地状态码后直接关掉程序
                if (upDateTime.AddHours(1) < DateTime.Now)
                {
                    OperateIniFile.WriteIniData("root", "code", "", "config.ini");
                    Application.Exit();
                }
            }

            checkStatus.Interval = new Random().Next(20, 30)*60*1000;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            // GetAppCode | 获取程序数据
            var url = "http://w.eydata.net/4ec486e2143192d2"; //  这里改成自己的地址

            IDictionary<string, string> parameters = new Dictionary<string, string>();

            try
            {
                //  这里改成自己的参数名称
                parameters.Add("StatusCode", _statusCode);
                parameters.Add("UserName", _userName);

                var ret = WebPost.ApiPost(url, parameters);

                if (ret.Length > 0)
                {
                    textBox1.Text = ret;
                }
                // GetExpired | 获取用户的到期时间
                parameters.Clear();
                url = "http://w.eydata.net/1f6208e5b7cc9208";  //  这里改成自己的地址

                //  这里改成自己的参数名称
                parameters.Add("UserName", _userName);

                ret = WebPost.ApiPost(url, parameters);

                if (ret.Length > 0)
                {
                    textBox2.Text = ret;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("网络连接失败!");
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 程序关闭前退出登录 
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            // 	退出登录(LogOut) url
            var url = "http://w.eydata.net/61b3173a01089775";  //  这里改成自己的地址

            //  这里改成自己的参数名称
            parameters.Add("StatusCode", _statusCode);
            parameters.Add("UserName", _userName);
            var retValue = WebPost.ApiPost(url, parameters);
            if (retValue == "1")
            {
                // 退出成功,清除本地状态码
                OperateIniFile.WriteIniData("root", "code", "", "config.ini");
            }
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //GetVariable | 获取变量数据
            var url = "	http://w.eydata.net/be75fabedfc71a0c";  //  这里改成自己的地址
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            try
            {
                //  这里改成自己的参数名称
                parameters.Add("StatusCode", _statusCode);
                parameters.Add("UserName", _userName);
                parameters.Add("VariableId", "3652");
                parameters.Add("VariableName", "key");

                var ret = WebPost.ApiPost(url, parameters);

                textBox3.Text = ret;

                // 下面这两个从服务器生成
                string key =
                    "391146076880583923314164689182313608097379713994925208970436750853488633415603657266438949373535782173410774903651464734958784053017296195604068502132993";
                string modulus =
                    "11547090288523796658666851907631210330575664106344780819952933937245533589260763077668144577201460663490827085181080816430132278373265676722017548292919491";

                textBox4.Text = RsaHelper.DecryptString(ret, key, modulus);

            }
            catch (Exception)
            {
                MessageBox.Show("网络连接失败!");
            }
        }
    }
}
