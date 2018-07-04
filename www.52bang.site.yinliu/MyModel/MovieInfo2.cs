using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace www._52bang.site.yinliu.MyModel
{
    public class MovieInfo2
    {
        public string MovieName;
        public string Url;//url encode后的结果

        public MovieInfo2(String movieName,String url)
        {
            this.MovieName = movieName;
            this.Url = url;
        }
    }
}
