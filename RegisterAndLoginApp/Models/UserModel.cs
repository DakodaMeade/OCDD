namespace RegisterAndLoginApp.Models
{
    /*
 * Dakoda Meade
 * CST-350
 * Activity 2-2
 * User Model class 
 */
    public class UserModel
    {
        // Properties 
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }


        // to string method
        public override string ToString()
        {
            return "Username = " + username + " Password = " + password;
        }

    }
}
