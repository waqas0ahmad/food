using Food.DAL;
using Food.Extentions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Web;

namespace Food.Models
{
    public class User
    {
        public long Id { get; set; }
        public string DisplayName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserTypes UserType { get; set; }
        private DatabaseManager db=null;

        public User()
        {
            db = new DatabaseManager();
        }
        public void Add()
        {
            string q = $"select * from [user] where username='{Username}'";
            if (db.Select(q).ToListof<User>().Count > 0)
            {
                throw new Exception("User already exists.",new Exception($"Exception in line number: {new StackFrame(1, true).GetFileLineNumber()};"));
            }
            var hasPassword = Password.HashPassword();
            q = $"INSERT INTO [USER](USERNAME,PASSWORD,DISPLAYNAME,USERTYPE)VALUES('{Username}','{hasPassword}','{DisplayName}','{(int)UserType}')";
            try
            {
                db.InsertUpdateDelete(q);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool Verified()
        {
            var hasPassword = Password.HashPassword();
            string q = $"SELECT * FROM [USER] WHERE USERNAME='{Username}'";
            try
            {
                var list=db.Select(q).ToListof<User>();
                if (list.Count > 0)
                {
                    var user=list.FirstOrDefault();
                    if (user != null)
                    {
                        if (user.Username == Username && user.Password == hasPassword)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }
    }
    public enum UserTypes
    {
        Admin = 1,
        Restaurant = 2,
        Company = 3
    }
}