using AutoMapper;
using Teste.Web.Core;
using Teste.Web.Models;

namespace Teste.Web.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Cliente, ClienteModel>().ReverseMap();
        }
    }
}