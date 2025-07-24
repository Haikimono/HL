using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Proposal.Models;

namespace Proposal.DAC
{
    public class ProposalListDAC
    {
        private readonly string _connectionString;

        public ProposalListDAC(string connectionString)
        {
            _connectionString = connectionString;
        }

        public (List<ProposalList> Items, int TotalCount, int TotalPages) GetProposals(
            int? year, int? status, int page, int pageSize)
        {
            var items = new List<ProposalList>();
            int totalCount = 0;

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            string filterSql = "";
            if (year.HasValue) filterSql += " AND p.proposal_year = @Year";
            if (status.HasValue) filterSql += " AND p.status = @Status";

            string sql = $@"
            SELECT COUNT(*) FROM ProposalsDB.dbo.proposal p WHERE 1=1 {filterSql};
                SELECT 
                    p.*, 
                    a.affiliation_name,
                    s.status_name,
                    g.group_name
                FROM ProposalsDB.dbo.proposal p
                INNER JOIN ProposalsDB.dbo.[user] u ON p.user_id = u.user_id
                INNER JOIN ProposalsDB.dbo.[affiliation] a ON u.shozoku_id = a.affiliation_id
                INNER JOIN ProposalsDB.dbo.[proposal_status] s ON p.status = s.status_id
                INNER JOIN ProposalsDB.dbo.[group_info] g ON p.proposal_id = g.proposal_id
                WHERE 1=1 {filterSql}
                ORDER BY p.created_time DESC
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
            ";


            using var cmd = new SqlCommand(sql, conn);
            if (year.HasValue) cmd.Parameters.AddWithValue("@Year", year.Value.ToString());
            if (status.HasValue) cmd.Parameters.AddWithValue("@Status", status.Value);
            cmd.Parameters.AddWithValue("@Offset", (page - 1) * pageSize);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
                totalCount = reader.GetInt32(0);
            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    items.Add(new ProposalList
                    {
                        ProposalYear = reader.GetString(reader.GetOrdinal("proposal_year")),
                        ProposalId = reader.GetInt32(reader.GetOrdinal("proposal_id")),
                        Status = reader.GetString(reader.GetOrdinal("status_name")),
                        ProposalName = reader.GetString(reader.GetOrdinal("proposal_name")),
                        UserName = reader.GetString(reader.GetOrdinal("name")),
                        GroupName = reader.GetString(reader.GetOrdinal("group_name")),
                        Affiliation = reader.GetString(reader.GetOrdinal("affiliation_name")),
                        CreatedTime = reader.GetDateTime(reader.GetOrdinal("created_time")),
                        Point = reader.GetInt32(reader.GetOrdinal("point")),
                        Decision = reader.GetInt32(reader.GetOrdinal("decision")),
                        AdditionalSubmission = reader.GetBoolean(reader.GetOrdinal("additional_submission")),
                        EvaluationSection = reader.GetString(reader.GetOrdinal("evaluation_section_id")),
                        ResponsibleSection1 = reader.GetString(reader.GetOrdinal("responsible_section_id1")),
                        ResponsibleSection2 = reader.GetString(reader.GetOrdinal("responsible_section_id2")),
                        ResponsibleSection3 = reader.GetString(reader.GetOrdinal("responsible_section_id3")),
                        ResponsibleSection4 = reader.GetString(reader.GetOrdinal("responsible_section_id4")),
                        ResponsibleSection5 = reader.GetString(reader.GetOrdinal("responsible_section_id5")),
                        AwardType = reader.GetInt32(reader.GetOrdinal("award_type")),
                    });
                }
            }

            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            return (items, totalCount, totalPages);
        }
        public List<ProposalStatus> GetProposalStatuses()
        {
            var statuses = new List<ProposalStatus>();

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            string sql = "SELECT status_id, status_name FROM [proposal_status] ORDER BY status_id";

            using var cmd = new SqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                statuses.Add(new ProposalStatus
                {
                    Id = reader.GetInt32(reader.GetOrdinal("status_id")),
                    StatusName = reader.GetString(reader.GetOrdinal("status_name"))
                });
            }

            return statuses;
        }
    }
}
