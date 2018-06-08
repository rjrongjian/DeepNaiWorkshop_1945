using DeepNaiWorkshop_6001.MyTool;
using FileCreator.MyTool;
using System;
using System.IO;
using www_52bang_site_enjoy.MyModel;
using www_52bang_site_enjoy.MyTool;

namespace www_52bang_site_enjoy.Service
{
    class MemberService
    {
        internal Member Recharge(double money,long qq)
        {
            Member member = null;
            MyJsonUtil<Member> myJsonUtil = new MyJsonUtil<Member>();
            string memberPath = MySystemUtil.GetMemberPath(qq);
            if (File.Exists(memberPath))
            {
                string content = MyFileUtil.readFileAll(memberPath);
               
                member = myJsonUtil.parseJsonStr(content);
                //判断用户是否过期
                long currentTime = Convert.ToInt64(MyDateUtil.GetTimeStamp(DateTime.Now));
                if (currentTime > member.Time)//说明已经过期
                {
                    long outOfDate = currentTime + AddTime(money);
                    member.DateDesp = MyDateUtil.ConvertStringToDateTime(""+outOfDate).ToString("yyyy-MM-dd HH:mm:ss");
                    member.Time = outOfDate;
                    member.Type = 3;
                }
                else//还没有过期
                {
                    member.Time = member.Time+ AddTime(money);
                    member.DateDesp = MyDateUtil.ConvertStringToDateTime(""+member.Time).ToString("yyyy-MM-dd HH:mm:ss");
                    member.Type = 3;
                }
                MyFileUtil.writeToFile(memberPath, myJsonUtil.parseJsonObj(member));
            }
            else
            {
                member = new Member();
                long currentTime = Convert.ToInt64(MyDateUtil.GetTimeStamp(DateTime.Now));
                long outOfDate = currentTime + AddTime(money);
                member.DateDesp = MyDateUtil.ConvertStringToDateTime(""+outOfDate).ToString("yyyy-MM-dd HH:mm:ss");
                member.Time = outOfDate;
                member.Type = 3;
                MyFileUtil.writeToFile(memberPath, myJsonUtil.parseJsonObj(member));
            }
            return member;
        }

        private long AddTime(double money)
        {
            if (money == SystemConfig.MoneyForWeekPay)
            {
                return 7L * 24 * 60 * 60 * 1000;
            }
            else if(money == SystemConfig.MoneyForMonthPay)
            {
                return 30L * 24 * 60 * 60 * 1000;
            }
            else
            {
                return 0L;
            }
            
        }

        internal Member GetMemberDate(long fromQQ)
        {
            Member member = null;
            string memberPath = MySystemUtil.GetMemberPath(fromQQ);
            if (File.Exists(memberPath))
            {
                MyJsonUtil<Member> myJsonUtil = new MyJsonUtil<Member>();
                string content = MyFileUtil.readFileAll(memberPath);
                member = myJsonUtil.parseJsonStr(content);
                long currentTime = Convert.ToInt64(MyDateUtil.GetTimeStamp(DateTime.Now));
                if (currentTime > member.Time)//说明已经过期
                {
                    member.Type = 2;
                }
            }
            else
            {
                member = new Member();
                member.Type = 1;//1不是会员 2 会员过期 3 正常会员
                
            }
            return member;
        }
    }
}
