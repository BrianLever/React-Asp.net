using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace RPMS.Data.CareBridge.Dto
{
    public class RequestWithParam<TParam> where TParam: class
    {
        [JsonProperty(PropertyName ="params")]
        public TParam Value { get; set; }

        public RequestWithParam()
        {
            Value = null;
        }

        public RequestWithParam(TParam value)
        {
            Value = value ?? throw new NullReferenceException(nameof(value));
        }
    }
}
