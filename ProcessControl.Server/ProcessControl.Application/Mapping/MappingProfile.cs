using AutoMapper;
using ProcessControl.Application.DTOs;
using ProcessControl.Domain.Entities;

namespace ProcessControl.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapeamento de Entidade para DTO
            CreateMap<Processo, ProcessoDto>();
            CreateMap<HistoricoProcesso, HistoricoProcessoDto>();

            // Mapeamento de DTO para Entidade
            CreateMap<CreateProcessoDto, Processo>();
            CreateMap<UpdateProcessoDto, Processo>();
            CreateMap<CreateHistoricoProcessoDto, HistoricoProcesso>();
            CreateMap<UpdateHistoricoProcessoDto, HistoricoProcesso>();
        }
    }
}
