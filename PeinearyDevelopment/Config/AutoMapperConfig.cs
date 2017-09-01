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

            mapperConfigurationExpression.CreateMap<PostDto, Post>()
                                         .ForMember(contract => contract.PostedOn, conf => conf.MapFrom(dto => dto.PostedOn.Value));
            mapperConfigurationExpression.CreateMap<PostDto, PostSummary>()
                                         .ForMember(contract => contract.ContentSummary, conf => conf.MapFrom(dto => (dto.MarkdownContent.Length > 255) ? dto.MarkdownContent.Substring(0, 255) : dto.MarkdownContent))
                                         .ForMember(contract => contract.CommentsCount, conf => conf.MapFrom(dto => dto.Comments.Count))
                                         .ForMember(contract => contract.Tags, conf => conf.MapFrom(dto => dto.Tags))
                                         .ForMember(contract => contract.PostedOn, conf => conf.MapFrom(dto => dto.PostedOn.Value));
            mapperConfigurationExpression.CreateMap<PostDto, NavigationPost>();
            mapperConfigurationExpression.CreateMap<PostTagDto, Tag>()
                                         .ConvertUsing((dto, contract) => new Tag
                                            {
                                                Description = dto.Tag.Description,
                                                Name = dto.Tag.Name,
                                                Slug = dto.Tag.Slug
                                            });
            mapperConfigurationExpression.CreateMap<ResultSetDto<PostDto>, ResultSet<PostSummary>>();

            var mapperConfiguration = new MapperConfiguration(mapperConfigurationExpression);
            return new Mapper(mapperConfiguration);
        }
    }
}
