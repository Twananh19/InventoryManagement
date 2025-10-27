using System;
using System.Threading.Tasks;
using GoodManagement.Models;
using GoodManagement.Services;
using Newtonsoft.Json;

namespace GoodManagement.Services
{
    /// <summary>
    /// Service để ghi audit log tự động
    /// THÀNH VIÊN 1: Audit Service
    /// TODO: Integrate vào tất cả CRUD operations
    /// Cách sử dụng:
    /// await AuditService_T1.LogAsync(userId, "CREATE", "Products", productId, null, JsonConvert.SerializeObject(product));
    /// </summary>
    public static class AuditService_T1
    {
        public static async Task LogAsync(
            int userId,
            string action,
            string tableName,
            int? recordId,
            object? oldValue,
            object? newValue,
            string? description = null)
        {
            try
            {
                using var context = new AppDbContext();

                var auditLog = new AuditLog_T1
                {
                    UserId = userId,
                    Action = action,
                    TableName = tableName,
                    RecordId = recordId,
                    OldValue = oldValue != null ? JsonConvert.SerializeObject(oldValue) : null,
                    NewValue = newValue != null ? JsonConvert.SerializeObject(newValue) : null,
                    Description = description,
                    IpAddress = GetLocalIPAddress(),
                    Timestamp = DateTime.Now
                };

                context.Set<AuditLog_T1>().Add(auditLog);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log error but don't throw - audit failure shouldn't break main operation
                Console.WriteLine($"Audit log failed: {ex.Message}");
            }
        }

        public static async Task LogLoginAsync(int userId, bool success)
        {
            var action = success ? "LOGIN" : "LOGIN_FAILED";
            var description = success ? "User logged in successfully" : "Failed login attempt";
            
            await LogAsync(userId, action, "Users", userId, null, null, description);
        }

        public static async Task LogLogoutAsync(int userId)
        {
            await LogAsync(userId, "LOGOUT", "Users", userId, null, null, "User logged out");
        }

        private static string GetLocalIPAddress()
        {
            try
            {
                var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
                return "127.0.0.1";
            }
            catch
            {
                return "Unknown";
            }
        }

        /// <summary>
        /// Log CREATE operation
        /// </summary>
        public static async Task LogCreateAsync<T>(int userId, string tableName, int recordId, T entity)
        {
            await LogAsync(userId, "CREATE", tableName, recordId, null, entity, 
                $"Created new {tableName} record");
        }

        /// <summary>
        /// Log UPDATE operation
        /// </summary>
        public static async Task LogUpdateAsync<T>(int userId, string tableName, int recordId, T oldEntity, T newEntity)
        {
            await LogAsync(userId, "UPDATE", tableName, recordId, oldEntity, newEntity,
                $"Updated {tableName} record");
        }

        /// <summary>
        /// Log DELETE operation
        /// </summary>
        public static async Task LogDeleteAsync<T>(int userId, string tableName, int recordId, T entity)
        {
            await LogAsync(userId, "DELETE", tableName, recordId, entity, null,
                $"Deleted {tableName} record");
        }
    }
}
