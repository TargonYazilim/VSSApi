using Core.DataAccess;
using DataAccess.Context;
using DataAccess.Dal.Abstract;
using Entities.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Shared.Models.StoreProcedure;
using System.Data;
using System.Text.Json;

namespace DataAccess.Dal.Concrete
{
    public class OrderDal : EntityRepository<Order, DataContext>, IOrderDal
    {

        public async Task<OrderResult?> GetOrderProcedure(int LOGICALREF)
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

        public async Task<OrderDetailResult?> GetOrderDetailProcedure(string SiparisNumarasi)
        {
            using (DataContext _context = new DataContext())
            {
                var connection = _context.Database.GetDbConnection();
                await using var command = connection.CreateCommand();
                command.CommandText = "OrderLineRead";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@SiparisNumarasi", SqlDbType.NVarChar) { Value = SiparisNumarasi });

                if (connection.State == ConnectionState.Closed)
                {
                    await connection.OpenAsync();
                }

                await using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    var resultJson = reader.GetString(0);
                    return JsonSerializer.Deserialize<OrderDetailResult>(resultJson);
                }
                return null;
            }
        }

        public async Task<OrderBarcodeScanResult?> ScanOrderBarcodeProcedure(string Barkod)
        {
            using (DataContext _context = new DataContext())
            {
                var connection = _context.Database.GetDbConnection();
                await using var command = connection.CreateCommand();
                command.CommandText = "GetItemByBarcode";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@Barkod", SqlDbType.NVarChar) { Value = Barkod });

                if (connection.State == ConnectionState.Closed)
                {
                    await connection.OpenAsync();
                }

                await using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    var resultJson = reader.GetString(0);
                    return JsonSerializer.Deserialize<OrderBarcodeScanResult>(resultJson);
                }
                return null;
            }
        }

        public async Task<Order?> GetOrderBySiparisNumarasi(string SiparisNumarasi, int userId)
        {
            using (DataContext _context = new DataContext())
            {
                return await _context.Set<Order>().Include(i => i.OrderDetails).FirstOrDefaultAsync(p => p.siparisNumarasi == SiparisNumarasi && p.UserId == userId);
            }
        }
    }
}
