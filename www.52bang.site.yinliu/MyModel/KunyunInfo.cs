﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using www._52bang.site.yinliu.MyModel;

namespace www_52bang_site_enjoy.MyModel
{
    public class KunyunInfo
    {
        public string name;
        public List<MovieInfo2> url;
        public int resourceTYpe;//1 m3u8资源 2 直接观看

        public KunyunInfo(string name,List<MovieInfo2> url,int resourceTYpe)
        {
            this.name = name;
            this.url = url;
            this.resourceTYpe = resourceTYpe;
        }
    }
}
