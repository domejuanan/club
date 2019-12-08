using System;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using club.Models;
using club.Repositories;
using club.Resources;
using club.Responses;

namespace club.Services
{
    public interface IUserService
    {
        Task<UserResponse> Authenticate(string email, string password);
        Task<UserListResponse> GetAll(int pageNum = 1, int pageSize = 50);
        Task<UserResponse> GetById(int id);
        Task<UserResponse> Create(UserSaveResource userSave);
        Task<UserResponse> Update(int id, UserSaveResource user);
        Task<UserResponse> Delete(int id);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        
        public async Task<UserResponse> Authenticate(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            var userExist = await _userRepository.FindByEmail(email);

            // check if username exists
            if (userExist == null)
                return new UserResponse(404, "Username incorrect", "Username", "User not found.");

            try
            {
                if (!VerifyPasswordHash(password, userExist.PasswordHash, userExist.PasswordSalt))
                    return new UserResponse(400, "Password incorrect", "Password", "Password incorrect.");
            }
            catch (ArgumentNullException ex)
            {
                return new UserResponse(400, "Invalid password", "Password", ex.Message);
            }
            catch (ArgumentException ex)
            {
                return new UserResponse(400, "Invalid password", "Password", ex.Message);
            }
            catch (Exception ex)
            {
                return new UserResponse(400, "Unexpected error", "Error", ex.Message);
            }

            // authentication successful, return userReponse
            var userDto = _mapper.Map<User, UserResource>(userExist);
            return new UserResponse(userDto);
        }

        public async Task<UserListResponse> GetAll(int pageNum = 1, int pageSize = 50)
        {
            if (pageNum < 1 || pageSize < 1)
                return new UserListResponse(400, "Wrong pagination", "Pagination",
                    "The pageNum and pageSize params must be greater than zero.");
            
            int totalRecords = await _userRepository.CountAsync();
            var users = await _userRepository.ListAsync(pageNum, pageSize);
            var resourcesUsers = _mapper.Map<IEnumerable<User>, IEnumerable<UserResource>>(users);
            var resourceListUsers = new UserListResource(resourcesUsers, pageNum, pageSize, totalRecords);

            return new UserListResponse(resourceListUsers);
        }

        public async Task<UserResponse> GetById(int id)
        {
            var userExist = await _userRepository.FindByIdAsync(id);

            // check if user exists
            if (userExist == null)
                return new UserResponse(404, "User id not found", "Id", "User not found.");

            // return userReponse
            var userDto = _mapper.Map<User, UserResource>(userExist);
            return new UserResponse(userDto);
        }

        public async Task<UserResponse> Create(UserSaveResource userSave)
        {
            // validation
            if (string.IsNullOrWhiteSpace(userSave.Password))
                return new UserResponse(400, "Password required", "Password", "Password is required.");

            var userExist = await _userRepository.FindByEmail(userSave.Email);

            if (userExist != null)
                return new UserResponse(400, "User already exists", "Email", "Email " + userSave.Email + " is already taken");

            try
            {
                byte[] passwordHash;
                byte[] passwordSalt;

                CreatePasswordHash(userSave.Password, out passwordHash, out passwordSalt);

                var user = _mapper.Map<UserSaveResource, User>(userSave);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                await _userRepository.AddAsync(user);

                var userResource = _mapper.Map<User, UserResource>(user);

                return new UserResponse(userResource);

            }
            catch (ArgumentNullException ex)
            {
                return new UserResponse(400, "Invalid password", "Password", ex.Message);
            }
            catch (ArgumentException ex)
            {
                return new UserResponse(400, "Invalid password", "Password", ex.Message);
            }
            catch (Exception ex)
            {
                return new UserResponse(400, "Unexpected error", "Error", ex.Message);
            }
        }

        public async Task<UserResponse> Update(int id, UserSaveResource user)
        {
            var userExist = await _userRepository.FindByIdAsync(id);

            if (userExist == null)
                return new UserResponse(404, "User id not found", "Id", "User not found.");

            if (userExist.Email != user.Email)
            {
                var userExistUsername = await _userRepository.FindByEmail(user.Email);

                // username has changed so check if the new username is already taken
                if (userExistUsername != null)
                    return new UserResponse(400, "User already exists", "Username", "Email " + user.Email + " is already taken");
            }

            // update user properties
            userExist.FirstName = user.FirstName;
            userExist.LastName = user.LastName;
            userExist.Email = user.Email;

            try
            {
                // update password if it was entered
                if (!string.IsNullOrWhiteSpace(user.Password))
                {
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash(user.Password, out passwordHash, out passwordSalt);

                    userExist.PasswordHash = passwordHash;
                    userExist.PasswordSalt = passwordSalt;
                }

                _userRepository.Update(userExist);

                var userResource = _mapper.Map<User, UserResource>(userExist);
                return new UserResponse(userResource);

            }
            catch (Exception ex)
            {
                return new UserResponse(400, "Unexpected error", "Error", ex.Message);
            }
        }

        public async Task<UserResponse> Delete(int id)
        {
            var existingItem = await _userRepository.FindByIdAsync(id);


            if (existingItem == null)
                return new UserResponse(404, "User id not found", "Id", "User not found.");

            try
            {
                _userRepository.Remove(existingItem);
                var responseResource = _mapper.Map<User, UserResource>(existingItem);
                return new UserResponse(responseResource);
            }
            catch (Exception ex)
            {
                return new UserResponse(400, "Unexpected error", "Error", ex.Message);
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

        private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (passwordHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (passwordSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordSalt");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }

            return true;
        }
    }
}