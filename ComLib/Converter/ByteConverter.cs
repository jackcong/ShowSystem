using System.Text;

namespace ComLib.Converter
{
    public static class ByteConverter
    {
        public static byte[] ToByteArrayUtf8(this string param)
        {
            // TODO: condition check not complete
            if(param==null)
                return new byte[0];
            return Encoding.UTF8.GetBytes(param);
        }

        public static string ToStringUtf8(this byte[] param)
        {
            return Encoding.UTF8.GetString(param);
        }
    }
}
