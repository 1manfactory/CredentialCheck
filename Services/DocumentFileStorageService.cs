namespace CredentialCheck.Services;

public class DocumentFileStorageService
{
	private readonly IConfiguration _configuration;
	private readonly IWebHostEnvironment _environment;

	public DocumentFileStorageService(
		IConfiguration configuration,
		IWebHostEnvironment environment)
	{
		_configuration = configuration;
		_environment = environment;
	}

	public async Task<StoredDocumentFile> SaveAsync(IFormFile file)
	{
		var configuredPath = _configuration["FileStorage:DocumentPath"];

		if (string.IsNullOrWhiteSpace(configuredPath))
		{
			throw new InvalidOperationException("File storage path is not configured.");
		}

		var storageRoot = Path.IsPathRooted(configuredPath)
			? configuredPath
			: Path.Combine(_environment.ContentRootPath, configuredPath);

		Directory.CreateDirectory(storageRoot);

		var fileExtension = Path.GetExtension(file.FileName);
		var storedFileName = $"{Guid.NewGuid()}{fileExtension}";
		var storagePath = Path.Combine(storageRoot, storedFileName);

		await using var stream = File.Create(storagePath);
		await file.CopyToAsync(stream);

		return new StoredDocumentFile(
			file.FileName,
			storedFileName,
			file.ContentType,
			file.Length,
			storagePath
		);
	}
}

public record StoredDocumentFile(
	string OriginalFileName,
	string StoredFileName,
	string ContentType,
	long FileSizeBytes,
	string StoragePath
);
