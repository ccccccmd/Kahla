﻿using Aiursoft.Pylon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aiursoft.Pylon;

namespace Kahla.Server.Models.ApiViewModels
{
    public class UploadFileViewModel : AiurProtocal
    {
        public string SavedFileName { get; set; }
        public int FileKey { get; set; }
        public long FileSize { get; set; }
    }

    public class UploadImageViewModel : AiurProtocal
    {
        public int FileKey { get; set; }
        public string DownloadPath { get; set; }
    }
}
