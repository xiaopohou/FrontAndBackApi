using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace CommonLib
{
    /// <summary>
    ///     公历农历日期处理帮助类
    /// </summary>
    public sealed class ChineseCalendarHelper
    {
        public const string ChineseNumber = "〇一二三四五六七八九";
        public const string CelestialStem = "甲乙丙丁戊己庚辛壬癸";
        public const string TerrestrialBranch = "子丑寅卯辰巳午未申酉戌亥";
        public const string Animals = "鼠牛虎兔龙蛇马羊猴鸡狗猪";
        private static readonly ChineseLunisolarCalendar Calendar = new ChineseLunisolarCalendar();
        public static readonly string[] ChineseWeekName = {"星期天", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"};

        public static readonly string[] ChineseDayName =
        {
            "初一", "初二", "初三", "初四", "初五", "初六", "初七", "初八", "初九", "初十",
            "十一", "十二", "十三", "十四", "十五", "十六", "十七", "十八", "十九", "二十",
            "廿一", "廿二", "廿三", "廿四", "廿五", "廿六", "廿七", "廿八", "廿九", "三十"
        };

        public static readonly string[] ChineseMonthName =
        {
            "正", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "十二"
        };

        public static readonly string[] Constellations =
        {
            "白羊座", "金牛座", "双子座", "巨蟹座", "狮子座", "处女座", "天秤座", "天蝎座", "射手座",
            "摩羯座", "水瓶座", "双鱼座"
        };

        public static readonly string[] BirthStones =
        {
            "钻石", "蓝宝石", "玛瑙", "珍珠", "红宝石", "红条纹玛瑙", "蓝宝石", "猫眼石", "黄宝石",
            "土耳其玉", "紫水晶", "月长石，血石"
        };

        private string _mLunarYearSexagenary, _mLunarYearAnimal;
        private string _mLunarYearText, _mLunarMonthText, _mLunarDayText;
        private DateTime _mSolarDate;
        private string _mSolarWeekText, _mSolarConstellation, _mSolarBirthStone;

        /// <summary>
        ///     根据指定阳历日期计算星座＆诞生石
        /// </summary>
        /// <param name="date">指定阳历日期</param>
        /// <param name="constellation">星座</param>
        /// <param name="birthstone">诞生石</param>
        public static void CalcConstellation(DateTime date, out string constellation, out string birthstone)
        {
            var i = Convert.ToInt32(date.ToString("MMdd"));
            int j;
            if (i >= 321 && i <= 419)
                j = 0;
            else if (i >= 420 && i <= 520)
                j = 1;
            else if (i >= 521 && i <= 621)
                j = 2;
            else if (i >= 622 && i <= 722)
                j = 3;
            else if (i >= 723 && i <= 822)
                j = 4;
            else if (i >= 823 && i <= 922)
                j = 5;
            else if (i >= 923 && i <= 1023)
                j = 6;
            else if (i >= 1024 && i <= 1121)
                j = 7;
            else if (i >= 1122 && i <= 1221)
                j = 8;
            else if (i >= 1222 || i <= 119)
                j = 9;
            else if (i >= 120 && i <= 218)
                j = 10;
            else if (i >= 219 && i <= 320)
                j = 11;
            else
            {
                constellation = "未知星座";
                birthstone = "未知诞生石";
                return;
            }
            constellation = Constellations[j];
            birthstone = BirthStones[j];

            #region 星座划分

            //白羊座：   3月21日------4月19日     诞生石：   钻石   
            //金牛座：   4月20日------5月20日   诞生石：   蓝宝石   
            //双子座：   5月21日------6月21日     诞生石：   玛瑙   
            //巨蟹座：   6月22日------7月22日   诞生石：   珍珠   
            //狮子座：   7月23日------8月22日   诞生石：   红宝石   
            //处女座：   8月23日------9月22日   诞生石：   红条纹玛瑙   
            //天秤座：   9月23日------10月23日     诞生石：   蓝宝石   
            //天蝎座：   10月24日-----11月21日     诞生石：   猫眼石   
            //射手座：   11月22日-----12月21日   诞生石：   黄宝石   
            //摩羯座：   12月22日-----1月19日   诞生石：   土耳其玉   
            //水瓶座：   1月20日-----2月18日   诞生石：   紫水晶   
            //双鱼座：   2月19日------3月20日   诞生石：   月长石，血石  

            #endregion
        }

        #region 构造函数

        public ChineseCalendarHelper()
            : this(DateTime.Now.Date)
        {
        }

        /// <summary>
        ///     从指定的阳历日期创建中国日历信息实体类
        /// </summary>
        /// <param name="date">指定的阳历日期</param>
        public ChineseCalendarHelper(DateTime date)
        {
            IsLeapMonth = false;
            _mSolarDate = date;
            LoadFromSolarDate();
        }

        private void LoadFromSolarDate()
        {
            IsLeapMonth = false;
            _mLunarYearSexagenary = null;
            _mLunarYearAnimal = null;
            _mLunarYearText = null;
            _mLunarMonthText = null;
            _mLunarDayText = null;
            _mSolarWeekText = null;
            _mSolarConstellation = null;
            _mSolarBirthStone = null;

            LunarYear = Calendar.GetYear(_mSolarDate);
            LunarMonth = Calendar.GetMonth(_mSolarDate);
            var leapMonth = Calendar.GetLeapMonth(LunarYear);

            if (leapMonth == LunarMonth)
            {
                IsLeapMonth = true;
                LunarMonth -= 1;
            }
            else if (leapMonth > 0 && leapMonth < LunarMonth)
            {
                LunarMonth -= 1;
            }

            LunarDay = Calendar.GetDayOfMonth(_mSolarDate);

            CalcConstellation(_mSolarDate, out _mSolarConstellation, out _mSolarBirthStone);
        }

        #endregion

        #region 日历属性

        /// <summary>
        ///     阳历日期
        /// </summary>
        public DateTime SolarDate
        {
            get { return _mSolarDate; }
            set
            {
                if (_mSolarDate.Equals(value))
                    return;
                _mSolarDate = value;
                LoadFromSolarDate();
            }
        }

        /// <summary>
        ///     星期几
        /// </summary>
        public string SolarWeekText
        {
            get
            {
                if (string.IsNullOrEmpty(_mSolarWeekText))
                {
                    var i = (int) _mSolarDate.DayOfWeek;
                    _mSolarWeekText = ChineseWeekName[i];
                }
                return _mSolarWeekText;
            }
        }

        /// <summary>
        ///     阳历星座
        /// </summary>
        public string SolarConstellation
        {
            get { return _mSolarConstellation; }
        }

        /// <summary>
        ///     阳历诞生石
        /// </summary>
        public string SolarBirthStone
        {
            get { return _mSolarBirthStone; }
        }

        /// <summary>
        ///     阴历年份
        /// </summary>
        public int LunarYear { get; private set; }

        /// <summary>
        ///     阴历月份
        /// </summary>
        public int LunarMonth { get; private set; }

        /// <summary>
        ///     是否阴历闰月
        /// </summary>
        public bool IsLeapMonth { get; private set; }

        /// <summary>
        ///     阴历月中日期
        /// </summary>
        public int LunarDay { get; private set; }

        /// <summary>
        ///     阴历年干支
        /// </summary>
        public string LunarYearSexagenary
        {
            get
            {
                if (string.IsNullOrEmpty(_mLunarYearSexagenary))
                {
                    var y = Calendar.GetSexagenaryYear(SolarDate);
                    _mLunarYearSexagenary = CelestialStem.Substring((y - 1)%10, 1) +
                                            TerrestrialBranch.Substring((y - 1)%12, 1);
                }
                return _mLunarYearSexagenary;
            }
        }

        /// <summary>
        ///     阴历年生肖
        /// </summary>
        public string LunarYearAnimal
        {
            get
            {
                if (string.IsNullOrEmpty(_mLunarYearAnimal))
                {
                    var y = Calendar.GetSexagenaryYear(SolarDate);
                    _mLunarYearAnimal = Animals.Substring((y - 1)%12, 1);
                }
                return _mLunarYearAnimal;
            }
        }


        /// <summary>
        ///     阴历年文本
        /// </summary>
        public string LunarYearText
        {
            get
            {
                if (string.IsNullOrEmpty(_mLunarYearText))
                {
                    _mLunarYearText = Animals.Substring(
                        Calendar.GetSexagenaryYear(new DateTime(LunarYear, 1, 1))%12 - 1, 1);
                    var sb = new StringBuilder();
                    var year = LunarYear;
                    int d;
                    do
                    {
                        d = year%10;
                        sb.Insert(0, ChineseNumber[d]);
                        year = year/10;
                    } while (year > 0);
                    _mLunarYearText = sb.ToString();
                }
                return _mLunarYearText;
            }
        }

        /// <summary>
        ///     阴历月文本
        /// </summary>
        public string LunarMonthText
        {
            get
            {
                if (string.IsNullOrEmpty(_mLunarMonthText))
                {
                    _mLunarMonthText = (IsLeapMonth ? "闰" : "") + ChineseMonthName[LunarMonth - 1];
                }
                return _mLunarMonthText;
            }
        }

        /// <summary>
        ///     阴历月中日期文本
        /// </summary>
        public string LunarDayText
        {
            get
            {
                if (string.IsNullOrEmpty(_mLunarDayText))
                    _mLunarDayText = ChineseDayName[LunarDay - 1];
                return _mLunarDayText;
            }
        }

        #endregion

        #region 阴历转阳历

        /// <summary>
        ///     获取指定年份春节当日（正月初一）的阳历日期
        /// </summary>
        /// <param name="year">指定的年份</param>
        private static DateTime GetLunarNewYearDate(int year)
        {
            var dt = new DateTime(year, 1, 1);
            var cnYear = Calendar.GetYear(dt);
            var cnMonth = Calendar.GetMonth(dt);

            var num1 = 0;
            var num2 = Calendar.IsLeapYear(cnYear) ? 13 : 12;

            while (num2 >= cnMonth)
            {
                num1 += Calendar.GetDaysInMonth(cnYear, num2--);
            }

            num1 = num1 - Calendar.GetDayOfMonth(dt) + 1;
            return dt.AddDays(num1);
        }

        /// <summary>
        ///     阴历转阳历
        /// </summary>
        /// <param name="year">阴历年</param>
        /// <param name="month">阴历月</param>
        /// <param name="day">阴历日</param>
        /// <param name="IsLeapMonth">是否闰月</param>
        public static DateTime GetDateFromLunarDate(int year, int month, int day, bool IsLeapMonth)
        {
            if (year < 1902 || year > 2100)
                throw new Exception("只支持1902～2100期间的农历年");
            if (month < 1 || month > 12)
                throw new Exception("表示月份的数字必须在1～12之间");

            if (day < 1 || day > Calendar.GetDaysInMonth(year, month))
                throw new Exception("农历日期输入有误");

            int num1 = 0, num2 = 0;
            var leapMonth = Calendar.GetLeapMonth(year);

            if (((leapMonth == month + 1) && IsLeapMonth) || (leapMonth > 0 && leapMonth <= month))
                num2 = month;
            else
                num2 = month - 1;

            while (num2 > 0)
            {
                num1 += Calendar.GetDaysInMonth(year, num2--);
            }

            var dt = GetLunarNewYearDate(year);
            return dt.AddDays(num1 + day - 1);
        }

        /// <summary>
        ///     阴历转阳历
        /// </summary>
        /// <param name="date">阴历日期</param>
        /// <param name="IsLeapMonth">是否闰月</param>
        public static DateTime GetDateFromLunarDate(DateTime date, bool IsLeapMonth)
        {
            return GetDateFromLunarDate(date.Year, date.Month, date.Day, IsLeapMonth);
        }

        #endregion

        #region 从阴历创建日历

        /// <summary>
        ///     从阴历创建日历实体
        /// </summary>
        /// <param name="year">阴历年</param>
        /// <param name="month">阴历月</param>
        /// <param name="day">阴历日</param>
        /// <param name="IsLeapMonth">是否闰月</param>
        public static ChineseCalendarHelper FromLunarDate(int year, int month, int day, bool IsLeapMonth)
        {
            var dt = GetDateFromLunarDate(year, month, day, IsLeapMonth);
            return new ChineseCalendarHelper(dt);
        }

        /// <summary>
        ///     从阴历创建日历实体
        /// </summary>
        /// <param name="date">阴历日期</param>
        /// <param name="IsLeapMonth">是否闰月</param>
        public static ChineseCalendarHelper FromLunarDate(DateTime date, bool IsLeapMonth)
        {
            return FromLunarDate(date.Year, date.Month, date.Day, IsLeapMonth);
        }

        /// <summary>
        ///     从阴历创建日历实体
        /// </summary>
        /// <param name="date">表示阴历日期的8位数字，例如：20070209</param>
        /// <param name="IsLeapMonth">是否闰月</param>
        public static ChineseCalendarHelper FromLunarDate(string date, bool IsLeapMonth)
        {
            var rg = new Regex(@"^\d{7}(\d)$");
            var mc = rg.Match(date);
            if (!mc.Success)
            {
                throw new Exception("日期字符串输入有误！");
            }
            var dt =
                DateTime.Parse(string.Format("{0}-{1}-{2}", date.Substring(0, 4), date.Substring(4, 2),
                    date.Substring(6, 2)));
            return FromLunarDate(dt, IsLeapMonth);
        }

        #endregion
    }
}