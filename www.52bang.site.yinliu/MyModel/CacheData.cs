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

        static CacheData()
        {
            linkList = new List<ComboxInfo>();
            linkList.Add(new ComboxInfo { index = 0, moduleName = "m3u8资源" });//优先使用酷云的m3u8资源，利用我的解析器观看
            linkList.Add(new ComboxInfo { index = 1, moduleName = "web资源" });//优先使用酷云的在线web资源


            resourceApiList = new List<ResourceApiInfo>();
            resourceApiList.Add(new ResourceApiInfo { index = 0, moduleName = "解析器1", apiUrl= "http://jx.618g.com/?url=" });//解析器1
            resourceApiList.Add(new ResourceApiInfo { index = 0, moduleName = "解析器2", apiUrl = "http://www.52bang.site/dyxf/parse.html?url=" });//解析器2

        }
    }
}
