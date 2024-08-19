using Core.DataAccess;
using DataAccess.Context;
using DataAccess.Dal.Abstract;
using Entities.Concrete;
using Entities.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Shared.Models.CreateUpdate;
using Shared.Models.StoreProcedure;
using System.Data;
using System.Text;
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
                command.CommandText = "VB_OrderAndOrderDetails";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@salesmanref", SqlDbType.Int) { Value = LOGICALREF });

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
                return JsonSerializer.Deserialize<OrderResult>(jsonStringBuilder.ToString());
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

        public async Task<Order?> GetOrderBySiparisNumarasi(int userId, string siparisNumarasi)
        {
            using (DataContext _context = new DataContext())
            {
                return await _context.Set<Order>().Include(i => i.OrderDetails).ThenInclude(i => i.Scans).FirstOrDefaultAsync(p => p.UserId == userId && p.siparisNumarasi == siparisNumarasi);
            }
        }

        public async Task<Order?> GetOrderBySiparisNumarasiAndOrderId(int userId, string siparisNumarasi, int? orderId)
        {
            using (DataContext _context = new DataContext())
            {
                return await _context.Set<Order>().Include(i => i.OrderDetails).ThenInclude(i => i.Scans).FirstOrDefaultAsync(x => x.UserId == userId && (x.siparisNumarasi == siparisNumarasi || (orderId != null && x.Id == orderId)));
            }
        }
    }
}
