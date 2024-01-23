


using AutoMapper;
using ControleDeLancamentos.DTOs;
using ControleDeLancamentos.Domain.Entities;

public class DefaultAutoMapperProfile : Profile
{
    public DefaultAutoMapperProfile()
    {
        CreateMap<LancamentoDTO, Lancamento>();
        CreateMap<ContaBancariaDTO, ContaBancaria>();
    }
}
 