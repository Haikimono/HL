using System;
using System.Data;
using System.Data.SqlClient;
using Proposal.Models;


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
            INSERT INTO Te_proposals_detail (
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
                groupmember2_affiliation,
                groupmember2_department,
                groupmember2_section,
                groupmember2_subsection,
                groupmember3_affiliation,
                groupmember3_department,
                groupmember3_section,
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
                @group1BuSho,
                @group1KaBumon,
                @group1Kakari,
                @group2,
                @group2BuSho,
                @group2KaBumon,
                @group2Kakari,
                @group3,
                @group3BuSho,
                @group3KaBumon,
                @group3Kakari,
                @hezuChecked,
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
            
            SELECT TOP 1 id FROM Te_proposals_detail ORDER BY id DESC;
            ";

            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@userId", pModel.UserId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@status", pModel.Status ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@proposalYear", pModel.TeianYear ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@pdName", pModel.TeianDaimei ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@proposalType", pModel.TeianShurui.HasValue ? (int)pModel.TeianShurui.Value : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@proposalKbn", pModel.TeianKbn.HasValue ? (int)pModel.TeianKbn.Value : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@from", pModel.Shozoku.HasValue ? (int)pModel.Shozoku.Value : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@nameOrRepName", pModel.ShimeiOrDaihyoumei ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@groupName", pModel.GroupMei ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group1", pModel.GroupZenin1.HasValue ? (int)pModel.GroupZenin1.Value : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group1BuSho", pModel.GroupZenin1BuSho ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group1KaBumon", pModel.GroupZenin1KaBumon ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group1Kakari", pModel.GroupZenin1KakariTantou ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group2", pModel.GroupZenin2.HasValue ? (int)pModel.GroupZenin2.Value : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group2BuSho", pModel.GroupZenin2BuSho ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group2KaBumon", pModel.GroupZenin2KaBumon ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group2Kakari", pModel.GroupZenin2KakariTantou ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group3", pModel.GroupZenin3.HasValue ? (int)pModel.GroupZenin3.Value : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group3BuSho", pModel.GroupZenin3BuSho ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group3KaBumon", pModel.GroupZenin3KaBumon ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group3Kakari", pModel.GroupZenin3KakariTantou ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@hezuChecked", pModel.DaiijishinsashaHezuIsChecked);
            cmd.Parameters.AddWithValue("@hezuShozoku", (int)pModel.DaiijishinsashaShozoku);
            cmd.Parameters.AddWithValue("@hezuBuSho", pModel.DaiijishinsashaBuSho ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@hezuKaBumon", pModel.DaiijishinsashaKaBumon ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@hezuShimei", pModel.DaiijishinsashaShimei ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@hezuKanshoku", pModel.DaiijishinsashaKanshokun ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@shumuKa", (int)pModel.ShumuKa);
            cmd.Parameters.AddWithValue("@kankeiKa", (int)pModel.KankeiKa);
            cmd.Parameters.AddWithValue("@genjo", pModel.GenjyoMondaiten ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@kaizen", pModel.Kaizenan ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@koukaJissi", (int)pModel.KoukaJishi);
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
                    groupmember2_affiliation,
                    groupmember2_department,
                    groupmember2_section,
                    groupmember2_subsection,
                    groupmember3_affiliation,
                    groupmember3_department,
                    groupmember3_section,
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
                FROM Te_proposals_detail 
                WHERE id = @id";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
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
            UPDATE Te_proposals_detail
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
                groupmember2_affiliation = @group2,
                groupmember2_department = @group2BuSho,
                groupmember2_section = @group2KaBumon,
                groupmember2_subsection = @group2Kakari,
                groupmember3_affiliation = @group3,
                groupmember3_department = @group3BuSho,
                groupmember3_section = @group3KaBumon,
                groupmember3_subsection = @group3Kakari,
                daiijishinsashaHezuIsChecked = @hezuChecked,
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
            cmd.Parameters.AddWithValue("@proposalType", (int)pModel.TeianShurui);
            cmd.Parameters.AddWithValue("@proposalKbn", (int)pModel.TeianKbn);
            cmd.Parameters.AddWithValue("@from", (int)pModel.Shozoku);
            cmd.Parameters.AddWithValue("@nameOrRepName", pModel.ShimeiOrDaihyoumei ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@groupName", pModel.GroupMei ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group1", (int)pModel.GroupZenin1);
            cmd.Parameters.AddWithValue("@group1BuSho", pModel.GroupZenin1BuSho ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group1KaBumon", pModel.GroupZenin1KaBumon ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group1Kakari", pModel.GroupZenin1KakariTantou ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group2", (int)pModel.GroupZenin2);
            cmd.Parameters.AddWithValue("@group2BuSho", pModel.GroupZenin2BuSho ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group2KaBumon", pModel.GroupZenin2KaBumon ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group2Kakari", pModel.GroupZenin2KakariTantou ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group3", (int)pModel.GroupZenin3);
            cmd.Parameters.AddWithValue("@group3BuSho", pModel.GroupZenin3BuSho ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group3KaBumon", pModel.GroupZenin3KaBumon ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@group3Kakari", pModel.GroupZenin3KakariTantou ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@hezuChecked", pModel.DaiijishinsashaHezuIsChecked);
            cmd.Parameters.AddWithValue("@hezuShozoku", (int)pModel.DaiijishinsashaShozoku);
            cmd.Parameters.AddWithValue("@hezuBuSho", pModel.DaiijishinsashaBuSho ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@hezuKaBumon", pModel.DaiijishinsashaKaBumon ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@hezuShimei", pModel.DaiijishinsashaShimei ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@hezuKanshoku", pModel.DaiijishinsashaKanshokun ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@shumuKa", (int)pModel.ShumuKa);
            cmd.Parameters.AddWithValue("@kankeiKa", (int)pModel.KankeiKa);
            cmd.Parameters.AddWithValue("@genjo", pModel.GenjyoMondaiten ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@kaizen", pModel.Kaizenan ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@koukaJissi", (int)pModel.KoukaJishi);
            cmd.Parameters.AddWithValue("@kouka", pModel.Kouka ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@submissiondate", pModel.Submissiondate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@createddate", pModel.Createddate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@id", pModel.Id);

            cmd.ExecuteNonQuery();
        }
        
        // 提案書一覧登録
        public void SqlInsertproposals(CreateModel pModel)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            string sql = @"
            INSERT INTO Te_proposals (
                id,
                user_id,
                proposal_year,
                business_year,
                status,
                pd_name,
                user_name,
                [from],
                submitted_at,
                point,
                decision,
                resubmission,
                review_section,
                related_sections,
                award_division,
                updated_at,
                created_at,
                [delete]
            )
            SELECT 
                id,
                user_id,
                proposal_year,
                proposal_year,
                status,
                pd_name,
                nameOrrepresentativename,
                [from],
                submissiondate,
                NULL,
                NULL,
                1,
                NULL,
                relatedsection,
                NULL,
                updatedate,
                createddate,
                0
            FROM
                Te_proposals_detail
            WHERE
                Te_proposals_detail.id = @id
                ";

            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@id", pModel.Id ?? (object)DBNull.Value);

            cmd.ExecuteNonQuery();

        }

        // 提案書一覧更新
        public void SqlUpdateproposals(CreateModel pModel)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            string sql = @"
                        UPDATE T
                        SET
                            T.user_id = D.user_id,
                            T.proposal_year = D.proposal_year,
                            T.status = D.status,
                            T.pd_name = D.pd_name,
                            T.user_name = D.nameOrrepresentativename,
                            T.[from] = D.[from],
                            T.submitted_at = D.submissiondate,
                            T.related_sections = D.relatedsection,
                            T.updated_at = D.updatedate
                        FROM Te_proposals T
                        JOIN Te_proposals_detail D ON T.id = D.id
                        WHERE T.id = @id;
                         ";

            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@id", pModel.Id ?? (object)DBNull.Value);

            cmd.ExecuteNonQuery();
        }
    }
}
