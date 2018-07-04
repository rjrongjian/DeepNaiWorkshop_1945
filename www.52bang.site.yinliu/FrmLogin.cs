using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WinFrom_WebApi_Demo;
using www._52bang.site.yinliu;
using www._52bang.site.yinliu.MyTool;
using www_52bang_site_enjoy.MyTool;

namespace www._52bang.site.tk.yinliu
{
    public partial class FrmLogin : Form
    {
        private MyPlugin myPlugin;
        public FrmLogin()
        {
            InitializeComponent();
        }

        public FrmLogin(MyPlugin myPlugin) : this()
        {
            this.myPlugin = myPlugin;
        }


        private void btnLogin_Click(object sender, EventArgs e)
        {
            // 用户登录(UserLogin) url
            var url = "http://w.eydata.net/839c03e916a2a8e5"; //  这里改成自己的地址

            IDictionary<string, string> parameters = new Dictionary<string, string>();
        
            try
            {
                var code = site.yinliu.OperateIniFile.ReadIniData("root", "code", "", "config.ini");
                var upName = site.yinliu.OperateIniFile.ReadIniData("root", "upName", "", "config.ini");
                if (code.Length > 0 && upName.Length>0)
                {
                    // 	退出登录(LogOut) url
                    var logOutUrl = "http://w.eydata.net/61b3173a01089775"; //  这里改成自己的地址

                    //  这里改成自己的参数名称
                    parameters.Add("StatusCode", code);
                    parameters.Add("UserName", upName);


                    WebPost.ApiPost(logOutUrl, parameters);

                    parameters.Clear();
                }

                //  这里改成自己的参数名称
                parameters.Add("UserName", txtLoginUserName.Text.Trim());
                parameters.Add("UserPwd", txtLoginUserPwd.Text);
                parameters.Add("Version", "1.1");
                parameters.Add("Mac", "");



                var ret = WebPost.ApiPost(url, parameters);

                if (ret.Length == 32)
                {
                    site.yinliu.OperateIniFile.WriteIniData("root", "code", ret, "config.ini");
                    site.yinliu.OperateIniFile.WriteIniData("root", "upName", txtLoginUserName.Text.Trim(), "config.ini");
                    if (ckbRememberMe.Checked)
                    {
                        site.yinliu.OperateIniFile.WriteIniData("root", "name", txtLoginUserName.Text, "config.ini");
                        site.yinliu.OperateIniFile.WriteIniData("root", "pwd", txtLoginUserPwd.Text, "config.ini");
                    }
                    else
                    {
                        site.yinliu.OperateIniFile.WriteIniData("root", "name", "", "config.ini");
                        site.yinliu.OperateIniFile.WriteIniData("root", "pwd", "", "config.ini");
                    }

                    //FrmMain frm = new FrmMain(ret,txtLoginUserName.Text.Trim());
                    this.Hide();
                    //frm.Show();
                    myPlugin.InitMainform(ret, txtLoginUserName.Text.Trim());
                }
                else
                {
                    String errInfo = YiYouCode.getValue(ret);
                    if (string.IsNullOrWhiteSpace(errInfo))
                    {
                        MessageBox.Show("登录失败,错误代码: " + ret);
                    }
                    else
                    {
                        MessageBox.Show(errInfo);
                    }
                    
                }
            }
            catch (Exception e18)
            {
                MyLogUtil.ErrToLog("异常原因,"+ e18);
                MessageBox.Show("网络连接失败!");
            }
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            var uName = site.yinliu.OperateIniFile.ReadIniData("root", "name", "", "config.ini");
            var uPwd = site.yinliu.OperateIniFile.ReadIniData("root", "pwd", "", "config.ini");
            if (uName.Length > 0)
            {
                ckbRememberMe.Checked = true;
                txtLoginUserName.Text = uName;
                txtLoginUserPwd.Text = uPwd;
            }
        }

        private void btnReg_Click(object sender, EventArgs e)
        {
            // UserRegin | 用户注册 url
            var url = "http://w.eydata.net/cfe22fbe2935fc5e"; //  这里改成自己的地址

            IDictionary<string, string> parameters = new Dictionary<string, string>();

            try
            {
                //  这里改成自己的参数名称
                parameters.Add("UserName", txtRegUserName.Text.Trim());
                parameters.Add("UserPwd", txtRegPwd.Text);
                parameters.Add("Email", txtRegEmail.Text);
                parameters.Add("Mac", "");

                var ret = WebPost.ApiPost(url, parameters);

                if (ret == "1")
                {
                    MessageBox.Show("注册成功!");
                }
                else
                {
                    MessageBox.Show("注册失败,错误代码: " + ret);
                }
            }
            catch (Exception e19)
            {
                MyLogUtil.ErrToLog("异常原因," + e19);
                MessageBox.Show("网络连接失败!");
            }
        }

        private void btnRecharge_Click(object sender, EventArgs e)
        {
            // UserRecharge | 用户充值
            var url = "http://w.eydata.net/b6af9bff0a8a373c"; //  这里改成自己的地址

            IDictionary<string, string> parameters = new Dictionary<string, string>();

            try
            {
                //  这里改成自己的参数名称
                parameters.Add("UserName", txtReUserName.Text.Trim());
                parameters.Add("CardPwd", txtReCard.Text);
                parameters.Add("Referral", txtReReferral.Text);

                var ret = WebPost.ApiPost(url, parameters);

                if (int.Parse(ret) >= 1)
                {
                    MessageBox.Show("充值成功!");
                }
                else
                {
                    MessageBox.Show("充值失败,错误代码: " + ret);
                }
            }
            catch (Exception e20)
            {
                MyLogUtil.ErrToLog("异常原因," + e20);
                MessageBox.Show("网络连接失败!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // UpdatePwd | 修改用户密码
            var url = "http://w.eydata.net/5ff969d2bef16403"; //  这里改成自己的地址

            IDictionary<string, string> parameters = new Dictionary<string, string>();

            try
            {
                //  这里改成自己的参数名称
                parameters.Add("UserName", txtUpPwdUserName.Text);
                parameters.Add("UserPwd", txtUpPwd1.Text);
                parameters.Add("NewUserPwd", txtUpPwd2.Text);

                var ret = WebPost.ApiPost(url, parameters);

                if (int.Parse(ret) >= 1)
                {
                    MessageBox.Show("修改密码成功!");
                }
                else
                {
                    MessageBox.Show("修改密码失败,错误代码: " + ret);
                }
            }
            catch (Exception e21)
            {
                MyLogUtil.ErrToLog("异常原因," + e21);
                MessageBox.Show("网络连接失败!");
            }
        }

        private void btnRetrieve_Click(object sender, EventArgs e)
        {
            // UpdatePwdByEmail | 用户密码找回
            var url = "http://w.eydata.net/e82b0f76670b450a"; //  这里改成自己的地址

            IDictionary<string, string> parameters = new Dictionary<string, string>();

            try
            {
                //  这里改成自己的参数名称
                parameters.Add("UserName", txtRetrieveUserName.Text);
                parameters.Add("Email", txtRetrieveEmail.Text);

                var ret = WebPost.ApiPost(url, parameters);

                if (int.Parse(ret) >= 1)
                {
                    MessageBox.Show("找回密码成功,请注意邮箱查收!");
                }
                else
                {
                    MessageBox.Show("找回密码失败,错误代码: " + ret);
                }
            }
            catch (Exception e22)
            {
                MyLogUtil.ErrToLog("异常原因," + e22);
                MessageBox.Show("网络连接失败!");
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("联系客服购买充值码：周卡-6元，月卡-22元，季卡-58元");
        }
    }
}
