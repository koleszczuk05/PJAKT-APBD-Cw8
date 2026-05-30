using PJAKT_APBD_Cw8.DTOs;

namespace PJAKT_APBD_Cw8.Services;

public interface IDbService
{
    Task<IEnumerable<PatientResponse>> GetPatientsAsync(string? search, CancellationToken cancellationToken);
    Task AssignBedAsync(string patientPesel, BedAssignmentRequest request, CancellationToken cancellationToken);
}