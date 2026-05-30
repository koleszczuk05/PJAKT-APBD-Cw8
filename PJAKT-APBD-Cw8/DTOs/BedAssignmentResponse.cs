namespace PJAKT_APBD_Cw8.DTOs;

public record BedAssignmentResponse(
    int Id, 
    DateTime From, 
    DateTime? To, 
    BedResponse Bed
);