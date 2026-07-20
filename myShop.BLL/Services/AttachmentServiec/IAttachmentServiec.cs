using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.BLL.Services.AttachmentServiec
{
    public interface IAttachmentServiec
    {

        // upload file
        public Task<string?> UploadAsync(Stream fileStream, string filename, string folderPath);

        // delete file
        public bool Delete(string filename , string foldername);

    }
}
