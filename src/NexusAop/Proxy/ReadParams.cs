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
        protected virtual Task Read(
            MethodInfo targetMethod,
            object[] args)
        {
            if (!CheckMethod(targetMethod))
            {
                return Task.CompletedTask;
            }

            var key = GetAttributeKeys(targetMethod, args);

            if (key is not null)
                _nexusAopCustomService.Start(key);

            return Task.CompletedTask;
        }

        private bool CheckMethod(
            MethodInfo targetMethod)
        {
            return targetMethod.CustomAttributes.Any(x => x.AttributeType == typeof(CustomAspectAttribute));
        }

        protected virtual Dictionary<string, object> GetAttributeKeys(
            MethodInfo targetMethod,
            object[] args)
        {
            var attributes = new Dictionary<string, object>();
            var customAspectAttributes = targetMethod.GetCustomAttributes(typeof(CustomAspectAttribute), true);

            if (customAspectAttributes.Length > 0)
            {
                CustomAspectAttribute aspect = (CustomAspectAttribute)customAspectAttributes[0];

                foreach (var property in aspect.Properties)
                {
                    attributes.Add(property.Key, property.Value);
                }
            }
            return attributes;

        }
    }
}
