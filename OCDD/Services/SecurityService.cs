﻿using OCDD.Models;

/*
 * Dakoda Meade
 * Security Servce calss
 * handles the interaction between the controllers and the secuirty DAO
 */
namespace OCDD.Services
{
	public class SecurityService
	{
        // initailize DAO
        SecurityDAO securityDAO = new SecurityDAO();

        // Method for validating a user trying to login
        public bool IsValid(UserModel user)
        {
            return securityDAO.FindByEmailAndPassword(user);
        }

        // Method for finding if the user already exists
        public bool UserExists(UserModel user)
        {
            return securityDAO.FindUserByEmail(user);
        }

        // Method for adding new user
        public void AddUser(UserModel user)
        {
            securityDAO.AddUser(user);
        }


        // Method for getting the user id
        public int GetUserId(UserModel user)
        {
            return securityDAO.FindUserIDByEmail(user);
        }

        //Method for getting the returninf the user from the id
        public UserModel GetUserById(int userId)
        {
            return securityDAO.GetUserById(userId);
        }

        // updates the user personal inforamtion
        public void UpdateProfile(UserModel user) 
        {
            securityDAO.UpdateProfile(user);
        }
        // update the users password
        public void UpdatePassword(UserModel user)
        {
            securityDAO.UpdatePassword(user);
        }
    }
}
