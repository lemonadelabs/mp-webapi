namespace MPWebAPI.Models
{
    public interface IDocumentUser
    {
        MerlinPlanUser User { get; set; }
        string UserId { get; set; }
    }
}