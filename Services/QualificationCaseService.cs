using CredentialCheck.Data;
using CredentialCheck.Dtos;
using CredentialCheck.Mappers;
using CredentialCheck.Models;
using Microsoft.EntityFrameworkCore;

namespace CredentialCheck.Services;

public class QualificationCaseService
{
	private readonly AppDbContext _dbContext;

	public QualificationCaseService(AppDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<List<QualificationCaseResponse>> GetAllAsync()
	{
		var cases = await _dbContext.QualificationCases
			.OrderByDescending(c => c.CreatedAt)
			.ToListAsync();

		return cases
			.Select(QualificationCaseMapper.ToResponse)
			.ToList();
	}

	public async Task<QualificationCaseResponse?> GetByIdAsync(Guid id)
	{
		var qualificationCase = await _dbContext.QualificationCases
			.FirstOrDefaultAsync(c => c.Id == id);

		return qualificationCase is null
			? null
			: QualificationCaseMapper.ToResponse(qualificationCase);
	}

	public async Task<QualificationCaseResponse> CreateAsync(CreateQualificationCaseRequest request)
	{
		var normalizedEmail = request.ApplicantEmail.Trim().ToLowerInvariant();

		var emailExists = await _dbContext.QualificationCases
			.AnyAsync(c => c.ApplicantEmail == normalizedEmail);

		if (emailExists)
		{
			throw new InvalidOperationException("A qualification case with this email already exists.");
		}

		var qualificationCase = new QualificationCase
		{
			ApplicantName = request.ApplicantName.Trim(),
			ApplicantEmail = normalizedEmail,
			Country = request.Country.Trim(),
			Status = QualificationCaseStatus.Draft,
			CreatedAt = DateTimeOffset.UtcNow
		};

		_dbContext.QualificationCases.Add(qualificationCase);
		await _dbContext.SaveChangesAsync();

		return QualificationCaseMapper.ToResponse(qualificationCase);
	}
}