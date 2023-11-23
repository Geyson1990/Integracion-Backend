using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Contable.Authorization.Institution.Dto;
using Contable.Authorization.Institution.InstitutionDto;
using Contable.Authorization.Users.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contable.Authorization.Institution
{
    public interface IInstitutionAppService: IApplicationService
    {
        Task<PagedResultDto<GetInstitutionDto>> GetInstitution();

        Task<GetCreateOrUpdateInstitutionDto> GetInstitutionForEdit(NullableIdDto<long> input);

        Task CreateOrUpdateInstitution(GetInstitutionDto input);

        Task DeleteInstitution(EntityDto<long> input);
    }
}
