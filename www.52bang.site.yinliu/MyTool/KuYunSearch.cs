using HttpCodeLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using www._52bang.site.tk.yinliu.MyModel;
using www._52bang.site.yinliu.MyModel;
using www_52bang_site_enjoy.MyModel;

namespace www_52bang_site_enjoy.MyTool
{
    public class KuYunSearch
    {
        
        /// <summary>
        /// 优先m3u8资源需要播放器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<KunyunInfo> Search(string key)
        {
            
            string detailPre = "http://kuyunzy.cc";
            if (string.IsNullOrWhiteSpace(key))
            {
                return new List<KunyunInfo>();
            }
            //如果用户搜索的结尾带结尾符，要去掉
            String keyTemp = key;
            if (keyTemp.EndsWith(CacheData.TheSameWithSearchKeySplit))
            {
                keyTemp = keyTemp.Substring(0, keyTemp.Length - CacheData.TheSameWithSearchKeySplit.Length);
            }
            List<KunyunInfo> list = new List<KunyunInfo>();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("searchword", keyTemp);
            string content = HttpPost("http://kuyunzy.cc/search.asp", dic);
            //Console.WriteLine("响应的内容" + content);
            MatchCollection mList;
            string regex = "<td height=\"20\" align=\"left\"><a href=\"(.+?)\" target=\"_blank\">(.+?)</a></td>";

            mList = Regex.Matches(content, regex);
            if (mList.Count>1)//说明匹配到多个资源，先让用户选择具体的资源，防止搜出来的是多个电视剧资源，炸屏
            {
                if (key.EndsWith(CacheData.TheSameWithSearchKeySplit))//说明用户要搜索电影名与关键字名相同的电影
                {
                    foreach(Match m in mList)
                    {
                        if ((m.Groups[2].ToString() + CacheData.TheSameWithSearchKeySplit).Equals(key)){
                            string url = detailPre + m.Groups[1].ToString().Trim();
                            string name = m.Groups[2].ToString();
                            //获取链接
                            string strRes = HttpGet(url, "");
                            //<h1>来源:kkm3u8</h1> //存在就不再获取
                            MatchCollection match = Regex.Matches(strRes, "<h1>来源:kkm3u8</h1>[\\s\\S]+?checked />全选");
                            if (match.Count > 0)
                            {
                                //Console.WriteLine("匹配后：" + match[0].ToString());
                                string kkm3u8 = match[0].ToString();
                                //解析串
                                mList = Regex.Matches(kkm3u8, "<input type='checkbox' name='copy_yah' id='copy_yah' value='(.+?)'  checked/> <a>(.+?)\\$");
                                List<MovieInfo2> urlList = new List<MovieInfo2>();

                                foreach (Match m3u8Math in mList)
                                {

                                    urlList.Add(new MovieInfo2(m3u8Math.Groups[2].ToString(), m3u8Math.Groups[1].ToString()));
                                }
                                list.Add(new KunyunInfo(name, urlList, 1));

                            }
                            else
                            {
                                //<h1>来源:kkyun</h1>,[\\s\\S]+? 包含换行符的匹配
                                match = Regex.Matches(strRes, "<h1>来源:kkyun</h1>[\\s\\S]+?checked />全选");
                                if (match.Count > 0)
                                {
                                    string kkyun = match[0].ToString();
                                    //解析串，取第一个就行链接
                                    mList = Regex.Matches(kkyun, "<input type='checkbox' name='copy_yah' id='copy_yah' value='(.+?)'  checked/> <a>(.+?)\\$");
                                    List<MovieInfo2> urlList = new List<MovieInfo2>();

                                    foreach (Match m3u8Math in mList)
                                    {
                                        
                                        urlList.Add(new MovieInfo2(m3u8Math.Groups[2].ToString(), m3u8Math.Groups[1].ToString()));
                                    }
                                    list.Add(new KunyunInfo(name, urlList, 2));
                                }
                                else
                                {
                                    //如果该资源没有链接，则忽略
                                    continue;
                                }
                            }

                            return list;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }


                foreach (Match m in mList)
                {
                    string name = m.Groups[2].ToString();
                    if (name.Equals(key))//搜索的电影名和资源重名，需要给资源加个
                    {
                        name = name + CacheData.TheSameWithSearchKeySplit;
                    }
                    list.Add(new KunyunInfo(name, null, 2));
                }
            }
            else if(mList.Count == 1){
                foreach (Match m in mList)
                {
                    string url = detailPre + m.Groups[1].ToString().Trim();
                    string name = m.Groups[2].ToString();
                    //获取链接
                    string strRes = HttpGet(url, "");
                    //<h1>来源:kkm3u8</h1> //存在就不再获取
                    MatchCollection match = Regex.Matches(strRes, "<h1>来源:kkm3u8</h1>[\\s\\S]+?checked />全选");
                    if (match.Count > 0)
                    {
                        //Console.WriteLine("匹配后：" + match[0].ToString());
                        string kkm3u8 = match[0].ToString();
                        //解析串，取第一个就行链接
                        mList = Regex.Matches(kkm3u8, "<input type='checkbox' name='copy_yah' id='copy_yah' value='(.+?)'  checked/> <a>(.+?)\\$");
                        List<MovieInfo2> urlList = new List<MovieInfo2>();
                       
                        foreach (Match m3u8Math in mList)
                        {
                            urlList.Add(new MovieInfo2(m3u8Math.Groups[2].ToString(), m3u8Math.Groups[1].ToString()));
                        }
                        list.Add(new KunyunInfo(name, urlList, 1));

                    }
                    else
                    {
                        //<h1>来源:kkyun</h1>,[\\s\\S]+? 包含换行符的匹配
                        match = Regex.Matches(strRes, "<h1>来源:kkyun</h1>[\\s\\S]+?checked />全选");
                        if (match.Count > 0)
                        {
                            string kkyun = match[0].ToString();
                            //解析串，取第一个就行链接
                            mList = Regex.Matches(kkyun, "<input type='checkbox' name='copy_yah' id='copy_yah' value='(.+?)'  checked/> <a>(.+?)\\$");
                            List<MovieInfo2> urlList = new List<MovieInfo2>();

                            foreach (Match m3u8Math in mList)
                            {
                                
                                urlList.Add(new MovieInfo2(m3u8Math.Groups[2].ToString(), m3u8Math.Groups[1].ToString()));
                            }
                            list.Add(new KunyunInfo(name, urlList, 2));
                        }
                        else
                        {
                            //如果该资源没有链接，则忽略
                            continue;
                        }
                    }
                }
            }

            
            
            return list;
        }

        /// <summary>
        /// 优先酷云在线资源
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<KunyunInfo> Search2(string key)
        {
            string detailPre = "http://kuyunzy.cc";
            if (string.IsNullOrWhiteSpace(key))
            {
                return new List<KunyunInfo>();
            }
            //如果用户搜索的结尾带结尾符，要去掉
            String keyTemp = key;
            if (keyTemp.EndsWith(CacheData.TheSameWithSearchKeySplit))
            {
                keyTemp = keyTemp.Substring(0, keyTemp.Length - CacheData.TheSameWithSearchKeySplit.Length);
            }
            List<KunyunInfo> list = new List<KunyunInfo>();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("searchword", keyTemp);
            string content = HttpPost("http://kuyunzy.cc/search.asp", dic);
            //Console.WriteLine("响应的内容" + content);
            MatchCollection mList;
            string regex = "<td height=\"20\" align=\"left\"><a href=\"(.+?)\" target=\"_blank\">(.+?)</a></td>";

            mList = Regex.Matches(content, regex);
            if (mList.Count > 1)//说明匹配到多个资源，先让用户选择具体的资源，防止搜出来的是多个电视剧资源，炸屏
            {

                if (key.EndsWith(CacheData.TheSameWithSearchKeySplit))//说明用户要搜索电影名与关键字名相同的电影
                {
                    foreach (Match m in mList)
                    {
                        if ((m.Groups[2].ToString() + CacheData.TheSameWithSearchKeySplit).Equals(key))
                        {
                            string url = detailPre + m.Groups[1].ToString().Trim();
                            string name = m.Groups[2].ToString();
                            //获取链接
                            string strRes = HttpGet(url, "");
                            //<h1>来源:kkyun</h1>,[\\s\\S]+? 包含换行符的匹配
                            MatchCollection match = Regex.Matches(strRes, "<h1>来源:kkyun</h1>[\\s\\S]+?checked />全选");
                            if (match.Count > 0)
                            {
                                //Console.WriteLine("匹配后：" + match[0].ToString());
                                string kkm3u8 = match[0].ToString();
                                //解析串，取第一个就行链接
                                mList = Regex.Matches(kkm3u8, "<input type='checkbox' name='copy_yah' id='copy_yah' value='(.+?)'  checked/> <a>(.+?)\\$");
                                List<MovieInfo2> urlList = new List<MovieInfo2>();

                                foreach (Match m3u8Math in mList)
                                {
                                    //urlList.Add(m3u8Math.Groups[1].ToString());
                                    urlList.Add(new MovieInfo2(m3u8Math.Groups[2].ToString(), m3u8Math.Groups[1].ToString()));

                                }
                                list.Add(new KunyunInfo(name, urlList, 2));

                            }
                            else
                            {
                                //<h1>来源:kkm3u8</h1> //存在就不再获取
                                match = Regex.Matches(strRes, "<h1>来源:kkm3u8</h1>[\\s\\S]+?checked />全选");
                                if (match.Count > 0)
                                {
                                    string kkyun = match[0].ToString();
                                    //解析串，取第一个就行链接
                                    mList = Regex.Matches(kkyun, "<input type='checkbox' name='copy_yah' id='copy_yah' value='(.+?)'  checked/> <a>(.+?)\\$");
                                    List<MovieInfo2> urlList = new List<MovieInfo2>();

                                    foreach (Match m3u8Math in mList)
                                    {
                                        //urlList.Add(m3u8Math.Groups[1].ToString());
                                        urlList.Add(new MovieInfo2(m3u8Math.Groups[2].ToString(), m3u8Math.Groups[1].ToString()));

                                    }
                                    list.Add(new KunyunInfo(name, urlList, 1));
                                }
                                else
                                {
                                    //如果该资源没有链接，则忽略
                                    continue;
                                }
                            }
                            return list;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }

                foreach (Match m in mList)
                {
                    string name = m.Groups[2].ToString();
                    if (name.Equals(key))//搜索的电影名和资源重名，需要给资源加个
                    {
                        name = name + CacheData.TheSameWithSearchKeySplit;
                    }
                    list.Add(new KunyunInfo(name, null, 2));
                }
            }
            else if (mList.Count == 1)
            {
                foreach (Match m in mList)
                {
                    string url = detailPre + m.Groups[1].ToString().Trim();
                    string name = m.Groups[2].ToString();
                    //获取链接
                    string strRes = HttpGet(url, "");
                    //<h1>来源:kkyun</h1>,[\\s\\S]+? 包含换行符的匹配
                    MatchCollection match = Regex.Matches(strRes, "<h1>来源:kkyun</h1>[\\s\\S]+?checked />全选");
                    if (match.Count > 0)
                    {
                        //Console.WriteLine("匹配后：" + match[0].ToString());
                        string kkm3u8 = match[0].ToString();
                        //Console.WriteLine("匹配到的字符串："+ kkm3u8);
                        //解析串，取第一个就行链接
                        mList = Regex.Matches(kkm3u8, "<input type='checkbox' name='copy_yah' id='copy_yah' value='(.+?)'  checked/> <a>(.+?)\\$"); 
                        List<MovieInfo2> urlList = new List<MovieInfo2>();

                        foreach (Match m3u8Math in mList)
                        {
                            //urlList.Add(m3u8Math.Groups[1].ToString());
                            urlList.Add(new MovieInfo2(m3u8Math.Groups[2].ToString(), m3u8Math.Groups[1].ToString()));
                        }
                        list.Add(new KunyunInfo(name, urlList, 2));

                    }
                    else
                    {
                        //<h1>来源:kkm3u8</h1> //存在就不再获取
                        match = Regex.Matches(strRes, "<h1>来源:kkm3u8</h1>[\\s\\S]+?checked />全选"); 
                        if (match.Count > 0)
                        {
                            string kkyun = match[0].ToString();
                            //解析串，取第一个就行链接
                            mList = Regex.Matches(kkyun, "<input type='checkbox' name='copy_yah' id='copy_yah' value='(.+?)'  checked/> <a>(.+?)\\$");
                            List<MovieInfo2> urlList = new List<MovieInfo2>();

                            foreach (Match m3u8Math in mList)
                            {
                                //urlList.Add(m3u8Math.Groups[1].ToString());
                                urlList.Add(new MovieInfo2(m3u8Math.Groups[2].ToString(), m3u8Math.Groups[1].ToString()));
                            }
                            list.Add(new KunyunInfo(name, urlList, 1));
                        }
                        else
                        {
                            //如果该资源没有链接，则忽略
                            continue;
                        }
                    }
                }
            }



            return list;
        }

        public static List<KunyunInfo> Search(string key,int index)
        {
            if (index == 0)
            {
                return Search(key);//优先使用酷云的m3u8资源，利用我的解析器观看
                
            }
            else if(index == 1)
            {
                return Search2(key);//优先使用酷云的在线web资源
            }
            else
            {
                return new List<KunyunInfo>();
            }
        }

        /// <summary>  
        /// 指定Post地址使用Get 方式获取全部字符串  
        /// </summary>  
        /// <param name="url">请求后台地址</param>  
        /// <returns></returns>  
        public static string HttpPost(string url, Dictionary<string, string> dic)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            #region 添加Post 参数  
            StringBuilder builder = new StringBuilder();
            int i = 0;
            foreach (var item in dic)
            {
                if (i > 0)
                    builder.Append("&");
                builder.AppendFormat("{0}={1}", item.Key, item.Value);
                i++;
            }
            
            byte[] data = Encoding.GetEncoding("gb2312").GetBytes(builder.ToString());
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            #endregion
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取响应内容  
            using (StreamReader reader = new StreamReader(stream, Encoding.GetEncoding("gb2312")))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }

        public static string HttpGet(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (string.IsNullOrWhiteSpace(postDataStr) ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=gb2312";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("gb2312"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

    }
}
