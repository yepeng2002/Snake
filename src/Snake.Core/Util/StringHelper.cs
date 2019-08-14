using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Snake.Core.Util
{
    public class StringHelper
    {
        /// <summary>
        /// ；
        /// </summary>
        public static char SeparatorChar = ';';

        public static string FilterPrefixStr = "Filter.";

        private StringHelper()
        {
        }

        public static string GetGuid()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 获取指定分隔符第一次出现以后的子字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string SubstringAfter(string str, string separator)
        {
            if (IsEmpty(str))
            {
                return str;
            }
            if (separator == null)
            {
                return string.Empty;
            }
            int _index = str.IndexOf(separator);
            return _index == -1 ? str : str.Substring(_index + separator.Length);
        }

        /// <summary>
        /// 获取指定分隔符最后一次出现以后的子字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public static string SubstringAfterLast(string str, string separator)
        {
            if (IsEmpty(str))
            {
                return str;
            }
            if (separator == null)
            {
                return string.Empty;
            }

            int _index = str.LastIndexOf(separator);
            return _index == -1 ? str : str.Substring(_index + separator.Length);
        }

        /// <summary>
        /// 获取指定分隔符第一次出现以前的子字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public static string SubstringBefore(string str, string separator)
        {
            if (IsEmpty(str))
            {
                return str;
            }

            if (separator == null)
            {
                return string.Empty;
            }

            int _index = str.IndexOf(separator);
            return _index == -1 ? str : str.Substring(0, _index);
        }

        /// <summary>
        /// 获取指定分隔符最后一次出现以前的子字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public static string SubstringBeforeLast(string str, string separator)
        {
            if (IsEmpty(str))
            {
                return str;
            }

            if (separator == null)
            {
                return string.Empty;
            }

            int _index = str.LastIndexOf(separator);
            return _index == -1 ? str : str.Substring(0, _index);
        }

        public static string TranslateSqlChar(string input)
        {
            var sb = new StringBuilder();
            foreach (char c in input)
            {
                switch (c)
                {
                    case '\'':
                        sb.Append("''");
                        break;
                    case '%':
                    case '[':
                    case '_':
                        sb.Append("[" + c + "]");
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }

            string ret = sb.ToString();
            return ret;
        }

        public static string GetInitials(string spell)
        {
            if (string.IsNullOrEmpty(spell) == false)
            {
                spell = spell.Replace('】', ' ').Replace('【', ' ').Replace('：', ' ').Replace('@', ' ').Replace('！', ' ').Replace('~', ' ').Replace('%', ' ').Replace('…', ' ').Replace('&', ' ').Replace('（', ' ').Replace('）', ' ');
                spell = spell.Replace('^', ' ').Replace('$', ' ').Replace('{', ' ').Replace('}', ' ').Replace('[', ' ').Replace(']', ' ').Replace('(', ' ').Replace(')', ' ').Replace('“', ' ').Replace('*', ' ').Replace('.', ' ').Replace('_', ' ').Replace('-', ' ').Replace('%', ' ').Replace('#', ' ').Replace('!', ' ').Replace('~', ' ').Replace(',', ' ');
                spell = spell.Replace('@', ' ').Replace('，', ' ').Replace('。', ' ').Replace('？', ' ').Replace('》', ' ').Replace('《', ' ').Replace('；', ' ').Replace('：', ' ').Replace('>', ' ').Replace(':', ' ').Replace('<', ' ').Replace('|', ' ');
                spell = spell.Replace(" ", "");
                if (spell.Length > 20)
                {
                    spell = spell.Substring(0, 19);
                }
                return spell;
            }
            else
            {
                return spell;
            }
        }

        public static decimal ToDecimal(string str)
        {
            if (!IsEmpty(str))
            {
                try
                {
                    return Convert.ToDecimal(str);
                }
                catch (FormatException formatException)
                {
                    throw new Exception(String.Format("传入字符串:{0},转换成数值类型失败，详细信息：{1}", str, formatException));
                }
            }
            else
                return 0;
        }

        public static float ToFloat(string str)
        {
            float value = 0;
            if (!IsEmpty(str))
            {
                try
                {
                    float.TryParse(str, out value);
                    return value;
                }
                catch (FormatException formatException)
                {
                    return value;
                }
            }
            else
                return value;
        }

        public static long ToLong(string str)
        {
            if (!IsEmpty(str))
            {
                try
                {
                    return long.Parse(str);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            else
                return 0;
        }

        public static ushort ToUshort(string str)
        {
            if (!IsEmpty(str))
            {
                try
                {
                    return ushort.Parse(str);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            else
                return 0;
        }

        /// <summary>
        /// 将字符串转换成int返回
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static int Toint(string str)
        {
            if (!IsEmpty(str))
            {
                int tmp;
                if (int.TryParse(str, out tmp))
                    return tmp;
                else
                    return -1;
            }
            else
                return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool ToBool(string str)
        {
            if (!IsEmpty(str))
            {
                return str == "1" ? true : false;
            }
            else
                return false;
        }

        public static DateTime ToDatetime(string str)
        {
            if (IsDate(str))
            {
                return Convert.ToDateTime(str);
            }
            else
                return DateTime.Now;
        }

        /// <summary>
        /// 将字符串转换成decimal保留小数点2位返回
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static decimal ToMoney(string str)
        {
            return Math.Round(ToDecimal(str), 2);
        }

        /// <summary>
        /// 将decimal保留小数点2位返回字符串
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static string ToMoneyString(decimal dec)
        {
            dec = Math.Round(dec, 2);
            return dec.ToString();
        }

        public static object ObjectToObjectByType(string valueType, object value)
        {
            object obj = null;
            switch (valueType.ToUpper())
            {
                case "LONG":
                    obj = ToLong(value.ToString());
                    break;
                case "INT":
                    obj = Toint(value.ToString());
                    break;
                case "STRING":
                    obj = value.ToString();
                    break;
                case "DECIMAL":
                    obj = ToDecimal(value.ToString());
                    break;
                case "DATETIME":
                    if (IsDate(value.ToString()))
                    {
                        obj = Convert.ToDateTime(value.ToString());
                    }
                    else
                    {
                        obj = value;
                    }
                    break;
                default:
                    obj = value;
                    break;
            }
            return obj;
        }

        public static object StringToObjectByType(string valueType, string value)
        {
            object obj = null;
            switch (valueType.ToUpper())
            {
                case "LONG":
                    obj = ToLong(value);
                    break;
                case "INT":
                    obj = Toint(value);
                    break;
                case "STRING":
                    obj = value;
                    break;
                case "DECIMAL":
                    obj = ToDecimal(value);
                    break;
                case "DATETIME":
                    obj = value;
                    break;
                default:
                    throw new Exception("类型转换失败!");
            }
            return obj;
        }

        public static Int64 GUIDHashCodeToInt64()
        {
            return Math.Abs(Guid.NewGuid().GetHashCode());
        }

        public static int GUIDHashCodeToInt32()
        {
            return Math.Abs(Guid.NewGuid().GetHashCode());
        }

        public static string GUIDHashCodeToString()
        {
            return Math.Abs(Guid.NewGuid().GetHashCode()).ToString();
        }

        public static string FormatMoneyToString(decimal value)
        {
            return string.Format("{0:n2}", value); ;
        }

        public static string FormatMoneyToString(string value)
        {
            return string.Format("{0:n2}", ToDecimal(value)); ;
        }

        #region 判断

        public static bool IsEmpty(string str)
        {
            return (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str));
        }

        public static bool IsNotEmpty(string str)
        {
            return !IsEmpty(str);
        }

        public static bool IsDate(string date)
        {
            bool flag = true;
            try
            {
                Convert.ToDateTime(date);
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        /// <summary>
        /// 判断字符串是否是数字型(包括整型和浮点型)
        /// </summary>
        public static bool IsNumber(string str)
        {
            if (IsEmpty(str))
            {
                return false;
            }

            var _regExpr = new Regex(@"^(-|\+)?(0?|(\d)+)\.?(\d)*$", RegexOptions.Singleline);
            return _regExpr.IsMatch(str);
        }

        public static bool IsInteger(string str)
        {
            if (IsEmpty(str))
            {
                return false;
            }

            var _regExpr = new Regex(@"^(-|\+)?\d+$", RegexOptions.Singleline);
            return _regExpr.IsMatch(str);
        }

        #endregion

        #region 数字与汉字
        /// <summary>
        /// 汉字转数字
        /// </summary>
        /// <param name="chineseStr1"></param>
        /// <returns></returns>
        public static string ChineseTONumber(string chineseStr1)
        {
            string numStr = "0123456789";
            string chineseStr = "零一二三四五六七八九";
            char[] c = chineseStr1.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                int index = chineseStr.IndexOf(c[i]);
                if (index != -1)
                    c[i] = numStr.ToCharArray()[index];
            }
            numStr = null;
            chineseStr = null;
            return new string(c);
        }
        /// <summary>
        /// 数字转汉字
        /// </summary>
        /// <param name="numberStr"></param>
        /// <returns></returns>
        public static string NumberToChinese(string numberStr)
        {
            string numStr = "0123456789";
            string chineseStr = "零一二三四五六七八九";
            char[] c = numberStr.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                int index = numStr.IndexOf(c[i]);
                if (index != -1)
                    c[i] = chineseStr.ToCharArray()[index];
            }
            numStr = null;
            chineseStr = null;
            return new string(c);
        }
        #endregion
    }
}
