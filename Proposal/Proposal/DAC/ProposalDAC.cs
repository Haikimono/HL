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

        public List<TeProposal> GetProposals()
        {
            var proposals = new List<TeProposal>();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = @"
                    SELECT * FROM ProposalsDB.dbo.Te_proposals 
                    WHERE [delete] = 0 
                    ORDER BY created_at DESC";

                using (var cmd = new SqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        proposals.Add(new TeProposal
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                            ProposalYear = reader.GetInt32(reader.GetOrdinal("proposal_year")),
                            BusinessYear = reader.GetInt32(reader.GetOrdinal("business_year")),
                            Status = reader.GetInt32(reader.GetOrdinal("status")),
                            PdName = reader.GetString(reader.GetOrdinal("pd_name")),
                            UserName = reader.GetString(reader.GetOrdinal("user_name")),
                            From = reader.IsDBNull(reader.GetOrdinal("from")) ? null : reader.GetInt32(reader.GetOrdinal("from")),
                            SubmittedAt = reader.IsDBNull(reader.GetOrdinal("submitted_at")) ? null : reader.GetDateTime(reader.GetOrdinal("submitted_at")),
                            Point = reader.IsDBNull(reader.GetOrdinal("point")) ? null : reader.GetInt32(reader.GetOrdinal("point")),
                            Decision = reader.IsDBNull(reader.GetOrdinal("decision")) ? null : reader.GetInt32(reader.GetOrdinal("decision")),
                            Resubmission = reader.GetBoolean(reader.GetOrdinal("resubmission")),
                            ReviewSection = reader.IsDBNull(reader.GetOrdinal("review_section")) ? null : reader.GetInt32(reader.GetOrdinal("review_section")),
                            RelatedSections = reader.IsDBNull(reader.GetOrdinal("related_sections")) ? null : reader.GetInt32(reader.GetOrdinal("related_sections")),
                            AwardDivision = reader.IsDBNull(reader.GetOrdinal("award_division")) ? null : reader.GetInt32(reader.GetOrdinal("award_division")),
                            UpdatedAt = reader.GetDateTime(reader.GetOrdinal("updated_at")),
                            CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                            Delete = reader.GetBoolean(reader.GetOrdinal("delete"))
                        });
                    }
                }
            }

            return proposals;
        }
    }
}

