using DeepNaiWorkshop_6001.MyTool;
using FileCreator.MyTool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using www._52bang.site.tk.yinliu.MyModel.index;
using www._52bang.site.tk.yinliu.MyModel.rot;
using www_52bang_site_enjoy.MyModel;

namespace www_52bang_site_enjoy.MyTool
{
    class MySystemUtil
    {
        /// <summary>  
        /// 获取当前执行的dll的目录 例如： D:\dir\dir\
        /// </summary>  
        /// <returns></returns>  
        public static string GetPath()
        {
            string str = Assembly.GetExecutingAssembly().CodeBase;
            int start = 8;// 去除file:///  
            int end = str.LastIndexOf('/');// 去除文件名xxx.dll及文件名前的/  
            str = str.Substring(start, end - start);
            str = str + "/";
            str = Path.GetDirectoryName(str) + "\\";
            return str;
           
        }
        /// <summary>
        /// 获取插件dll根目录，用于存放此dll应用信息
        /// </summary>
        /// <returns></returns>
        public static string GetDllRoot()
        {
            string dirPath = GetPath()+@"local\";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            return dirPath;
        }
        /// <summary>
        /// 获取本地qq对话存储路径,每天一个
        /// </summary>
        /// <returns></returns>
        public static string GetQQDialogueDir()
        {
            string dirPath = GetDllRoot()+@"QQDialogueLog\";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            dirPath = dirPath + DateTime.Now.ToString("yyyy-MM-dd") + @"\";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            return dirPath;

        }
        /// <summary>
        /// 获取会员文件存放目录
        /// </summary>
        /// <returns></returns>
        public static string GetMemberPath()
        {
            string dirPath = GetDllRoot() + @"Member\";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            return dirPath;
        }

        /// <summary>
        /// 获取本地配置文件
        /// </summary>
        /// <returns></returns>
        public static SystemConfigJson GetSystemConfigJson()
        {
            try
            {
                if (!File.Exists(GetSystemConfigJsonPath()))
                {
                    return null;
                }
                else
                {
                    MyJsonUtil<SystemConfigJson> myJsonUtil = new MyJsonUtil<SystemConfigJson>();
                    string content = MyFileUtil.readFileAll(GetSystemConfigJsonPath());
                    SystemConfigJson systemConfigJson = myJsonUtil.parseJsonStr(content);
                    return systemConfigJson;
                }

            }catch(Exception e)
            {
                
                return null;
            }
            
            
        }

        /// <summary>
        /// 获取配置文件的目录
        /// </summary>
        /// <returns></returns>
        public static string GetSystemConfigJsonPath()
        {
            string dirPath = GetDllRoot() + "SystemConfig.txt";
            return dirPath;
        }
        /// <summary>
        /// 获取会员文件完整路径
        /// </summary>
        /// <param name="qq"></param>
        /// <returns></returns>
        public static string GetMemberPath(long qq)
        {
            return GetMemberPath() + qq + ".txt";
        }

        /// <summary>
        /// 获取基础配置
        /// </summary>
        /// <returns></returns>
        public static BaseJson GetBaseJsonConfig()
        {
            string dirPath = GetDllRoot() + "BaseJson.txt";
            if (File.Exists(dirPath))
            {
                MyJsonUtil<BaseJson> myJsonUtil = new MyJsonUtil<BaseJson>();
                BaseJson baseJson = myJsonUtil.parseJsonStr(MyFileUtil.readFileAll(dirPath));
                return baseJson;
            }
            else
            {
                return null;
            }
        }

        public static string GetBaseJsonPath()
        {
            return GetDllRoot() + "BaseJson.txt"; 
        }

        public static string GetMovieJsonPath()
        {
            return GetDllRoot() + "MovieConfig.txt"; 
        }

        public static MovieConfig GetMovieJsonConfig()
        {
            string dirPath = GetDllRoot() + "MovieConfig.txt";
            if (File.Exists(dirPath))
            {
                MyJsonUtil<MovieConfig> myJsonUtil = new MyJsonUtil<MovieConfig>();
                MovieConfig movieConfig = myJsonUtil.parseJsonStr(MyFileUtil.readFileAll(dirPath));
                return movieConfig;
            }
            else
            {
                return null;
            }
        }

        

    }
}
