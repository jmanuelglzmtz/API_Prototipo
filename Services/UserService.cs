using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApi.Dtos;
using WebApi.Entities;
using WebApi.Helpers;


namespace WebApi.Services
{
    public interface IUserService
    {
        UserRolModulePermissionDto Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        User GetById(Guid id);
        User GetByUserName(string userName);
        User Create(User user, string password);
        void Update(User user, string password = null);
        void Delete(int id);
    }

    public class UserService : IUserService
    {
        private DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }

        public UserRolModulePermissionDto Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;
              
            AppUtilities.tenantChange(_context);  
            //usuario
            User user = _context.Users.SingleOrDefault(x => x.UserName == username);                                      

            // check if username exists
            if (user == null)
                return null;                      

            // check if password is correct and update de hash 
            if (!VerifyPasswordWithUpdate(password, user))
                return null;                

            
            var userModulePermission = (from u in _context.Users.Where(x=>x.Id == user.Id)
                    join ur in _context.UserRole on u.Id equals ur.UserId
                    join r in _context.Role on ur.RoleId equals r.Id
                    join rm in _context.RoleModule on r.Id equals rm.RoleId
                    join m in _context.Module on rm.ModuleId equals m.Id   
                    join rp in _context.RolePermission on r.Id equals rp.RoleId
                    join p in _context.Permission on rp.PermissionId equals p.Id   
                    select new {  u.UserName,  roleName = r.Name , moduleName = m.Name, typeName = p.Type, iconName = m.Icon, FirstName = u.FirstName, ImageProfile=u.ImageProfile  }).ToList()
                    ; 
            UserRolModulePermissionDto myUser = new UserRolModulePermissionDto();
            
            
            
            var mygroup = userModulePermission
                    .GroupBy(y=>y.UserName)                    
                    .ToList()
                    ;
            List<RolDto> lstMyRoles = new List<RolDto>();    
            foreach(var group in mygroup)
            {
                RolDto myRoles = new RolDto();  
                var myRol = userModulePermission
                                .Where(y=>y.UserName == group.Key)
                                .GroupBy(y=>y.roleName)                                
                                .ToList();
                
                List<ModuleDto> lstMyModules = new List<ModuleDto>();
                foreach (var rol in myRol)
                {
                    

                    var mymodule = userModulePermission
                                    .Where(y=>(y.UserName==group.Key && y.roleName==rol.Key) )
                                    .GroupBy(y=>y.moduleName)
                                    .ToList();
                    foreach(var module in mymodule)
                    {
                        ModuleDto myModules = new ModuleDto();
                        TypeDto myTypes = new TypeDto();
                        myTypes.TypeName= new List<string>();

                        var myType = userModulePermission
                                        .Where(y=>(y.UserName==group.Key && y.roleName==rol.Key && y.moduleName==module.Key) )                                        
                                        .ToList();
                        
                        foreach(var type in myType)
                        {
                            string ty;
                            ty=type.typeName;
                            myTypes.TypeName.Add(ty);
                        }

                        myModules.Types=myTypes;                        

                        myModules.IconName = userModulePermission
                                        .Where(y=>(y.UserName==group.Key && y.roleName==rol.Key && y.moduleName==module.Key) )                                        
                                        .FirstOrDefault().iconName;
                        
                        myModules.ModuleName=module.Key;
                        
                        lstMyModules.Add(myModules);
                    }                                        
                    myRoles.RolName=rol.Key;
                    myRoles.Modules=lstMyModules;
                    lstMyRoles.Add(myRoles);
                }
                myUser.FirstName=group.FirstOrDefault().FirstName;

                myUser.ImageProfile = group.FirstOrDefault().ImageProfile;

                myUser.UserName=group.Key;

                myUser.Roles=lstMyRoles;
            }
            //var json = Newtonsoft.Json.JsonConvert.SerializeObject(myUser);

            AppUtilities.tenantClose(_context);
            // authentication successful            
            return myUser;
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users;
        }

        public User GetById(Guid id)
        {
            return _context.Users.Find(id);
        }
        public User GetByUserName(string userName)
        {
            AppUtilities.tenantChange(_context);
            return _context.Users.Where(x=>x.UserName == userName).FirstOrDefault();
        }
        public User Create(User user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (_context.Users.Any(x => x.UserName == user.UserName))
                throw new AppException("Username \"" + user.UserName + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public void Update(User userParam, string password = null)
        {
            var user = _context.Users.Find(userParam.Id);

            if (user == null)
                throw new AppException("User not found");

            if (userParam.UserName != user.UserName)
            {
                // username has changed so check if the new username is already taken
                if (_context.Users.Any(x => x.UserName == userParam.UserName))
                    throw new AppException("Username " + userParam.UserName + " is already taken");
            }

            // update user properties
            user.FirstName = userParam.FirstName;
            user.LastName = userParam.LastName;
            user.UserName = userParam.UserName;

            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        // private helper methods

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    
        private bool VerifyPasswordWithUpdate(string password, User user)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (user.PasswordHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (user.PasswordSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(user.PasswordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != user.PasswordHash[i]) return false;
                }
            }

            Update(user,password);

            return true;
        }
 
    }
}