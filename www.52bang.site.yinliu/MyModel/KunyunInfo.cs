using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace www_52bang_site_enjoy.MyModel
{
    public class KunyunInfo
    {
        public string name;
        public List<string> url;
        public int resourceTYpe;//1 m3u8资源 2 直接观看

        public KunyunInfo(string name,List<string> url,int resourceTYpe)
        {
            this.name = name;
            this.url = url;
            this.resourceTYpe = resourceTYpe;
        }
    }
}
