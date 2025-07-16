using Microsoft.AspNetCore.Http.HttpResults;
using Proposal.DAC;
using Proposal.Models;
using System.Reflection;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        

        ///// <summary>
        ///// 提案書一覧登録
        ///// </summary>
        //public void Insertproposals(CreateModel pModel)
        //{
        //    _createDAC.SqlInsertproposals(pModel);
        //}

        ///// <summary>
        ///// 提案書一覧更新
        ///// </summary>
        //public void Updateproposals(CreateModel pModel)
        //{
        //    _createDAC.SqlUpdateproposals(pModel);
        //}

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
                model.ProposalTypeId = row["proposal_type"] == DBNull.Value ? null : (int?)Convert.ToInt32(row["proposal_type"]);
                model.TeianKbn = row["proposal_kbn"] == DBNull.Value ? TeianKbn.Select : (TeianKbn)Convert.ToInt32(row["proposal_kbn"]);
                model.Shozoku = row["from"] == DBNull.Value ? Shozoku.Select : (Shozoku)Convert.ToInt32(row["from"]);
                model.ShimeiOrDaihyoumei = row["nameOrrepresentativename"].ToString();
                model.GroupMei = row["groupname"].ToString();
                
                // グループの全員①
                model.GroupZenin1 = row["groupmember1_affiliation"] == DBNull.Value ? Shozoku.Select : (Shozoku)Convert.ToInt32(row["groupmember1_affiliation"]);
                model.GroupZenin1BuSho = row["groupmember1_department"].ToString();
                model.GroupZenin1KaBumon = row["groupmember1_section"].ToString();
                model.GroupZenin1KakariTantou = row["groupmember1_subsection"].ToString();
                
                // グループの全員②
                model.GroupZenin2 = row["groupmember2_affiliation"] == DBNull.Value ? Shozoku.Select : (Shozoku)Convert.ToInt32(row["groupmember2_affiliation"]);
                model.GroupZenin2BuSho = row["groupmember2_department"].ToString();
                model.GroupZenin2KaBumon = row["groupmember2_section"].ToString();
                model.GroupZenin2KakariTantou = row["groupmember2_subsection"].ToString();
                
                // グループの全員③
                model.GroupZenin3 = row["groupmember3_affiliation"] == DBNull.Value ? Shozoku.Select : (Shozoku)Convert.ToInt32(row["groupmember3_affiliation"]);
                model.GroupZenin3BuSho = row["groupmember3_department"].ToString();
                model.GroupZenin3KaBumon = row["groupmember3_section"].ToString();
                model.GroupZenin3KakariTantou = row["groupmember3_subsection"].ToString();
                
                // 第一次審査者
                model.DaiijishinsashaHezuIsChecked = Convert.ToBoolean(row["daiijishinsashaHezuIsChecked"]);
                model.DaiijishinsashaShozoku = row["firstevieweraffiliation"] == DBNull.Value ? Shozoku.Select : (Shozoku)Convert.ToInt32(row["firstevieweraffiliation"]);
                model.DaiijishinsashaBuSho = row["firsteviewerdepartment"].ToString();
                model.DaiijishinsashaKaBumon = row["firsteviewersection"].ToString();
                model.DaiijishinsashaShimei = row["firsteviewername"].ToString();
                model.DaiijishinsashaKanshokun = row["firsteviewertitle"].ToString();
                
                // 主務課・関係課
                model.ShumuKa = row["responsiblesection"] == DBNull.Value ? ShumuKa.Select : (ShumuKa)Convert.ToInt32(row["responsiblesection"]);
                model.KankeiKa = row["relatedsection"] == DBNull.Value ? KankeiKa.Select : (KankeiKa)Convert.ToInt32(row["relatedsection"]);
                
                // 現状・問題点・改善案・効果
                model.GenjyoMondaiten = row["currentsituation"].ToString();
                model.Kaizenan = row["improvementproposal"].ToString();
                model.KoukaJishi = row["isImplemented"] == DBNull.Value ? KoukaJishi.Select : (KoukaJishi)Convert.ToInt32(row["isImplemented"]);
                model.Kouka = row["effect"].ToString();
                
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

        //// 提案種類の一覧を取得するメソッド（DAC層のメソッドを呼び出すだけ）
        public List<ProposalType> GetProposalTypes()
        {
            // DAC（Data Access Component）層から提案種類の一覧を取得して返す
            return _createDAC.GetProposalTypes();
        }

        //// 提案区分の一覧を取得するメソッド（DAC層のメソッドを呼び出すだけ）


        //// 提案所属の一覧を取得するメソッド（DAC層のメソッドを呼び出すだけ）



        /// <summary>
        /// 任意のリストをSelectListItemのリストに変換するプライベートメソッド
        /// </summary>
        /// <typeparam name="T">変換元の型</typeparam>
        /// <param name="items">変換元リスト</param>
        /// <param name="getValue">値を取得するFunc</param>
        /// <param name="getText">表示テキストを取得するFunc</param>
        /// <param name="addDefault">先頭に「選択してください」を追加するか</param>
        /// <returns>SelectListItemのリスト</returns>
        private List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> GetSelectListItems<T>(List<T> items, Func<T, string> getValue, Func<T, string> getText, bool addDefault = true)
        {
            var list = items.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = getValue(x),
                Text = getText(x)
            }).ToList();
            if (addDefault)
            {
                list.Insert(0, new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "", Text = "選択してください" });
            }
            return list;
        }

        /// <summary>
        /// 提案種類のSelectListItemリストを取得
        /// </summary>
        public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> GetProposalTypeSelectList()
        {
            // 提案種類のリストを取得（データベースなどから）
            var types = GetProposalTypes();

            // 提案種類のリストを <select> 用の SelectListItem に変換して返す
            // 第1引数：値（value）にするプロパティ（Id）
            // 第2引数：表示名（text）にするプロパティ（Type）
            return GetSelectListItems(types, x => x.Id.ToString(), x => x.Type);
        }

        /// <summary>
        /// 提案区分のSelectListItemリストを取得
        /// </summary>
        /// 

        /// <summary>
        /// 提案所属のSelectListItemリストを取得
        /// </summary>





        /// <summary>
        /// すべてのドロップダウンリストをまとめて取得するメソッド
        /// </summary>
        public DropdownsViewModel GetDropdowns()
        {
            return new DropdownsViewModel
            {
                //提案種類
                ProposalTypes = GetProposalTypeSelectList()
                // 他のドロップダウンもここに追加可能

                //提案区分

                //提案所属
            };
        }
    }

    /// <summary>
    /// すべてのドロップダウンリストを格納するViewModel
    /// </summary>
    public class DropdownsViewModel
    {
        //提案種類
        public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> ProposalTypes { get; set; }
        // 他のドロップダウンもここに追加可能

        //提案区分

        //提案所属
    }
}
