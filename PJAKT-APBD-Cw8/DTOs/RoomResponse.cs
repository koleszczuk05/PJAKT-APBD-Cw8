namespace PJAKT_APBD_Cw8.DTOs;

public record RoomResponse(
    string Id, 
    bool HasTv, 
    WardResponse Ward
);