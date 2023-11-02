using eFootBallClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eFBWeb
{
    public partial class Main : System.Web.UI.Page
    {
        public static string DBConn
        {
            get
            {
                return WebConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
            }
        }

        public int Hour
        {
            get
            {
                if (HttpContext.Current == null || HttpContext.Current.Session["Hour"] == null)
                {
                    return 0;
                }
                return HttpContext.Current.Session["Hour"].ToInt().Value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.IsPostBack) { return; }

            grdData.DataBind();

        }

        protected void LoadData(object sender, EventArgs e)
        {
            HttpContext.Current.Session["Hour"] = int.Parse(spiHour.Number.ToString());
            grdData.DataBind();
        }

        protected void grdData_DataBinding(object sender, EventArgs e)
        {
            using (SqlDataAdapter da = new SqlDataAdapter("EXEC PGetListMatch " + Hour + "," + (chkShowAll.Checked ? "1" : "0") + "", DBConn))
            {
                using (DataSet ds = new DataSet())
                {
                    da.Fill(ds);
                    grdData.DataSource = ds.Tables[0];
                }
            }
        }

        protected void grdData_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName.Equals("ns_tx_gap"))
            {
                if (e.CellValue.ToDouble().HasValue)
                {
                    if (e.CellValue.ToDouble().Value > 0)
                    {
                        e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccffee");
                    }
                }
            }
            else if (e.DataColumn.FieldName.Equals("wl_point"))
            {
                if (e.CellValue.ToDouble().HasValue)
                {
                    if (e.CellValue.ToDouble().Value > 1)
                    {
                        e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccffee");
                    }
                }
            }
            else if (e.DataColumn.FieldName.Equals("his_home_corner_text"))
            {
                if (string.IsNullOrEmpty(e.CellValue.ToString()) == false)
                {
                    double? val = e.CellValue.ToString().Split(' ')[0].ToDouble();
                    if (val.HasValue)
                    {
                        if (val.Value >= 5)
                        {
                            e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccffee");
                        }
                        else if (val.Value <= 3)
                        {
                            e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffba");
                        }
                    }
                }
            }
            else if (e.DataColumn.FieldName.Equals("his_away_corner_text"))
            {
                if (string.IsNullOrEmpty(e.CellValue.ToString()) == false)
                {
                    double? val = e.CellValue.ToString().Split(' ')[0].ToDouble();
                    if (val.HasValue)
                    {
                        if (val.Value >= 5)
                        {
                            e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccffee");
                        }
                        else if (val.Value <= 3)
                        {
                            e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffba");
                        }
                    }
                }
            }
            else if (e.DataColumn.FieldName.Equals("his_btts_text"))
            {
                if (string.IsNullOrEmpty(e.CellValue.ToString()) == false)
                {
                    double? val = e.CellValue.ToString().Split('%')[0].ToDouble();
                    if (val.HasValue)
                    {
                        if (val.Value >= 80)
                        {
                            e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccffee");
                        }
                    }
                }
            }
            else if (e.DataColumn.FieldName.EndsWith("_note"))
            {
                if (string.IsNullOrEmpty(e.CellValue.ToString()) == false)
                {
                    bool IsTai = e.CellValue.ToString().StartsWith("Tài");
                    if (IsTai)
                    {
                        e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccffee");
                    }

                    bool IsBTai = e.CellValue.ToString().StartsWith("B-Tài");
                    if (IsBTai)
                    {
                        e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#d71868");
                        e.Cell.ForeColor = System.Drawing.Color.White;
                        e.Cell.Font.Bold = true;
                    }
                }
            }
        }
    }
}