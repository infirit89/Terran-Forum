using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerranForum.Application.Services;

namespace TerranForum.Infrastructure.Services
{
    public class LocalFileService : IFileService
    {
        public LocalFileService(IHostEnvironment environment)
        {
            _Environtment = environment;

            // temporary; maybe use firebase in the future?
            ContentPath = "/";
            UploadedImagesPath = Path.Join(ContentPath, "uploadedImages");
            if (!Directory.Exists(UploadedImagesPath))
                Directory.CreateDirectory(UploadedImagesPath);
        }

        public string ContentPath { get; private set; } = null!;
        public string UploadedImagesPath { get; private set; }

        private readonly IHostEnvironment _Environtment;
    }
}
