using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace App.Shared.Serialization
{
    public sealed class PrivateSetterContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            if (!property.Writable && member is PropertyInfo propertyInfo)
                property.Writable = propertyInfo.GetSetMethod(true) is not null;

            return property;
        }
    }
}