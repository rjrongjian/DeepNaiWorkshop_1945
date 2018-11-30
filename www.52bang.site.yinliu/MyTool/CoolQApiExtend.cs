using FileCreator.MyTool;
using HttpCodeLib;
using Newbe.CQP.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using www_52bang_site_enjoy.MyModel;

namespace www_52bang_site_enjoy.MyTool
{
    public class CoolQApiExtend
    {
        /// <summary>
        /// 解析群列表
        /// </summary>
        /// <param name="groupInfo">密文</param>
        /// <returns></returns>
        public static List<GroupInfo> ParseGroupList(string groupInfo)
        {
            MyLogUtil.ToLog("获取的群列表："+ groupInfo);
            byte[] bt = Convert.FromBase64String(groupInfo);
            int weizhi = 0;
            double groups = 0;
            string groupname3 = "";
            List<GroupInfo> groupInfoList = new List<GroupInfo>();
            for (int i = 0; i < 4; i++)
            {
                groups += bt[i] * Math.Pow(256, 3 - i);
                weizhi++;
            }
            for (int j = 0; j < Convert.ToInt32(groups); j++)
            {
                double chang = 0;
                for (int i = 4; i < 6; i++)
                {
                    chang += bt[weizhi] * Math.Pow(256, 5 - i);
                    weizhi++;
                }
                double qunhao = 0;
                for (int i = 6; i < 14; i++)
                {
                    qunhao += bt[weizhi] * Math.Pow(256, 13 - i);
                    weizhi++;
                }
                long groupId = Convert.ToInt64(qunhao.ToString());
                MyLogUtil.ToLog("群号：" + qunhao.ToString());
                weizhi++;
                weizhi++;
                int namechang = Convert.ToInt32(chang) - 10;
                List<byte> listname = new List<byte>();
                for (int i = 0; i < namechang; i++)
                {
                    listname.Add(bt[weizhi]);
                    weizhi++;
                }
                byte[] byt = listname.ToArray();
                groupname3 = Encoding.Default.GetString(byt);
                MyLogUtil.ToLog("群名：" + groupname3.ToString());
                groupInfoList.Add(new GroupInfo(groupname3,groupId,0));
            }

            return groupInfoList;

        }
        
        public static List<GroupInfo> GetGroupList(ICoolQApi api)
        {
            string content = api.CQ_getGroupList();
            MyLogUtil.ErrToLog("获取的群信息："+content);
            return ParseGroupList(content);
        }


        public static List<GroupInfo> GetGroupList2(ICoolQApi api)
        {
            List<GroupInfo> list = new List<GroupInfo>();
            try
            {
                //CQLogger.GetInstance().AddLog(string.Format("[↓][帐号] 取群列表", new object[0]));
                string url = "http://qun.qq.com/cgi-bin/qun_mgr/get_group_list";
                var postData = new Dictionary<string, string>();
                XJHTTP xJHTTP = new XJHTTP();
                CookieContainer cookieContainer = xJHTTP.StringToCookie("qun.qq.com",api.GetCookies());


                postData.Add("bkn", api.GetCsrfToken().ToString());
                HttpResults httpResults = xJHTTP.PostHtml(url, "http://qun.qq.com/member.html", "bkn=" + api.GetCsrfToken().ToString(), false, cookieContainer, 15000);

                string sourceString = httpResults.Html;
                MyLogUtil.ToLog(httpResults.Html);
                var strReg = "{\"gc\":([1-9][0-9]{4,10}),\"gn\":\"(.*?)\",\"owner\":([1-9][0-9]{4,10})}";
                Regex reg = new Regex(strReg);
                MatchCollection matches = reg.Matches(sourceString);
                MyJsonUtil<GroupInfo2> myJsonUtil = new MyJsonUtil<GroupInfo2>();
                foreach (Match match in matches)
                {
                    GroupInfo2 g = myJsonUtil.parseJsonStr(match.Value);
                    list.Add(new GroupInfo(g.gn, g.gc, g.owner));
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                MyLogUtil.ErrToLog("获取群列表出现错误，原因：" + ex);
            }
            return list;
        }

        public static List<CQGroupMemberInfo> GetGroupMemberList(long groupNumber, ICoolQApi api)
        {
            List<CQGroupMemberInfo> list = new List<CQGroupMemberInfo>();
            
            try
            {
                String gc = groupNumber.ToString();
                String bkn = api.GetCsrfToken().ToString();
                string url = "http://qun.qq.com/cgi-bin/qun_mgr/search_group_members";
                String cookie = api.GetCookies();
                var postData = new Dictionary<string, string>();
                postData.Add("gc", gc);
                postData.Add("st", "0");
                postData.Add("end", "20");
                postData.Add("sort", "0");
                postData.Add("bkn", bkn);
                
                HttpResults httpResults = new HttpResults();
                HttpHelpers httpHelpers = new HttpHelpers();
                httpResults = httpHelpers.GetHtml(new HttpItems
                {
                    URL = url,
                    UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:17.0) Gecko/20100101 Firefox/17.0",
                    Cookie = cookie,
                    Allowautoredirect = true,
                    Referer= "http://qun.qq.com/member.html",
                    Method = "post",
                    Postdata = ("gc="+ gc+ "&st=0&end=20&sort=0&bkn="+ bkn),
                    ContentType = "application/x-www-form-urlencoded"
                });
                //string sourceString = HttpPost(url, postData, "http://qun.qq.com/member.html", api.GetCookies());
                String sourceString = httpResults.Html;
                MyLogUtil.ToLog("获取的结果："+sourceString);
                MyLogUtil.ToLog("获取的gc："+ gc);
                MyLogUtil.ToLog("获取的bkn:" + bkn);
                MyLogUtil.ToLog("获取的cookie:" + cookie);

                var memberPattern = @"\{""card"":""(.*?)"",.*?,""uin"":\d+\}";
                Regex regex = new Regex(memberPattern);
                MatchCollection matches = regex.Matches(sourceString);

                string qqPattern = @"(?<=""uin"":)\d+";
                string namePattern = "(?<=\"nick\":\").*?(?=\",)";
                string rolePatern = @"(?<=""role"":)\d+";
                foreach (Match match in matches)
                {
                    CQGroupMemberInfo item = new CQGroupMemberInfo
                    {
                        GroupNumber = groupNumber,
                        QQNumber = Convert.ToInt64(Regex.Match(match.Value, qqPattern).Value),
                        QQName = Regex.Match(match.Value, namePattern).Value
                    };
                    string role = Regex.Match(match.Value, rolePatern).Value;
                    item.Authority = (role.Equals("0")) ? "群主" : ((role.Equals("1")) ? "管理" : "成员");
                    list.Add(item);
                }
                //log.DebugFormat("{0} GetGroupMemberList -> {1}", groupNumber, JsonConvert.SerializeObject(list));
            }
            catch (Exception ex)
            {
                //log.ErrorFormat("获得群成员{0}出错：{1}", groupNumber, ex.Message);
                MyLogUtil.ToLog("抛了异常:" +ex);
                MessageBox.Show("抛了异常");
            }
            
            return list;
        }


        public class GroupInfo2
        {
            /// <summary>
            /// Gc
            /// </summary>
            public long gc { get; set; }
            /// <summary>
            /// 酷Q群管插件
            /// </summary>
            public string gn { get; set; }
            /// <summary>
            /// Owner
            /// </summary>
            public long owner { get; set; }
        }

        public class CQGroupMemberInfo
        {
            public String Authority { get; set; }
            public long GroupNumber { get; set; }
            public long QQNumber { get; set; }
            public String QQName { get; set; }
        }
    }
}
