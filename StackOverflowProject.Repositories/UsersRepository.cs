using System;
using System.Collections.Generic;
using System.Linq;
using StackOverflowProject.DomainModels;

namespace StackOverflowProject.Repositories
{
    public interface IUsersRepository
    {
        void InsertUser(User u);
        void UpdateUserDetails(User u);
        void UpdateUserPassword(User u);
        void DeleteUser(int UserID);
        List<User> GetUsers();
        List<User> GetUsersByEmailAndPassword(string email, string password);
        List<User> GetUsersByEmail(string email);
        List<User> GetUsersByUserID(int UserID);
        int GetLatestUserID();
    }
    public class UsersRepository : IUsersRepository
    {
        StackOverflowDatabaseDbContext db;
        public UsersRepository()
        {
            db = new StackOverflowDatabaseDbContext();
        }

        public void DeleteUser(int UserID)
        {
            User us = db.Users.Where(temp => temp.UserID == UserID).FirstOrDefault();
            if(us != null)
            {
                db.Users.Remove(us);
                db.SaveChanges();
            }
        }

        public int GetLatestUserID()
        {
            int uid = db.Users
                .Select(temp => temp.UserID)
                .Max();
            return uid;
        }

        public List<User> GetUsers()
        {
            List<User> us = db.Users
                .Where(temp =>
            temp.IsAdmin == false)
                .OrderBy(temp => temp.Name)
                .ToList();
            return us;
        }

        public List<User> GetUsersByEmail(string email)
        {
            List<User> us = db.Users.Where(temp => temp.Email.Equals(email)).ToList();
            return us;
        }

        public List<User> GetUsersByEmailAndPassword(string email, string password)
        {
            List<User> us = db.Users.Where(temp => temp.Email.Equals(email) && temp.PasswordHash.Equals(password)).ToList();
            return us;
        }

        public List<User> GetUsersByUserID(int UserID)
        {
            List<User> us = db.Users
                .Where(temp => temp.UserID == UserID
                )
                .ToList();
            return us;
        }

        public void InsertUser(User u)
        {
            db.Users.Add(u);
            db.SaveChanges();
        }

        public void UpdateUserDetails(User u)
        {
            User us = db.Users.Where(temp => temp.UserID == u.UserID).FirstOrDefault();
            if (us != null)
            {
                us.Name = u.Name;
                us.Mobile = u.Mobile;
                db.SaveChanges();
            }
        }

        public void UpdateUserPassword(User u)
        {
            User us = db.Users.Where(temp => temp.UserID == u.UserID).FirstOrDefault();
            if(us != null)
            {
                us.PasswordHash = u.PasswordHash;
                db.SaveChanges();
            }
        }
    }
}
