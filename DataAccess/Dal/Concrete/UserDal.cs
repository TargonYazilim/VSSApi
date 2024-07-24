using Core.DataAccess;
using DataAccess.Context;
using DataAccess.Dal.Abstract;
using Entities;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;

namespace DataAccess.Dal.Concrete
{
    public class UserDal : EntityRepository<User, DataContext>, IUserDal
    {
        public async Task<BaseResult?> Login(User user)
        {

            using (DataContext _context = new DataContext())
            {
                var connection = _context.Database.GetDbConnection();
                await using var command = connection.CreateCommand();
                command.CommandText = "UserRead";
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
                    return JsonSerializer.Deserialize<BaseResult>(resultJson);
                }
                return null;
            }
        }
    }
}