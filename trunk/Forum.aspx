<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    EnableEventValidation="false" CodeFile="Forum.aspx.cs" Inherits="Forum" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Appuntamenti per escursioni in MTB</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPanel" runat="Server">
    <div id="ContentPanel" class="ContentPanel">
        <h1>
            Appuntamenti per escursioni in MTB</h1>
        <asp:Repeater ID="Appointments" runat="server" OnItemDataBound="Appointments_ItemDataBound">
            <HeaderTemplate>
            </HeaderTemplate>
            <ItemTemplate>
                <div style="border: solid 1px blue;text-align: left; margin:10px;">
                    <div>
                        Appuntamento creato da
                        <b><%#DataBinder.Eval(Container.DataItem, "Name")%></b>
                         
                        <%#Helper.FormatDate((DateTime)DataBinder.Eval(Container.DataItem, "PostingDate"))%>.</div>
                    <div>
                        Quando:
                        <b><%#Helper.FormatDate((DateTime)DataBinder.Eval(Container.DataItem, "AppointmentDate"))%></b></div>
                    <div>
                        Descrizione:
                        <%#DataBinder.Eval(Container.DataItem, "Message")%></div>
                        <div>Risposte:</div>
                    <asp:Repeater ID="Posts" runat="server">
                        <HeaderTemplate>
                            <table border="1" width="100%">
                                <tr>
                                    <th>
                                        Data
                                    </th>
                                    <th>
                                        Messaggio
                                    </th>
                                    <th>
                                        Mittente
                                    </th>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%#DataBinder.Eval(Container.DataItem, "PostingDate")%>
                                </td>
                                <td>
                                    <%#DataBinder.Eval(Container.DataItem, "Message")%>
                                </td>
                                <td>
                                    <%#DataBinder.Eval(Container.DataItem, "Name")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                    <div style=" padding: 20px; margin: 20px;">
                        <div>
                            Scrivi qui il tuo messaggio</div>
                        <asp:TextBox ID="Message" runat="server" Height="200px" TextMode="MultiLine" Width="100%"></asp:TextBox>
                        <div>
                            Firma</div>
                        <asp:TextBox ID="Name" runat="server" Width="100%"></asp:TextBox>
                    </div>
                    <asp:Button ID="ButtonSend" runat="server" Text="Invia" OnClick="ButtonSend_Click"
                        OnClientClick="onSendPost();" />
                </div>
            </ItemTemplate>
            <FooterTemplate>
            </FooterTemplate>
        </asp:Repeater>
    </div>

    <script type="text/javascript">
        window.onload = function() {
            document.getElementById('ctl00_ContentPanel_Name').value = getCookie("mtbscoutuser");
        }
        function onSendPost() {
            setCookie("mtbscoutuser", document.getElementById('ctl00_ContentPanel_Name').value, 365);
        }
    </script>

</asp:Content>
