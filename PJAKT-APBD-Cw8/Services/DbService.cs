using Microsoft.EntityFrameworkCore;
using PJAKT_APBD_Cw8.DTOs;
using PJAKT_APBD_Cw8.Exceptions;
using PJAKT_APBD_Cw8.Infrastructure;
using PJAKT_APBD_Cw8.Models;

namespace PJAKT_APBD_Cw8.Services;

public class DbService(HospitalContext ctx) : IDbService
{
    public async Task<IEnumerable<PatientResponse>> GetPatientsAsync(string? search, CancellationToken cancellationToken)
    {
        var query = ctx.Patients.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p => 
                EF.Functions.Like(p.FirstName, $"%{search}%") || 
                EF.Functions.Like(p.LastName, $"%{search}%"));
        }

        return await query.Select(p => new PatientResponse(
            p.Pesel, 
            p.FirstName, 
            p.LastName, 
            p.Age, 
            p.Sex ? "Male" : "Female",
            p.Admissions.Select(a => new AdmissionResponse(
                a.Id, 
                a.AdmissionDate, 
                a.DischargeDate, 
                new WardResponse(a.Ward.Id, a.Ward.Name, a.Ward.Description))),
            p.BedAssignments.Select(ba => new BedAssignmentResponse(
                ba.Id, 
                ba.From, 
                ba.To, 
                new BedResponse(
                    ba.Bed.Id, 
                    new BedTypeResponse(ba.Bed.BedType.Id, ba.Bed.BedType.Name, ba.Bed.BedType.Description), 
                    new RoomResponse(ba.Bed.RoomId, ba.Bed.Room.HasTv, new WardResponse(ba.Bed.Room.WardId, ba.Bed.Room.Ward.Name, ba.Bed.Room.Ward.Description)))))
        )).ToListAsync(cancellationToken);
    }

    public async Task AssignBedAsync(string patientPesel, BedAssignmentRequest request, CancellationToken cancellationToken)
    {
        if (!await ctx.Patients.AnyAsync(p => p.Pesel == patientPesel, cancellationToken))
            throw new NotFoundException("Nie znaleziono pacjenta o podanym PESEL.");

        var freeBed = await ctx.Beds
            .Where(b => b.BedType.Name == request.BedType && b.Room.Ward.Name == request.Ward)
            .FirstOrDefaultAsync(b => !b.BedAssignments.Any(a => 
                (request.To == null || a.From < request.To) && (a.To == null || a.To > request.From)), cancellationToken);

        if (freeBed == null)
            throw new NotFoundException("Brak wolnego łóżka wymaganego typu na wskazanym oddziale w podanym terminie.");

        if (request.To.HasValue && request.From >= request.To.Value)
            throw new BadRequestException("Data zakończenia nie może być wcześniejsza niż data rozpoczęcia.");

        ctx.BedAssignments.Add(new BedAssignment 
        { 
            PatientPesel = patientPesel, 
            BedId = freeBed.Id, 
            From = request.From, 
            To = request.To 
        });
        
        await ctx.SaveChangesAsync(cancellationToken);
    }
}