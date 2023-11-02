<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="eFBWeb.Main" %>

<%@ Register Assembly="DevExpress.Web.v21.1, Version=21.1.11.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .rectangle {
            width: 20px;
            height: 20px;
            text-align: center;
            padding-top: 8px;
            vertical-align: central;
            color: white;
            font-weight: bolder;
        }

        .score {
            background-color: white;
            color: #76cfc3;
            text-align: center;
            padding: 4px;
        }

        .corner {
            background-color: white;
            color: #7e87d0;
            text-align: center;
            padding: 4px;
        }

        .yellow {
            background-color: white;
            color: #deba21;
            text-align: center;
            padding: 4px;
        }

        .red {
            background-color: white;
            color: #a80f0f;
            text-align: center;
            padding: 4px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <dx:ASPxListBox ID="lstMatch3" runat="server" OnDataBinding="lstMatch_DataBinding" Width="100%" SelectionMode="Multiple" ForceDataBinding="true" Height="700">
                <ItemTemplate>
                    <table style="width: 100%; padding: 8px; margin: 8px" border="1">
                        <tr>
                            <td style="width: 20%; text-align: center">
                                <table>
                                    <tr>
                                        <td style="text-align: left">
                                            <a href="https://trauscore.com/match/<%#Eval("id")%>/" target="_blank"><%#Eval("id")%></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left">
                                            <asp:Label runat="server" Text='<%#Eval("tournament_name")%>' Font-Bold="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left">
                                            <asp:Label runat="server" Text='<%#Eval("match_time", "{0:dd/MM/yyyy HH:mm}")%>' />
                                        </td>
                                    </tr>
                                </table>
                            </td>

                            <td style="width: 25%; text-align: right; padding-right: 10px">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="padding-right: 10px;">
                                            <asp:Label runat="server" Text='<%#Eval("home_team_name") %>' Font-Bold="true" Font-Size="X-Large" />
                                        </td>

                                        <td>
                                            <asp:Label runat="server" Text='<%#Eval("home_score") %>' Font-Bold="true" Font-Size="X-Large" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; padding-top: 8px" colspan="2">
                                            <table border="1" style="width:100%">
                                                <tr>
                                                    <th style="padding: 4px"></th>
                                                    <th style="padding: 4px">0-15</th>
                                                    <th style="padding: 4px">15-30</th>
                                                    <th style="padding: 4px">30-45</th>
                                                    <th style="padding: 4px; background-color: aqua">H1</th>
                                                    <th style="padding: 4px">45-60</th>
                                                    <th style="padding: 4px">60-75</th>
                                                    <th style="padding: 4px">75-90</th>
                                                    <th style="padding: 4px; background-color: aquamarine">H2</th>
                                                    <th style="padding: 4px; background-color: antiquewhite">FT</th>
                                                </tr>
                                                <tr>
                                                    <td style="padding: 4px">Bàn thắng</td>
                                                    <td class="score">
                                                        <%#Eval("p_00_15_home_score") %>
                                                    </td>
                                                    <td class="score">
                                                        <%#Eval("p_15_30_home_score") %>
                                                    </td>
                                                    <td class="score">
                                                        <%#Eval("p_30_45_home_score") %>
                                                    </td>
                                                    <td class="score" style="font-weight: bold;">
                                                        <%#Eval("home_score_h1") %>
                                                    </td>
                                                    <td class="score">
                                                        <%#Eval("p_45_60_home_score") %>
                                                    </td>
                                                    <td class="score">
                                                        <%#Eval("p_60_75_home_score") %>
                                                    </td>
                                                    <td class="score">
                                                        <%#Eval("p_75_90_home_score") %>
                                                    </td>
                                                    <td class="score" style="font-weight: bold">
                                                        <%#Eval("home_score_h2") %>
                                                    </td>
                                                    <td class="score" style="font-weight: bolder">
                                                        <%#Eval("home_score") %>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="padding: 4px">Phạt góc</td>
                                                    <td class="corner">
                                                        <%#Eval("p_00_15_home_corner") %>
                                                    </td>
                                                    <td class="corner">
                                                        <%#Eval("p_15_30_home_corner") %>
                                                    </td>
                                                    <td class="corner">
                                                        <%#Eval("p_30_45_home_corner") %>
                                                    </td>
                                                    <td class="corner" style="font-weight: bold">
                                                        <%#Eval("home_corner_h1") %>
                                                    </td>
                                                    <td class="corner">
                                                        <%#Eval("p_45_60_home_corner") %>
                                                    </td>
                                                    <td class="corner">
                                                        <%#Eval("p_60_75_home_corner") %>
                                                    </td>
                                                    <td class="corner">
                                                        <%#Eval("p_75_90_home_corner") %>
                                                    </td>
                                                    <td class="corner" style="font-weight: bold">
                                                        <%#Eval("home_corner_h2") %>
                                                    </td>
                                                    <td class="corner" style="font-weight: bolder">
                                                        <%#Eval("home_corner") %>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="padding: 4px">Thẻ vàng</td>
                                                    <td class="yellow">
                                                        <%#Eval("p_00_15_home_yellow") %>
                                                    </td>
                                                    <td class="yellow">
                                                        <%#Eval("p_15_30_home_yellow") %>
                                                    </td>
                                                    <td class="yellow">
                                                        <%#Eval("p_30_45_home_yellow") %>
                                                    </td>
                                                    <td class="yellow" style="font-weight: bold">
                                                        <%#Eval("home_yellow_h1") %>
                                                    </td>
                                                    <td class="yellow">
                                                        <%#Eval("p_45_60_home_yellow") %>
                                                    </td>
                                                    <td class="yellow">
                                                        <%#Eval("p_60_75_home_yellow") %>
                                                    </td>
                                                    <td class="yellow">
                                                        <%#Eval("p_75_90_home_yellow") %>
                                                    </td>
                                                    <td class="yellow" style="font-weight: bold">
                                                        <%#Eval("home_yellow_h2") %>
                                                    </td>
                                                    <td class="yellow" style="font-weight: bolder">
                                                        <%#Eval("home_yellow") %>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="padding: 4px">Thẻ đỏ</td>
                                                    <td class="red">
                                                        <%#Eval("p_00_15_home_red") %>
                                                    </td>
                                                    <td class="red">
                                                        <%#Eval("p_15_30_home_red") %>
                                                    </td>
                                                    <td class="red">
                                                        <%#Eval("p_30_45_home_red") %>
                                                    </td>
                                                    <td class="red" style="font-weight: bold">
                                                        <%#Eval("home_red_h1") %>
                                                    </td>
                                                    <td class="red">
                                                        <%#Eval("p_45_60_home_red") %>
                                                    </td>
                                                    <td class="red">
                                                        <%#Eval("p_60_75_home_red") %>
                                                    </td>
                                                    <td class="red">
                                                        <%#Eval("p_75_90_home_red") %>
                                                    </td>
                                                    <td class="red" style="font-weight: bold">
                                                        <%#Eval("home_red_h2") %>
                                                    </td>
                                                    <td class="red" style="font-weight: bolder">
                                                        <%#Eval("home_red") %>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>

                            <td style="width: 25%; text-align: right; padding-right: 10px">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="padding-right: 10px;">
                                            <asp:Label runat="server" Text='<%#Eval("away_team_name") %>' Font-Bold="true" Font-Size="X-Large" />
                                        </td>

                                        <td>
                                            <asp:Label runat="server" Text='<%#Eval("away_score") %>' Font-Bold="true" Font-Size="X-Large" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center; padding-top: 8px" colspan="2">
                                            <table border="1" style="width:100%">
                                                <tr>
                                                    <th style="padding: 4px"></th>
                                                    <th style="padding: 4px">0-15</th>
                                                    <th style="padding: 4px">15-30</th>
                                                    <th style="padding: 4px">30-45</th>
                                                    <th style="padding: 4px; background-color: aqua">H1</th>
                                                    <th style="padding: 4px">45-60</th>
                                                    <th style="padding: 4px">60-75</th>
                                                    <th style="padding: 4px">75-90</th>
                                                    <th style="padding: 4px; background-color: aquamarine">H2</th>
                                                    <th style="padding: 4px; background-color: antiquewhite">FT</th>
                                                </tr>
                                                <tr>
                                                    <td style="padding: 4px">Bàn thắng</td>
                                                    <td class="score">
                                                        <%#Eval("p_00_15_away_score") %>
                                                    </td>
                                                    <td class="score">
                                                        <%#Eval("p_15_30_away_score") %>
                                                    </td>
                                                    <td class="score">
                                                        <%#Eval("p_30_45_away_score") %>
                                                    </td>
                                                    <td class="score" style="font-weight: bold;">
                                                        <%#Eval("away_score_h1") %>
                                                    </td>
                                                    <td class="score">
                                                        <%#Eval("p_45_60_away_score") %>
                                                    </td>
                                                    <td class="score">
                                                        <%#Eval("p_60_75_away_score") %>
                                                    </td>
                                                    <td class="score">
                                                        <%#Eval("p_75_90_away_score") %>
                                                    </td>
                                                    <td class="score" style="font-weight: bold">
                                                        <%#Eval("away_score_h2") %>
                                                    </td>
                                                    <td class="score" style="font-weight: bolder">
                                                        <%#Eval("away_score") %>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="padding: 4px">Phạt góc</td>
                                                    <td class="corner">
                                                        <%#Eval("p_00_15_away_corner") %>
                                                    </td>
                                                    <td class="corner">
                                                        <%#Eval("p_15_30_away_corner") %>
                                                    </td>
                                                    <td class="corner">
                                                        <%#Eval("p_30_45_away_corner") %>
                                                    </td>
                                                    <td class="corner" style="font-weight: bold">
                                                        <%#Eval("away_corner_h1") %>
                                                    </td>
                                                    <td class="corner">
                                                        <%#Eval("p_45_60_away_corner") %>
                                                    </td>
                                                    <td class="corner">
                                                        <%#Eval("p_60_75_away_corner") %>
                                                    </td>
                                                    <td class="corner">
                                                        <%#Eval("p_75_90_away_corner") %>
                                                    </td>
                                                    <td class="corner" style="font-weight: bold">
                                                        <%#Eval("away_corner_h2") %>
                                                    </td>
                                                    <td class="corner" style="font-weight: bolder">
                                                        <%#Eval("away_corner") %>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="padding: 4px">Thẻ vàng</td>
                                                    <td class="yellow">
                                                        <%#Eval("p_00_15_away_yellow") %>
                                                    </td>
                                                    <td class="yellow">
                                                        <%#Eval("p_15_30_away_yellow") %>
                                                    </td>
                                                    <td class="yellow">
                                                        <%#Eval("p_30_45_away_yellow") %>
                                                    </td>
                                                    <td class="yellow" style="font-weight: bold">
                                                        <%#Eval("away_yellow_h1") %>
                                                    </td>
                                                    <td class="yellow">
                                                        <%#Eval("p_45_60_away_yellow") %>
                                                    </td>
                                                    <td class="yellow">
                                                        <%#Eval("p_60_75_away_yellow") %>
                                                    </td>
                                                    <td class="yellow">
                                                        <%#Eval("p_75_90_away_yellow") %>
                                                    </td>
                                                    <td class="yellow" style="font-weight: bold">
                                                        <%#Eval("away_yellow_h2") %>
                                                    </td>
                                                    <td class="yellow" style="font-weight: bolder">
                                                        <%#Eval("away_yellow") %>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="padding: 4px">Thẻ đỏ</td>
                                                    <td class="red">
                                                        <%#Eval("p_00_15_away_red") %>
                                                    </td>
                                                    <td class="red">
                                                        <%#Eval("p_15_30_away_red") %>
                                                    </td>
                                                    <td class="red">
                                                        <%#Eval("p_30_45_away_red") %>
                                                    </td>
                                                    <td class="red" style="font-weight: bold">
                                                        <%#Eval("away_red_h1") %>
                                                    </td>
                                                    <td class="red">
                                                        <%#Eval("p_45_60_away_red") %>
                                                    </td>
                                                    <td class="red">
                                                        <%#Eval("p_60_75_away_red") %>
                                                    </td>
                                                    <td class="red">
                                                        <%#Eval("p_75_90_away_red") %>
                                                    </td>
                                                    <td class="red" style="font-weight: bold">
                                                        <%#Eval("away_red_h2") %>
                                                    </td>
                                                    <td class="red" style="font-weight: bolder">
                                                        <%#Eval("away_red") %>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </dx:ASPxListBox>
        </div>
    </form>
</body>
</html>
