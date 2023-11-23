using System;
namespace Contable.Application.DialogSpaces.Dto
{
    public class DialogSpaceReportGetDataDto
    {

        public int Id { get; set; }

        public string DialogSpaceCaseName { get; set; }
        public string DialogSpaceCode { get; set; }
        
        public string SocialConflictCode { get; set; }
        public string SocialConflictCaseName { get; set; }

        public string UnidadTerritorialName { get; set; }
        public string UnidadTerritorialUbigeo { get; set; }



        public string SectorName { get; set; }
        public string SectorUbigeo { get; set; }

        public DateTime? EndDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? PublicationDate { get; set; }
        public DateTime? CreationTime { get; set; }

        public string RegisteredUser { get; set; }
        public DateTime? LastModificationTime { get; set; }

        public string ChangeUser { get; set; }

        public int? CountOpenCommitments { get; set; }

        public int? CountClosedCommitments { get; set; }

    }
}
