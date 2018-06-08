using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using www._52bang.site.tk.yinliu.MyModel;
using www_52bang_site_enjoy.MyModel;

namespace www_52bang_site_enjoy.MyTool
{
    /// <summary>
    /// 各大影视平台的链接转换器
    /// </summary>
    public class MyLinkCoverter
    {
        
        /// <summary>
        /// 转长链接
        /// </summary>
        /// <param name="cqMsg">返回的消息[CQ: rich, url=https://v.youku.com/v_show/id_XMzU5NDQzNzIxNg==.html?sharefrom=iphone,text=极限挑战 第四季20180510 会员解读版第2期:演技滑铁卢!内含男人帮最想删的片段 ]</param>
        /// <rturns></returns>
        public static MovieInfo Covert(string cqMsg)
        {
            //解析酷q消息体中的url链接
            MovieInfo movie = parseMsg(cqMsg);
            movie.OriginalUrl = CacheData.resourceApiList[CacheData.MovieConfig.SelectedParserIndex].apiUrl + movie.Url;
            //组成可观看的视频地址
            movie.Url = MyUrlTool.UrlEncode(CacheData.resourceApiList[CacheData.MovieConfig.SelectedParserIndex].apiUrl + movie.Url);
            
            
            return movie;
        }
        /// <summary>
        /// [CQ:share,url=http://www.iqiyi.com/v_19rrel12t4.html#vfrm=19-9-0-1,title=21克拉,content=《21克拉》是由何念导演，郭京飞、迪丽热巴、大鹏、包贝尔主演的电影,image=http://pic2.qiyipic.com/image/20180517/0f/26/v_114103078_m_601_m4.jpg]
        /// 分享的链接都会被转换成这种格式（所以目前普通网页也会触发转地址）
        /// [CQ: rich, url=https://v.youku.com/v_show/id_XMzU5NDQzNzIxNg==.html?sharefrom=iphone,text=极限挑战 第四季20180510 会员解读版第2期:演技滑铁卢!内含男人帮最想删的片段 ]
        /// </summary>
        /// <param name="cqMsg"></param>
        /// <returns></returns>
        public static MovieInfo parseMsg(string cqMsg)
        {
            if((cqMsg.Contains("CQ: rich") || cqMsg.Contains("CQ:rich")))
            {
                string movieName = cqMsg.Substring(cqMsg.IndexOf(",text="), cqMsg.IndexOf("]") - cqMsg.IndexOf(",text="));
                movieName = movieName.Replace(",text=", "");
                string url = cqMsg.Substring(cqMsg.IndexOf("url="), cqMsg.IndexOf(",text=") - cqMsg.IndexOf("url="));
                url = url.Replace("url=", "");
                MovieInfo movieInfo = new MovieInfo(movieName, url, url);
                return movieInfo;
            }
            else
            {
                string movieName = cqMsg.Substring(cqMsg.IndexOf(",title="), cqMsg.IndexOf(",content=") - cqMsg.IndexOf(",title="));
                movieName = movieName.Replace(",title=","");
                string url = cqMsg.Substring(cqMsg.IndexOf("url="), cqMsg.IndexOf(",title=") - cqMsg.IndexOf("url="));
                url = url.Replace("url=", "");
                MovieInfo movieInfo = new MovieInfo(movieName, url, url);
                return movieInfo;
            }
            
        }

        /// <summary>
        /// 判断用户发过来的信息是否是分享的视频的消息体
        /// </summary>
        /// <param name="cqMsg">返回的消息[CQ: rich, url=https://v.youku.com/v_show/id_XMzU5NDQzNzIxNg==.html?sharefrom=iphone,text=极限挑战 第四季20180510 会员解读版第2期:演技滑铁卢!内含男人帮最想删的片段 ]</param>
        /// 分享的链接都会被转换成这种格式
        /// [CQ:share,url=http://www.iqiyi.com/v_19rrel12t4.html#vfrm=19-9-0-1,title=21克拉,content=《21克拉》是由何念导演，郭京飞、迪丽热巴、大鹏、包贝尔主演的电影,image=http://pic2.qiyipic.com/image/20180517/0f/26/v_114103078_m_601_m4.jpg]
        /// <rturns></returns>
        public static bool JugePlatform(string cqMsg)
        {
            if((cqMsg.Contains("CQ: rich") || cqMsg.Contains("CQ:rich"))&&cqMsg.Contains("url=")&&cqMsg.Contains("text="))
            {
                return true;
            }
            else if((cqMsg.Contains("CQ:share") || cqMsg.Contains("CQ: share"))&& cqMsg.Contains("url=") && cqMsg.Contains("title=") && cqMsg.Contains("content=") && cqMsg.Contains("image="))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 例用suo.im转短链接
        /// </summary>
        /// <param name="cqMsg"></param>
        /// <returns></returns>
        public static MyResponse<MovieInfo> CovertInSuoIm(string cqMsg)
        {
            //不做是否是支持的转换平台
            /*
            bool isContain = false;
            foreach(string canParsePlatform in SystemConfig.CanParsePlatform)
            {
                if (cqMsg.Contains(canParsePlatform))
                {
                    isContain = true;
                    break;
                }
            }

            if (!isContain)
            {

                return new MyResponse<MovieInfo>(1,null);
            }
            */

            MovieInfo movieInfo = Covert(cqMsg);
            //转成短网址，例用suo.im
            movieInfo.Url = MyUrlTool.HttpGet("http://suo.im/api.php?url=" + movieInfo.Url, "");
            

            MyResponse<MovieInfo> myResponse = new MyResponse<MovieInfo>(0, movieInfo);
            return myResponse;
        }
        /// <summary>
        /// 将长连接转为短链接
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string CovertUrlInSuoIm(string url)
        {
            //转成短网址，例用suo.im
            string result = MyUrlTool.HttpGet("http://suo.im/api.php?url=" + url, "");
            return result;
        }

        public static MyResponse<string> ParsePlatform(string msg)
        {
            string shareUrl = msg;
            if (!string.IsNullOrWhiteSpace(shareUrl) && (shareUrl.Contains("http") || shareUrl.Contains("https")))
            {
                if (!(shareUrl.StartsWith("http://") || shareUrl.StartsWith("https://")))
                {
                    if (shareUrl.Contains("http://"))
                    {
                        shareUrl = shareUrl.Substring(shareUrl.IndexOf("http://"));
                        
                    }
                    else
                    {
                        shareUrl = shareUrl.Substring(shareUrl.IndexOf("https://"));
                    }
                }
                //去掉链接末尾的汉字
                shareUrl = Regex.Replace(shareUrl, @"[\u4e00-\u9fa5]", ""); //去除汉字
                return new MyResponse<string>(0,shareUrl);
            }
            else
            {
                return new MyResponse<string>(1, "");
            }
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="v">true 需要拼接视频接口</param>
        /// <returns></returns>
        public static string CovertUrlInSuoIm(string url, bool v)
        {
            string urlTemp = "";
            if (v)
            {
                urlTemp = CacheData.resourceApiList[CacheData.MovieConfig.SelectedParserIndex].apiUrl + MyUrlTool.UrlEncode( url);
                //MyLogUtil.ToLog("看看地址：" + urlTemp);
                return CovertUrlInSuoIm(urlTemp);
            }
            else//直接转短链
            {
                return CovertUrlInSuoIm(url);
            }
        }
    }
}
