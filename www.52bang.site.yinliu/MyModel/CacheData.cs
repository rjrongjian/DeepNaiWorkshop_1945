using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using www._52bang.site.tk.yinliu.MyModel.index;
using www._52bang.site.tk.yinliu.MyModel.rot;
using www._52bang.site.yinliu.MyModel;
using www_52bang_site_enjoy.MyModel;

namespace www._52bang.site.tk.yinliu.MyModel
{
    public class CacheData
    {
        public static BaseJson BaseJson;
        public static MovieConfig MovieConfig;
        internal static List<GroupInfo> GroupList;
        public static bool IsRunMovieRot = false;

        public static List<ComboxInfo> linkList;//选择的资源转链方式
        public static List<ResourceApiInfo> resourceApiList;//既能转链m3u8,又能转链影视vip,pc和手机都能看
        public static String expire;
        public static String NotifyInfo;//通知信息
        public static String TheSameWithSearchKeySplit = "#"; //搜索内容和内容重名时，通过判断内容中包含此内容确定就是搜此资源

        static CacheData()
        {
            linkList = new List<ComboxInfo>();
            linkList.Add(new ComboxInfo { index = 0, moduleName = "m3u8资源" });//优先使用酷云的m3u8资源，利用我的解析器观看
            linkList.Add(new ComboxInfo { index = 1, moduleName = "web资源" });//优先使用酷云的在线web资源


            resourceApiList = new List<ResourceApiInfo>();
            //resourceApiList.Add(new ResourceApiInfo { index = 0, moduleName = "最大解析", apiUrlForHttpsResource = "http://zuidajiexi.net/m3u8.html?url=", apiUrlForHttpResource = "http://zuidajiexi.net/m3u8.html?url=" });//解析器1 无广告，群里不能看
            //resourceApiList.Add(new ResourceApiInfo { index = 1, moduleName = "急速解析", apiUrlForHttpsResource = "https://api.rzhyi.com/index.php?url=", apiUrlForHttpResource = "http://jx.rzhyi.com/index.php?url=" });//解析器1 有广告
            //resourceApiList.Add(new ResourceApiInfo { index = 1, moduleName = "酷云解析", apiUrlForHttpsResource = "https://www.bihaijx.com/kkm3u8.php?url=", apiUrlForHttpResource = "http://www.bihaijx.com/kkm3u8.php?url=" });//解析器1 有广告
            //resourceApiList.Add(new ResourceApiInfo { index = 1, moduleName = "000o", apiUrlForHttpsResource = "http://000o.cc/jx/ty.php?url=", apiUrlForHttpResource = "http://000o.cc/jx/ty.php?url=" });//解析器1 有广告
            //resourceApiList.Add(new ResourceApiInfo { index = 1, moduleName = "佛系解析", apiUrlForHttpsResource = "http://api.vsvc.cc/index.php?url=", apiUrlForHttpResource = "http://api.vsvc.cc/index.php?url=" });//解析器1 有广告
            //resourceApiList.Add(new ResourceApiInfo { index = 1, moduleName = "517解析", apiUrlForHttpsResource = "http://cn.bjbanshan.cn/jiexi.php?url=", apiUrlForHttpResource = "http://cn.bjbanshan.cn/jiexi.php?url=" });//解析器1 有广告
            //resourceApiList.Add(new ResourceApiInfo { index = 1, moduleName = "冰柠解析", apiUrlForHttpsResource = "https://www.jisyi.com/?url=", apiUrlForHttpResource = "https://www.jisyi.com/?url=" });//解析器1 有广告
            //resourceApiList.Add(new ResourceApiInfo { index = 1, moduleName = "馍片解析", apiUrlForHttpsResource = "http://jx.mopian.cc/?url=", apiUrlForHttpResource = "http://jx.mopian.cc/?url=" });//解析器1 有广告
            //resourceApiList.Add(new ResourceApiInfo { index = 1, moduleName = "优优解析", apiUrlForHttpsResource = "http://api.uuyingshi.com/?url=", apiUrlForHttpResource = "http://api.uuyingshi.com/?url=" });//解析器1 有广告
            //resourceApiList.Add(new ResourceApiInfo { index = 1, moduleName = "贼牛解析", apiUrlForHttpsResource = "http://api.zeiniu.top/?url=", apiUrlForHttpResource = "http://api.zeiniu.top/?url=" });//解析器1 有广告
            //resourceApiList.Add(new ResourceApiInfo { index = 1, moduleName = "百玉阁解析", apiUrlForHttpsResource = "http://api.baiyug.top/?url=", apiUrlForHttpResource = "http://api.baiyug.top/?url=" });//解析器1 有广告

            //备用解析接口
            // 
            //  
            // 
            // 
            //冰柠云解析 


        }
    }
}
