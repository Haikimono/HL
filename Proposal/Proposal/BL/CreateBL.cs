using Microsoft.AspNetCore.Http.HttpResults;
using Proposal.DAC;
using Proposal.Models;
using System.Reflection;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace Proposal.BL
{
    public class CreateBL
    {
        private readonly CreateDAC _createDAC = new CreateDAC();

        public CreateBL()
        {

        }

        public CreateBL(string connectionString)
        {
            _createDAC = new CreateDAC(connectionString);
        }

        /// <summary>
        /// 提案書詳細登録
        /// </summary>
        public int Insertproposals_detail(CreateModel pModel)
        {
            //作成日
            pModel.Createddate = DateTime.Now.ToString();

            //提出日
            //一時保存
            if (pModel.Status == 1)
            {
                pModel.Submissiondate = null;
            }
            //提出
            else if(pModel.Status == 11)
            {
                pModel.Submissiondate = DateTime.Now.ToString();
            }

            return _createDAC.SqlInsertproposals_detail(pModel);
        }

        /// <summary>
        /// 提案書詳細更新
        /// </summary>
        public void Updateproposals_detail(CreateModel pModel)
        {
            //提出日
            //一時保存
            if (pModel.Status == 1)
            {
                pModel.Submissiondate = null;
            }
            //提出
            else if (pModel.Status == 11)
            {
                pModel.Submissiondate = DateTime.Now.ToString();
            }

            //提案書詳細更新
            _createDAC.SqlUpdateproposals_detail(pModel);
        } 

        /// <summary>
        /// 提案書の詳細情報を取得し、モデルに設定
        /// </summary>
        public void GetProposalDetailById(CreateModel model)
        {
            var dataTable = _createDAC.GetProposalDetailById(model.Id);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                var row = dataTable.Rows[0]; // 指定IDのデータを取得

                // モデルに設定
                model.Id = row["id"].ToString();
                model.Status = row["status"] == DBNull.Value ? null : (int?)Convert.ToInt32(row["status"]);
                model.TeianYear = row["proposal_year"].ToString();
                model.TeianDaimei = row["pd_name"].ToString();
                model.ProposalTypeId = row["proposal_type"].ToString();
                model.ProposalKbnId = row["proposal_kbn"].ToString();
                model.AffiliationId = row["from"].ToString();
                model.FirstReviewerAffiliationId = row["firstevieweraffiliation"].ToString();
                model.FirstReviewerDepartmentId = row["firsteviewerdepartment"].ToString();
                model.FirstReviewerSectionId = row["firsteviewersection"].ToString();
                model.FirstReviewerName = row["firsteviewername"].ToString();
                model.FirstReviewerTitle = row["firsteviewertitle"].ToString();
                model.EvaluationSectionId = row["evaluation_section_id"].ToString();
                model.ResponsibleSectionId1 = row["responsible_section_id1"].ToString();
                model.ResponsibleSectionId2 = row["responsible_section_id2"].ToString();
                model.ResponsibleSectionId3 = row["responsible_section_id3"].ToString();
                model.ResponsibleSectionId4 = row["responsible_section_id4"].ToString();
                model.ResponsibleSectionId5 = row["responsible_section_id5"].ToString();
                model.KoukaJishi = row["isImplemented"] == DBNull.Value ? (KoukaJishi?)null : (KoukaJishi)Convert.ToInt32(row["isImplemented"]);
                
                // 添付ファイル
                model.TenpuFileName1 = row["attachmentfilename1"].ToString();
                model.TenpuFileName2 = row["attachmentfilename2"].ToString();
                model.TenpuFileName3 = row["attachmentfilename3"].ToString();
                model.TenpuFileName4 = row["attachmentfilename4"].ToString();
                model.TenpuFileName5 = row["attachmentfilename5"].ToString();
                model.TenpuFilePath1 = row["attachmentfilepath1"].ToString();
                model.TenpuFilePath2 = row["attachmentfilepath2"].ToString();
                model.TenpuFilePath3 = row["attachmentfilepath3"].ToString();
                model.TenpuFilePath4 = row["attachmentfilepath4"].ToString();
                model.TenpuFilePath5 = row["attachmentfilepath5"].ToString();
                
                // 日付
                model.Submissiondate = row["submissiondate"].ToString();
                model.Createddate = row["createddate"].ToString();
            }
        }

        /// <summary>
        /// ユーザー情報を取得し、モデルに設定
        /// </summary>     
        public void GetUserInfoByUserId(CreateModel model)
        {
            var dataTable = _createDAC.GetUserInfoByUserId(model.UserId);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                var row = dataTable.Rows[0]; // 指定IDのデータを取得

                model.AffiliationName = row["affiliation_name"].ToString();
                model.DepartmentName = row["department_name"].ToString();
                model.SectionName = row["section_name"].ToString();
                model.SubsectionName = row["subsection_name"].ToString();
            }
        }

        // 提案区分の一覧を取得するメソッド（DAC層のメソッドを呼び出すだけ）

        public List<SelectListItem> GetProposalKbnList()
        {
            var list = _createDAC.GetProposalKbnList();
            list.Insert(0, new SelectListItem { Value = "", Text = "選択してください" });
            return list;
        }

        /// <summary>
        /// 提案の種類のSelectListItemリストを取得
        /// </summary>
        public List<SelectListItem> GetProposalTypeList()
        {
            var list = _createDAC.GetProposalTypeList();
            list.Insert(0, new SelectListItem { Value = "", Text = "選択してください" });
            return list;
        }


        /// <summary>
        /// 提案所属のSelectListItemリストを取得
        /// </summary>
        public List<SelectListItem> GetAffiliations()
        {
            var list = _createDAC.GetAffiliations().Select(a => new SelectListItem { Value = a.Id, Text = a.Shozoku }).ToList();
            list.Insert(0, new SelectListItem { Value = "", Text = "選択してください" });
            return list;
        }

        /// <summary>
        /// 部・署のSelectListItemリストを取得
        /// </summary>
        public List<SelectListItem> GetDepartments()
        {
            var list = _createDAC.GetDepartments().Select(d => new SelectListItem { Value = d.Department_id, Text = d.Department_name }).ToList();
            list.Insert(0, new SelectListItem { Value = "", Text = "選択してください" });
            return list;
        }

        /// <summary>
        /// 課・部門のSelectListItemリストを取得
        /// </summary>
        public List<SelectListItem> GetSections()
        {
            var list = _createDAC.GetSections().Select(s => new SelectListItem { Value = s.Section_id, Text = s.Section_name }).ToList();
            list.Insert(0, new SelectListItem { Value = "", Text = "選択してください" });
            return list;
        }

        /// <summary>
        /// 係・担当のSelectListItemリストを取得
        /// </summary>
        public List<SelectListItem> GetSubsections()
        {
            var list = _createDAC.GetSubsections().Select(s => new SelectListItem { Value = s.Subsection_id, Text = s.Subsection_name }).ToList();
            list.Insert(0, new SelectListItem { Value = "", Text = "選択してください" });
            return list;
        }

        /// <summary>
        /// 実施のSelectListItemリストを取得
        /// </summary>
        public List<SelectListItem> GetKoukaJishiSelectList()
        {
            return Enum.GetValues(typeof(KoukaJishi)).Cast<KoukaJishi>().Select(e => new SelectListItem
            {
                Value = ((int)e).ToString(),
                Text = (e.GetType().GetField(e.ToString()).GetCustomAttribute<System.ComponentModel.DescriptionAttribute>()?.Description) ?? e.ToString()
            }).ToList();
        }

        /// <summary>
        /// 主務課のSelectListItemリストを取得
        /// </summary>
        public List<SelectListItem> GetShumukaList()
        {
            return GetSections();
        }

        /// <summary>
        /// 関係課のSelectListItemリストを取得
        /// </summary>
        public List<SelectListItem> GetKankeikaList()
        {
            return GetSections();
        }

        /// <summary>
        /// すべてのドロップダウンリストをまとめて取得するメソッド
        /// </summary>
        public DropdownsViewModel GetDropdowns()
        {
            return new DropdownsViewModel
            {
                //提案所属
                Affiliations = GetAffiliations(),
                //部・署
                Departments = GetDepartments(),
                //課・部門
                Sections = GetSections(),
                //係・担当
                Subsections = GetSubsections(),
                //提案の種類
                ProposalTypes = GetProposalTypeList(),
                //提案区分
                ProposalKbn = GetProposalKbnList(),
                //実施
                KoukaJishiList = GetKoukaJishiSelectList(),
                //主務課
                Shumukas = GetShumukaList(),
                //関係課
                Kankeikas = GetKankeikaList(),
            };
        }

        //グループデータの削除
        public void DeleteGroupInfo(string proposalId)
        {
            // 既存のグループデータを削除する
            _createDAC.DeleteGroupInfo(proposalId);
        }

        //グループデータの登録
        public void InsertGroupInfo(string proposalId, List<GroupMemberModel> members)
        {
            // グループデータを挿入する
            _createDAC.InsertGroupInfo(proposalId, members);
        }
    }

    /// <summary>
    /// すべてのドロップダウンリストを格納するViewModel
    /// </summary>
    public class DropdownsViewModel
    {
        //提案所属
        public List<SelectListItem> Affiliations { get; set; }

        //部・署
        public List<SelectListItem> Departments { get; set; }

        //課・部門
        public List<SelectListItem> Sections { get; set; }

        //係・担当
        public List<SelectListItem> Subsections { get; set; }

        //提案種類
        public List<SelectListItem> ProposalTypes { get; set; }

        //提案区分
        public List<SelectListItem> ProposalKbn { get; set; }

        //実施
        public List<SelectListItem> KoukaJishiList { get; set; }

        //主務課
        public List<SelectListItem> Shumukas { get; set; }

        //関係課
        public List<SelectListItem> Kankeikas { get; set; }
    }
}
