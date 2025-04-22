using AutoMapper;
using MonitorarTarefas.Domain.Entities;
using MonitorarTarefas.API.DTOs;

namespace MonitorarTarefas.API.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Tarefa, TarefaDTO>().ReverseMap();
            CreateMap<Projeto, ProjetoDTO>().ReverseMap();
            CreateMap<HistoricoTarefa, HistoricoTarefaDTO>().ReverseMap();
        }
    }

}
