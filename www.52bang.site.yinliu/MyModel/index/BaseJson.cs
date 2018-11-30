using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace www._52bang.site.tk.yinliu.MyModel.index
{
    public class BaseJson
    {
        public Dictionary<string, string> Keys = new Dictionary<string, string>();//关键词 关键字-##-1-##-1  关键字-##-此关键字在群聊中生效-##-此关键字在私聊中生效
        public Dictionary<long,int> CheckedQQQun = new Dictionary<long, int>();//被选中的QQ群
        public string NewerEnterQun;//新人入群欢迎词
        public bool IsAutoAddQun = true;//是否允许用户自动加群
        public bool IsAutoAddFriend = true;//是否允许用户自动加好友

    }
}
