using DataAccess.Context;
using DataAccess.Dal.Abstract;
using Shared.Models.StoreProcedure;
using System.Data;
using System.Text.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Dal.Concrete
{
    public class BarcodeDal : IBarcodeDal
    {
        public async Task<Barcode?> GetAllBarcodes()
        {
            using (DataContext _context = new DataContext())
            {
                var connection = _context.Database.GetDbConnection();
                await using var command = connection.CreateCommand();
                command.CommandText = "VB_GetAllItemsWithBarcodeAndUnit";
                command.CommandType = CommandType.StoredProcedure;

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
                return JsonSerializer.Deserialize<Barcode>(jsonStringBuilder.ToString());
            }
        }
    }
}
