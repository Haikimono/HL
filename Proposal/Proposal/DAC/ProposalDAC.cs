using System;
using System.Data.SqlClient;
using Proposal.Models;
using Microsoft.Extensions.Configuration;

namespace Proposal.DAC
{
    public class ProposalDAC
    {
        private readonly string _connectionString;

        public ProposalDAC(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ProposalList> GetProposals()
        {
            var proposals = new List<ProposalList>();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = @"
                    SELECT * FROM ProposalsDB.dbo.proposal
                    ORDER BY created_time DESC";

                using (var cmd = new SqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        proposals.Add(new ProposalList
                        {
                            ProposalId = reader.GetString(reader.GetOrdinal("proposal_id")),
                            UserId = reader.GetString(reader.GetOrdinal("user_id")), // int
                            ProposalYear = reader.GetString(reader.GetOrdinal("proposal_year")), // nvarchar(10)
                            //BusinessYear = reader.GetString(reader.GetOrdinal("business_year")), // nvarchar(10)
                            Status = reader.GetInt32(reader.GetOrdinal("status")), // int
                            ProposalName = reader.GetString(reader.GetOrdinal("proposal_name")), // nvarchar(255)
                            UserName = reader.GetString(reader.GetOrdinal("proposal_name")), // nvarchar(255)
                            //From = reader.IsDBNull(reader.GetOrdinal("from"))? null : reader.GetInt32(reader.GetOrdinal("from")), // int?
                            //SubmittedAt = reader.IsDBNull(reader.GetOrdinal("submitted_at")) ? null : reader.GetDateTime(reader.GetOrdinal("submitted_at")), // datetime2?
                            //Point = reader.IsDBNull(reader.GetOrdinal("point"))? null: reader.GetInt32(reader.GetOrdinal("point")), // int?
                            //Decision = reader.IsDBNull(reader.GetOrdinal("decision"))? null : reader.GetInt32(reader.GetOrdinal("decision")), // int?
                            //Resubmission = reader.GetBoolean(reader.GetOrdinal("resubmission")), // bit
                            //ReviewSection = reader.IsDBNull(reader.GetOrdinal("review_section")) ? null: reader.GetInt32(reader.GetOrdinal("review_section")), // int?
                            //RelatedSections = reader.IsDBNull(reader.GetOrdinal("related_sections")) ? null : reader.GetInt32(reader.GetOrdinal("related_sections")), // int?
                            //AwardDivision = reader.IsDBNull(reader.GetOrdinal("award_division")) ? null : reader.GetInt32(reader.GetOrdinal("award_division")), // int?
                            //UpdatedAt = reader.GetDateTime(reader.GetOrdinal("updated_at")), // datetime2
                            //CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")), // datetime2
                            //Delete = reader.GetBoolean(reader.GetOrdinal("delete")) // bit
                        });

                    }
                }
            }

            return proposals;
        }
    }
}

