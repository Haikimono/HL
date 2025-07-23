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
                groupmember1_affiliation,
                groupmember1_name,
                groupmember1_department,
                groupmember1_section,
                groupmember1_subsection,
                groupmember2_affiliation,
                groupmember2_name,
                groupmember2_department,
                groupmember2_section,
                groupmember2_subsection,
                groupmember3_affiliation,
                groupmember3_name,
                groupmember3_department,
                groupmember3_section,
                groupmember3_subsection,
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
                @group1,
                @group1Name,
                @group1BuSho,
                @group1KaBumon,
                @group1Kakari,
                @group2,
                @group2Name,
                @group2BuSho,
                @group2KaBumon,
                @group2Kakari,
                @group3,
                @group3Name,
                @group3BuSho,
                @group3KaBumon,
                @group3Kakari,
                @hezuShozoku,
                @hezuBuSho,
                @hezuKaBumon,
                @hezuShimei,
                @hezuKanshoku,
                @shumuKa,
                @kankeiKa,
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
            cmd.Parameters.AddWithValue("@group1", pModel.GroupZenin1AffiliationId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group1BuSho", pModel.GroupZenin1DepartmentId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group1KaBumon", pModel.GroupZenin1SectionId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group1Kakari", pModel.GroupZenin1SubsectionId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group1Name", pModel.GroupZenin1Name ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group2", pModel.GroupZenin2AffiliationId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group2BuSho", pModel.GroupZenin2DepartmentId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group2KaBumon", pModel.GroupZenin2SectionId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group2Kakari", pModel.GroupZenin2SubsectionId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group2Name", pModel.GroupZenin2Name ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group3", pModel.GroupZenin3AffiliationId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group3BuSho", pModel.GroupZenin3DepartmentId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group3KaBumon", pModel.GroupZenin3SectionId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group3Kakari", pModel.GroupZenin3SubsectionId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group3Name", pModel.GroupZenin3Name ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@hezuShozoku", pModel.FirstReviewerAffiliationId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@hezuBuSho", pModel.FirstReviewerDepartmentId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@hezuKaBumon", pModel.FirstReviewerSectionId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@hezuShimei", pModel.FirstReviewerName ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@hezuKanshoku", pModel.FirstReviewerTitle ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@shumuKa", pModel.ShumuKaId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@kankeiKa", pModel.KankeiKaId ?? (object)DBNull.Value);
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
                groupmember1_affiliation = @group1,
                groupmember1_department = @group1BuSho,
                groupmember1_section = @group1KaBumon,
                groupmember1_subsection = @group1Kakari,
                groupmember1_name = @group1Name,
                groupmember2_affiliation = @group2,
                groupmember2_department = @group2BuSho,
                groupmember2_section = @group2KaBumon,
                groupmember2_subsection = @group2Kakari,
                groupmember2_name = @group2Name,
                groupmember3_affiliation = @group3,
                groupmember3_department = @group3BuSho,
                groupmember3_section = @group3KaBumon,
                groupmember3_subsection = @group3Kakari,
                groupmember3_name = @group3Name,
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
            cmd.Parameters.AddWithValue("@group1", pModel.GroupZenin1AffiliationId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group1BuSho", pModel.GroupZenin1DepartmentId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group1KaBumon", pModel.GroupZenin1SectionId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group1Kakari", pModel.GroupZenin1SubsectionId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group1Name", pModel.GroupZenin1Name ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group2", pModel.GroupZenin2AffiliationId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group2BuSho", pModel.GroupZenin2DepartmentId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group2KaBumon", pModel.GroupZenin2SectionId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group2Kakari", pModel.GroupZenin2SubsectionId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group2Name", pModel.GroupZenin2Name ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group3", pModel.GroupZenin3AffiliationId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group3BuSho", pModel.GroupZenin3DepartmentId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group3KaBumon", pModel.GroupZenin3SectionId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group3Kakari", pModel.GroupZenin3SubsectionId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group3Name", pModel.GroupZenin3Name ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@hezuShozoku", pModel.FirstReviewerAffiliationId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@hezuBuSho", pModel.FirstReviewerDepartmentId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@hezuKaBumon", pModel.FirstReviewerSectionId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@hezuShimei", pModel.FirstReviewerName ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@hezuKanshoku", pModel.FirstReviewerTitle ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@shumuKa", pModel.ShumuKaId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@kankeiKa", pModel.KankeiKaId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@genjo", pModel.GenjyoMondaiten ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@kaizen", pModel.Kaizenan ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@koukaJissi", (object) (pModel.KoukaJishi.HasValue ? (int)pModel.KoukaJishi.Value : DBNull.Value));
            cmd.Parameters.AddWithValue("@kouka", pModel.Kouka ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@submissiondate", pModel.Submissiondate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@createddate", pModel.Createddate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@id", pModel.Id);

            cmd.ExecuteNonQuery();
        }

        //// 提案書一覧登録
        //public void SqlInsertproposals(CreateModel pModel)
        //{
        //    using var conn = new SqlConnection(_connectionString);
        //    conn.Open();

        //    string sql = @"
        //    INSERT INTO Te_proposals (
        //        id,
        //        user_id,
        //        proposal_year,
        //        business_year,
        //        status,
        //        pd_name,
        //        user_name,
        //        [from],
        //        submitted_at,
        //        point,
        //        decision,
        //        resubmission,
        //        review_section,
        //        related_sections,
        //        award_division,
        //        updated_at,
        //        created_at,
        //        [delete]
        //    )
        //    SELECT 
        //        id,
        //        user_id,
        //        proposal_year,
        //        proposal_year,
        //        status,
        //        pd_name,
        //        nameOrrepresentativename,
        //        [from],
        //        submissiondate,
        //        NULL,
        //        NULL,
        //        1,
        //        NULL,
        //        relatedsection,
        //        NULL,
        //        updatedate,
        //        createddate,
        //        0
        //    FROM
        //        proposal
        //    WHERE
        //        proposal.id = @id
        //        ";

        //    using var cmd = new SqlCommand(sql, conn);

        //    cmd.Parameters.AddWithValue("@id", pModel.Id ?? (object)DBNull.Value);

        //    cmd.ExecuteNonQuery();

        //}

        //// 提案書一覧更新
        //public void SqlUpdateproposals(CreateModel pModel)
        //{
        //    using var conn = new SqlConnection(_connectionString);
        //    conn.Open();

        //    string sql = @"
        //                UPDATE T
        //                SET
        //                    T.user_id = D.user_id,
        //                    T.proposal_year = D.proposal_year,
        //                    T.status = D.status,
        //                    T.pd_name = D.pd_name,
        //                    T.user_name = D.nameOrrepresentativename,
        //                    T.[from] = D.[from],
        //                    T.submitted_at = D.submissiondate,
        //                    T.related_sections = D.relatedsection,
        //                    T.updated_at = D.updatedate
        //                FROM Te_proposals T
        //                JOIN proposal D ON T.id = D.id
        //                WHERE T.id = @id;
        //                 ";

        //    using var cmd = new SqlCommand(sql, conn);

        //    cmd.Parameters.AddWithValue("@id", pModel.Id ?? (object)DBNull.Value);

        //    cmd.ExecuteNonQuery();
        //}

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

        // 部・署（Busho）をデータベースから取得するメソッド
        // 注释掉Busho、Kabumon、KakariTantou相关类型和方法
        // public List<Busho> GetBusho()
        // {
        //    
        //    var result = new List<Busho>();

        //    using (var conn = new SqlConnection(_connectionString))
        //    {
        //        conn.Open();

        //        var cmd = new SqlCommand("SELECT department_id, department_name FROM user_department", conn);

        //        using (var reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                result.Add(new Busho
        //                {
        //                    Department_id = reader.GetString(0),
        //                    Department_name = reader.GetString(1)
        //                });
        //            }
        //        }
        //    }
        //    // 結果のリストを返す
        //    return result;
        //}

        // 課・部門（Kabumon）をデータベースから取得するメソッド
        // public List<Kabumon> GetKabumon()
        // {

        //    var result = new List<Kabumon>();

        //    using (var conn = new SqlConnection(_connectionString))
        //    {
        //        conn.Open();

        //        var cmd = new SqlCommand("SELECT section_id, section_name FROM user_section", conn);

        //        using (var reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                result.Add(new Kabumon
        //                {
        //                    Section_id = reader.GetString(0),
        //                    Section_name = reader.GetString(1)
        //                });
        //            }
        //        }
        //    }
        //    // 結果のリストを返す
        //    return result;
        //}


        // 係・担当（KakariTantou）をデータベースから取得するメソッド
        // public List<KakariTantou> GetKakariTantou()
        // {

        //    var result = new List<KakariTantou>();

        //    using (var conn = new SqlConnection(_connectionString))
        //    {
        //        conn.Open();

        //        var cmd = new SqlCommand("SELECT subsection_id, subsection_name FROM user_subsection", conn);

        //        using (var reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                result.Add(new KakariTantou
        //                {
        //                    Subsection_id = reader.GetString(0),
        //                    Subsection_name = reader.GetString(1)
        //                });
        //            }
        //        }
        //    }
        //    // 結果のリストを返す
        //    return result;
        //}


    }
}
