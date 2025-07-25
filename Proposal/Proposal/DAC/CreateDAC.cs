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
                affiliation_id,
                department_id,
                section_id,
                subsection_id,
                proposal_name,
                proposal_type,
                proposal_year,
                status,
                evaluation_section_id,
                responsible_section_id1,
                responsible_section_id2,
                responsible_section_id3,
                responsible_section_id4,
                responsible_section_id5,
                first_reviewer_affiliation_id,
                first_reviewer_department_id,
                first_reviewer_section_id,
                first_reviewer_subsection_id,
                first_reviewer_user_id,
                genjyomondaiten,
                kaizenan,
                koukaJishi,
                kouka,
                created_time,
                updated_time
            )
            SELECT
                @user_id,
                u.shozoku_id,
                u.department_id,
                u.section_id,
                u.subsection_id,
                @proposal_name,
                @proposal_type,
                @proposal_year,
                @status,
                @evaluation_section_id,
                @responsible_section_id1,
                @responsible_section_id2,
                @responsible_section_id3,
                @responsible_section_id4,
                @responsible_section_id5,
                @first_reviewer_affiliation_id,
                @first_reviewer_department_id,
                @first_reviewer_section_id,
                @first_reviewer_subsection_id,
                @first_reviewer_user_id,
                @genjyomondaiten,
                @kaizenan,
                @koukaJishi,
                @kouka,
                SYSDATETIME(),
                SYSDATETIME()
            FROM [user] u
            WHERE u.user_id = @user_id;
            SELECT TOP 1 proposal_id FROM proposal ORDER BY proposal_id DESC;
            ";

            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@user_id", pModel.UserId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@proposal_name", pModel.TeianDaimei ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@proposal_type", pModel.ProposalTypeId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@proposal_year", pModel.TeianYear ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@status", pModel.Status ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@evaluation_section_id", pModel.EvaluationSectionId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@responsible_section_id1", pModel.ResponsibleSectionId1 ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@responsible_section_id2", pModel.ResponsibleSectionId2 ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@responsible_section_id3", pModel.ResponsibleSectionId3 ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@responsible_section_id4", pModel.ResponsibleSectionId4 ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@responsible_section_id5", pModel.ResponsibleSectionId5 ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@first_reviewer_affiliation_id", pModel.FirstReviewerAffiliationId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@first_reviewer_department_id", pModel.FirstReviewerDepartmentId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@first_reviewer_section_id", pModel.FirstReviewerSectionId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@first_reviewer_subsection_id", pModel.FirstReviewerSubsectionId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@first_reviewer_user_id", pModel.FirstReviewerUserId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@genjyomondaiten", pModel.GenjyoMondaiten ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@kaizenan", pModel.Kaizenan ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@koukaJishi", (object)(pModel.KoukaJishi.HasValue ? (int)pModel.KoukaJishi.Value : DBNull.Value));
            cmd.Parameters.AddWithValue("@kouka", pModel.Kouka ?? (object)DBNull.Value);
            
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
                user_id = @user_id,
                status = @status,
                proposal_year = @proposal_year,
                proposal_name = @proposal_name,
                proposal_type = @proposal_type,
                affiliation_id = @affiliation_id,
                department_id = @department_id,
                section_id = @section_id,
                subsection_id = @subsection_id,
                name = @name,
                evaluation_section_id = @evaluation_section_id,
                responsible_section_id1 = @responsible_section_id1,
                responsible_section_id2 = @responsible_section_id2,
                responsible_section_id3 = @responsible_section_id3,
                responsible_section_id4 = @responsible_section_id4,
                responsible_section_id5 = @responsible_section_id5,
                first_reviewer_affiliation_id = @first_reviewer_affiliation_id,
                first_reviewer_department_id = @first_reviewer_department_id,
                first_reviewer_section_id = @first_reviewer_section_id,
                first_reviewer_subsection_id = @first_reviewer_subsection_id,
                first_reviewer_user_id = @first_reviewer_user_id,
                genjyomondaiten = @genjyomondaiten,
                kaizenan = @kaizenan,
                koukaJishi = @koukaJishi,
                kouka = @kouka,
                updated_time = SYSDATETIME()
            WHERE 
                proposal_id = @proposal_id;
            ";

            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@user_id", pModel.UserId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@status", pModel.Status ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@proposal_year", pModel.TeianYear ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@proposal_name", pModel.TeianDaimei ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@proposal_type", pModel.ProposalTypeId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@affiliation_id", pModel.AffiliationId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@department_id", pModel.DepartmentId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@section_id", pModel.SectionId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@subsection_id", pModel.SubsectionId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@name", pModel.ShimeiOrDaihyoumei ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@evaluation_section_id", pModel.EvaluationSectionId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@responsible_section_id1", pModel.ResponsibleSectionId1 ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@responsible_section_id2", pModel.ResponsibleSectionId2 ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@responsible_section_id3", pModel.ResponsibleSectionId3 ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@responsible_section_id4", pModel.ResponsibleSectionId4 ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@responsible_section_id5", pModel.ResponsibleSectionId5 ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@first_reviewer_affiliation_id", pModel.FirstReviewerAffiliationId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@first_reviewer_department_id", pModel.FirstReviewerDepartmentId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@first_reviewer_section_id", pModel.FirstReviewerSectionId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@first_reviewer_subsection_id", pModel.FirstReviewerSubsectionId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@first_reviewer_user_id", pModel.FirstReviewerUserId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@genjyomondaiten", pModel.GenjyoMondaiten ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@kaizenan", pModel.Kaizenan ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@koukaJishi", (object)(pModel.KoukaJishi.HasValue ? (int)pModel.KoukaJishi.Value : DBNull.Value));
            cmd.Parameters.AddWithValue("@kouka", pModel.Kouka ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@proposal_id", pModel.Id);

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
        public void InsertGroupInfo(CreateModel model)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            // 既存のグループデータを削除
            var delCmd = new SqlCommand("DELETE FROM group_info WHERE proposal_id = @proposalId", conn);
            delCmd.Parameters.AddWithValue("@proposalId", model.Id);
            delCmd.ExecuteNonQuery();

            // 10人分のメンバー情報を挿入
            var sql = @"INSERT INTO group_info (
                proposal_id,
                group_name,
                department_id_1, section_id_1, subsection_id_1, name_1,
                department_id_2, section_id_2, subsection_id_2, name_2,
                department_id_3, section_id_3, subsection_id_3, name_3,
                department_id_4, section_id_4, subsection_id_4, name_4,
                department_id_5, section_id_5, subsection_id_5, name_5,
                department_id_6, section_id_6, subsection_id_6, name_6,
                department_id_7, section_id_7, subsection_id_7, name_7,
                department_id_8, section_id_8, subsection_id_8, name_8,
                department_id_9, section_id_9, subsection_id_9, name_9,
                department_id_10, section_id_10, subsection_id_10, name_10
            ) VALUES (
                @proposal_id,
                @group_name,
                @department_id_1, @section_id_1, @subsection_id_1, @name_1,
                @department_id_2, @section_id_2, @subsection_id_2, @name_2,
                @department_id_3, @section_id_3, @subsection_id_3, @name_3,
                @department_id_4, @section_id_4, @subsection_id_4, @name_4,
                @department_id_5, @section_id_5, @subsection_id_5, @name_5,
                @department_id_6, @section_id_6, @subsection_id_6, @name_6,
                @department_id_7, @section_id_7, @subsection_id_7, @name_7,
                @department_id_8, @section_id_8, @subsection_id_8, @name_8,
                @department_id_9, @section_id_9, @subsection_id_9, @name_9,
                @department_id_10, @section_id_10, @subsection_id_10, @name_10
            )";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@proposal_id", model.Id);
            cmd.Parameters.AddWithValue("@group_name", model.GroupMei ?? (object)DBNull.Value);
            for (int i = 0; i < 10; i++)
            {
                var m = (model.GroupMembers != null && i < model.GroupMembers.Count) ? model.GroupMembers[i] : null;
                cmd.Parameters.AddWithValue($"@department_id_{i + 1}", m?.DepartmentId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue($"@section_id_{i + 1}", m?.SectionId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue($"@subsection_id_{i + 1}", m?.SubsectionId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue($"@name_{i + 1}", m?.Name ?? (object)DBNull.Value);
            }
            cmd.ExecuteNonQuery();
        }

        // 指定proposal_idでgroup_infoの10人分のメンバー情報を取得
        public DataTable GetGroupInfoByProposalId(string proposalId)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            string sql = @"SELECT 
                department_id_1, section_id_1, subsection_id_1, name_1,
                department_id_2, section_id_2, subsection_id_2, name_2,
                department_id_3, section_id_3, subsection_id_3, name_3,
                department_id_4, section_id_4, subsection_id_4, name_4,
                department_id_5, section_id_5, subsection_id_5, name_5,
                department_id_6, section_id_6, subsection_id_6, name_6,
                department_id_7, section_id_7, subsection_id_7, name_7,
                department_id_8, section_id_8, subsection_id_8, name_8,
                department_id_9, section_id_9, subsection_id_9, name_9,
                department_id_10, section_id_10, subsection_id_10, name_10
            FROM group_info WHERE proposal_id = @proposalId";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@proposalId", proposalId);
            using var adapter = new SqlDataAdapter(cmd);
            var dataTable = new DataTable();
            adapter.Fill(dataTable);
            return dataTable;
        }

    }
}
