using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace EasyWeixin.Web.Tests
{
    [TestClass]
    public class TestDate
    {
        [TestMethod]
        public void TestMethod1()
        {
            DateTime t1 = new DateTime(2015, 8, 15, 10, 2, 1);
            DateTime t2 = new DateTime(2015, 8, 16, 23, 59, 59);
            var a = DateDiff(t1, t2);
            var b = GetTimeSpan(t1, t2);
            var c = GetTimeSpan(t2, t1);
            Trace.WriteLine(a);
            Trace.WriteLine(b);
            Trace.WriteLine(c);
        }

        [TestMethod]
        public void TestTimeSpan()
        {
            var timespan = new TimeSpan(0, 10, 0);
            Trace.WriteLine(timespan.TotalMilliseconds);
            Trace.WriteLine(timespan.TotalSeconds);
            Trace.WriteLine(timespan.TotalMinutes);
            Trace.WriteLine(timespan.TotalHours);
            Trace.WriteLine(timespan.TotalDays);
        }

        public static string DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            string dateDiff = null;
            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            dateDiff = ts.Days.ToString() + "天" + ts.Hours.ToString() + "小时" + ts.Minutes.ToString() + "分钟" + ts.Seconds.ToString() + "秒";
            return dateDiff;
        }

        public static int GetTimeSpan(DateTime StartDate, DateTime EndDate)
        {
            System.TimeSpan timeSpan = (System.TimeSpan)(EndDate - StartDate);
            return timeSpan.Days;
        }
    }
}
