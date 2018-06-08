using Newbe.CQP.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFrom_WebApi_Demo;
using www._52bang.site.tk.yinliu;
using www._52bang.site.tk.yinliu.MyModel;
using www._52bang.site.tk.yinliu.MyTool;
using www_52bang_site_enjoy.MyModel;
using www_52bang_site_enjoy.MyTool;

namespace www._52bang.site.yinliu
{
    public class MyPlugin : PluginBase
    {
        private MainForm mainForm;
        private www._52bang.site.tk.yinliu.FrmLogin frmLogin;
        public MyPlugin(ICoolQApi coolQApi) : base(coolQApi)
        {
            
            frmLogin = new www._52bang.site.tk.yinliu.FrmLogin(this);
            frmLogin.Show();
            //mainForm = new MainForm(CoolQApi);
            //mainForm.Show();
        }
        /// <summary>
        /// AppId需要与程序集名称相同
        /// </summary>
        public override string AppId => "www.52bang.site.yinliu";

        /// <summary>
        /// 监听私聊事件
        /// </summary>
        /// <param name="subType"></param>
        /// <param name="sendTime"></param>
        /// <param name="fromQQ"></param>
        /// <param name="msg"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        public override int ProcessPrivateMessage(int subType, int sendTime, long fromQQ, string msg, int font)
        {

            try
            {
                if(CacheData.MovieConfig.IsCanPrivateMessage)
                {
                    //用户发来的消息日志
                    //MyLogUtil.WriteQQDialogueLog(fromQQ, msg);

                    //说明用户回复的是指定的关键词
                    string replayContent2 = null;
                    try
                    {
                        replayContent2 = CacheData.BaseJson.Keys[msg + "-##-1-##-1"];
                        CoolQApi.SendPrivateMsg(fromQQ, replayContent2);
                        return base.ProcessPrivateMessage(subType, sendTime, fromQQ, msg, font);
                    }
                    catch (KeyNotFoundException ke)
                    {
                        try
                        {
                            replayContent2 = CacheData.BaseJson.Keys[msg + "-##-0-##-1"];
                            CoolQApi.SendPrivateMsg(fromQQ, replayContent2);
                            return base.ProcessPrivateMessage(subType, sendTime, fromQQ, msg, font);
                        }
                        catch (KeyNotFoundException ke2)
                        {

                        }
                    }


                    //判断用户是否是想搜取
                    if (msg.StartsWith(CacheData.MovieConfig.SearchCommand))
                    {
                        String url = "";
                        try
                        {
                            string searchContent = msg.Substring(1);
                            if (string.IsNullOrWhiteSpace(searchContent))
                            {
                                //CoolQApi.SendPrivateMsg(fromQQ, "请输入要搜索的内容，例如：搜黑豹");
                                CoolQApi.SendPrivateMsg(fromQQ, CacheData.MovieConfig.NoSearchedMovie);
                                return base.ProcessPrivateMessage(subType, sendTime, fromQQ, msg, font);
                            }
                            else
                            {
                                CoolQApi.SendPrivateMsg(fromQQ, "正在寻找资源，请稍等...");
                                List<KunyunInfo> list = KuYunSearch.Search(searchContent, CacheData.MovieConfig.ConvertLinkIndex);
                                
                                if (list == null || list.Count == 0)
                                {
                                    //CoolQApi.SendPrivateMsg(fromQQ, "暂时未找到此资源");
                                    CoolQApi.SendPrivateMsg(fromQQ, CacheData.MovieConfig.NoSearchedMovie);
                                    return base.ProcessPrivateMessage(subType, sendTime, fromQQ, msg, font);
                                }
                                else if (list.Count == 1)//说明找到了具体电影的链接
                                {
                                    string replayContent = CacheData.MovieConfig.HaveSearchedMovie;
                                    StringBuilder sb = new StringBuilder(" ");
                                    foreach (KunyunInfo k in list)
                                    {

                                        replayContent = replayContent.Replace("{电影名}", k.name);
                                        foreach (string res in k.url)
                                        {
                                            if (k.resourceTYpe == 1)//m3u8
                                            {
                                                sb.Append(MyLinkCoverter.CovertUrlInSuoIm(res, true) + Environment.NewLine);
                                            }
                                            else//直接观看链接
                                            {
                                                sb.Append(MyLinkCoverter.CovertUrlInSuoIm(res, false) + Environment.NewLine);
                                            }

                                        }

                                    }
                                    replayContent = replayContent.Replace("{电影链接}", sb.ToString());
                                    CoolQApi.SendPrivateMsg(fromQQ, replayContent);
                                    return base.ProcessPrivateMessage(subType, sendTime, fromQQ, msg, font);
                                }
                                else//说明找到了相关的好几个电影
                                {
                                    StringBuilder sb = new StringBuilder("我找到了多个相关资源，请聊天回复以下具体某个资源获取观影链接：" + Environment.NewLine);
                                    foreach (KunyunInfo k in list)
                                    {
                                        sb.Append(CacheData.MovieConfig.SearchCommand + k.name + Environment.NewLine);
                                    }
                                    CoolQApi.SendPrivateMsg(fromQQ, sb.ToString());
                                    return base.ProcessPrivateMessage(subType, sendTime, fromQQ, msg, font);
                                }
                            }

                        }
                        catch (Exception e2)
                        {
                            CoolQApi.SendPrivateMsg(fromQQ, "小喵出现问题，请过会再来尝试");
                            return base.ProcessPrivateMessage(subType, sendTime, fromQQ, msg, font);
                        }
                    }

                    //判断是否是指定的视频平台链接
                    if (MyLinkCoverter.JugePlatform(msg))
                    {
                        MyResponse<MovieInfo> myResponse = MyLinkCoverter.CovertInSuoIm(msg);
                        //MyLogUtil.ToLog("看看日志：" + myResponse.Msg.MovieName + "_链接：" + myResponse.Msg.Url);
                        if (myResponse.Code == 0)//获取成功了
                        {
                            string temp = CacheData.MovieConfig.HaveSearchedMovie;
                            temp = temp.Replace("{电影名}", myResponse.Msg.MovieName);
                            temp = temp.Replace("{电影链接}", myResponse.Msg.Url);
                            //给用户回复的信息日志
                            //MyLogUtil.WriteQQDialogueLogOfMe(fromQQ, myResponse.Msg.MovieName + " " + myResponse.Msg.Url);
                            CoolQApi.SendPrivateMsg(fromQQ, temp);
                            return base.ProcessPrivateMessage(subType, sendTime, fromQQ, msg, font);
                        }
                        else
                        {
                            //给用户回复的信息日志
                            //MyLogUtil.WriteQQDialogueLogOfMe(fromQQ, SystemConfig.NoConvertPlatform);
                            CoolQApi.SendPrivateMsg(fromQQ, CacheData.MovieConfig.NoSearchedMovie);
                            return base.ProcessPrivateMessage(subType, sendTime, fromQQ, msg, font);
                        }

                    }


                    //解析分享过来的是不是指定平台的链接(拦截形如xxxhttp://www.baidu.comxxx,但是有些新闻也是这种格式的)
                    MyResponse<string> sharedUrl = null;
                    sharedUrl = MyLinkCoverter.ParsePlatform(msg);
                    if (sharedUrl.Code == 0)//正常链接
                    {
                        sharedUrl.Msg = MyLinkCoverter.CovertUrlInSuoIm(sharedUrl.Msg, true);
                        //给用户回复的信息日志
                        //MyLogUtil.WriteQQDialogueLogOfMe(fromQQ, "主人，这是你的观影地址：" + " " + sharedUrl.Msg + "，由于需要加载影片，请耐心等待，如果不能播放，请刷新或换浏览器，更多好玩的电影跟班，关注微信公众号[电影信封]");
                        string temp = CacheData.MovieConfig.HaveSearchedMovie;
                        temp = temp.Replace("{电影名}", "");
                        temp = temp.Replace("{电影链接}", sharedUrl.Msg);
                        //CoolQApi.SendPrivateMsg(fromQQ, "主人，这是你的观影地址：" + " " + sharedUrl.Msg + "，由于需要加载影片，请耐心等待，如果不能播放，请刷新或换浏览器，更多好玩的电影跟班，关注微信公众号[电影信封]");
                        //CoolQApi.SendPrivateMsg(fromQQ, "请确保发送给我的是主流视频平台的http或https链接，不要带其他信息，否则不能正常观看呦");
                        CoolQApi.SendPrivateMsg(fromQQ, temp);
                        return base.ProcessPrivateMessage(subType, sendTime, fromQQ, msg, font);
                    }
                    return base.ProcessPrivateMessage(subType, sendTime, fromQQ, msg, font);
                }
                else
                {
                    return base.ProcessPrivateMessage(subType, sendTime, fromQQ, msg, font);

                }
                
            }
            catch (Exception e)
            {
                MyLogUtil.ErrToLog(fromQQ, "发生不被期待的异常：" + e);
                return base.ProcessPrivateMessage(subType, sendTime, fromQQ, msg, font);
            }
        }

        internal void InitMainform(string ret, string loginUserName)
        {
            mainForm = new MainForm(CoolQApi,ret,loginUserName);
            mainForm.Show();
        }

        /// <summary>
        /// 好友添加请求
        /// </summary>
        /// <param name="subType">事件类型。固定为1。</param>
        /// <param name="sendTime">	事件发生时间的时间戳。</param>
        /// <param name="fromQq">事件来源QQ。</param>
        /// <param name="msg">附言内容。</param>
        /// <param name="responseFlag">反馈标识(处理请求用)</param>
        /// <returns></returns>
        public override int ProcessAddFriendRequest(int subType, int sendTime, long fromQq, string msg, string responseFlag)
        {
            if (CacheData.BaseJson.IsAutoAddFriend)//是否自动添加好友
            {
                //有人添加好友，直接加为我的好友
                CoolQApi.SetFriendAddRequest(responseFlag, 1, System.DateTime.Now.ToString("yyyyMMddHHmmss"));
            }


            return base.ProcessAddFriendRequest(subType, sendTime, fromQq, msg, responseFlag);
        }

        /// <summary>
        /// 处理好友已添加事件（此事件监听不到）
        /// </summary>
        /// <param name="subType">事件类型。固定为1</param>
        /// <param name="sendTime">事件发生时间的时间戳</param>
        /// <param name="fromQq">事件来源QQ</param>
        /// <returns></returns>
        public override int ProcessFriendsAdded(int subType, int sendTime, long fromQq)
        {

            //给用户回复的信息日志
            //MyLogUtil.WriteQQDialogueLogOfMe(fromQq, SystemConfig.MsgWhenFriendsAdded);
            //CoolQApi.SendPrivateMsg(fromQq, SystemConfig.MsgWhenFriendsAdded);

            return base.ProcessFriendsAdded(subType, sendTime, fromQq);
        }

        /// <summary>
        /// 处理讨论组消息
        /// </summary>
        /// <param name="subType">消息类型，目前固定为1</param>
        /// <param name="sendTime">消息发送时间的时间戳</param>
        /// <param name="fromDiscuss">消息来源讨论组号</param>
        /// <param name="fromQq">发送此消息的QQ号码</param>
        /// <param name="msg">消息内容</param>
        /// <param name="font">消息所使用字体</param>
        /// <returns></returns>
        public override int ProcessDiscussGroupMessage(int subType, int sendTime, long fromDiscuss, long fromQq, string msg, int font)
        {

            //mainForm.displayMsg2("处理讨论组消息：" + subType + "," + sendTime + "," + fromDiscuss + "," + fromQq + "," + msg + "," + font);
            return base.ProcessDiscussGroupMessage(subType, sendTime, fromDiscuss, fromQq, msg, font);
        }

        /// <summary>
        /// 处理群管理员变动事件
        /// </summary>
        /// <param name="subType">事件类型。1为被取消管理员，2为被设置管理员</param>
        /// <param name="sendTime">事件发生时间的时间戳</param>
        /// <param name="fromGroup">事件来源群号</param>
        /// <param name="target">被操作的QQ</param>
        /// <returns></returns>
        public override int ProcessGroupAdminChange(int subType, int sendTime, long fromGroup, long target)
        {
            //mainForm.displayMsg2("处理群管理员变动事件：" + subType + "," + sendTime + "," + fromGroup + "," + target);

            return base.ProcessGroupAdminChange(subType, sendTime, fromGroup, target);
        }
        /// <summary>
        /// 处理群成员数量减少事件
        /// </summary>
        /// <param name="subType">事件类型。1为群员离开；2为群员被踢为；3为自己(即登录号)被踢</param>
        /// <param name="sendTime">事件发生时间的时间戳</param>
        /// <param name="fromGroup">事件来源群号</param>
        /// <param name="fromQq">事件来源QQ</param>
        /// <param name="target">被操作的QQ</param>
        /// <returns></returns>
        public override int ProcessGroupMemberDecrease(int subType, int sendTime, long fromGroup, long fromQq, long target)
        {
            //mainForm.displayMsg2("处理群成员数量减少事件：" + subType + "," + sendTime + "," + fromGroup + "," + fromQq + "," + target);

            return base.ProcessGroupMemberDecrease(subType, sendTime, fromGroup, fromQq, target);
        }
        /// <summary>
        /// 处理群成员添加事件
        /// </summary>
        /// <param name="subType">事件类型。1为管理员已同意；2为管理员邀请</param>
        /// <param name="sendTime">事件发生时间的时间戳</param>
        /// <param name="fromGroup">事件来源群号</param>
        /// <param name="fromQq">事件来源QQ</param>
        /// <param name="target">被操作的QQ</param>
        /// <returns></returns>
        public override int ProcessGroupMemberIncrease(int subType, int sendTime, long fromGroup, long fromQq, long target)
        {
            //mainForm.displayMsg2("处理群成员添加事件：" + subType + "," + sendTime + "," + fromGroup + "," + fromQq + "," + target);
            if (!string.IsNullOrWhiteSpace(CacheData.BaseJson.NewerEnterQun))
            {
                string content = CacheData.BaseJson.NewerEnterQun.Replace("{@新用户}", CoolQCode.At(target));

                //成员添加后，进行@用户，外加欢迎与
                if (subType == 1)//账号为管理员，同意群员入群后触发
                {
                    //CoolQApi.SendGroupMsg(fromGroup, CoolQCode.At(target) + "欢迎加入我们的大家庭");
                    //mainForm.displayMsg2("---给用户发送欢迎语："+ CoolQCode.At(target) + "欢迎加入我们的大家庭");
                    CoolQApi.SendGroupMsg(fromGroup, content);
                }
                else if (subType == 2)//群员邀请好友，好友同意并入群后触发
                {
                    //CoolQApi.SendGroupMsg(fromGroup, CoolQCode.At(fromQq) + "邀请" + CoolQCode.At(target) + "加入我们的大家庭，热烈欢迎");
                    //mainForm.displayMsg2("---给用户发送欢迎语：" + CoolQCode.At(fromQq) + "邀请" + CoolQCode.At(target) + "加入我们的大家庭，热烈欢迎");
                    CoolQApi.SendGroupMsg(fromGroup, content);
                }
            }



            return base.ProcessGroupMemberIncrease(subType, sendTime, fromGroup, fromQq, target);
        }
        /// <summary>
        /// 处理群聊消息
        /// </summary>
        /// <param name="subType">消息类型，目前固定为1</param>
        /// <param name="sendTime">消息发送时间的时间戳</param>
        /// <param name="fromGroup">消息来源群号</param>
        /// <param name="fromQq">发送此消息的QQ号码</param>
        /// <param name="fromAnonymous">发送此消息的匿名用户</param>
        /// <param name="msg">消息内容</param>
        /// <param name="font">消息所使用字体</param>
        /// <returns></returns>
        public override int ProcessGroupMessage(int subType, int sendTime, long fromGroup, long fromQq, string fromAnonymous, string msg, int font)
        {
            MyDictionaryUtil<long, int> myDictionaryUtil = new MyDictionaryUtil<long, int>();
            int result = myDictionaryUtil.GetValue(CacheData.BaseJson.CheckedQQQun, fromGroup);
            //说明用户回复的是指定的关键词
            string replayContent2 = null;
            if (result == 1)//监控此群
            {
                
                try
                {
                    replayContent2 = CacheData.BaseJson.Keys[msg + "-##-1-##-1"];
                    CoolQApi.SendGroupMsg(fromGroup, replayContent2);
                    return base.ProcessGroupMessage(subType, sendTime, fromGroup, fromQq, fromAnonymous, msg, font);
                }
                catch (KeyNotFoundException ke)
                {
                    try
                    {
                        replayContent2 = CacheData.BaseJson.Keys[msg + "-##-1-##-0"];
                        CoolQApi.SendGroupMsg(fromGroup, replayContent2);
                        return base.ProcessGroupMessage(subType, sendTime, fromGroup, fromQq, fromAnonymous, msg, font);
                    }
                    catch (KeyNotFoundException ke2)
                    {

                    }
                }
            }
            result = myDictionaryUtil.GetValue(CacheData.MovieConfig.CheckedQQQun, fromGroup);
            if (result == 1)
            {
                //
                string contentStr = msg.Trim();
                bool b = true;
                if (CacheData.MovieConfig.IsNeed)
                {
                    string at = CoolQCode.At(CoolQApi.GetLoginQQ());
                    if (msg.StartsWith(at))//需@机器人触发指令
                    {
                        contentStr = contentStr.Replace(at, "");
                        contentStr = contentStr.Trim();
                    }
                    else
                    {
                        b = false;
                    }


                }

                if (b)
                {
                    if (contentStr.StartsWith(CacheData.MovieConfig.SearchCommand))
                    {
                        String url = "";
                        try
                        {
                            string searchContent = contentStr.Substring(1);
                            if (string.IsNullOrWhiteSpace(searchContent))
                            {
                                //CoolQApi.SendPrivateMsg(fromQQ, "请输入要搜索的内容，例如：搜黑豹");
                                //CoolQApi.SendPrivateMsg(fromQQ, CacheData.MovieConfig.NoSearchedMovie);
                                replayContent2 = CacheData.MovieConfig.NoSearchedMovieInQun;
                                replayContent2 = replayContent2.Replace("{@用户}", CoolQCode.At(fromQq));
                                CoolQApi.SendGroupMsg(fromGroup, replayContent2);
                                return base.ProcessGroupMessage(subType, sendTime, fromGroup, fromQq, fromAnonymous, msg, font);
                            }
                            else
                            {
                                //CoolQApi.SendPrivateMsg(fromQQ, "正在寻找资源，请稍等...");
                                List<KunyunInfo> list = KuYunSearch.Search(searchContent, CacheData.MovieConfig.ConvertLinkIndex);
                                if (list == null || list.Count == 0)
                                {
                                    //CoolQApi.SendPrivateMsg(fromQQ, "暂时未找到此资源");
                                    string temp = CacheData.MovieConfig.HaveSearchedMovieInQun;
                                    CoolQApi.SendGroupMsg(fromGroup, temp);
                                        

                                    return base.ProcessGroupMessage(subType, sendTime, fromGroup, fromQq, fromAnonymous, msg, font);
                                }
                                else if (list.Count == 1)//说明找到了具体电影的链接
                                {

                                    string replayContent = CacheData.MovieConfig.HaveSearchedMovieInQun;
                                    StringBuilder sb = new StringBuilder(" ");
                                    foreach (KunyunInfo k in list)
                                    {

                                        replayContent = replayContent.Replace("{电影名}", k.name);
                                        foreach (string res in k.url)
                                        {
                                            if (k.resourceTYpe == 1)//m3u8
                                            {
                                                sb.Append(MyLinkCoverter.CovertUrlInSuoIm(res, true) + Environment.NewLine);
                                            }
                                            else//直接观看链接
                                            {
                                                sb.Append(MyLinkCoverter.CovertUrlInSuoIm(res, false) + Environment.NewLine);
                                            }

                                        }

                                    }
                                    replayContent = replayContent.Replace("{电影链接}", sb.ToString());
                                    replayContent = replayContent.Replace("{@用户}", CoolQCode.At(fromQq));
                                    CoolQApi.SendGroupMsg(fromGroup, replayContent);
                                    return base.ProcessGroupMessage(subType, sendTime, fromGroup, fromQq, fromAnonymous, msg, font);
                                }
                                else//说明找到了相关的好几个电影
                                {
                                    StringBuilder sb = new StringBuilder("我找到了多个相关资源，请保持搜索格式，聊天回复以下具体某个资源获取观影链接：" + Environment.NewLine);
                                    foreach (KunyunInfo k in list)
                                    {
                                        sb.Append(CacheData.MovieConfig.SearchCommand + k.name + Environment.NewLine);
                                    }
                                    CoolQApi.SendGroupMsg(fromGroup, sb.ToString());
                                    return base.ProcessGroupMessage(subType, sendTime, fromGroup, fromQq, fromAnonymous, msg, font);
                                }
                            }

                        }
                        catch (Exception e2)
                        {
                            //CoolQApi.SendPrivateMsg(fromQQ, "小喵出现问题，请过会再来尝试");
                            MyLogUtil.ErrToLog("接收群聊信息后处理过程出现异常，原因：" + e2);
                            return base.ProcessGroupMessage(subType, sendTime, fromGroup, fromQq, fromAnonymous, msg, font);
                        }
                    }
                }
            }
                
            //mainForm.displayMsg2("处理群聊消息：" + subType + "," + sendTime + "," + fromGroup + "," + fromQq + "," + fromAnonymous + "," + msg + "," + font);

            return base.ProcessGroupMessage(subType, sendTime, fromGroup, fromQq, fromAnonymous, msg, font);
        }
        /// <summary>
        /// 处理群文件上传事件
        /// </summary>
        /// <param name="subType"></param>
        /// <param name="sendTime"></param>
        /// <param name="fromGroup"></param>
        /// <param name="fromQq"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public override int ProcessGroupUpload(int subType, int sendTime, long fromGroup, long fromQq, string file)
        {
            //mainForm.displayMsg2("处理群文件上传事件：" + subType + "," + sendTime + "," + fromGroup + "," + fromQq + "," + file);

            return base.ProcessGroupUpload(subType, sendTime, fromGroup, fromQq, file);
        }
        /// <summary>
        /// 处理加群请求（有加群请求）
        /// </summary>
        /// <param name="subType">请求类型。1为他人申请入群；2为自己(即登录号)受邀入群</param>
        /// <param name="sendTime">请求发送时间戳</param>
        /// <param name="fromGroup">要加入的群的群号</param>
        /// <param name="fromQq">发送此请求的QQ号码</param>
        /// <param name="msg">附言内容</param>
        /// <param name="responseMark">用于处理请求的标识</param>
        /// <returns></returns>
        public override int ProcessJoinGroupRequest(int subType, int sendTime, long fromGroup, long fromQq, string msg, string responseMark)
        {

            //mainForm.displayMsg2("处理加群请求：" + subType + "," + sendTime + "," + fromGroup + "," + fromQq + "," + msg + "," + responseMark);
            //判断此群是否在监控范围内
            MyDictionaryUtil<long, int> myDictionaryUtil = new MyDictionaryUtil<long, int>();
            int result = myDictionaryUtil.GetValue(CacheData.BaseJson.CheckedQQQun, fromGroup);
            if (result == 1)//监控此群
            {
                if (CacheData.BaseJson.IsAutoAddQun)
                {
                    //自动加群处理
                    CoolQApi.SetGroupAddRequest(responseMark, CoolQAddGroupRequestType.Normal, CoolQRequestResult.Allow);//请求通过

                }
            }


            return base.ProcessJoinGroupRequest(subType, sendTime, fromGroup, fromQq, msg, responseMark);
        }
    }
}
