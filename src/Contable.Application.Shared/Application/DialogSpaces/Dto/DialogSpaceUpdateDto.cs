using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.DialogSpaces.Dto
{
    public class DialogSpaceUpdateDto : EntityDto
    {
        public bool ReplaceCode { get; set; }
        public int ReplaceYear { get; set; }
        public int ReplaceCount { get; set; }

        public string CaseName { get; set; }

        public DateTime? EndDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? PublicationDate { get; set; }

        public int? Term { get; set; }
        public int? TermType { get; set; }


        public int? Type { get; set; }

        public EntityDto DialogSpaceType { get; set; }
        public EntityDto Person { get; set; }
        public EntityDto SocialConflict { get; set; }
        public List<DialogSpaceLocationRelationDto> Locations { get; set; }
        public List<DialogSpaceLeaderRelationDto> Leaders { get; set; }
    }
}
