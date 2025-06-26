using System.Globalization;
using AutoMapper;
using Teste.Web.Core;
using Teste.Web.Models;

namespace Teste.Web.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            var cultura = new CultureInfo("pt-BR");

            // De Entidade → ViewModel (decimal → string com formatação)
            CreateMap<Produto, ProdutoModel>()
                .ForMember(dest => dest.ValorVenda, opt => opt.MapFrom(src => src.ValorVenda.ToString("N2", cultura)))
                .ForMember(dest => dest.PesoBruto, opt => opt.MapFrom(src => src.PesoBruto.ToString("N3", cultura)))
                .ForMember(dest => dest.PesoLiquido, opt => opt.MapFrom(src => src.PesoLiquido.ToString("N3", cultura)));

            // De ViewModel → Entidade (string → decimal)
            CreateMap<ProdutoModel, Produto>()
                .ForMember(dest => dest.ValorVenda, opt => opt.MapFrom(src => decimal.Parse(src.ValorVenda, cultura)))
                .ForMember(dest => dest.PesoBruto, opt => opt.MapFrom(src => decimal.Parse(src.PesoBruto, cultura)))
                .ForMember(dest => dest.PesoLiquido, opt => opt.MapFrom(src => decimal.Parse(src.PesoLiquido, cultura)));

            CreateMap<Cliente, ClienteModel>().ReverseMap();
        }
    }
}