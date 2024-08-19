using DataAccess.Context;
using DataAccess.Dal.Abstract;
using Microsoft.Data.SqlClient;
using Shared.Models.StoreProcedure;
using System.Data;
using System.Text.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Dal.Concrete
{
    public class IrsaliyeDal : IIrsaliyeDal
    {
        public async Task<IrsaliyeProcedure?> GetIrsaliye(string siparisNumarasi)
        {
            using (DataContext _context = new DataContext())
            {
                var connection = _context.Database.GetDbConnection();
                await using var command = connection.CreateCommand();
                command.CommandText = "VB_GetInvoiceByOrderCode";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@SiparisKodu", SqlDbType.NVarChar) { Value = siparisNumarasi });

                if (connection.State == ConnectionState.Closed)
                {
                    await connection.OpenAsync();
                }

                await using var reader = await command.ExecuteReaderAsync();
                StringBuilder jsonStringBuilder = new StringBuilder();

                while (await reader.ReadAsync())
                {
                    var resultJsonPart = reader.GetString(0);
                    jsonStringBuilder.Append(resultJsonPart);
                }
                reader.Close();
                return JsonSerializer.Deserialize<IrsaliyeProcedure>(jsonStringBuilder.ToString());
            }
        }
    }
}
