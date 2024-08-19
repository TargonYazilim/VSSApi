using Core.DataAccess;
using DataAccess.Context;
using DataAccess.Dal.Abstract;
using Entities;
using Entities.Concrete;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Login;
using Shared.Models.StoreProcedure;
using System.Data;
using System.Text.Json;

namespace DataAccess.Dal.Concrete
{
    public class UserDal : EntityRepository<User, DataContext>, IUserDal
    {
        public async Task<(bool hasMacAddress, int? Id)> CheckMacAddressAsync(string macAddress)
        {
            using (DataContext _context = new DataContext())
            {
                var result = await _context.Set<User>()
                    .Where(x => x.MACADDRESS == macAddress).Select(item => new User()
                    {
                        Id = item.Id,
                        MACADDRESS = item.MACADDRESS,
                    }).FirstOrDefaultAsync();
                var hasMacAddress = result == null ? false : true;
                return (hasMacAddress, result?.Id);
            }
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            using (DataContext _context = new DataContext())
            {
                return await _context.Set<User>()
                    .Where(x => x.username == username).FirstOrDefaultAsync();
            }
        }

        public async Task<LoginStoreProcedureResult?> Login(User user)
        {

            using (DataContext _context = new DataContext())
            {
                var connection = _context.Database.GetDbConnection();
                await using var command = connection.CreateCommand();
                command.CommandText = "VB_UserRead";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@username", SqlDbType.NVarChar, 50) { Value = user.username });
                command.Parameters.Add(new SqlParameter("@password", SqlDbType.NVarChar, 50) { Value = user.password });

                if (connection.State == ConnectionState.Closed)
                {
                    await connection.OpenAsync();
                }

                await using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    var resultJson = reader.GetString(0);
                    return JsonSerializer.Deserialize<LoginStoreProcedureResult>(resultJson);
                }
                return null;
            }
        }
    }
}