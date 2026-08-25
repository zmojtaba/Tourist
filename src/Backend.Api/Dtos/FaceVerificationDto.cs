namespace Backend.Api.Dtos
{
    public class FaceVerificationDto
    {
        public Guid UserId { get; set; }
        public List<IFormFile> Images { get; set; }
        
    }
}
