namespace CredentialCheck.Services;

public class DocumentFileStorageService
{
	private const long MaxFileSizeBytes = 10 * 1024 * 1024;

	private static readonly HashSet<string> AllowedContentTypes = new(StringComparer.OrdinalIgnoreCase)
	{
		"application/pdf",
		"image/jpeg",
		"image/png"
	};

	private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
	{
		".pdf",
		".jpg",
		".jpeg",
		".png"
	};

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
		ValidateFile(file);

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
		var storedFileName = $"{Guid.NewGuid()}{fileExtension.ToLowerInvariant()}";
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

	private static void ValidateFile(IFormFile file)
	{
		if (file.Length <= 0)
		{
			throw new InvalidOperationException("File is empty.");
		}

		if (file.Length > MaxFileSizeBytes)
		{
			throw new InvalidOperationException("File is too large. Maximum size is 10 MB.");
		}

		var extension = Path.GetExtension(file.FileName);

		if (!AllowedExtensions.Contains(extension))
		{
			throw new InvalidOperationException("File extension is not allowed.");
		}

		if (!AllowedContentTypes.Contains(file.ContentType))
		{
			throw new InvalidOperationException("File content type is not allowed.");
		}
	}
}

public record StoredDocumentFile(
	string OriginalFileName,
	string StoredFileName,
	string ContentType,
	long FileSizeBytes,
	string StoragePath
);