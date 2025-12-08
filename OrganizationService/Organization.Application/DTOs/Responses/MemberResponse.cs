namespace Organization.Application.DTOs.Responses;

public class MemberResponse
{
    public bool IsCanChange { get; set; }
    public List<MemberUserResponse> Members { get; set; }
}