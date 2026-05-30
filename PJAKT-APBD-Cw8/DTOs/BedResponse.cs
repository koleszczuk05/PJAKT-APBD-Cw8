namespace PJAKT_APBD_Cw8.DTOs;

public record BedResponse(
    int Id, 
    BedTypeResponse BedType, 
    RoomResponse Room
);