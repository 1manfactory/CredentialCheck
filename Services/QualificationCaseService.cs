using CredentialCheck.Data;
using CredentialCheck.Dtos;
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

    public async Task<List<QualificationCase>> GetAllAsync()
    {
        return await _dbContext.QualificationCases
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<QualificationCase?> GetByIdAsync(Guid id)
    {
        return await _dbContext.QualificationCases
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<QualificationCase> CreateAsync(CreateQualificationCaseRequest request)
    {
        var qualificationCase = new QualificationCase
        {
            ApplicantName = request.ApplicantName.Trim(),
            ApplicantEmail = request.ApplicantEmail.Trim(),
            Country = request.Country.Trim(),
            Status = QualificationCaseStatus.Draft,
            CreatedAt = DateTimeOffset.UtcNow
        };

        _dbContext.QualificationCases.Add(qualificationCase);
        await _dbContext.SaveChangesAsync();

        return qualificationCase;
    }
}