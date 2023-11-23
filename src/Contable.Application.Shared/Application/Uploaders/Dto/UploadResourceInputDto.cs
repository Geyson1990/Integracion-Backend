using Contable.Application.Records.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.Uploaders.Dto
{
    public class UploadResourceInputDto
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Size { get; set; }
        public string Extension { get; set; }
        public string ClassName { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public string NewName { get; set; }
        public string SectionFolder { get; set; }
        public string NewSectionFolder { get; set; }
        public string Resource { get; set; }
        public bool UpdatePath { get; set; }
        public RecordResourceTypeDto RecordResourceType { get; set; }
        public string Description{ get; set; }

    }
}
