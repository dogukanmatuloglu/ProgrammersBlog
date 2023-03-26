using AutoMapper;
using ProgrammersBlog.Entities.Dtos;
using ProgrammersBlog.Mvc.Areas.Admin.Models;

namespace ProgrammersBlog.Mvc.AutoMapper.Profiles
{
    public class ViewModelProfiles:Profile
    {
        public ViewModelProfiles()
        {
            CreateMap<ArticleAddViewModel, ArticleAddDto>();
            CreateMap<ArticleUpdateDto, ArticleUpdateViewModel>().ReverseMap();
        }
    }
}
