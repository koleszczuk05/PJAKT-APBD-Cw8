namespace PJAKT_APBD_Cw8.DTOs;

public record BedAssignmentRequest(
    DateTime From, 
    DateTime? To, 
    string BedType, 
    string Ward
);