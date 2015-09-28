using AutoMapper;

namespace EasyWeixin.Web.Mappers
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<ModelToViewModelMappingProfile>();
                //  x.AddProfile<ViewModelToDomainMappingProfile>();
                x.AddProfile<ViewModelToModelMappingProfile>();
            });
        }
    }
}