using System;
using System.Data.SqlClient;
using Proposal.Models;

namespace Proposal.DAC
{
    public class CreateDAC
    {
        private readonly string _connectionString;

        public CreateDAC(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void TeianshoTeishutsu(CreateModel pModel)
        {
            //using var conn = new SqlConnection(_connectionString);
            //conn.Open();
            //string sql = "INSERT INTO" +
            //            " user_id, user_name, password, user_kbn FROM M_User WHERE user_id=@userId "+




            //                " user_id, user_name, password, user_kbn FROM M_User WHERE user_id=@userId";
            //using var cmd = new SqlCommand(sql, conn);
            //cmd.Parameters.AddWithValue("@userId", pModel.UserId);
            //using var reader = cmd.ExecuteReader();
          
        }
    }
}
