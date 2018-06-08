using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace www_52bang_site_enjoy.MyModel
{
    class SystemConfigJson
    {
        public String Command;//会员常见命令
        public String ResourceUrl;//付费资源的url，目前为 电影信封 微信公众号的页面
        public Dictionary<string, ResourceInfo> ResourceKl;//资源汇总
    }
}
