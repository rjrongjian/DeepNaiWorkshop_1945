using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace www._52bang.site.tk.yinliu.MyModel.rot
{
    public class MovieConfig
    {
        public Dictionary<long, int> CheckedQQQun;//被选中的QQ群
        public bool IsNeed;//是否需要在群里@机器人
        public string SearchCommand;//搜索命令
        public string NoSearchedMovieInQun;//未找到电影，机器人在群中的回复
        public string NoSearchedMovie;//未找到电影，机器人私聊的回复
        public string HaveSearchedMovie;//已找到电影时，机器人私聊给与的回复
        public string HaveSearchedMovieInQun;//已找到电影时，机器人在群里给与的回复
        public bool IsCanPrivateMessage;//是否可以私聊用户
        public int ConvertLinkIndex;//选择的资源类别
        public int SelectedParserIndex;//
    }
}
