<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="EditRoute.aspx.cs" Inherits="Routes_EditRoute" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Gestione percorsi</title>

    <script type="text/javascript">
        function frameLoaded(frame) {
            var mapDiv = frame.contentDocument.getElementById("gmap_div");
            frame.height = mapDiv ? "400px" : "40px";
            getGpsField().value = mapDiv ? "SI" : "";
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPanel" runat="Server">
    <div id="ContentPanel" class="ContentPanel" style="text-align: left">
        <h1>
            Inserisci o modifica i dati del tuo percorso</h1>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:HiddenField ID="RouteName" runat="server" />
                <div>
                    Titolo percorso:
                    <asp:RequiredFieldValidator ID="TextBoxTitleRequiredFieldValidator" runat="server"
                        ErrorMessage="Campo obbligatorio!" ControlToValidate="TextBoxTitle" Display="Dynamic"
                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                </div>
                <asp:TextBox ID="TextBoxTitle" runat="server" Width="100%" CausesValidation="True"></asp:TextBox>
                <div>
                    Descrizione:
                    <asp:RequiredFieldValidator ID="TextBoxDescriptionRequiredFieldValidator" runat="server"
                        ErrorMessage="Campo obbligatorio!" ControlToValidate="TextBoxDescription" Display="Dynamic"
                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                </div>
                <asp:TextBox ID="TextBoxDescription" runat="server" Width="100%" TextMode="MultiLine"
                    CausesValidation="True" Rows="5"></asp:TextBox>
                <div>
                    Tracciato GPS:
                    <asp:RequiredFieldValidator ID="TextBoxGPSRequiredFieldValidator" runat="server"
                        ErrorMessage="Campo obbligatorio!" ControlToValidate="TextBoxGPS" Display="Dynamic"
                        SetFocusOnError="false"></asp:RequiredFieldValidator>
                </div>
                <asp:TextBox ID="TextBoxGPS" Enabled="false" style="display:none;" runat="server" Width="100%" TextMode="SingleLine"
                    CausesValidation="True"></asp:TextBox>
                <iframe id="MapFrame" runat="server" frameborder="0"
                    width="100%" scrolling="no" height="400px"></iframe>
                <div style="text-align: center">
                    <asp:Button ID="ButtonSave" runat="server" Text="Salva" OnClick="ButtonSave_Click" /></div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
