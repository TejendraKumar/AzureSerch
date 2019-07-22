using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureSerchService.Models
{
    public class blobSearch
    {
      
        public string Content { get; set; }     
        public string ContentType { get; set; }
        public string SasToken { get; set; }
        public string  metadata_storage_path { get; set; }
        public bool IsAlreadyDecoded { get; set; }
        public string metadata_content_type { get; set; }
    }
}