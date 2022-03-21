using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using System;
using System.Reflection;

namespace FrontDesk.Common.Extensions.JsonContractResolvers
{
    public class ContextTypeOnlyJsonContractResolver<T> : DefaultContractResolver
    {
        private readonly Type _targetType = typeof(T);

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {

            JsonProperty property = base.CreateProperty(member, memberSerialization);
            property.ShouldSerialize = instance => property.DeclaringType == _targetType || property.DeclaringType.IsAssignableFrom(_targetType);
            return property;
        }
    }
}
