namespace Primal.Models
{
    public class CreateFreeCompanyModel
    {
        public int PlayerId { get; set; }
        public string Name { get; set; }
    }

    public class JoinFreeCompanyModel
    {
        public int PlayerId { get; set; }
        public string Code { get; set; }
    }
}