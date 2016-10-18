using System;
using System.Collections.Generic;

namespace Script.I200.Core
{
    public class YuanbeiException: Exception
    {
        private Dictionary<string,object> _parameters { get; set; }

        public YuanbeiException()
        {
            _parameters = new Dictionary<string, object>();
        }

        public YuanbeiException(string message) : base(message)
        {
            _parameters = new Dictionary<string, object>();
        }

        public YuanbeiException(string messageFormat, params object[] args) : base(string.Format(messageFormat, args))
        {
        }

        public YuanbeiException(string message, Dictionary<string, object> parameters)
            : base(message)
        {
            _parameters = parameters;
        }

        public void AddParameter(string key, object value)
        {
            _parameters.Add(key,value);
        }


    }
}
