namespace PJAKT_APBD_Cw8.DTOs;

public record AdmissionResponse(
    int Id, 
    DateTime AdmissionDate, 
    DateTime? DischargeDate, 
    WardResponse Ward
);