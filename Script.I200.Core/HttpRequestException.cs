namespace Script.I200.Core
{
    public class YuanbeiHttpRequestException:YuanbeiException
    {
        public YuanbeiHttpRequestException(int code, string message)
            : base(message)
        {
            Code = code;
        }

        public int Code { get; set; }

    }
}
