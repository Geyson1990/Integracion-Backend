using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Contable.Application.Extensions;
using Contable.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Contable.Application.SectorRoles.Dto;
using Contable.Application.SectorRoles;

namespace Contable.Application
{
    [AbpAuthorize(AppPermissions.Pages_Maintenance_RoleSector)]
    public class SectorRoleAppService : ContableAppServiceBase, ISectorRoleAppService
    {
        private readonly IRepository<SectorRole> _sectorRoleRepository;

        public SectorRoleAppService(IRepository<SectorRole> sectorRoleRepository)
        {
            _sectorRoleRepository = sectorRoleRepository;
        }

        [AbpAuthorize(AppPermissions.Pages_Maintenance_RoleSector_Create)]
        public async Task Create(SectorRoleCreateDto input)
        {
            await _sectorRoleRepository.InsertAsync(ValidateEntity(ObjectMapper.Map<SectorRole>(input)));
        }

        [AbpAuthorize(AppPermissions.Pages_Maintenance_RoleSector_Delete)]
        public async Task Delete(EntityDto input)
        {
            VerifyCount(await _sectorRoleRepository.CountAsync(p => p.Id == input.Id));

            await _sectorRoleRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_Maintenance_RoleSector)]
        public async Task<SectorRoleGetDto> Get(EntityDto input)
        {
            VerifyCount(await _sectorRoleRepository.CountAsync(p => p.Id == input.Id));

            return ObjectMapper.Map<SectorRoleGetDto>(await _sectorRoleRepository.GetAsync(input.Id));
        }

        [AbpAuthorize(AppPermissions.Pages_Maintenance_RoleSector)]
        public async Task<PagedResultDto<SectorRoleGetAllDto>> GetAll(SectorRoleGetAllInputDto input)
        {
            var query = _sectorRoleRepository
                .GetAll()
                .LikeAllBidirectional(input.Filter.SplitByLike()
                    .Select(word => (Expression<Func<SectorRole, bool>>) (expression => EF.Functions.Like(expression.Name, $"%{word}%"))).ToArray());

            var count = await query.CountAsync();
            var output = query.OrderBy(input.Sorting).PageBy(input);

            return new PagedResultDto<SectorRoleGetAllDto>(count, ObjectMapper.Map<List<SectorRoleGetAllDto>>(output));
        }

        [AbpAuthorize(AppPermissions.Pages_Maintenance_RoleSector_Edit)]
        public async Task Update(SectorRoleUpdateDto input)
        {
            VerifyCount(await _sectorRoleRepository.CountAsync(p => p.Id == input.Id));

            await _sectorRoleRepository.UpdateAsync(ValidateEntity(ObjectMapper.Map(input, await _sectorRoleRepository.GetAsync(input.Id))));
        }

        private SectorRole ValidateEntity(SectorRole input)
        {
            input.Name.IsValidOrException(DefaultTitleMessage, $"El nombre del rol del sector es obligatorio");
            input.Name.VerifyTableColumn(SectorConsts.NameMinLength, SectorConsts.NameMaxLength, DefaultTitleMessage, $"El nombre del role del sector no debe exceder los {SectorConsts.NameMaxLength} caracteres");

            return input;
        }
    }
}
