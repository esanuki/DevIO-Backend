using AutoMapper;
using DevIO.Api.Interop.ViewModels;
using DevIO.Business.Models;

namespace DevIO.Api.Configuration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Fornecedor, FornecedorViewModel>().ReverseMap();
            CreateMap<Endereco, EnderecoViewModel>().ReverseMap();
            CreateMap<Produto, ProdutoViewModel>()
                .ForMember(dest => dest.NomeFornecedor, src => src.MapFrom(c => c.Fornecedor.Nome));
            CreateMap<ProdutoViewModel, Produto>()
                .ForMember(dest => dest.Fornecedor, src => src.Ignore());
        }
    }
}
