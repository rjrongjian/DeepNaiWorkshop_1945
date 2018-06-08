using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace www_52bang_site_enjoy.MyModel
{
    public class MyResponse<T>
    {
        public int Code;//0 成功 1 失败
        public T Msg;//提示信息

        public MyResponse(int code,T msg)
        {
            Code = code;
            Msg = msg;
        }
        
    }
}
