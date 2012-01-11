<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    EnableEventValidation="false" ValidateRequest="false" CodeFile="Forum.aspx.cs"
    Inherits="Forum" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Appuntamenti per escursioni in MTB</title>
    <link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.16/themes/base/jquery-ui.css"
        type="text/css" />

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js"></script>

    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.16/jquery-ui.min.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPanel" runat="Server">
    <div id="ContentPanel" class="ContentPanel">
        <h1>
            Appuntamenti per escursioni in MTB</h1>
        <asp:Repeater ID="Appointments" runat="server" OnItemDataBound="Appointments_ItemDataBound">
            <HeaderTemplate>
            </HeaderTemplate>
            <ItemTemplate>
                <div style="border: solid 1px blue; text-align: left; margin: 10px; padding: 10px;">
                    <div>
                        Appuntamento creato da <b>
                            <%#DataBinder.Eval(Container.DataItem, "Name")%></b>
                        <%#Helper.FormatDateTime((DateTime)DataBinder.Eval(Container.DataItem, "PostingDate"))%>.
                        <asp:ImageButton OnClick="ButtonDeleteAppointment_Click" OnClientClick="return confirm('Sicuro di voler cancellare l\'appuntamento?');"
                            runat="server" ID="ButtonDelete" ImageUrl="~/Images/recycle.png" AlternateText="Elimina"
                            ToolTip="Elimina" /></div>
                    <div>
                        Quando: <b>
                            <%#Helper.FormatDate((DateTime)DataBinder.Eval(Container.DataItem, "AppointmentDate"))%></b></div>
                    <div style="font-size: larger; font-weight: bold; font-variant: small-caps">
                        <%#DataBinder.Eval(Container.DataItem, "Message")%></div>
                    <div>
                        <asp:Button runat="server" ID="ButtonToggle" Text="Visualizza commenti" UseSubmitBehavior="False" />
                        <%#((Iesi.Collections.ISet)DataBinder.Eval(Container.DataItem, "AppointmentPosts")).Count%>
                        commenti</div>
                    <asp:Panel runat="server" ID="CommentsPanel" Style="display: none">
                        <div>
                            Risposte e commenti:</div>
                        <asp:Repeater ID="Posts" runat="server">
                            <HeaderTemplate>
                                <table border="1" width="100%" class="AppointmentPosts">
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
                                    <td style="width: 150px">
                                        <%#DataBinder.Eval(Container.DataItem, "PostingDate")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container.DataItem, "Message")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container.DataItem, "Name")%>
                                    </td>
                                    <td style="width: 20px">
                                        <asp:ImageButton OnClick="ButtonDelete_Click" OnClientClick="return confirm('Sicuro di voler cancellare il commento?');"
                                            runat="server" ID="ButtonDelete" ImageUrl="~/Images/recycle.png" AlternateText="Elimina"
                                            ToolTip="Elimina" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                        <div style="padding: 20px; margin: 20px;">
                            <div>
                                Scrivi qui il tuo commento, metti il tuo nome e premi il tasto 'Invia messaggio'</div>
                            <asp:TextBox ID="Message" runat="server" Height="100px" TextMode="MultiLine" Width="100%"></asp:TextBox>
                            <div>
                                Scrivi qui il tuo nome</div>
                            <asp:TextBox ID="Name" CssClass="name" runat="server" Width="100%"></asp:TextBox>
                        </div>
                        <div style="text-align: center; margin: 20px;">
                            <asp:Button ID="ButtonSend" runat="server" Text="Invia messaggio" OnClick="ButtonSend_Click" />
                        </div>
                    </asp:Panel>
                </div>
            </ItemTemplate>
            <FooterTemplate>
            </FooterTemplate>
        </asp:Repeater>
        <button onclick="onCreateAppointment(this);return false;">
            Crea appuntamento...</button>
        <div id="addAppointment" style="display: none; text-align: left; margin: 10px;">
            <div style="padding: 20px; margin: 20px;">
                <div>
                    Scrivi qui una descrizione, metti il tuo nome e premi il tasto 'Crea appuntamento'</div>
                <asp:TextBox ID="Message" runat="server" Height="100px" TextMode="MultiLine" Width="100%"></asp:TextBox>
                <div>
                    Inserisci qui la data dell'appuntamento:</div>
                <div>
                    <asp:TextBox ID="Date" runat="server" TextMode="SingleLine"></asp:TextBox>
                </div>
                <div>
                    Scrivi qui il tuo nome:
                </div>
                <asp:TextBox ID="Name" CssClass="name" runat="server" Width="200px"></asp:TextBox>
                <div>
                </div>
                <div style="text-align: center; margin: 20px;">
                    <asp:Button ID="ButtonCreate" runat="server" Text="Crea appuntamento" OnClick="ButtonCreate_Click" />
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        mtb$ = jQuery.noConflict();
        mtb$(function() {
            mtb$('input.name').val(getCookie("mtbscoutuser"));

        });
        function onSendPost(id) {
            setCookie("mtbscoutuser", mtb$('#' + id).val(), 365);
        }
        function onCreateAppointment(sender) {
            mtb$(sender).hide();
            mtb$('#addAppointment').show();
            try {
                mtb$("#ctl00_ContentPanel_Date").datepicker({
                    dateFormat: 'dd-mm-yy',
                    dayNames: ['Domenica', 'Lunedì', 'Martedì', 'Mercoledì', 'Giovedì', 'Venerdì', 'Sabato'],
                    dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mer', 'Gio', 'Ven', 'Sab'],
                    dayNamesMin: ['Do', 'Lu', 'Ma', 'Me', 'Gi', 'Ve', 'Sa'],
                    monthNames: ['Gennaio', 'Febbraio', 'Marzo', 'Aprile', 'Maggio', 'Giugno', 'Luglio', 'Agosto', 'Settembre', 'Ottobre', 'Novembre', 'Dicembre'],
                    gotoCurrent: true,
                    nextText: 'Successivo',
                    prevText: 'Precedente',
                    firstDay: 1
                });
            }
            catch (e) {
                alert(e);
            }
        }
        function onToggle(sender, id) {
            var el = mtb$('#' + id).toggle();
            var txt = el.is(":visible") ? "Nascondi commenti" : "Visualizza commenti";
            mtb$(sender).val(txt);
        }
    </script>

    </div>
</asp:Content>
