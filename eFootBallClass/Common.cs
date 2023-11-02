using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace eFootBallClass
{
    public class Common
    {
        public static string DBConn
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
            }
        }
        public static DataSet LoadMatch(int nextHour, string Type)
        {
            List<Match> lstMatch = GetListMatch().Where(c => c.match_time >= DateTime.Now.AddHours(nextHour - 2) && c.match_time <= DateTime.Now.AddHours(nextHour + 2)).ToList();
            using (SqlConnection myConn = new SqlConnection(DBConn))
            {
                myConn.Open();
                UpdateListMatch(lstMatch, myConn);
                UpdateMatchInfo(lstMatch, nextHour, myConn);

                using (SqlDataAdapter da = new SqlDataAdapter("EXEC PGetListMatch " + nextHour + ",'" + Type + "'", myConn))
                {
                    using (DataSet ds = new DataSet())
                    {
                        da.Fill(ds);
                        return ds;
                    }
                }
            }
        }

        private static void UpdateListMatch(List<Match> lstMatch, SqlConnection myConn)
        {
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM DMatch", myConn))
            {
                using (DataTable dt = new DataTable())
                {
                    da.Fill(dt);
                    foreach (Match match in lstMatch)
                    {
                        try
                        {
                            DataRow dr = dt.AsEnumerable().FirstOrDefault(c => c["id"].ToString() == match.id.Value.ToString());
                            if (dr == null)
                            {
                                dr = dt.Rows.Add();
                                dr["IsLoadHistory"] = false;
                            }

                            InsertMatch(dr, match);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }

                    dt.Update(da);
                }
            }
        }

        private static void UpdateMatchInfo(List<Match> lstMatch, int nextHour, SqlConnection myConn)
        {
            using (SqlDataAdapter da = new SqlDataAdapter("EXEC PGetListMatchToLoad " + nextHour, myConn))
            {
                using (DataTable dt = new DataTable())
                {
                    da.Fill(dt);

                    foreach (DataRow dr in dt.Rows)
                    {
                        string matchId = dr["id"].ToString();

                        Match match = lstMatch.FirstOrDefault(c => c.id.ToString() == matchId.ToString());
                        if (match != null)
                        {
                            InsertOdd(match, myConn);
                        }
                        UpdateMatchStatistic(dr, matchId, myConn);
                        UpdateTimeLine(dr, matchId, myConn);
                        bool IsLoadHistory = (bool)dr["IsLoadHistory"];
                        if (IsLoadHistory == false)
                        {
                            string home_team_id = dr["home_team_id"].ToString();
                            string away_team_id = dr["away_team_id"].ToString();
                            UpdateMatchHistory(matchId, home_team_id, away_team_id, myConn);
                        }
                    }

                    dt.Update(da);
                }
            }
        }

        private static void UpdateMatchStatistic(DataRow drMatch, string matchId, SqlConnection myConn)
        {
            List<MatchStatistic> lstStatistic = GetMatchStatistic(matchId);
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
        private static void UpdateTimeLine(DataRow drMatch, string matchId, SqlConnection myConn)
        {
            List<MatchTimeLine> lstTimeline = GetMatchTimeLine(matchId);
            if(lstTimeline.Count <= 0) { return; }
            using (SqlCommand cmd = new SqlCommand("DELETE DMatchTimeLine WHERE match_id = " + matchId, myConn))
            {
                cmd.ExecuteNonQuery();
            }
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM DMatchTimeLine WHERE match_id = " + matchId, myConn))
            {
                using (DataTable dt = new DataTable())
                {
                    da.Fill(dt);
                    try
                    {
                        drMatch["position"] = lstTimeline.Max(c => c.position);
                    }
                    catch { }


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
        private static void UpdateMatchHistory(string match_id, string homeTeamId, string awayTeamId, SqlConnection myConn)
        {
            List<Match> lstMatch = GetMatchHistory(homeTeamId, awayTeamId);
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM DMatch", myConn))
            {
                using (DataTable dt = new DataTable())
                {
                    da.Fill(dt);
                    foreach (Match match in lstMatch)
                    {
                        DataRow dr = dt.AsEnumerable().FirstOrDefault(c => c["id"].ToString() == match.id.Value.ToString());
                        if (dr == null)
                        {
                            dr = dt.Rows.Add();
                            dr["IsLoadHistory"] = true;
                        }

                        InsertMatch(dr, match);
                        UpdateMatchStatistic(dr, match.id.ToString(), myConn);
                        UpdateTimeLine(dr, match.id.ToString(), myConn);
                    }

                    dt.Update(da);
                }
            }

            using (SqlCommand cmd = new SqlCommand("UPDATE DMatch SET IsLoadHistory = 1 WHERE id = " + match_id, myConn))
            {
                cmd.ExecuteNonQuery();
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
        private static void InsertOdd(Match match, SqlConnection myConn)
        {
            if (match.odds._1 != null && match.odds._1.Count > 0 && match.odds._3 != null && match.odds._3.Count > 0)
            {
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM DMatchOdd WHERE 1=0", myConn))
                {
                    using (DataTable dt = new DataTable())
                    {
                        da.Fill(dt);

                        // Old
                        DataRow drOld = dt.Rows.Add();
                        drOld["match_id"] = match.id.ToSqlParam();
                        drOld["last_update_time"] = DateTime.Now.AddMinutes(-1);

                        if (match.odds._1 != null && match.odds._1.FirstOrDefault(c => c.type == "1") != null)
                        {
                            drOld["hdc"] = match.odds._1.FirstOrDefault(c => c.type == "1").ovalue0.ToSqlParam();
                        }

                        if (match.odds._1 != null && match.odds._1.FirstOrDefault(c => c.type == "1") != null)
                        {
                            drOld["hdc_home"] = match.odds._1.FirstOrDefault(c => c.type == "1").value0.ToSqlParam();
                        }

                        if (match.odds._1 != null && match.odds._1.FirstOrDefault(c => c.type == "2") != null)
                        {
                            drOld["hdc_way"] = match.odds._1.FirstOrDefault(c => c.type == "2").value0.ToSqlParam();
                        }

                        if (match.odds._3 != null && match.odds._3.FirstOrDefault(c => c.type == "over") != null)
                        {
                            drOld["tx"] = match.odds._3.FirstOrDefault(c => c.type == "over").ovalue0.ToSqlParam();
                        }

                        if (match.odds._3 != null && match.odds._3.FirstOrDefault(c => c.type == "over") != null)
                        {
                            drOld["tx_over"] = match.odds._3.FirstOrDefault(c => c.type == "over").value0.ToSqlParam();
                        }

                        if (match.odds._3 != null && match.odds._3.FirstOrDefault(c => c.type == "under") != null)
                        {
                            drOld["tx_under"] = match.odds._3.FirstOrDefault(c => c.type == "under").value0.ToSqlParam();
                        }

                        // Now
                        DataRow dr = dt.Rows.Add();
                        dr["match_id"] = match.id.ToSqlParam();
                        dr["last_update_time"] = DateTime.Now;
                        if (match.odds._1 != null && match.odds._1.FirstOrDefault(c => c.type == "1") != null)
                        {
                            dr["hdc"] = match.odds._1.FirstOrDefault(c => c.type == "1").ovalue.ToSqlParam();
                        }

                        if (match.odds._1 != null && match.odds._1.FirstOrDefault(c => c.type == "1") != null)
                        {
                            dr["hdc_home"] = match.odds._1.FirstOrDefault(c => c.type == "1").value.ToSqlParam();
                        }

                        if (match.odds._1 != null && match.odds._1.FirstOrDefault(c => c.type == "2") != null)
                        {
                            dr["hdc_way"] = match.odds._1.FirstOrDefault(c => c.type == "2").value.ToSqlParam();
                        }

                        if (match.odds._3 != null && match.odds._3.FirstOrDefault(c => c.type == "over") != null)
                        {
                            dr["tx"] = match.odds._3.FirstOrDefault(c => c.type == "over").ovalue.ToSqlParam();
                        }

                        if (match.odds._3 != null && match.odds._3.FirstOrDefault(c => c.type == "over") != null)
                        {
                            dr["tx_over"] = match.odds._3.FirstOrDefault(c => c.type == "over").value.ToSqlParam();
                        }

                        if (match.odds._3 != null && match.odds._3.FirstOrDefault(c => c.type == "under") != null)
                        {
                            dr["tx_under"] = match.odds._3.FirstOrDefault(c => c.type == "under").value.ToSqlParam();
                        }

                        try
                        {
                            dt.Update(da);
                        }
                        catch (Exception ex)
                        {
                            //throw ex;
                        }
                    }
                }
            }
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

            return lstMatch;
        }
        private static List<MatchStatistic> GetMatchStatistic(string matchId)
        {
            string ketQua = GetApiData("https://trauscore.com/api/match/statistic?langId=32&matchId=" + matchId);
            return JsonConvert.DeserializeObject<List<MatchStatistic>>(ketQua);
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

        public void GetMatchOdds(long matchId, int typeId, long bookId = 138) // 1 = hdc, 3 = tx
        {
            string ketQua = GetApiData("https://trauscore.com/api/match/odd/logs?langId=32&matchId=" + matchId + "&bookId=" + bookId + "&typeId=" + typeId);
        }




        public static List<MatchTimeLine> GetMatchTimeLine(string matchId)
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
