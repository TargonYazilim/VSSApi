namespace Entities.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string MACADDRESS { get; set; }
        public string? role { get; set; }
        public int LOGICALREF { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
