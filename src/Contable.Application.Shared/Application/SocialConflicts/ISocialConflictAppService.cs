using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Contable.Application.SocialConflicts.Dto;
using Contable.Application.Ubigeos.Dto;
using Contable.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contable.Application.SocialConflicts
{
    public interface ISocialConflictAppService : IApplicationService
    {
        Task<EntityDto> Create(SocialConflictCreateDto input);
        Task Delete(EntityDto input);
        Task<PagedResultDto<SocialConflictGetAllDto>> GetAll(SocialConflictGetAllInputDto input);
        Task<PagedResultDto<SocialConflictActorGetAllDto>> GetAllActors(SocialConflictActorGetAllInputDto input);
        Task<PagedResultDto<SocialConflictCompromiseGetAllDto>> GetAllCompromises(SocialConflictCompromiseGetAllInputDto input);
        Task<SocialConflictGetDataDto> Get(NullableIdDto input);
        Task<EntityDto> CreateNote(SocialConflictNoteCreateDto input);
        Task<EntityDto> UpdateNote(SocialConflictNoteUpdateDto input);
        Task<EntityDto> CreateResource(SocialConflictCreateResourceDto input);
        Task<EntityDto> UpdateResource(SocialConflictCreateResourceDto input);
        Task DeleteResource(EntityDto input);
        Task Update(SocialConflictUpdateDto input);
        Task DeleteNote(EntityDto input);
        Task<FileDto> GetMatrizToExcel(SocialConflictGetAllInputDto input);
        Task<FileDto> GetManagementToExcel(SocialConflictGetAllInputDto input);
        Task<FileDto> GetStateToExcel(SocialConflictGetAllInputDto input);
        Task<FileDto> GetActorMatrizToExcel(SocialConflictActorGetAllInputDto input);
        Task<FileDto> GetCaseRelevantFactsToExcel(SocialConflictGetAllInputDto input);
        Task<FileDto> GetCaseGestionesRealizadasToExcel(SocialConflictGetAllInputDto input);
        Task<FileDto> GetCaseHechosViolenciaToExcel(SocialConflictGetAllInputDto input);
        Task<FileDto> GetCaseSituacionActualToExcel(SocialConflictGetAllInputDto input);
        Task<FileDto> GetCaseNivelRiesgoToExcel(SocialConflictGetAllInputDto input);
        Task<FileDto> GetCaseEstadoActualToExcel(SocialConflictGetAllInputDto input);
        
    }
}
