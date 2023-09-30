using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Constants;
using SerenityHospital.Business.Exceptions.Images;
using SerenityHospital.Business.Extensions;
using SerenityHospital.Business.ExternalServices.Interfaces;
using Microsoft.Extensions.Hosting;

namespace SerenityHospital.Business.ExternalServices.Implements;

public class FileService:IFileService
{
    private readonly IHostEnvironment _env;

    public FileService(IHostEnvironment env)
    {
        _env = env;
    }

    public void Delete(string? path)
    {
        if (String.IsNullOrEmpty(path) || String.IsNullOrWhiteSpace(path)) throw new ImagePathIsNullOrWhiteSpaceException();
        if (!path.StartsWith(RootConstant.Root))
            path = Path.Combine(RootConstant.Root, path);
        if (File.Exists(path))
            File.Delete(path);
    }

    public async Task SaveAsync(IFormFile file, string path)
    {
        using FileStream fs = new FileStream(Path.Combine(RootConstant.Root, path), FileMode.Create);
    }

    private string _renameFile(IFormFile file)
        => Guid.NewGuid() + Path.GetExtension(file.FileName);

    private void _checkDirectory(string path)
    {
        if (!Directory.Exists(Path.Combine(RootConstant.Root, path)))
        {
            Directory.CreateDirectory(Path.Combine(RootConstant.Root, path));
        }
    }

    public async Task<string> UploadAsync(IFormFile file, string path, string contentType = "image", int mb = 3)
    {
        if (!file.IsSizeValid(mb)) throw new SizeNotValidException();
        if (!file.IsTypeValid(contentType)) throw new TypeNotValidException();

        string newFileName = _renameFile(file);
        _checkDirectory(path);
        path = Path.Combine(path, newFileName);
        await SaveAsync(file, path);
        return path;
    }
}

