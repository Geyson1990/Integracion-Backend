using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Contable.Application.DirectoryIndustries;
using Contable.Application.DirectoryIndustries.Dto;
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
using Abp.UI;
using Contable.Application.Exporting;
using Contable.Application.Exporting.Dto;
using Contable.Dto;
using Contable.Application.DirectoryGovernments.Dto;

namespace Contable.Application
{
    [AbpAuthorize(AppPermissions.Pages_Catalog_DirectoryIndustry)]
    public class DirectoryIndustryAppService : ContableAppServiceBase, IDirectoryIndustryAppService
    {
        private readonly IRepository<DirectoryIndustry> _directoryIndustryRepository;
        private readonly IRepository<DirectorySector> _directorySectorRepository;
        private readonly IRepository<Department> _departmentRepository;
        private readonly IRepository<District> _districtRepository;
        private readonly IDirectoryIndustryExcelExporter _directoryIndustryExcelExporter;
        public DirectoryIndustryAppService(
            IRepository<DirectoryIndustry> directoryIndustryRepository,
            IRepository<DirectorySector> directorySectorRepository, 
            IRepository<Department> departmentRepository, 
            IRepository<District> districtRepository,
            IDirectoryIndustryExcelExporter directoryIndustryExcelExporter)
        {
            _directoryIndustryRepository = directoryIndustryRepository;
            _directorySectorRepository = directorySectorRepository;
            _departmentRepository = departmentRepository;
            _districtRepository = districtRepository;
            _directoryIndustryExcelExporter = directoryIndustryExcelExporter;
        }

        [AbpAuthorize(AppPermissions.Pages_Catalog_DirectoryIndustry_Create)]
        public async Task Create(DirectoryIndustryCreateDto input)
        {
            await _directoryIndustryRepository.InsertAsync(await ValidateEntity(
                directoryIndustry: ObjectMapper.Map<DirectoryIndustry>(input),
                districtId: input.District == null ? -1 : input.District.Id,
                directorySectoryId: input.DirectorySector == null ? -1 : input.DirectorySector.Id));
        }

        [AbpAuthorize(AppPermissions.Pages_Catalog_DirectoryIndustry_Delete)]
        public async Task Delete(EntityDto input)
        {
            VerifyCount(await _directoryIndustryRepository.CountAsync(p => p.Id == input.Id));

            await _directoryIndustryRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_Catalog_DirectoryIndustry)]
        public async Task<DirectoryIndustryGetDataDto> Get(NullableIdDto input)
        {
            var output = new DirectoryIndustryGetDataDto();

            if (input.Id.HasValue)
            {
                VerifyCount(await _directoryIndustryRepository.CountAsync(p => p.Id == input.Id));

                output.DirectoryIndustry = ObjectMapper.Map<DirectoryIndustryGetDto>(_directoryIndustryRepository
                    .GetAll()
                    .Include(p => p.DirectorySector)
                    .Include(p => p.District)
                    .ThenInclude(p => p.Province)
                    .ThenInclude(p => p.Department)
                    .Where(p => p.Id == input.Id)
                    .First());
            }

            output.Departments = ObjectMapper.Map<List<DirectoryIndustryDepartmentDto>>(_departmentRepository
                .GetAll()
                .Include(p => p.Provinces)
                .ThenInclude(p => p.Districts)
                .OrderBy(p => p.Name)
                .ToList())
                .Select(p =>
                {
                    p.Provinces = p.Provinces.OrderBy(p => p.Name).ToList();

                    foreach(var province in p.Provinces)
                    {
                        province.Districts = province
                            .Districts
                            .OrderBy(p => p.Name)
                            .ToList();
                    }

                    return p;
                }).ToList();

            output.Sectors = ObjectMapper.Map<List<DirectoryIndustrySectorDto>>(_directorySectorRepository
                .GetAll()
                .OrderBy(p => p.Name)
                .ToList());

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Catalog_DirectoryIndustry)]
        public async Task<PagedResultDto<DirectoryIndustryGetAllDto>> GetAll(DirectoryIndustryGetAllInputDto input)
        {
            var query = _directoryIndustryRepository
                .GetAll()
                .Include(p => p.DirectorySector)
                .Include(p => p.District)
                .ThenInclude(p => p.Province)
                .ThenInclude(p => p.Department)
                .LikeAllBidirectional(input
                    .Filter
                    .SplitByLike()
                    .Select(word => (Expression<Func<DirectoryIndustry, bool>>)(expression => EF.Functions.Like(expression.Name, $"%{word}%"))).ToArray());

            var count = await query.CountAsync();
            var output = query.OrderBy(input.Sorting).PageBy(input);

            return new PagedResultDto<DirectoryIndustryGetAllDto>(count, ObjectMapper.Map<List<DirectoryIndustryGetAllDto>>(output));
        }
        [AbpAuthorize(AppPermissions.Pages_Catalog_DirectoryIndustry)]
        public async Task<FileDto> GetDirectoryIndustryMatrizToExcel(DirectoryIndustryGetAllInputDto input)
        {
            var query = _directoryIndustryRepository
               .GetAll()
               .Include(p => p.DirectorySector)
               .Include(p => p.District)
               .ThenInclude(p => p.Province)
               .ThenInclude(p => p.Department)
               .LikeAllBidirectional(input
                   .Filter
                   .SplitByLike()
                   .Select(word => (Expression<Func<DirectoryIndustry, bool>>)(expression => EF.Functions.Like(expression.Name, $"%{word}%"))).ToArray());
            
            var output = await query.OrderBy(input.Sorting).ToListAsync();
            var data = new List<DirectoryIndustryExcelExportDto>();

            foreach (var directoryIndustry in output)
            {
                var item = new DirectoryIndustryExcelExportDto();
                item.NameEmpresa = directoryIndustry.Name;
                item.Sector = directoryIndustry.DirectorySector.Name;
                item.Address = directoryIndustry.Address;
                item.PhoneNumber = directoryIndustry.PhoneNumber;
                item.Url = directoryIndustry.Url;
                item.Departamento = directoryIndustry.District.Province.Department.Name;
                item.Departamento = directoryIndustry.District.Province.Name;
                item.Distrito = directoryIndustry.District.Name;
                item.Habilitado = directoryIndustry.Enabled == false ? "No" : "Si";
                //item.CheckName = input.CheckName;
                data.Add(item);
            }

            return _directoryIndustryExcelExporter.ExportMatrizToFile(data, input.checkNameEmpresa, input.checkSector, input.checkDireccion, input.checkTelefono, input.checkPaginaWeb, input.checkDepartamento, input.checkProvincia, input.checkDistrito, input.CheckHabilitado);
        }
        [AbpAuthorize(AppPermissions.Pages_Catalog_DirectoryIndustry_Edit)]
        public async Task Update(DirectoryIndustryUpdateDto input)
        {
            VerifyCount(await _directoryIndustryRepository.CountAsync(p => p.Id == input.Id));

            await _directoryIndustryRepository.UpdateAsync(await ValidateEntity(
                directoryIndustry: ObjectMapper.Map<DirectoryIndustry>(input),
                districtId: input.District == null ? -1 : input.District.Id,
                directorySectoryId: input.DirectorySector == null ? -1 : input.DirectorySector.Id));
        }

        private async Task<DirectoryIndustry> ValidateEntity(DirectoryIndustry directoryIndustry, int districtId, int directorySectoryId)
        {
            directoryIndustry.Name.IsValidOrException(DefaultTitleMessage, $"El nombre de la empresa privada es obligatoria");
            directoryIndustry.Name.VerifyTableColumn(DirectoryIndustryConsts.NameMinLength,
                DirectoryIndustryConsts.NameMaxLength,
                DefaultTitleMessage,
                $"El nombre de la empresa privada no debe exceder los {DirectoryIndustryConsts.NameMaxLength} caracteres");

            directoryIndustry.PhoneNumber.VerifyTableColumn(DirectoryIndustryConsts.PhoneNumberMinLength,
                DirectoryIndustryConsts.PhoneNumberMaxLength,
                DefaultTitleMessage,
                $"El teléfono no debe exceder los {DirectoryIndustryConsts.PhoneNumberMaxLength} caracteres");
            directoryIndustry.EmailAddress.VerifyTableColumn(DirectoryIndustryConsts.EmailAddressMinLength,
                DirectoryIndustryConsts.EmailAddressMaxLength,
                DefaultTitleMessage,
                $"El correo electrónico no debe exceder los {DirectoryIndustryConsts.EmailAddressMaxLength} caracteres");
            directoryIndustry.Url.VerifyTableColumn(DirectoryIndustryConsts.UrlMinLength,
                DirectoryIndustryConsts.UrlMaxLength,
                DefaultTitleMessage,
                $"La página web no debe exceder los {DirectoryIndustryConsts.UrlMaxLength} caracteres");
            directoryIndustry.Address.VerifyTableColumn(DirectoryIndustryConsts.AddressMinLength,
                DirectoryIndustryConsts.AddressMaxLength,
                DefaultTitleMessage,
                $"La dirección no debe exceder los {DirectoryIndustryConsts.AddressMaxLength} caracteres");
            directoryIndustry.AdditionalInformation.VerifyTableColumn(DirectoryIndustryConsts.AdditionalInformationMinLength,
                DirectoryIndustryConsts.AdditionalInformationMaxLength,
                DefaultTitleMessage,
                $"La información adicional no debe exceder los {DirectoryIndustryConsts.AdditionalInformationMaxLength} caracteres");

            if (await _districtRepository.CountAsync(p => p.Id == districtId) == 0)
                throw new UserFriendlyException("Aviso", "El distrito seleccionado ya no existe o fue eliminado. Verifique la información antes de continuar");

            var district = await _districtRepository.GetAsync(districtId);

            directoryIndustry.District = district;
            directoryIndustry.DistrictId = district.Id;

            if (await _directorySectorRepository.CountAsync(p => p.Id == directorySectoryId) == 0)
                throw new UserFriendlyException("Aviso", "El sector seleccionado ya no existe o fue eliminado. Verifique la información antes de continuar");

            var sector = await _directorySectorRepository.GetAsync(directorySectoryId);

            directoryIndustry.DirectorySector = sector;
            directoryIndustry.DirectorySectorId = sector.Id;

            return directoryIndustry;
        }
    }
}
