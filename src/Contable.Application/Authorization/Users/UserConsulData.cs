using Abp.Authorization;
using Abp.Domain.Repositories;
using Contable.Application;
using Contable.Application.CrisisCommittees.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Linq.Extensions;
using Abp.UI;
using Contable.Application.CrisisCommittees;
using Contable.Application.Extensions;
using Contable.Authorization;
using Contable.Authorization.Users;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Abp.Extensions;
using Contable.Authorization.Users.Dto;

namespace Contable.Authorization.Users
{
    [AbpAuthorize]
    public class UserConsulData : ContableAppServiceBase , IUserConsulData
    { 
        private readonly IRepository<SectorContacFocal> _sectorContacFocalRepository;
    public UserConsulData(
        IRepository<SectorContacFocal> sectorContacFocalRepository)
    {
            _sectorContacFocalRepository = sectorContacFocalRepository;
        }

    
        [AbpAuthorize]
        public  UserDNIDto GetUserDataDni(string Dni)
        {
            var user = new UserDNIDto();
            if(Dni == "75171616") {
                user = new UserDNIDto { Nombres = "randal", Materno = "userrr1", Paterno = "el paternmo", DNI = "75171616" };
            }
            return user;
        }

        [AbpAuthorize]
        public string GetUserDataRuc(int Ruc)
        {
            string r = "randall ruct";
            return r;
        }
    }
}
