using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Contable.Application.DirectoryGovernments.Dto;
using Contable.Application.DirectoryGovernmentTypes;
using Contable.Application.DirectoryGovernmentTypes.Dto;
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
    [AbpAuthorize(AppPermissions.Pages_Maintenance_DirectoryGovernmentType)]
    public class DirectoryGovernmentTypeAppService: ContableAppServiceBase, IDirectoryGovernmentTypeAppService
    {
        private readonly IRepository<DirectoryGovernmentType> _directoryGovernmentTypeRepository;
        private readonly IDirectoryGovernmentTypeExcelExporter _directoryGovernmentTypeExporterExcel;

        public DirectoryGovernmentTypeAppService(IRepository<DirectoryGovernmentType> directoryGovernmentTypeRepository,
            IDirectoryGovernmentTypeExcelExporter directoryGovernmentTypeExcelExporter)
        {
            _directoryGovernmentTypeRepository = directoryGovernmentTypeRepository;
            _directoryGovernmentTypeExporterExcel = directoryGovernmentTypeExcelExporter;
        }

        [AbpAuthorize(AppPermissions.Pages_Maintenance_DirectoryGovernmentType_Create)]
        public async Task Create(DirectoryGovernmentTypeCreateDto input)
        {
            await _directoryGovernmentTypeRepository.InsertAsync(ValidateEntity(ObjectMapper.Map<DirectoryGovernmentType>(input)));
        }

        [AbpAuthorize(AppPermissions.Pages_Maintenance_DirectoryGovernmentType_Delete)]
        public async Task Delete(EntityDto input)
        {
            VerifyCount(await _directoryGovernmentTypeRepository.CountAsync(p => p.Id == input.Id));

            await _directoryGovernmentTypeRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_Maintenance_DirectoryGovernmentType)]
        public async Task<DirectoryGovernmentTypeGetDto> Get(EntityDto input)
        {
            VerifyCount(await _directoryGovernmentTypeRepository.CountAsync(p => p.Id == input.Id));

            return ObjectMapper.Map<DirectoryGovernmentTypeGetDto>(await _directoryGovernmentTypeRepository.GetAsync(input.Id));
        }

        [AbpAuthorize(AppPermissions.Pages_Maintenance_DirectoryGovernmentType)]
        public async Task<PagedResultDto<DirectoryGovernmentTypeGetAllDto>> GetAll(DirectoryGovernmentTypeGetAllInputDto input)
        {
            var query = _directoryGovernmentTypeRepository
                .GetAll()
                .LikeAllBidirectional(input
                    .Filter
                    .SplitByLike()
                    .Select(word => (Expression<Func<DirectoryGovernmentType, bool>>)(expression => EF.Functions.Like(expression.Name, $"%{word}%"))).ToArray());

            var count = await query.CountAsync();
            var output = query.OrderBy(input.Sorting).PageBy(input);

            return new PagedResultDto<DirectoryGovernmentTypeGetAllDto>(count, ObjectMapper.Map<List<DirectoryGovernmentTypeGetAllDto>>(output));
        }

        [AbpAuthorize(AppPermissions.Pages_Maintenance_DirectoryGovernmentType)]
        public async Task<FileDto> GetDirectoryGovernmentTypeMatrizToExcel(DirectoryGovernmentTypeGetAllInputDto input)
        {
            var query = _directoryGovernmentTypeRepository
               .GetAll()
               .LikeAllBidirectional(input
                   .Filter
                   .SplitByLike()
                   .Select(word => (Expression<Func<DirectoryGovernmentType, bool>>)(expression => EF.Functions.Like(expression.Name, $"%{word}%"))).ToArray());

            var output = await query.OrderBy(input.Sorting).ToListAsync();
            var data = new List<DirectoryGovernmentTypeExcelExportDto>();

            foreach (var directoryGovernmentType in output)
            {
                var item = new DirectoryGovernmentTypeExcelExportDto();
                item.Name = directoryGovernmentType.Name;
                //item.index = directoryGovernmentType.;
                item.enabled = directoryGovernmentType.Enabled == false ? "No" : "Si";
                data.Add(item);
            }
            return _directoryGovernmentTypeExporterExcel.ExportMatrizToFile(data, input.CheckName, input.CheckEnabled);
        }

        [AbpAuthorize(AppPermissions.Pages_Maintenance_DirectoryGovernmentType_Edit)]
        public async Task Update(DirectoryGovernmentTypeUpdateDto input)
        {
            VerifyCount(await _directoryGovernmentTypeRepository.CountAsync(p => p.Id == input.Id));

            await _directoryGovernmentTypeRepository.UpdateAsync(ValidateEntity(ObjectMapper.Map(input, await _directoryGovernmentTypeRepository.GetAsync(input.Id))));
        }

        private DirectoryGovernmentType ValidateEntity(DirectoryGovernmentType input)
        {
            input.Name.IsValidOrException(DefaultTitleMessage, $"El nombre del tipo de entidad pública es obligatorio");
            input.Name.VerifyTableColumn(
                DirectoryGovernmentTypeConsts.NameMinLength, 
                DirectoryGovernmentTypeConsts.NameMaxLength, 
                DefaultTitleMessage, 
                $"El nombre del tipo de entidad pública no debe exceder los {DirectoryGovernmentTypeConsts.NameMaxLength} caracteres");
            
            return input;
        }
    }
}
