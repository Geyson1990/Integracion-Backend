﻿using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.AlertResponsibles.Dto
{
    public class AlertResponsibleGetAllDto : EntityDto
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public bool Tracing { get; set; }
        public bool Enabled { get; set; }
    }
}
