using Framework.Mapping.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Mapping.Models
{
    public class PropertyMapping
    {
        public PropertyInfo PropertyInfo { get; set; }

        public MapperBaseAttribute MapperAttribute { get; set; }

        public PropertyMapping(PropertyInfo propertyInfo, MapperBaseAttribute baseMapperAttribute)
        {
            this.PropertyInfo = propertyInfo;
            this.MapperAttribute = baseMapperAttribute;
        }
    }
}
