using Common;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Web;

namespace wsGetFBInfo
{
    public partial class Service : ServiceBase
    {
        public static string DBConn
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
            }
        }

        Timer tmrStart, tmrStart_BDL, tmrUpdateHistory, tmrUpdateHistory_BDL;
        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            tmrStart_BDL = new Timer(60000);
            tmrStart_BDL.Elapsed += TmrStart_BDL_Elapsed;
            tmrStart_BDL.Enabled = true;
            tmrStart_BDL.AutoReset = true;
            tmrStart_BDL.Start();

            tmrUpdateHistory_BDL = new Timer(3000);
            tmrUpdateHistory_BDL.Elapsed += TmrUpdateHistory_BDL_Elapsed;
            tmrUpdateHistory_BDL.Enabled = true;
            tmrUpdateHistory_BDL.AutoReset = true;
            tmrUpdateHistory_BDL.Start();


            tmrStart = new Timer(60000);
            tmrStart.Elapsed += TmrStart_Elapsed;
            tmrStart.Enabled = true;
            tmrStart.AutoReset = true;
            //tmrStart.Start();

            tmrUpdateHistory = new Timer(60000);
            tmrUpdateHistory.Elapsed += TmrUpdateHistory_Elapsed;
            tmrUpdateHistory.Enabled = true;
            tmrUpdateHistory.AutoReset = true;
            //tmrUpdateHistory.Start();
        }

        public void TmrUpdateHistory_BDL_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (tmrUpdateHistory_BDL != null) { tmrUpdateHistory_BDL.Enabled = false; }
            try
            {
                using (SqlConnection myConn = new SqlConnection(DBConn))
                {
                    myConn.Open();
                    UpdateMatchHistory_BDL(myConn);
                }
            }
            catch (Exception ex)
            {
                MyApp.Log.GhiLog("TmrUpdateHistory_BDL_Elapsed", ex);
            }
            finally
            {
                if (tmrUpdateHistory_BDL != null) { tmrUpdateHistory_BDL.Enabled = true; }
            }
        }

        public void TmrStart_BDL_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (tmrStart_BDL != null) { tmrStart_BDL.Enabled = false; }
            try
            {

                List<DMatch_BDL> lstMatch = GetListMatch_BDL();

                using (SqlConnection myConn = new SqlConnection(DBConn))
                {
                    myConn.Open();
                    //SaveListMatch_BDL(lstMatch, myConn);
                    UpdateListMatch_BDL(myConn);
                }
            }
            catch (Exception ex)
            {
                MyApp.Log.GhiLog("TmrUpdateHistory_Elapsed", ex);
            }
            finally
            {
                if (tmrStart_BDL != null) { tmrStart_BDL.Enabled = true; }
            }
        }


        public void TmrStart_Elapsed(object sender, ElapsedEventArgs e)
        {
            tmrStart.Enabled = false;
            try
            {
                List<Match> lstMatch = GetListMatch();

                using (SqlConnection myConn = new SqlConnection(DBConn))
                {
                    myConn.Open();
                    UpdateListMatch(lstMatch, myConn);
                }
            }
            catch (Exception ex)
            {
                MyApp.Log.GhiLog("TmrStart_Elapsed", ex);
            }
            finally
            {
                tmrStart.Enabled = true;
            }
        }
        private void TmrUpdateHistory_Elapsed(object sender, ElapsedEventArgs e)
        {
            tmrUpdateHistory.Enabled = false;
            try
            {
                List<Match> lstMatch = GetListMatch();

                using (SqlConnection myConn = new SqlConnection(DBConn))
                {
                    myConn.Open();
                    UpdateMatchHistory(lstMatch, myConn);
                }
            }
            catch (Exception ex)
            {
                MyApp.Log.GhiLog("TmrUpdateHistory_Elapsed", ex);
            }
            finally
            {
                tmrUpdateHistory.Enabled = true;
            }
        }



        protected override void OnStop()
        {
            MyApp.Log.GhiLog("OnStop", "OnStop");
        }



        private static List<Match> GetListMatch()
        {
            DateTime current = DateTime.Now;
            DateTime back3h = current.AddMinutes(-180);
            DateTime next3h = current.AddMinutes(180);

            List<string> lstDate = new List<string>();
            lstDate.Add(DateTime.Now.ToString("yyyy-MM-dd"));
            lstDate.Add(DateTime.Now.AddMinutes(-180).ToString("yyyy-MM-dd"));
            lstDate.Add(DateTime.Now.AddMinutes(180).ToString("yyyy-MM-dd"));

            List<Match> lstMatch = new List<Match>();
            foreach (string date in lstDate.Distinct())
            {
                string ketQua = GetApiData("https://trauscore.com/api/match/list?langId=32&sportId=1&date=" + date + "&utcOffset=420");
                lstMatch.AddRange(JsonConvert.DeserializeObject<List<Match>>(ketQua));
            }

            foreach (Match item in lstMatch)
            {
                item.match_time = item.match_time.AddHours(7);
            }

            MyApp.Log.GhiLog("GetListMatch", lstMatch.Count.ToString());
            return lstMatch.Where(c => c.match_time >= DateTime.Now.AddHours(-2) && !string.IsNullOrEmpty(c.round) && ((c.odds._1 != null && c.odds._1.Count > 0) || (c.odds._121 != null && c.odds._121.Count > 0))).ToList();
        }

        private static List<DMatch_BDL> GetListMatch_BDL()
        {
            DateTime current = DateTime.Now;
            DateTime back3h = current.AddMinutes(-180);
            DateTime next3h = current.AddMinutes(180);

            List<string> lstDate = new List<string>();
            lstDate.Add(DateTime.Now.ToString("dd-MM-yyyy"));
            lstDate.Add(DateTime.Now.AddMinutes(-360).ToString("dd-MM-yyyy"));
            lstDate.Add(DateTime.Now.AddMinutes(360).ToString("dd-MM-yyyy"));

            List<DMatch_BDL> lstMatch = new List<DMatch_BDL>();
            foreach (string date in lstDate.Distinct())
            {
                string ketQua = GetApiData("https://www.bongdalu5.com/football/schedule/" + date);

                int idx_mDataArray = ketQua.IndexOf("_mDataArray");
                int idx_sclassData = ketQua.IndexOf("var _sclassData", idx_mDataArray);
                string mDataArray = ketQua.Substring(idx_mDataArray + 15, idx_sclassData - idx_mDataArray - 15).Trim();
                string[] MatchStrArr = mDataArray.Split('!');

                foreach (string MatchStr in MatchStrArr)
                {
                    try
                    {
                        string[] SoLieuArr = MatchStr.Split('^');
                        string[] arr_match_time = SoLieuArr[6].Split(',');
                        DateTime match_time = new DateTime(arr_match_time[0].ToInt().Value, arr_match_time[1].ToInt().Value + 1, arr_match_time[2].ToInt().Value, arr_match_time[3].ToInt().Value, arr_match_time[4].ToInt().Value, arr_match_time[5].ToInt().Value);
                        string[] arr_hdc = SoLieuArr[7].Split('_');
                        string[] arr_tx = SoLieuArr[8].Split('_');

                        if (arr_hdc.Length <= 1 || arr_tx.Length <= 1) { continue; }
                        lstMatch.Add(new DMatch_BDL()
                        {
                            id = SoLieuArr[1].ToInt().Value,
                            match_time = match_time.AddHours(7),
                            state_id = SoLieuArr[4].ToInt(),
                            league_id = SoLieuArr[2].ToInt(),

                            home_name = HttpUtility.HtmlDecode(SoLieuArr[10]),
                            away_name = HttpUtility.HtmlDecode(SoLieuArr[11]),

                            home_score = SoLieuArr[12].ToInt(),
                            away_score = SoLieuArr[13].ToInt(),

                            home_score_h1 = SoLieuArr[14].ToInt(),
                            away_score_h1 = SoLieuArr[15].ToInt(),

                            home_rank = SoLieuArr[16].ToInt(),
                            away_rank = SoLieuArr[17].ToInt(),

                            home_red = SoLieuArr[18].ToInt(),
                            away_red = SoLieuArr[19].ToInt(),

                            home_yellow = SoLieuArr[19].ToInt(),
                            away_yellow = SoLieuArr[20].ToInt(),

                            home_corner = SoLieuArr[24].ToInt(),
                            away_corner = SoLieuArr[25].ToInt(),

                            hdc = arr_hdc[1].ToDouble(),
                            tx = arr_tx[1].ToDouble(),
                        });
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }


            }
            return lstMatch;
            //return lstMatch.Where(c => c.match_time >= DateTime.Now.AddHours(-2) && !string.IsNullOrEmpty(c.round) && ((c.odds._1 != null && c.odds._1.Count > 0) || (c.odds._121 != null && c.odds._121.Count > 0))).ToList();
        }
        private static void UpdateListMatch(List<Match> lstMatch, SqlConnection myConn)
        {
            foreach (Match match in lstMatch)
            {
                SaveMatch(match, myConn, false);
            }
        }

        private static void SaveMatch(Match match, SqlConnection myConn, bool IsHistory)
        {
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM DMatch WHERE id = " + match.id.Value.ToString(), myConn))
            {
                using (DataTable dt = new DataTable())
                {
                    da.Fill(dt);
                    DataRow dr = null;
                    if (dt.Rows.Count == 0)
                    {
                        dr = dt.Rows.Add();
                        dr["IsLoadHistory"] = false;
                    }
                    else
                    {
                        dr = dt.Rows[0];
                    }

                    InsertMatch(dr, match);


                    bool IsDangDienRa = IsHistory || (DateTime.Now >= match.match_time.AddMinutes(-5) && DateTime.Now <= match.match_time.AddHours(2));
                    if (IsDangDienRa)
                    {
                        int position = 0;
                        UpdateMatchTimeLine(match.id.Value.ToString(), myConn, ref position);
                        UpdateMatchStatistic(match.id.Value.ToString(), myConn);
                        dr["position"] = position;
                    }
                    dt.Update(da);
                }
            }
        }

        private static void SaveListMatch_BDL(List<DMatch_BDL> lstMatch, SqlConnection myConn)
        {
            foreach (DMatch_BDL match in lstMatch)
            {
                try
                {
                    using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM DMatch_BDL WHERE id = " + match.id.ToString(), myConn))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            da.Fill(dt);
                            DataRow dr = null;
                            if (dt.Rows.Count == 0)
                            {
                                dr = dt.Rows.Add();
                                dr["is_load_his"] = false;
                                dr["is_his"] = false;
                                dr["hdc_ns"] = match.hdc.ToSqlParam();
                                dr["tx_ns"] = match.tx.ToSqlParam();
                            }
                            else
                            {
                                dr = dt.Rows[0];
                            }
                            SaveMatch_BDL(dr, match);
                            dt.Update(da);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MyApp.Log.GhiLog("SaveListMatch_BDL", ex, match.id.ToString());
                    throw ex;
                }
            }
        }

        private static void SaveMatchById_BDL(string id, SqlConnection myConn)
        {
            try
            {
                if (myConn.State != ConnectionState.Open) { myConn.Open(); }
                string KetQua = GetApiData("https://www.bongdalu5.com/football/match/live-" + id);
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(KetQua);

                string findText = "document.getElementById(\"liveMt\").innerText = timeToText(ToLocalTime(";
                int match_time_idx = KetQua.IndexOf(findText) + findText.Length;

                HtmlNode nodeMatch = doc.GetElementbyId("match");
                HtmlNode nodeHome = nodeMatch.Descendants("div").FirstOrDefault(c => c.HasClass("home"));
                HtmlNode nodeGuest = nodeMatch.Descendants("div").FirstOrDefault(c => c.HasClass("guest"));
                HtmlNode node_liveHt = doc.GetElementbyId("liveHt");
                string[] score_h1 = node_liveHt.InnerText.Replace("(", "").Replace(")", "").Split('-');
                HtmlNode nodeStat = doc.GetElementbyId("dataMatchStatsBar");
                HtmlNode nodeHDPOdds = doc.GetElementbyId("hdpOdds");
                HtmlNode nodeHDPKeoSom_before = nodeHDPOdds?.Descendants("tr").FirstOrDefault(c => c.Attributes["data-comp"] != null && c.Attributes["data-comp"].Value == "lg_31");
                HtmlNode nodeHDPKeoSom_after = nodeHDPOdds?.Descendants("tr").LastOrDefault(c => c.Attributes["data-comp"] != null && c.Attributes["data-comp"].Value == "lg_31");
                HtmlNode nodeHDP_before = nodeHDPKeoSom_before?.Descendants("td").FirstOrDefault(c => c.Attributes["data-of"] != null);
                HtmlNode nodeHDP_after = nodeHDPKeoSom_after?.Descendants("td").LastOrDefault(c => c.Attributes["data-of"] != null);
                HtmlNode nodeOUOdds = doc.GetElementbyId("ouOdds");
                HtmlNode nodeOUKeoSom_before = nodeOUOdds?.Descendants("tr").FirstOrDefault(c => c.Attributes["data-comp"] != null && c.Attributes["data-comp"].Value == "ou_31");
                HtmlNode nodeOUKeoSom_after = nodeOUOdds?.Descendants("tr").LastOrDefault(c => c.Attributes["data-comp"] != null && c.Attributes["data-comp"].Value == "ou_31");
                HtmlNode nodeOU_before = nodeOUKeoSom_before?.Descendants("td").FirstOrDefault(c => c.Attributes["data-of"] != null);
                HtmlNode nodeOU_after = nodeOUKeoSom_after?.Descendants("td").LastOrDefault(c => c.Attributes["data-of"] != null);

                using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM DMatch_BDL WHERE id = " + id, myConn))
                {
                    using (DataTable dt = new DataTable())
                    {
                        da.Fill(dt);
                        DataRow dr;
                        if (dt.Rows.Count == 0)
                        {
                            dr = dt.Rows.Add();
                            dr["id"] = id;
                            dr["is_his"] = true;
                        }
                        else
                        {
                            dr = dt.Rows[0];
                        }

                        dr["match_time"] = DateTime.ParseExact(KetQua.Substring(match_time_idx, 16).Split('\'')[1], "yyyyMMddHHmmss", CultureInfo.InvariantCulture).AddHours(-1);
                        dr["state_id"] = -1;
                        dr["home_id"] = nodeHome.Descendants("span").FirstOrDefault(c => c.HasClass("name")).Descendants("span").FirstOrDefault().Attributes["onclick"].Value.Split('/')[3].Split('\'')[0];
                        dr["away_id"] = nodeGuest.Descendants("span").FirstOrDefault(c => c.HasClass("name")).Descendants("span").FirstOrDefault().Attributes["onclick"].Value.Split('/')[3].Split('\'')[0];
                        dr["home_score"] = doc.GetElementbyId("liveHS").InnerText.ToInt().ToSqlParam();
                        dr["away_score"] = doc.GetElementbyId("liveGS").InnerText.ToInt().ToSqlParam();
                        dr["home_score_h1"] = score_h1[0].Trim();
                        dr["away_score_h1"] = score_h1[1].Trim();


                        foreach (HtmlNode node in nodeStat.Descendants("div").Where(c => c.HasClass("data")))
                        {
                            string stat_name = HttpUtility.HtmlDecode(node.SelectNodes("span")[1].InnerText.Trim()).Trim();

                            if (stat_name.ToUpper() == "PHẠT GÓC")
                            {
                                dr["home_corner"] = HttpUtility.HtmlDecode(node.SelectNodes("span")[0].InnerText.Replace("%", "").Trim()).ToInt().Value;
                                dr["away_corner"] = HttpUtility.HtmlDecode(node.SelectNodes("span")[2].InnerText.Replace("%", "").Trim()).ToInt().Value;
                            }
                            else if (stat_name.ToUpper() == "PHẠT GÓC NỬA TRẬN")
                            {
                                dr["home_corner_h1"] = HttpUtility.HtmlDecode(node.SelectNodes("span")[0].InnerText.Replace("%", "").Trim()).ToInt().Value;
                                dr["away_corner_h1"] = HttpUtility.HtmlDecode(node.SelectNodes("span")[2].InnerText.Replace("%", "").Trim()).ToInt().Value;
                            }
                            else if (stat_name.ToUpper() == "SỐ LẦN SÚT BÓNG")
                            {
                                dr["home_shot"] = HttpUtility.HtmlDecode(node.SelectNodes("span")[0].InnerText.Replace("%", "").Trim()).ToInt().Value;
                                dr["away_shot"] = HttpUtility.HtmlDecode(node.SelectNodes("span")[2].InnerText.Replace("%", "").Trim()).ToInt().Value;
                            }
                            else if (stat_name.ToUpper() == "SÚT CẦU MÔN")
                            {
                                dr["home_shotot"] = HttpUtility.HtmlDecode(node.SelectNodes("span")[0].InnerText.Replace("%", "").Trim()).ToInt().Value;
                                dr["away_shotot"] = HttpUtility.HtmlDecode(node.SelectNodes("span")[2].InnerText.Replace("%", "").Trim()).ToInt().Value;
                            }
                            else if (stat_name.ToUpper() == "THẺ VÀNG")
                            {
                                dr["home_yellow"] = HttpUtility.HtmlDecode(node.SelectNodes("span")[0].InnerText.Replace("%", "").Trim()).ToInt().Value;
                                dr["away_yellow"] = HttpUtility.HtmlDecode(node.SelectNodes("span")[2].InnerText.Replace("%", "").Trim()).ToInt().Value;
                            }
                            else if (stat_name.ToUpper() == "THẺ ĐỎ")
                            {
                                dr["home_red"] = HttpUtility.HtmlDecode(node.SelectNodes("span")[0].InnerText.Replace("%", "").Trim()).ToInt().Value;
                                dr["away_red"] = HttpUtility.HtmlDecode(node.SelectNodes("span")[2].InnerText.Replace("%", "").Trim()).ToInt().Value;
                            }
                        }


                        if (nodeHDP_before != null)
                        {
                            dr["hdc_ns"] = nodeHDP_before.Attributes["data-of"].Value.Split(',')[1];
                        }
                        if (nodeHDP_after != null)
                        {
                            dr["hdc_now"] = nodeHDP_after.Attributes["data-of"].Value.Split(',')[1];
                        }

                        if (nodeOU_before != null)
                        {
                            dr["tx_ns"] = nodeOU_before.Attributes["data-of"].Value.Split(',')[1];
                        }

                        if (nodeOU_after != null)
                        {
                            dr["tx_now"] = nodeOU_after.Attributes["data-of"].Value.Split(',')[1];
                        }

                        dt.Update(da);
                    }
                }
            }
            catch (Exception ex)
            {
                MyApp.Log.GhiLog("SaveMatchById_BDL", ex, id);
                throw ex;
            }
        }
        private static void UpdateListMatch_BDL(SqlConnection myConn)
        {
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM DMatch_BDL WHERE home_id IS NULL OR state_id in (1,2,3) OR (match_time BETWEEN DATEADD(MINUTE,-15, GETDATE()) AND DATEADD(HOUR,6,GETDATE())) ORDER BY match_time ASC", myConn))
            {
                using (DataTable dt = new DataTable())
                {
                    da.Fill(dt);

                    foreach (DataRow dr in dt.Rows)
                    {
                        string id = dr["id"].ToString();
                        try
                        {
                            string KetQua = GetApiData("https://www.bongdalu5.com/football/match/live-" + id);

                            HtmlDocument doc = new HtmlDocument();
                            doc.LoadHtml(KetQua);
                            HtmlNode nodeStat = doc.GetElementbyId("dataMatchStatsBar");
                            foreach (HtmlNode node in nodeStat.Descendants("div").Where(c => c.HasClass("data")))
                            {
                                string stat_name = HttpUtility.HtmlDecode(node.SelectNodes("span")[1].InnerText.Trim()).Trim();

                                if (stat_name.ToUpper() == "PHẠT GÓC NỬA TRẬN")
                                {
                                    dr["home_corner_h1"] = HttpUtility.HtmlDecode(node.SelectNodes("span")[0].InnerText.Replace("%", "").Trim()).ToInt().Value;
                                    dr["away_corner_h1"] = HttpUtility.HtmlDecode(node.SelectNodes("span")[2].InnerText.Replace("%", "").Trim()).ToInt().Value;
                                }
                                else if (stat_name.ToUpper() == "SỐ LẦN SÚT BÓNG")
                                {
                                    dr["home_shot"] = HttpUtility.HtmlDecode(node.SelectNodes("span")[0].InnerText.Replace("%", "").Trim()).ToInt().Value;
                                    dr["away_shot"] = HttpUtility.HtmlDecode(node.SelectNodes("span")[2].InnerText.Replace("%", "").Trim()).ToInt().Value;
                                }
                                else if (stat_name.ToUpper() == "SÚT CẦU MÔN")
                                {
                                    dr["home_shotot"] = HttpUtility.HtmlDecode(node.SelectNodes("span")[0].InnerText.Replace("%", "").Trim()).ToInt().Value;
                                    dr["away_shotot"] = HttpUtility.HtmlDecode(node.SelectNodes("span")[2].InnerText.Replace("%", "").Trim()).ToInt().Value;
                                }
                            }

                            HtmlNode nodeHDPOdds = doc.GetElementbyId("hdpOdds");
                            HtmlNode nodeHDPKeoSom_before = nodeHDPOdds?.Descendants("tr").FirstOrDefault(c => c.Attributes["data-comp"] != null && c.Attributes["data-comp"].Value == "lg_31");
                            HtmlNode nodeHDPKeoSom_after = nodeHDPOdds?.Descendants("tr").LastOrDefault(c => c.Attributes["data-comp"] != null && c.Attributes["data-comp"].Value == "lg_31");

                            HtmlNode nodeHDP_before = nodeHDPKeoSom_before?.Descendants("td").FirstOrDefault(c => c.Attributes["data-of"] != null);
                            HtmlNode nodeHDP_after = nodeHDPKeoSom_after?.Descendants("td").LastOrDefault(c => c.Attributes["data-of"] != null);
                            if (nodeHDP_before != null)
                            {
                                dr["hdc_ns"] = nodeHDP_before.Attributes["data-of"].Value.Split(',')[1];
                            }
                            if (nodeHDP_after != null)
                            {
                                dr["hdc_now"] = nodeHDP_after.Attributes["data-of"].Value.Split(',')[1];
                            }


                            HtmlNode nodeOUOdds = doc.GetElementbyId("ouOdds");
                            HtmlNode nodeOUKeoSom_before = nodeOUOdds?.Descendants("tr").FirstOrDefault(c => c.Attributes["data-comp"] != null && c.Attributes["data-comp"].Value == "ou_31");
                            HtmlNode nodeOUKeoSom_after = nodeOUOdds?.Descendants("tr").LastOrDefault(c => c.Attributes["data-comp"] != null && c.Attributes["data-comp"].Value == "ou_31");
                            HtmlNode nodeOU_before = nodeOUKeoSom_before?.Descendants("td").FirstOrDefault(c => c.Attributes["data-of"] != null);
                            HtmlNode nodeOU_after = nodeOUKeoSom_after?.Descendants("td").LastOrDefault(c => c.Attributes["data-of"] != null);

                            if (nodeOU_before != null)
                            {
                                dr["tx_ns"] = nodeOU_before.Attributes["data-of"].Value.Split(',')[1];
                            }
                            if (nodeOU_after != null)
                            {
                                dr["tx_now"] = nodeOU_after.Attributes["data-of"].Value.Split(',')[1];
                            }


                            HtmlNode nodeLeague = doc.GetElementbyId("wi");
                            dr["league_name"] = HttpUtility.HtmlDecode(nodeLeague.InnerText);

                            //HtmlNode nodePosition = doc.GetElementbyId("liveSt");
                            //dr["position"] = nodePosition.InnerText;
                            foreach (HtmlNode nodeLiveTime in doc.DocumentNode.Descendants("td").Where(c => c.HasClass("liveTime_ht")))
                            {
                                int? position = nodeLiveTime.InnerText.Replace("'", "").ToInt();
                                if (dr["position"] == null || dr["position"] == DBNull.Value)
                                {
                                    dr["position"] = position;
                                }
                                else
                                {
                                    if (position.HasValue && position.Value > dr["position"].ToInt().Value)
                                    {
                                        dr["position"] = position;
                                    }
                                }
                            }

                            HtmlNode nodeMatch = doc.GetElementbyId("match");
                            HtmlNode nodeHome = nodeMatch.Descendants("div").FirstOrDefault(c => c.HasClass("home"));
                            HtmlNode nodeGuest = nodeMatch.Descendants("div").FirstOrDefault(c => c.HasClass("guest"));
                            dr["home_id"] = nodeHome.Descendants("span").FirstOrDefault(c => c.HasClass("name")).Descendants("span").FirstOrDefault().Attributes["onclick"].Value.Split('/')[3].Split('\'')[0];
                            dr["away_id"] = nodeGuest.Descendants("span").FirstOrDefault(c => c.HasClass("name")).Descendants("span").FirstOrDefault().Attributes["onclick"].Value.Split('/')[3].Split('\'')[0];

                            dr["last_update_time"] = DateTime.Now;
                        }
                        catch (Exception ex)
                        {
                            MyApp.Log.GhiLog("UpdateListMatch_BDL", ex, id);
                            throw ex;
                        }
                    }

                    dt.Update(da);
                }
            }
        }
        private static void SaveMatch_BDL(DataRow dr, DMatch_BDL match)
        {
            dr["id"] = match.id.ToSqlParam();
            dr["match_time"] = match.match_time.ToSqlParam();
            //dr["position"] = match.position.ToSqlParam();
            dr["state_id"] = match.state_id.ToSqlParam();
            dr["league_id"] = match.league_id.ToSqlParam();
            //dr["home_id"] = match.home_id.ToSqlParam();
            //dr["away_id"] = match.away_id.ToSqlParam();
            //dr["league_name"] = match.league_name.ToSqlParam();
            dr["home_name"] = match.home_name.ToSqlParam();
            dr["away_name"] = match.away_name.ToSqlParam();
            dr["home_rank"] = match.home_rank.ToSqlParam();
            dr["away_rank"] = match.away_rank.ToSqlParam();
            dr["home_score"] = match.home_score.ToSqlParam();
            dr["home_corner"] = match.home_corner.ToSqlParam();
            dr["home_red"] = match.home_red.ToSqlParam();
            dr["home_yellow"] = match.home_yellow.ToSqlParam();
            dr["home_score_h1"] = match.home_score_h1.ToSqlParam();
            //dr["home_corner_h1"] = match.home_corner_h1.ToSqlParam();
            dr["away_score"] = match.away_score.ToSqlParam();
            dr["away_corner"] = match.away_corner.ToSqlParam();
            dr["away_red"] = match.away_red.ToSqlParam();
            dr["away_yellow"] = match.away_yellow.ToSqlParam();
            dr["away_score_h1"] = match.away_score_h1.ToSqlParam();
            //dr["away_corner_h1"] = match.away_corner_h1.ToSqlParam();
            dr["hdc_on"] = match.hdc.ToSqlParam();
            dr["tx_on"] = match.tx.ToSqlParam();

            dr["last_update_time"] = DateTime.Now;
        }
        private static void UpdateMatchHistory_BDL(SqlConnection myConn)
        {
            DataTable dtMatch = MyApp.Dao.GetTable("SELECT TOP 10 * FROM DMatch_BDL WHERE match_time >= DATEADD(MINUTE,-120,GETDATE()) AND match_time <= DATEADD(HOUR,6,GETDATE()) AND home_id IS NOT NULL AND is_his = 0 AND is_load_his = 0 ORDER BY match_time ASC", null, myConn);

            if (dtMatch.Rows.Count == 0)
            {
                dtMatch = MyApp.Dao.GetTable("SELECT TOP 10 * FROM DMatch_BDL WHERE match_time >= DATEADD(MINUTE,-120,GETDATE()) AND home_id IS NOT NULL AND is_his = 0 AND is_load_his = 0 ORDER BY match_time ASC", null, myConn);
            }

            foreach (DataRow drMatch in dtMatch.Rows)
            {
                string id = drMatch["id"].ToString();
                try
                {
                    string KetQua = GetApiData("https://www.bongdalu5.com/football/match/h2h-" + id);
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(KetQua);
                    HtmlNode node_list_h2h = doc.GetElementbyId("e5_1");
                    if (node_list_h2h != null)
                    {
                        foreach (HtmlNode nodeH2H in node_list_h2h.Descendants("div").Where(c => c.Attributes["onclick"] != null && c.Attributes["onclick"].Value.StartsWith("goTo")))
                        {
                            string id_his = nodeH2H.Attributes["onclick"].Value.Split('/')[3].Split('-')[1].Split('\'')[0];
                            SaveMatchById_BDL(id_his, myConn);
                        }
                    }

                    HtmlNode node_list_home = doc.DocumentNode.Descendants("table").FirstOrDefault(c => c.Attributes["name"] != null && c.Attributes["name"].Value == "lm_home_data");
                    if (node_list_home != null)
                    {
                        foreach (HtmlNode nodeHome in node_list_home.Descendants("div").Where(c => c.Attributes["onclick"] != null && c.Attributes["onclick"].Value.StartsWith("goTo")))
                        {
                            string id_his = nodeHome.Attributes["onclick"].Value.Split('/')[3].Split('-')[1].Split('\'')[0];
                            SaveMatchById_BDL(id_his, myConn);
                        }
                    }

                    HtmlNode node_list_away = doc.DocumentNode.Descendants("table").FirstOrDefault(c => c.Attributes["name"] != null && c.Attributes["name"].Value == "lm_guest_data");
                    if (node_list_away != null)
                    {
                        foreach (HtmlNode nodeAway in node_list_away.Descendants("div").Where(c => c.Attributes["onclick"] != null && c.Attributes["onclick"].Value.StartsWith("goTo")))
                        {
                            string id_his = nodeAway.Attributes["onclick"].Value.Split('/')[3].Split('-')[1].Split('\'')[0];
                            SaveMatchById_BDL(id_his, myConn);
                        }
                    }

                    MyApp.Dao.ExecSQL("UPDATE DMatch_BDL SET is_load_his = 1 WHERE id = " + id, null, myConn);
                }
                catch (Exception ex)
                {
                    MyApp.Log.GhiLog("UpdateMatchHistory_BDL", ex, id);
                    throw ex;
                }
            }
        }

        private static void UpdateMatchHistory(List<Match> lstMatch, SqlConnection myConn)
        {
            foreach (Match match in lstMatch)
            {
                try
                {
                    using (SqlDataAdapter daMatch = new SqlDataAdapter("SELECT * FROM DMatch WHERE id = " + match.id.Value.ToString(), myConn))
                    {
                        using (DataTable dtMatch = new DataTable())
                        {
                            daMatch.Fill(dtMatch);
                            if (dtMatch.Rows.Count == 0) { continue; }
                            DataRow drMatch = dtMatch.Rows[0];
                            bool IsLoadHistory = (bool)drMatch["IsLoadHistory"];
                            if (IsLoadHistory) { continue; }

                            string home_team_id = drMatch["home_team_id"].ToString();
                            string away_team_id = drMatch["away_team_id"].ToString();
                            List<Match> lstMatchHis = GetMatchHistory(home_team_id, away_team_id);

                            foreach (Match matchHis in lstMatchHis)
                            {
                                SaveMatch(matchHis, myConn, true);
                            }
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand("UPDATE DMatch SET IsLoadHistory = 1 WHERE id = " + match.id.Value.ToString(), myConn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MyApp.Log.GhiLog("UpdateMatchHistory", ex, match.id.Value.ToString());
                }
            }
        }
        private static void UpdateMatchStatistic(string matchId, SqlConnection myConn)
        {
            List<MatchStatistic> lstStatistic = GetMatchStatistic(matchId);
            if (lstStatistic.Count == 0) { return; }
            using (SqlCommand cmd = new SqlCommand("DELETE DMatchStatistic WHERE match_id = " + matchId, myConn))
            {
                cmd.ExecuteNonQuery();
            }
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM DMatchStatistic WHERE match_id = " + matchId, myConn))
            {
                using (DataTable dt = new DataTable())
                {
                    da.Fill(dt);
                    foreach (MatchStatistic statistic in lstStatistic)
                    {
                        if (statistic.type_id == 1 || statistic.type_id == 2 || statistic.type_id == 9 || statistic.type_id == 12 || statistic.type_id == 14 || statistic.type_id == 15 || statistic.type_id == 17 || statistic.type_id == 18 || statistic.type_id == 78 || statistic.type_id == 85)
                        {
                            DataRow dr = dt.AsEnumerable().FirstOrDefault(c => c["type_id"].ToString() == statistic.type_id.ToString() && c["period"].ToString() == statistic.period.ToString());
                            if (dr == null)
                            {
                                dr = dt.Rows.Add();
                            }

                            dr["match_id"] = statistic.match_id.ToSqlParam();
                            dr["type_id"] = statistic.type_id.ToSqlParam();
                            dr["team1"] = statistic.team1.ToSqlParam();
                            dr["team2"] = statistic.team2.ToSqlParam();
                            dr["update_time"] = statistic.update_time.ToSqlParam();
                            dr["period"] = statistic.period.ToSqlParam();
                            dr["type_name"] = statistic.type_name.ToSqlParam();
                            dr["last_update_time"] = DateTime.Now;
                        }
                    }

                    dt.Update(da);
                }
            }
        }
        private static void UpdateMatchTimeLine(string matchId, SqlConnection myConn, ref int Position)
        {
            List<MatchTimeLine> lstTimeline = GetMatchTimeLine(matchId);
            if (lstTimeline.Count <= 0) { return; }

            try
            {
                Position = lstTimeline.Max(c => c.position);
            }
            catch { }

            using (SqlCommand cmd = new SqlCommand("DELETE DMatchTimeLine WHERE match_id = " + matchId, myConn))
            {
                cmd.ExecuteNonQuery();
            }
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM DMatchTimeLine WHERE match_id = " + matchId, myConn))
            {
                using (DataTable dt = new DataTable())
                {
                    da.Fill(dt);
                    foreach (MatchTimeLine timeline in lstTimeline)
                    {
                        if (timeline.list != null && timeline.list.Count > 0)
                        {
                            foreach (List item in timeline.list)
                            {
                                if (item.type_id == 9 || item.type_id == 18 || item.type_id == 22 || item.type_id == 30)
                                {
                                    DataRow dr = dt.AsEnumerable().FirstOrDefault(c => c["id"].ToString() == item.id.ToString());
                                    if (dr == null)
                                    {
                                        dr = dt.Rows.Add();
                                        dr["id"] = item.id.ToSqlParam();
                                    }
                                    dr["match_id"] = matchId.ToSqlParam();
                                    dr["position"] = timeline.position.ToSqlParam();
                                    dr["team"] = timeline.team.ToSqlParam();
                                    dr["time"] = item.time.ToSqlParam();
                                    dr["type_id"] = item.type_id.ToSqlParam(); //timeline.type_id.ToSqlParam();
                                    dr["type_name"] = item.type_name.ToSqlParam();
                                    dr["scores"] = item.scores.ToSqlParam();
                                }
                            }
                        }
                    }

                    dt.Update(da);
                }
            }
        }

        private static void InsertMatch(DataRow dr, Match match)
        {
            dr["id"] = match.id.ToSqlParam();
            dr["tournament_id"] = match.tournament_id.ToSqlParam();
            dr["match_time"] = match.match_time.ToSqlParam();
            dr["start_time"] = match.start_time.ToSqlParam();
            dr["end_time"] = match.end_time.ToSqlParam();
            dr["home_team_id"] = match.home_team_id.ToSqlParam();
            dr["away_team_id"] = match.away_team_id.ToSqlParam();
            dr["home_team_ranking"] = match.home_team_ranking.ToSqlParam();
            dr["away_team_ranking"] = match.away_team_ranking.ToSqlParam();
            dr["home_team_score"] = match.home_team_score.ToSqlParam();
            dr["away_team_score"] = match.away_team_score.ToSqlParam();
            dr["home_team_half_time_score"] = match.home_team_half_time_score.ToSqlParam();
            dr["away_team_half_time_score"] = match.away_team_half_time_score.ToSqlParam();
            dr["home_team_normal_time_score"] = match.home_team_normal_time_score.ToSqlParam();
            dr["away_team_normal_time_score"] = match.away_team_normal_time_score.ToSqlParam();
            dr["status"] = match.status.ToSqlParam();
            dr["status_code"] = match.status_code.ToSqlParam();
            dr["winner"] = match.winner.ToSqlParam();
            dr["season_id"] = match.season_id.ToSqlParam();
            dr["season"] = match.season.ToSqlParam();
            dr["time_played"] = match.time_played.ToSqlParam();
            dr["time_remaining"] = match.time_remaining.ToSqlParam();
            dr["time_running"] = match.time_running.ToSqlParam();
            dr["time_update"] = match.time_update.ToSqlParam();
            dr["lmt_mode"] = match.lmt_mode.ToSqlParam();
            dr["has_live"] = match.has_live.ToSqlParam();
            dr["weather"] = match.weather.ToSqlParam();
            dr["is_visible"] = match.is_visible.ToSqlParam();
            dr["is_hot"] = match.is_hot.ToSqlParam();
            dr["ranking_cal_mode"] = match.ranking_cal_mode.ToSqlParam();
            dr["home_team_corner"] = match.home_team_corner.ToSqlParam();
            dr["away_team_corner"] = match.away_team_corner.ToSqlParam();
            dr["home_team_yellow"] = match.home_team_yellow.ToSqlParam();
            dr["away_team_yellow"] = match.away_team_yellow.ToSqlParam();
            dr["home_team_red"] = match.home_team_red.ToSqlParam();
            dr["away_team_red"] = match.away_team_red.ToSqlParam();
            dr["home_team_overtime_score"] = match.home_team_overtime_score.ToSqlParam();
            dr["away_team_overtime_score"] = match.away_team_overtime_score.ToSqlParam();
            dr["home_team_penalty_score"] = match.home_team_penalty_score.ToSqlParam();
            dr["away_team_penalty_score"] = match.away_team_penalty_score.ToSqlParam();
            dr["previous_home_team_id"] = match.previous_home_team_id.ToSqlParam();
            dr["previous_away_team_id"] = match.previous_away_team_id.ToSqlParam();
            dr["previous_home_team_score"] = match.previous_home_team_score.ToSqlParam();
            dr["previous_away_team_score"] = match.previous_away_team_score.ToSqlParam();
            dr["tournament_name"] = match.tournament_name.ToSqlParam();
            dr["home_team_name"] = match.home_team_name.ToSqlParam();
            dr["away_team_name"] = match.away_team_name.ToSqlParam();
            dr["status_name"] = match.status_name.ToSqlParam();
            dr["stadium_name"] = match.stadium_name.ToSqlParam();
            dr["round"] = match.round.ToSqlParam();
            dr["round_type"] = match.round_type.ToSqlParam();
            dr["referee_name"] = match.referee_name.ToSqlParam();
            dr["category_name"] = match.category_name.ToSqlParam();
            dr["has_standings"] = match.has_standings.ToSqlParam();
            dr["has_knockouts"] = match.has_knockouts.ToSqlParam();
            dr["last_update_time"] = DateTime.Now;



            if (match.odds._1 != null && match.odds._1.FirstOrDefault(c => c.type == "1") != null)
            {
                dr["ns_hdc_now"] = match.odds._1.FirstOrDefault(c => c.type == "1").ovalue.ToSqlParam();
            }

            if (match.odds._1 != null && match.odds._1.FirstOrDefault(c => c.type == "1") != null)
            {
                dr["ns_hdc_now_home"] = match.odds._1.FirstOrDefault(c => c.type == "1").value.ToSqlParam();
            }

            if (match.odds._1 != null && match.odds._1.FirstOrDefault(c => c.type == "2") != null)
            {
                dr["ns_hdc_now_way"] = match.odds._1.FirstOrDefault(c => c.type == "2").value.ToSqlParam();
            }

            if (match.odds._1 != null && match.odds._1.FirstOrDefault(c => c.type == "1") != null)
            {
                dr["ns_hdc_before"] = match.odds._1.FirstOrDefault(c => c.type == "1").ovalue0.ToSqlParam();
            }

            if (match.odds._1 != null && match.odds._1.FirstOrDefault(c => c.type == "1") != null)
            {
                dr["ns_hdc_before_home"] = match.odds._1.FirstOrDefault(c => c.type == "1").value0.ToSqlParam();
            }

            if (match.odds._1 != null && match.odds._1.FirstOrDefault(c => c.type == "2") != null)
            {
                dr["ns_hdc_before_way"] = match.odds._1.FirstOrDefault(c => c.type == "2").value0.ToSqlParam();
            }

            if (match.odds._3 != null && match.odds._3.FirstOrDefault(c => c.type == "over") != null)
            {
                dr["ns_tx_now"] = match.odds._3.FirstOrDefault(c => c.type == "over").ovalue.ToSqlParam();
            }

            if (match.odds._3 != null && match.odds._3.FirstOrDefault(c => c.type == "over") != null)
            {
                dr["ns_tx_now_over"] = match.odds._3.FirstOrDefault(c => c.type == "over").value.ToSqlParam();
            }

            if (match.odds._3 != null && match.odds._3.FirstOrDefault(c => c.type == "under") != null)
            {
                dr["ns_tx_now_under"] = match.odds._3.FirstOrDefault(c => c.type == "under").value.ToSqlParam();
            }


            if (match.odds._3 != null && match.odds._3.FirstOrDefault(c => c.type == "over") != null)
            {
                dr["ns_tx_before"] = match.odds._3.FirstOrDefault(c => c.type == "over").ovalue0.ToSqlParam();
            }

            if (match.odds._3 != null && match.odds._3.FirstOrDefault(c => c.type == "over") != null)
            {
                dr["ns_tx_before_over"] = match.odds._3.FirstOrDefault(c => c.type == "over").value0.ToSqlParam();
            }

            if (match.odds._3 != null && match.odds._3.FirstOrDefault(c => c.type == "under") != null)
            {
                dr["ns_tx_before_under"] = match.odds._3.FirstOrDefault(c => c.type == "under").value0.ToSqlParam();
            }


            //////////////////////////


            if (match.odds._121.FirstOrDefault(c => c.type == "1") != null)
            {
                dr["hdc_now"] = match.odds._121.FirstOrDefault(c => c.type == "1").ovalue.ToSqlParam();
            }

            if (match.odds._121.FirstOrDefault(c => c.type == "1") != null)
            {
                dr["hdc_now_home"] = match.odds._121.FirstOrDefault(c => c.type == "1").value.ToSqlParam();
            }

            if (match.odds._121.FirstOrDefault(c => c.type == "2") != null)
            {
                dr["hdc_now_way"] = match.odds._121.FirstOrDefault(c => c.type == "2").value.ToSqlParam();
            }

            if (match.odds._121.FirstOrDefault(c => c.type == "1") != null)
            {
                dr["hdc_before"] = match.odds._121.FirstOrDefault(c => c.type == "1").ovalue0.ToSqlParam();
            }

            if (match.odds._121.FirstOrDefault(c => c.type == "1") != null)
            {
                dr["hdc_before_home"] = match.odds._121.FirstOrDefault(c => c.type == "1").value0.ToSqlParam();
            }

            if (match.odds._121.FirstOrDefault(c => c.type == "2") != null)
            {
                dr["hdc_before_way"] = match.odds._121.FirstOrDefault(c => c.type == "2").value0.ToSqlParam();
            }

            if (match.odds._122.FirstOrDefault(c => c.type == "over") != null)
            {
                dr["tx_now"] = match.odds._122.FirstOrDefault(c => c.type == "over").ovalue.ToSqlParam();
            }

            if (match.odds._122.FirstOrDefault(c => c.type == "over") != null)
            {
                dr["tx_now_over"] = match.odds._122.FirstOrDefault(c => c.type == "over").value.ToSqlParam();
            }

            if (match.odds._122.FirstOrDefault(c => c.type == "under") != null)
            {
                dr["tx_now_under"] = match.odds._122.FirstOrDefault(c => c.type == "under").value.ToSqlParam();
            }


            if (match.odds._122.FirstOrDefault(c => c.type == "over") != null)
            {
                dr["tx_before"] = match.odds._122.FirstOrDefault(c => c.type == "over").ovalue0.ToSqlParam();
            }

            if (match.odds._122.FirstOrDefault(c => c.type == "over") != null)
            {
                dr["tx_before_over"] = match.odds._122.FirstOrDefault(c => c.type == "over").value0.ToSqlParam();
            }

            if (match.odds._122.FirstOrDefault(c => c.type == "under") != null)
            {
                dr["tx_before_under"] = match.odds._122.FirstOrDefault(c => c.type == "under").value0.ToSqlParam();
            }
        }
        private static List<Match> GetMatchHistory(string homeTeamId, string awayTeamId)
        {
            List<Match> lstMatch = new List<Match>();
            string ketQuaH2H = GetApiData("https://trauscore.com/api/match/history/battle?langId=32&homeTeamId=" + homeTeamId + "&awayTeamId=" + awayTeamId + "&type=0&limit=5");
            string ketQuaHome = GetApiData("https://trauscore.com/api/team/history?langId=32&teamId=" + homeTeamId + "&type=3&limit=5&leagueId=0");
            string ketQuaAway = GetApiData("https://trauscore.com/api/team/history?langId=32&teamId=" + awayTeamId + "&type=3&limit=5&leagueId=0");
            lstMatch.AddRange(JsonConvert.DeserializeObject<List<Match>>(ketQuaH2H));
            lstMatch.AddRange(JsonConvert.DeserializeObject<List<Match>>(ketQuaHome));
            lstMatch.AddRange(JsonConvert.DeserializeObject<List<Match>>(ketQuaAway));
            return lstMatch;
        }
        private static List<MatchStatistic> GetMatchStatistic(string matchId)
        {
            string ketQua = GetApiData("https://trauscore.com/api/match/statistic?langId=32&matchId=" + matchId);
            return JsonConvert.DeserializeObject<List<MatchStatistic>>(ketQua);
        }
        private static List<MatchTimeLine> GetMatchTimeLine(string matchId)
        {
            string ketQua = GetApiData("https://trauscore.com/api/match/timeline?langId=32&matchId=" + matchId);
            return JsonConvert.DeserializeObject<List<MatchTimeLine>>(ketQua);
        }
        private static string GetApiData(string Url)
        {
            using (WebClient webClient = new WebClient() { Encoding = Encoding.UTF8 })
            {
                return webClient.DownloadString(Url);
            }
        }
    }
}
