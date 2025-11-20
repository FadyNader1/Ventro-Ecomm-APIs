using Ecomm.Errors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.FileProviders;
using System.Threading.Tasks;

namespace Ecomm.Helper
{
    public   class ImageSetting
    {
        public readonly IFileProvider fileProvider;

        public ImageSetting(IFileProvider fileProvider)
        {
            this.fileProvider = fileProvider;
        }
        public static   List<string> saveImage(IFormFileCollection files,string src)
        {
            var Images = new List<string>();
            var folderpath = Path.Combine("wwwroot", "Images", src);
            if (!Directory.Exists(folderpath))
                Directory.CreateDirectory(folderpath);

            if (files.Count > 0)
            {
                foreach(var file in files)
                {
                    var filename = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    var filePath= Path.Combine(folderpath,filename);
                  
                        using(var stream=new FileStream(filePath,FileMode.Create))
                        {
                             file.CopyTo(stream);
                        }
                    var dbPath = $"Images/{src}/{filename}";
                    Images.Add(dbPath);
                }
            }
            return Images;
        }

        public  void DeleteImage(string src)
        {
            var info= fileProvider.GetFileInfo(src);
            var path= info.PhysicalPath;
            Console.WriteLine("Exists: " + info.Exists);
            Console.WriteLine("PhysicalPath: " + info.PhysicalPath);
            if (File.Exists(path))
                File.Delete(path);
            return;

        }
             
    }
}
