using System;
using System.Data;
using System.Data.SqlClient;
using Proposal.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Proposal.DAC
{
    public class CreateDAC
    {
        private readonly string _connectionString;

        public CreateDAC()
        {

        }
        public CreateDAC(string connectionString)
        {
            _connectionString = connectionString;
        }

        //提案書詳細登録
        public int SqlInsertproposals_detail(CreateModel pModel)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            string sql = @"
            INSERT INTO proposal (
                user_id,
                status,
                proposal_year,
                pd_name,
                proposal_type,
                proposal_kbn,
                [from],
                nameOrrepresentativename,
                groupname,
                evaluation_section_id,
                responsible_section_id1,
                responsible_section_id2,
                responsible_section_id3,
                responsible_section_id4,
                responsible_section_id5,
                firstevieweraffiliation,
                firsteviewerdepartment,
                firsteviewersection,
                firsteviewername,
                firsteviewertitle,
                responsiblesection,
                relatedsection,
                currentsituation,
                improvementproposal,
                isImplemented,
                effect,
                attachmentfilename1,
                attachmentfilename2,
                attachmentfilename3,
                attachmentfilename4,
                attachmentfilename5,
                attachmentfilepath1,
                attachmentfilepath2,
                attachmentfilepath3,
                attachmentfilepath4,
                attachmentfilepath5,
                submissiondate,
                updatedate,
                createddate
            )
            VALUES (
                @userId,
                @status,
                @proposalYear,
                @pdName,
                @proposalType,
                @proposalKbn,
                @from,
                @nameOrRepName,
                @groupName,
                @evaluation_section_id,
                @responsible_section_id1,
                @responsible_section_id2,
                @responsible_section_id3,
                @responsible_section_id4,
                @responsible_section_id5,
                @hezuShozoku,
                @hezuBuSho,
                @hezuKaBumon,
                @hezuShimei,
                @hezuKanshoku,
                @genjo,
                @kaizen,
                @koukaJissi,
                @kouka,
                NULL, NULL, NULL, NULL, NULL,
                NULL, NULL, NULL, NULL, NULL,
                @submissiondate,
                SYSDATETIME(), 
                @createddate
            );
            
            SELECT TOP 1 id FROM proposal ORDER BY id DESC;
            ";

            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@userId", pModel.UserId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@status", pModel.Status ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@proposalYear", pModel.TeianYear ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@pdName", pModel.TeianDaimei ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@proposalType", pModel.ProposalTypeId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@proposalKbn", pModel.ProposalKbnId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@from", pModel.AffiliationId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@nameOrRepName", pModel.ShimeiOrDaihyoumei ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@groupName", pModel.GroupMei ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@evaluation_section_id", pModel.EvaluationSectionId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@responsible_section_id1", pModel.ResponsibleSectionId1 ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@responsible_section_id2", pModel.ResponsibleSectionId2 ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@responsible_section_id3", pModel.ResponsibleSectionId3 ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@responsible_section_id4", pModel.ResponsibleSectionId4 ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@responsible_section_id5", pModel.ResponsibleSectionId5 ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@hezuShozoku", pModel.FirstReviewerAffiliationId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@hezuBuSho", pModel.FirstReviewerDepartmentId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@hezuKaBumon", pModel.FirstReviewerSectionId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@hezuShimei", pModel.FirstReviewerName ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@hezuKanshoku", pModel.FirstReviewerTitle ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@genjo", pModel.GenjyoMondaiten ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@kaizen", pModel.Kaizenan ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@koukaJissi", (object) (pModel.KoukaJishi.HasValue ? (int)pModel.KoukaJishi.Value : DBNull.Value));
            cmd.Parameters.AddWithValue("@kouka", pModel.Kouka ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@submissiondate", pModel.Submissiondate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@createddate", pModel.Createddate ?? (object)DBNull.Value);
            
            var result = cmd.ExecuteScalar();

            return result != null ? Convert.ToInt32(result) : 0;
        }

        /// <summary>
        /// 提案書の詳細情報をIDで取得
        /// </summary>
        public DataTable GetProposalDetailById(string id)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            string sql = @"
                SELECT 
                    id,
                    user_id,
                    status,
                    proposal_year,
                    pd_name,
                    proposal_type,
                    proposal_kbn,
                    [from],
                    nameOrrepresentativename,
                    groupname,
                    groupmember1_affiliation,
                    groupmember1_department,
                    groupmember1_section,
                    groupmember1_subsection,
                    groupmember1_name,
                    groupmember2_affiliation,
                    groupmember2_department,
                    groupmember2_section,
                    groupmember2_subsection,
                    groupmember2_name,
                    groupmember3_affiliation,
                    groupmember3_department,
                    groupmember3_section,
                    groupmember3_name,
                    groupmember3_subsection,
                    daiijishinsashaHezuIsChecked,
                    firstevieweraffiliation,
                    firsteviewerdepartment,
                    firsteviewersection,
                    firsteviewername,
                    firsteviewertitle,
                    responsiblesection,
                    relatedsection,
                    currentsituation,
                    improvementproposal,
                    isImplemented,
                    effect,
                    attachmentfilename1,
                    attachmentfilename2,
                    attachmentfilename3,
                    attachmentfilename4,
                    attachmentfilename5,
                    attachmentfilepath1,
                    attachmentfilepath2,
                    attachmentfilepath3,
                    attachmentfilepath4,
                    attachmentfilepath5,
                    submissiondate,
                    updatedate,
                    createddate
                FROM proposal 
                WHERE id = @id";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            using var adapter = new SqlDataAdapter(cmd);
            var dataTable = new DataTable();
            adapter.Fill(dataTable);
            
            return dataTable;
        }




        /// <summary>
        /// ユーザー情報をUserIdで取得
        /// </summary>
        public DataTable GetUserInfoByUserId(string UserId)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            string sql = @"
                SELECT 
                    affiliation.affiliation_name,
                    department.department_name,
                    section.section_name,
                    subsection.subsection_name
                FROM 
                    [user] u 
                INNER JOIN
                    affiliation
                ON
                    affiliation.affiliation_id = u.shozoku_id
                INNER JOIN
                    department
                ON
                    department.department_id = u.department_id
                INNER JOIN
                    section
                ON
                    section.section_id = u.section_id
                INNER JOIN
                    subsection
                ON
                    subsection.subsection_id = u.subsection_id
                WHERE
                    u.user_id = @userid";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@userid", UserId);
            using var adapter = new SqlDataAdapter(cmd);
            var dataTable = new DataTable();
            adapter.Fill(dataTable);

            return dataTable;
        }

        //提案書詳細更新
        public void SqlUpdateproposals_detail(CreateModel pModel)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            string sql = @"
            UPDATE proposal
            SET
                user_id = @userId,
                status = @status,
                proposal_year = @proposalYear,
                pd_name = @pdName,
                proposal_type = @proposalType,
                proposal_kbn = @proposalKbn,
                [from] = @from,
                nameOrrepresentativename = @nameOrRepName,
                groupname = @groupName,
                evaluation_section_id = @evaluation_section_id,
                responsible_section_id1 = @responsible_section_id1,
                responsible_section_id2 = @responsible_section_id2,
                responsible_section_id3 = @responsible_section_id3,
                responsible_section_id4 = @responsible_section_id4,
                responsible_section_id5 = @responsible_section_id5,
                firstevieweraffiliation = @hezuShozoku,
                firsteviewerdepartment = @hezuBuSho,
                firsteviewersection = @hezuKaBumon,
                firsteviewername = @hezuShimei,
                firsteviewertitle = @hezuKanshoku,
                responsiblesection = @shumuKa,
                relatedsection = @kankeiKa,
                currentsituation = @genjo,
                improvementproposal = @kaizen,
                isImplemented = @koukaJissi,
                effect = @kouka,
                submissiondate  = @Submissiondate,
                updatedate = SYSDATETIME()
            WHERE 
                id = @id;
            ";

            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@userId", pModel.UserId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@status", pModel.Status ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@proposalYear", pModel.TeianYear ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@pdName", pModel.TeianDaimei ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@proposalType", pModel.ProposalTypeId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@proposalKbn", pModel.ProposalKbnId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@from", pModel.AffiliationId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@nameOrRepName", pModel.ShimeiOrDaihyoumei ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@groupName", pModel.GroupMei ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@evaluation_section_id", pModel.EvaluationSectionId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@responsible_section_id1", pModel.ResponsibleSectionId1 ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@responsible_section_id2", pModel.ResponsibleSectionId2 ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@responsible_section_id3", pModel.ResponsibleSectionId3 ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@responsible_section_id4", pModel.ResponsibleSectionId4 ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@responsible_section_id5", pModel.ResponsibleSectionId5 ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@hezuShozoku", pModel.FirstReviewerAffiliationId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@hezuBuSho", pModel.FirstReviewerDepartmentId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@hezuKaBumon", pModel.FirstReviewerSectionId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@hezuShimei", pModel.FirstReviewerName ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@hezuKanshoku", pModel.FirstReviewerTitle ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@genjo", pModel.GenjyoMondaiten ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@kaizen", pModel.Kaizenan ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@koukaJissi", (object) (pModel.KoukaJishi.HasValue ? (int)pModel.KoukaJishi.Value : DBNull.Value));
            cmd.Parameters.AddWithValue("@kouka", pModel.Kouka ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@submissiondate", pModel.Submissiondate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@createddate", pModel.Createddate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@id", pModel.Id);

            cmd.ExecuteNonQuery();
        }

      
        // 提案種類（ProposalType）の一覧をデータベースから取得するメソッド
        public List<ProposalType> GetProposalTypes()
        {
            var list = new List<ProposalType>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT kbn, kbn_name FROM proposal_kbn", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new ProposalType
                        {
                            Kbn = reader["kbn"].ToString(),
                            KbnName = reader["kbn_name"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        // 提案区分の一覧をデータベースから取得するメソッド
        public List<SelectListItem> GetProposalKbnList()
        {
            var list = new List<SelectListItem>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT kbn, kbn_name FROM proposal_kbn", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new SelectListItem
                        {
                            Value = reader["kbn"].ToString(),
                            Text = reader["kbn_name"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        public List<SelectListItem> GetProposalTypeList()
        {
            var list = new List<SelectListItem>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT type_id, type_name FROM proposal_type", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new SelectListItem
                        {
                            Value = reader["type_id"].ToString(),
                            Text = reader["type_name"].ToString()
                        });
                    }
                }
            }
            return list;
        }


        // 提案所属の一覧をデータベースから取得するメソッド
        public List<Affiliation> GetAffiliations()
        {
            var list = new List<Affiliation>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT affiliation_id, affiliation_name FROM affiliation", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Affiliation
                        {
                            Id = reader["affiliation_id"].ToString(),
                            Shozoku = reader["affiliation_name"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        // 部・署をデータベースから取得するメソッド
        public List<Department> GetDepartments()
        {
            var list = new List<Department>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT department_id, department_name FROM department", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Department
                        {
                            Department_id = reader["department_id"].ToString(),
                            Department_name = reader["department_name"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        // 課・部門をデータベースから取得するメソッド
        public List<Section> GetSections()
        {
            var list = new List<Section>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT section_id, section_name FROM section", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Section
                        {
                            Section_id = reader["section_id"].ToString(),
                            Section_name = reader["section_name"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        // 係・担当をデータベースから取得するメソッド
        public List<Subsection> GetSubsections()
        {
            var list = new List<Subsection>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT subsection_id, subsection_name FROM subsection", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Subsection
                        {
                            Subsection_id = reader["subsection_id"].ToString(),
                            Subsection_name = reader["subsection_name"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        // 既存のグループデータを削除するメソッド
        public void DeleteGroupInfo(string proposalId)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            var cmd = new SqlCommand("DELETE FROM group_info WHERE proposal_id = @proposalId", conn);
            cmd.Parameters.AddWithValue("@proposalId", proposalId);
            cmd.ExecuteNonQuery();
        }

        // グループデータを挿入するメソッド
        public void InsertGroupInfo(string proposalId, List<GroupMemberModel> members)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            foreach (var m in members)
            {
                if (string.IsNullOrEmpty(m.AffiliationId) && string.IsNullOrEmpty(m.DepartmentId) && string.IsNullOrEmpty(m.SectionId) && string.IsNullOrEmpty(m.SubsectionId) && string.IsNullOrEmpty(m.Name))
                    continue;
                var cmd = new SqlCommand(@"INSERT INTO group_info (proposal_id, affiliation_id, department_id, section_id, subsection_id, member_name) VALUES (@proposalId, @affiliationId, @departmentId, @sectionId, @subsectionId, @memberName)", conn);
                cmd.Parameters.AddWithValue("@proposalId", proposalId);
                cmd.Parameters.AddWithValue("@affiliationId", m.AffiliationId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@departmentId", m.DepartmentId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@sectionId", m.SectionId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@subsectionId", m.SubsectionId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@memberName", m.Name ?? (object)DBNull.Value);
                cmd.ExecuteNonQuery();
            }
        }

    }
}
