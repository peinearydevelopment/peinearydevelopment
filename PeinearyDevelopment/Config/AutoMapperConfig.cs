using AutoMapper;
using AutoMapper.Configuration;
using Contracts;
using Contracts.Blog;
using DataAccess.Contracts;
using DataAccess.Contracts.Blog;

namespace PeinearyDevelopment.Config
{
    public static class AutoMapperConfig
    {
        public static IMapper Configure()
        {
            var mapperConfigurationExpression = new MapperConfigurationExpression();

            mapperConfigurationExpression.CreateMap<PostDto, PostSummary>()
                                         .ForMember(contract => contract.ContentSummary, conf => conf.MapFrom(dto => dto.MarkdownContent.Substring(0, 255)))
                                         .ForMember(contract => contract.CommentsCount, conf => conf.MapFrom(dto => dto.Comments.Count));
            mapperConfigurationExpression.CreateMap<TagDto, Tag>();
            mapperConfigurationExpression.CreateMap<ResultSetDto<PostDto>, ResultSet<PostSummary>>();

            var mapperConfiguration = new MapperConfiguration(mapperConfigurationExpression);
            return new Mapper(mapperConfiguration);
        }
    }
}
