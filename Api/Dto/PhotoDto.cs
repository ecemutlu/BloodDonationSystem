namespace Api.Dto
{
	public class PhotoDto
	{
		public int DonorId { get; set; }
		public required string FileExtension { get; set; }
		public required byte[] Content { get; set; }
	}
}
