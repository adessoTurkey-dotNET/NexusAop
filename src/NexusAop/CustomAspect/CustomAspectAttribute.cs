using System;
using System.Collections.Generic;

namespace NexusAop.CustomAspect
{
    public class CustomAspectAttribute : Attribute
    {
        public Dictionary<string, object> Properties { get; }

        public CustomAspectAttribute(params object[] propertyValues)
        {
            Properties = new Dictionary<string, object>();

            if (propertyValues.Length % 2 != 0)
                throw new ArgumentException("Property values must be provided as name-value pairs.");

            for (int i = 0; i < propertyValues.Length; i += 2)
            {
                if (!(propertyValues[i] is string propertyName))
                    throw new ArgumentException("Property name must be a string.");

                Properties[propertyName] = propertyValues[i + 1];
            }
        }
    }
}
