using RegisterAndLoginApp.Models;
/*
 * Dakoda Meade
 * CST-350
 * Activity 2-2
 * Security Servce calss
 */
namespace RegisterAndLoginApp.Services
{
    public class SecurityService
    {
        //List<UserModel> knownUsers = new List<UserModel>();
        SecurityDAO securityDAO = new SecurityDAO();

        public SecurityService() 
        {
            /*
            knownUsers.Add(new UserModel { id = 0, username = "BillGates", password = "bigbucks"});
            knownUsers.Add(new UserModel { id = 1, username = "MarieCurie", password = "radioactive"});
            knownUsers.Add(new UserModel { id = 2, username = "WatsonCrick", password = "dna"});
            knownUsers.Add(new UserModel { id = 3, username = "AlexanderFlemming", password = "penicillin"});
            */
        }
        // is valid method for validating user
        public bool IsValid(UserModel user) 
        {
            return securityDAO.FindByNameAndPassword(user);
        }
    }
}
