using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Helpers
{
    public static class AppUtilities
    {
        public static void tenantChange(DataContext _context)
        {
            DbCommand cmd = _context.Database.GetDbConnection().CreateCommand();

            cmd.CommandText = "dbo.sp_set_session_context";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@key", SqlDbType.VarChar) { Value = "TenantId" });
            cmd.Parameters.Add(new SqlParameter("@value", SqlDbType.VarChar) { Value = "A4C482BF-1468-4460-BE9B-2C325926230D" });
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
                
            }
            cmd.ExecuteNonQuery();
            //return _context;

        }

        public static void tenantClose(DataContext _context)
        {
            DbCommand cmd = _context.Database.GetDbConnection().CreateCommand();

            cmd.CommandText = "dbo.sp_set_session_context";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@key", SqlDbType.VarChar) { Value =   "TenantId" });
            cmd.Parameters.Add(new SqlParameter("@value", SqlDbType.VarChar) { Value = "25AE09EF-E24E-494B-911F-F63CE9ED8458" });
                                                                                        
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
                
            }
            cmd.ExecuteNonQuery();
            //return _context;

        }

    }
}