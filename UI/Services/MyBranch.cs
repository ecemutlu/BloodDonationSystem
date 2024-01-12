using Api.Models;
using UI.Dto;

namespace UI.Services
{
    public class MyBranch
    {
        public BranchDto Branch { get; } = new BranchDto
        {
            Id = 1,
            Name = "Kızılay İzmir-Bornova Branch",
            CityId = 1,
            TownId = 1,
            Username = "branch1",
            Password = "se4458"
        };

        public LoginDto GetLoginInfo()
        {
            return new LoginDto { Username = Branch.Username, Password = Branch.Password };
        } 
        
    }
}
