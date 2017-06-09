<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="PushServe.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="text-align: center; margin: 20px auto; width: 1000px; height: auto; border: solid darkgray 1px">
            <div>
                <h3 style="width: 1000px; height: 40px; background-color: darkgray; margin: 0 auto; padding-top: 20px;">PushServe DashBorad</h3>
            </div>
            <div style="margin-top: 10px;margin-left:10px;margin-bottom:30px;">
                <asp:GridView ID="clientList" runat="server" CellPadding="4" ForeColor="Black" GridLines="Vertical" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px">
                    <AlternatingRowStyle BackColor="White" />
                    <FooterStyle BackColor="#CCCC99" />
                    <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                    <RowStyle BackColor="#F7F7DE" />
                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#FBFBF2" />
                    <SortedAscendingHeaderStyle BackColor="#848384" />
                    <SortedDescendingCellStyle BackColor="#EAEAD3" />
                    <SortedDescendingHeaderStyle BackColor="#575357" />
                </asp:GridView>
                <p style="text-align:left">当前用户数: <span id="usercount" runat="server"></span></p>
                <p style="text-align:left">当前链接数: <span id="linkcount" runat="server"></span></p>
                <p id="time" style="text-align:right;font-size:12px;margin-right:10px;margin-bottom:-20px;">最后更新时间: <span id="lastertime" runat="server"></span></p>
            </div>
        </div>
    </form>
    <script>
        window.setInterval(function () {
            window.location.reload();
        }, 1000 * 30);
    </script>
</body>
</html>

