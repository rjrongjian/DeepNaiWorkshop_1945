using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace www_52bang_site_enjoy.MyModel
{
    public class MovieInfo
    {
        public string MovieName;
        public string Url;//url encode后的结果
        public string OriginalUrl;//未encode的链接
        public MovieInfo(string movieName,string url,string originalUrl)
        {
            this.MovieName = movieName;
            this.Url = url;
            this.OriginalUrl = originalUrl;
        }
    }
}
