namespace API.Entities;

[Table("Photos")]
public class Photo
{
    public int Id { get; set; }
    public string Url { get; set; }
    public bool IsMain { get; set; }
    public bool IsApproved { get; set; }
    public string PublicId { get; set; }
    // Fully defining the relationship with AppUser entity
    public AppUser AppUser { get; set; }
    public int AppUserId { get; set; }
}