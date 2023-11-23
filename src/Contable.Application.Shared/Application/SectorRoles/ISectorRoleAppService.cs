using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Contable.Application.SectorRoles.Dto;
using Contable.Application.Sectors.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contable.Application.SectorRoles
{
    public interface ISectorRoleAppService : IApplicationService
    {
        Task Create(SectorRoleCreateDto input);
        Task Delete(EntityDto input);
        Task<SectorRoleGetDto> Get(EntityDto input);
        Task<PagedResultDto<SectorRoleGetAllDto>> GetAll(SectorRoleGetAllInputDto input);
        Task Update(SectorRoleUpdateDto input);
    }
}
