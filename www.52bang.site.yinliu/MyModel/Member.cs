using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace www_52bang_site_enjoy.MyModel
{
    class Member
    {
        public long Time { set; get; }//会员过期时间戳 0 代表不是会员
        public string DateDesp { set; get; }//会员到期格式化后的数据
        public int Type { set; get; }//1不是会员 2 会员过期 3 正常会员
    }
}
