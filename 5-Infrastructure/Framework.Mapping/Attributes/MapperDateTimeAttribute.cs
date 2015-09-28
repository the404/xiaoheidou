using Framework.Mapping.Base;
using Framework.Mapping.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Mapping.Attributes
{
    public enum MapperDateTimeType
    {
        Now,
    }

    public class MapperAutoUpdateDateTimeAttribute : MapperBaseAttribute, IMapperAttribute
    {
        public MapperDateTimeType MapperDateTimeType { get; set; }

        public bool IsRunEachTime
        {
            get { return true; }
        }

        public MapperAutoUpdateDateTimeAttribute(MapperDateTimeType mapperDateTimeType)
        {
            this.MapperDateTimeType = mapperDateTimeType;
        }

        public void Apply<TSource, TDestination>(AutoMapper.IMappingExpression<TSource, TDestination> mappingExpression, string name)
        {
            mappingExpression.ForMember(name, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}
