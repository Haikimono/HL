using System;
using System.Data.SqlClient;
using Proposal.Models;

namespace Proposal.DAC
{
    public class LoginDAC
    {
        private readonly string _connectionString;

        public LoginDAC(string connectionString)
        {
            _connectionString = connectionString;
        }

        public User GetUserById(LoginModel pModel)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            string sql = "SELECT user_id, user_name, password, user_kbn FROM M_User WHERE user_id=@userId";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@userId", pModel.UserId);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new User
                {
                    UserId = reader["user_id"].ToString(),
                    UserName = reader["user_name"].ToString(),
                    Password = reader["password"].ToString(),
                    UserKbn = reader["user_kbn"].ToString()
                };
            }
            return null;
        }
    }
}
