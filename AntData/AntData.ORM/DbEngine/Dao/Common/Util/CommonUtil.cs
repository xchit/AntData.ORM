﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
#if !NETSTANDARD
using System.Configuration;
#endif
using System.Security.Cryptography;
using System.Text;

namespace AntData.ORM.Common.Util
{
    public static class CommonUtil
    {

        /// <summary>
        /// 将NameValueCollection转成keyValue的格式好遍历
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, string>> AsKVP(
            this NameValueCollection source
        )
        {
            return source.AllKeys.SelectMany(
                source.GetValues,
                (k, v) => new KeyValuePair<string, string>(k, v));
        }


        /// <summary>
        ///  获得string对象的Hash值，每次耗时~5 微秒
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        //  static  HashAlgorithm hash = new MD5CryptoServiceProvider();
        public static String GetHashCodeOfSQL(String text)
        {
            text = text.Trim();

#if !NETSTANDARD
            //1微妙
            using (HashAlgorithm hash = new MD5CryptoServiceProvider())
            {
                //0.8微秒
                Byte[] temp = Encoding.Default.GetBytes(text);

                //4微秒
                Byte[] md5Data = hash.ComputeHash(temp);

                //0.3微秒
                return Convert.ToBase64String(md5Data);
            }
#else
            //1微妙
            using (HashAlgorithm hash = new HMACMD5())
            {
                //0.8微秒
                Byte[] temp = Encoding.UTF8.GetBytes(text);

                //4微秒
                Byte[] md5Data = hash.ComputeHash(temp);

                //0.3微秒
                return Convert.ToBase64String(md5Data);
            }
#endif
        }
#if !NETSTANDARD
        static readonly String AppIdComment = "/*" + ConfigurationManager.AppSettings["AppID"] + "*/";
#endif
        /// <summary>
        /// 给SQL打上APPID的Tag
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static String GetTaggedAppIDSql(String sql)
        {
            StringBuilder sb = new StringBuilder();
#if !NETSTANDARD
            sb.AppendLine(AppIdComment);
#endif
            sb.Append(sql);
            return sb.ToString();
        }

    }
}
