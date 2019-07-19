using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureSerchService.Models
{
    public class blobSearch
    {
      
        public string content { get; set; }       
        public string  metadata_storage_path { get; set; }
        public string metadata_content_type { get; set; }
    }
}