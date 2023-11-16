using Newtonsoft.Json;
using NexusAop.Cache;
using NexusAop.CustomAspect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NexusAop.Proxy
{
    public partial class NexusAopCustomProxy<TDecorated>
    {
        private bool CheckMethod(
            MethodInfo targetMethod)
        {
            return targetMethod.CustomAttributes.Any(x => x.AttributeType == typeof(CustomAspectAttribute));
        }

        protected virtual List<object> GetAttributeKeys(NexusAopContext context)
        {
            var attributes = new List<object>();
            var customAspectAttributes = context.TargetMethod.GetCustomAttributes(typeof(CustomAspectAttribute), true);

            if (customAspectAttributes.Length > 0)
            {
                CustomAspectAttribute aspect = (CustomAspectAttribute)customAspectAttributes[0];
                foreach (var property in aspect.Properties)
                {
                    attributes.Add(property.Key);
                    attributes.Add(property.Value);
                }
            }
            return attributes;

        }
    }
}
