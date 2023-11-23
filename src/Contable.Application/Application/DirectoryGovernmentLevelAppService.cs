using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Contable.Application.DirectoryGovernmentLevels;
using Contable.Application.DirectoryGovernmentLevels.Dto;
using Contable.Application.DirectoryGovernments.Dto;
using Contable.Application.Exporting;
using Contable.Application.Extensions;
using Contable.Authorization;
using Contable.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Contable.Application
{
    [AbpAuthorize(AppPermissions.Pages_Maintenance_DirectoryGovernmentLevel)]
    public class DirectoryGovernmentLevelAppService: ContableAppServiceBase, IDirectoryGovernmentLevelAppService
    {
        private readonly IRepository<DirectoryGovernmentLevel> _directoryGovernmentLevelRepository;
        private readonly IDirectoryGovernmentLevelExcelExporter _directoryGovernmentLevelExcelExporter;
        public DirectoryGovernmentLevelAppService(IRepository<DirectoryGovernmentLevel> directoryGovernmentLevelRepository,
            IDirectoryGovernmentLevelExcelExporter directoryGovernmentLevelExcelExporter)
        {
            _directoryGovernmentLevelRepository = directoryGovernmentLevelRepository;
            _directoryGovernmentLevelExcelExporter = directoryGovernmentLevelExcelExporter;
        }

        [AbpAuthorize(AppPermissions.Pages_Maintenance_DirectoryGovernmentLevel_Create)]
        public async Task Create(DirectoryGovernmentLevelCreateDto input)
        {
            await _directoryGovernmentLevelRepository.InsertAsync(ValidateEntity(ObjectMapper.Map<DirectoryGovernmentLevel>(input)));
        }

        [AbpAuthorize(AppPermissions.Pages_Maintenance_DirectoryGovernmentLevel_Delete)]
        public async Task Delete(EntityDto input)
        {
            VerifyCount(await _directoryGovernmentLevelRepository.CountAsync(p => p.Id == input.Id));

            await _directoryGovernmentLevelRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_Maintenance_DirectoryGovernmentLevel)]
        public async Task<DirectoryGovernmentLevelGetDto> Get(EntityDto input)
        {
            VerifyCount(await _directoryGovernmentLevelRepository.CountAsync(p => p.Id == input.Id));

            return ObjectMapper.Map<DirectoryGovernmentLevelGetDto>(await _directoryGovernmentLevelRepository.GetAsync(input.Id));
        }

        [AbpAuthorize(AppPermissions.Pages_Maintenance_DirectoryGovernmentLevel)]
        public async Task<PagedResultDto<DirectoryGovernmentLevelGetAllDto>> GetAll(DirectoryGovernmentLevelGetAllInputDto input)
        {
            var query = _directoryGovernmentLevelRepository
                .GetAll()
                .LikeAllBidirectional(input
                    .Filter
                    .SplitByLike()
                    .Select(word => (Expression<Func<DirectoryGovernmentLevel, bool>>)(expression => EF.Functions.Like(expression.Name, $"%{word}%"))).ToArray());

            var count = await query.CountAsync();
            var output = query.OrderBy(input.Sorting).PageBy(input);

            return new PagedResultDto<DirectoryGovernmentLevelGetAllDto>(count, ObjectMapper.Map<List<DirectoryGovernmentLevelGetAllDto>>(output));
        }

        [AbpAuthorize(AppPermissions.Pages_Maintenance_DirectoryGovernmentLevel)]
        public async Task<FileDto> GetDirectoryGovernmentLevelMatrizToExcel(DirectoryGovernmentLevelGetAllInputDto input)
        {
            var query = _directoryGovernmentLevelRepository
               .GetAll()
               .LikeAllBidirectional(input
                   .Filter
                   .SplitByLike()
                   .Select(word => (Expression<Func<DirectoryGovernmentLevel, bool>>)(expression => EF.Functions.Like(expression.Name, $"%{word}%"))).ToArray());

            var output = await query.OrderBy(input.Sorting).ToListAsync();
            var data = new List<DirectoryGovernmentLevelExcelExportDto>();

            foreach (var directoryGovernmentLevel in output)
            {
                var item = new DirectoryGovernmentLevelExcelExportDto();
                item.Name = directoryGovernmentLevel.Name;
                item.enabled = directoryGovernmentLevel.Enabled == false ? "No" : "Si";
                data.Add(item);
            }
            return _directoryGovernmentLevelExcelExporter.ExportMatrizToFile(data, input.checkName, input.checkEnabled);
        }

        [AbpAuthorize(AppPermissions.Pages_Maintenance_DirectoryGovernmentLevel_Edit)]
        public async Task Update(DirectoryGovernmentLevelUpdateDto input)
        {
            VerifyCount(await _directoryGovernmentLevelRepository.CountAsync(p => p.Id == input.Id));

            await _directoryGovernmentLevelRepository.UpdateAsync(ValidateEntity(ObjectMapper.Map(input, await _directoryGovernmentLevelRepository.GetAsync(input.Id))));
        }

        private DirectoryGovernmentLevel ValidateEntity(DirectoryGovernmentLevel input)
        {
            input.Name.IsValidOrException(DefaultTitleMessage, $"El nombre es obligatorio");
            input.Name.VerifyTableColumn(DirectoryGovernmentLevelConsts.NameMinLength, 
                DirectoryGovernmentLevelConsts.NameMaxLength, 
                DefaultTitleMessage, 
                $"El nombre no debe exceder los {DirectoryGovernmentLevelConsts.NameMaxLength} caracteres");
            
            return input;
        }
    }
}
