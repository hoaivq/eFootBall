using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Common
{
    public static class Extensions
    {
        public static T ToObject<T>(this string input) where T : class
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(input))
            {
                return (T)ser.Deserialize(sr);
            }
        }

        public static string ToXML<T>(this T ObjectToSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(ObjectToSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, ObjectToSerialize);
                return textWriter.ToString();
            }
        }

        public static int? ToInt(this object input)
        {
            try
            {
                return int.Parse(input.ToString());
            }
            catch
            {
                return null;
            }
        }

        public static long? ToLong(this object input)
        {
            try
            {
                if(input == null) { return null; }
                return long.Parse(input.ToString());
            }
            catch
            {
                return null;
            }
        }
        public static DateTime? ToDateTime(this object input)
        {
            try
            {
                return DateTime.Parse(input.ToString());
            }
            catch
            {
                return null;
            }
        }
        public static bool? ToBool(this object input)
        {
            try
            {
                return bool.Parse(input.ToString());
            }
            catch
            {
                return null;
            }
        }

        public static string ToStr(this object input)
        {
            if (input == null) { return string.Empty; }
            return input.ToString();
        }

        public static SqlParameter ToSqlParam(this object input, string ParamName, SqlDbType ParamType, int? ParamLength = null)
        {
            if (ParamLength.HasValue)
            {
                return new SqlParameter(ParamName, ParamType, ParamLength.Value) { Value = (input == null) ? DBNull.Value : input };
            }
            else
            {
                return new SqlParameter(ParamName, ParamType) { Value = (input == null) ? DBNull.Value : input };
            }
        }

        public static SqlParameter ToSqlParam(this object input, string ParamName)
        {
            return new SqlParameter(ParamName, (input == null) ? DBNull.Value : input);
        }
    }
}
