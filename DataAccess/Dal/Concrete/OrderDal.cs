using DataAccess.Context;
using DataAccess.Dal.Abstract;
using Entities.Concrete;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Shared.Models.StoreProcedure;
using System.Data;
using System.Text.Json;

namespace DataAccess.Dal.Concrete
{
    public class OrderDal : IOrderDal
    {
        public async Task<OrderResult?> GetOrder(int LOGICALREF)
        {

            using (DataContext _context = new DataContext())
            {
                var connection = _context.Database.GetDbConnection();
                await using var command = connection.CreateCommand();
                command.CommandText = "OrderRead";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@salesmanref", SqlDbType.Int) { Value = LOGICALREF });

                if (connection.State == ConnectionState.Closed)
                {
                    await connection.OpenAsync();
                }

                await using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    var resultJson = reader.GetString(0);
                    return JsonSerializer.Deserialize<OrderResult>(resultJson);
                }
                return null;
            }
        }
    }
}
