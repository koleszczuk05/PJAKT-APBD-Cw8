using Microsoft.AspNetCore.Mvc;
using PJAKT_APBD_Cw8.DTOs;
using PJAKT_APBD_Cw8.Services;

namespace PJAKT_APBD_Cw8.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController(IDbService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetPatients([FromQuery] string? search, CancellationToken cancellationToken)
    {
        return Ok(await service.GetPatientsAsync(search, cancellationToken));
    }

    [HttpPost("{id}/bedassignments")]
    public async Task<IActionResult> AssignBed(string id, [FromBody] BedAssignmentRequest request, CancellationToken cancellationToken)
    {
        await service.AssignBedAsync(id, request, cancellationToken);
        return StatusCode(201, "Łóżko przypisane pomyślnie.");
    }
}