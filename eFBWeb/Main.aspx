<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="eFBWeb.Main" %>

<%@ Register Assembly="DevExpress.Web.v21.1, Version=21.1.11.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
    </style>
    <script>
        function NavigateTS(s, e) {
            window.open("https://trauscore.com/match/" + s, "_blank");
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 100%; height: 100%; margin-left: auto; margin-right: auto">
            <dx:ASPxRoundPanel runat="server" Width="100%" HeaderText="Điều kiện tra cứu" ShowHeader="false" BackColor="White">
                <PanelCollection>
                    <dx:PanelContent>
                        <table>
                            <tr>
                                <td>
                                    <dx:ASPxSpinEdit Number="0" DecimalPlaces="0" Increment="1" runat="server" Caption="Giờ" ID="spiHour" Width="60" AutoPostBack="true" OnNumberChanged="LoadData"></dx:ASPxSpinEdit>
                                </td>

                                <td>
                                    <dx:ASPxCheckBox ID="chkShowAll" runat="server" Checked="false" OnCheckedChanged="LoadData" AutoPostBack="true" Text="Show All" Width="100"/>
                                </td>
                                <td style="width: 100%; text-align: right; padding-left: 8px">
                                    <dx:ASPxButton runat="server" Text="Tra cứu" ID="btnTraCuu" OnClick="LoadData" Height="30px" />
                                </td>
                            </tr>
                        </table>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxRoundPanel>
            <div style="height: 8px"></div>
            <dx:ASPxGridView Font-Size="7" ID="grdData" runat="server" Width="100%" ClientInstanceName="grdData" OnDataBinding="grdData_DataBinding" SettingsSearchPanel-Visible="true" SettingsPager-PageSize="9999" SettingsPager-Visible="false" SettingsBehavior-AllowSelectByRowClick="true" KeyFieldName="id" OnHtmlDataCellPrepared="grdData_HtmlDataCellPrepared" SettingsBehavior-AutoExpandAllGroups="true" Styles-GroupRow-Font-Bold="true">
                <Columns>
                    <dx:GridViewDataHyperLinkColumn FieldName="id" Caption="ID" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center">
                        <PropertiesHyperLinkEdit NavigateUrlFormatString="javascript:NavigateTS({0})" />
                    </dx:GridViewDataHyperLinkColumn>
                    <dx:GridViewDataTextColumn FieldName="tournament_name" Caption="Giải đấu" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" GroupIndex="0" />
                    <dx:GridViewDataDateColumn FieldName="match_time" Caption="Thời gian" PropertiesDateEdit-DisplayFormatString="dd/MM/yyyy HH:mm" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" />
                    <dx:GridViewDataTextColumn FieldName="position" Caption="Phút" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" />
                    <dx:GridViewDataTextColumn FieldName="his_btts_text" Caption="BTTS" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" />
                    <dx:GridViewDataTextColumn FieldName="wl_point" Caption="WL" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" Visible="true" />

                    <dx:GridViewBandColumn Caption="HOME" HeaderStyle-HorizontalAlign="Center">
                        <Columns>
                            <dx:GridViewDataTextColumn FieldName="home_team_name" Caption="Tên đội" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" />
                            <dx:GridViewBandColumn Caption="Lịch sử" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="True">
                                <Columns>
                                    <dx:GridViewDataTextColumn FieldName="his_home_corner_text" Caption="Góc" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" CellStyle-Wrap="False" />
                                    <dx:GridViewDataTextColumn FieldName="his_home_score_text" Caption="Banh" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" CellStyle-Wrap="False" />
                                </Columns>
                            </dx:GridViewBandColumn>
                            <dx:GridViewBandColumn Caption="Trực tiếp" HeaderStyle-HorizontalAlign="Center">
                                <Columns>
                                    <dx:GridViewDataTextColumn FieldName="home_shot_text" Caption="Sút" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" CellStyle-ForeColor="#007934" />
                                    <dx:GridViewDataTextColumn FieldName="home_corner" Caption="Góc" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" CellStyle-ForeColor="#ff6700" />
                                    <dx:GridViewDataTextColumn FieldName="home_score" Caption="Banh" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" CellStyle-Font-Bold="true" CellStyle-ForeColor="#2b0575" />
                                </Columns>
                            </dx:GridViewBandColumn>
                        </Columns>
                    </dx:GridViewBandColumn>

                    <dx:GridViewBandColumn Caption="AWAY" HeaderStyle-HorizontalAlign="Center">
                        <Columns>
                            <dx:GridViewBandColumn Caption="Trực tiếp" HeaderStyle-HorizontalAlign="Center">
                                <Columns>
                                    <dx:GridViewDataTextColumn FieldName="away_score" Caption="Banh" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" CellStyle-Font-Bold="true" CellStyle-ForeColor="#2b0575" />
                                    <dx:GridViewDataTextColumn FieldName="away_corner" Caption="Góc" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" CellStyle-ForeColor="#ff6700" />
                                    <dx:GridViewDataTextColumn FieldName="away_shot_text" Caption="Sút" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" CellStyle-ForeColor="#007934" />
                                </Columns>
                            </dx:GridViewBandColumn>
                            <dx:GridViewBandColumn Caption="Lịch sử" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="True">
                                <Columns>

                                    <dx:GridViewDataTextColumn FieldName="his_away_score_text" Caption="Banh" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" CellStyle-Wrap="False" />
                                    <dx:GridViewDataTextColumn FieldName="his_away_corner_text" Caption="Góc" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" CellStyle-Wrap="False" />

                                </Columns>
                            </dx:GridViewBandColumn>
                            <dx:GridViewDataTextColumn FieldName="away_team_name" Caption="Tên đội" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" />
                        </Columns>
                    </dx:GridViewBandColumn>

                    <dx:GridViewBandColumn Caption="TỶ LỆ" HeaderStyle-HorizontalAlign="Center">
                        <Columns>
                            <dx:GridViewBandColumn Caption="HDC" HeaderStyle-HorizontalAlign="Center">
                                <Columns>
                                    <dx:GridViewDataTextColumn FieldName="ns_hdc_before" Caption="Sớm" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" />
                                    <dx:GridViewDataTextColumn FieldName="ns_hdc_now" Caption="Trực tuyến" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" />
                                    <dx:GridViewDataTextColumn FieldName="ns_hdc_point" Caption="Chênh" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" />
                                    <dx:GridViewDataTextColumn FieldName="hdc_now" Caption="Now" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" Visible="false" />
                                </Columns>
                            </dx:GridViewBandColumn>
                            <dx:GridViewBandColumn Caption="TX" HeaderStyle-HorizontalAlign="Center">
                                <Columns>
                                    <dx:GridViewDataTextColumn FieldName="ns_tx_before" Caption="Sớm" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" />
                                    <dx:GridViewDataTextColumn FieldName="ns_tx_now" Caption="Trực tuyến" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" />
                                    <dx:GridViewDataTextColumn FieldName="ns_tx_gap" Caption="Chênh" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" />
                                    <dx:GridViewDataTextColumn FieldName="tx_now" Caption="Now" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" />
                                    <dx:GridViewDataTextColumn FieldName="tx_now_gap" Caption="Chênh" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" />
                                </Columns>
                            </dx:GridViewBandColumn>
                        </Columns>
                    </dx:GridViewBandColumn>
                    <dx:GridViewBandColumn Caption="ĐỀ XUẤT" HeaderStyle-HorizontalAlign="Center">
                        <Columns>
                            <dx:GridViewBandColumn Caption="TRƯỚC TRẬN" HeaderStyle-HorizontalAlign="Center">
                                <Columns>
                                    <dx:GridViewDataTextColumn FieldName="ns_corner_note" Caption="Góc" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" />
                                    <dx:GridViewDataTextColumn FieldName="ns_score_note" Caption="Banh" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" />
                                </Columns>
                            </dx:GridViewBandColumn>
                            <dx:GridViewBandColumn Caption="RUNNING" HeaderStyle-HorizontalAlign="Center">
                                <Columns>
                                    <dx:GridViewDataTextColumn FieldName="now_corner_note" Caption="Góc" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" />
                                    <dx:GridViewDataTextColumn FieldName="now_score_note" Caption="Banh" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" />
                                </Columns>
                            </dx:GridViewBandColumn>
                        </Columns>
                    </dx:GridViewBandColumn>
                </Columns>
            </dx:ASPxGridView>
        </div>
    </form>
</body>
</html>
