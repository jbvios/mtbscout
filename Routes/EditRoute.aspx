<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="EditRoute.aspx.cs" Inherits="Routes_EditRoute" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Gestione percorsi</title>
    <style>
        .comboLevel
        {
            text-align: justify;
            padding: 20px;
        }
    </style>

    <script type="text/javascript">
        function frameLoaded(frame) {
            var mapDiv = frame.contentDocument.getElementById("gmap_div");
            frame.height = mapDiv ? "400px" : "40px";
            getGpsField().value = mapDiv ? "x" : "";
            document.getElementById("TextBoxGPSMessage").innerHTML = mapDiv
                ? "CARICATO - Premi il pulsante sotto per sostituirlo"
                : "NON CARICATO - Premi il pulsante sotto per caricare un file GPX";
        }
        function imagesUploaded(frame) {
            getUpdateImagesButton().click();
        }
		
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPanel" runat="Server">
    <div id="ContentPanel" class="ContentPanel" style="text-align: left">
        <h1>
            Inserisci o modifica i dati del tuo percorso</h1>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:HiddenField ID="RouteName" runat="server" />
                        <div>
                            <span>Tracciato GPS:</span> <span id="TextBoxGPSMessage"></span>
                            <asp:RequiredFieldValidator ID="TextBoxGPSRequiredFieldValidator" runat="server"
                                ErrorMessage="Campo obbligatorio!" ControlToValidate="TextBoxGPS" Display="Dynamic"
                                SetFocusOnError="false"></asp:RequiredFieldValidator>
                        </div>
                        <asp:TextBox ID="TextBoxGPS" Enabled="false" Style="display: none;" runat="server"
                            Width="100%" TextMode="SingleLine" CausesValidation="True"></asp:TextBox>
                        <iframe id="MapFrame" runat="server" frameborder="0" width="100%" scrolling="no"
                            height="400px"></iframe>
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
                            Percentuale di ciclabilità:
                            <asp:RangeValidator ID="TextBoxCiclyngRangeValidator" runat="server" ErrorMessage="Inserire un valore fra 0 e 100!"
                                Type="Integer" ControlToValidate="TextBoxCiclyng" Display="Dynamic" SetFocusOnError="True"
                                MaximumValue="100" MinimumValue="0"></asp:RangeValidator>
                            <asp:RequiredFieldValidator ID="TextBoxCiclyngRequiredFieldValidator" runat="server"
                                ErrorMessage="Campo obbligatorio!" ControlToValidate="TextBoxCiclyng" Display="Dynamic"
                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                        </div>
                        <asp:TextBox ID="TextBoxCiclyng" runat="server" Width="100%" TextMode="SingleLine"
                            CausesValidation="True"></asp:TextBox>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                    <ContentTemplate>
                        <fieldset title="Difficoltà tecnica">
                            <legend>Difficoltà tecnica</legend>
                            <div class="comboLevel">
                                Salita:
                                <asp:DropDownList ID="DropDownListClimb" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListClimb_SelectedIndexChanged">
                                    <asp:ListItem>Seleziona una difficoltà</asp:ListItem>
                                    <asp:ListItem>TC</asp:ListItem>
                                    <asp:ListItem>MC</asp:ListItem>
                                    <asp:ListItem>BC</asp:ListItem>
                                    <asp:ListItem>OC</asp:ListItem>
                                    <asp:ListItem>EC</asp:ListItem>
                                </asp:DropDownList>
                                <asp:CheckBox ID="CheckBoxClimb" runat="server" Text="Significativi tratti con forti pendenze"
                                    AutoPostBack="true" OnCheckedChanged="CheckBoxClimb_CheckedChanged" />
                                <asp:Label Style="display: block;" ID="LabelClimb" runat="server"></asp:Label>
                            </div>
                            <div class="comboLevel">
                                Discesa:
                                <asp:DropDownList ID="DropDownListDown" runat="server" OnSelectedIndexChanged="DropDownListDown_SelectedIndexChanged"
                                    AutoPostBack="True">
                                    <asp:ListItem>Seleziona una difficoltà</asp:ListItem>
                                    <asp:ListItem>TC</asp:ListItem>
                                    <asp:ListItem>MC</asp:ListItem>
                                    <asp:ListItem>BC</asp:ListItem>
                                    <asp:ListItem>OC</asp:ListItem>
                                    <asp:ListItem>EC</asp:ListItem>
                                </asp:DropDownList>
                                <asp:CheckBox ID="CheckBoxDown" runat="server" Text="Significativi tratti con forti pendenze"
                                    AutoPostBack="true" OnCheckedChanged="CheckBoxDown_CheckedChanged" />
                                <asp:Label Style="display: block;" ID="LabelDown" runat="server"></asp:Label>
                            </div>
                            <div>
                                Difficoltà complessiva:
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorTextBoxDifficulty" runat="server"
                                    ErrorMessage="Campo obbligatorio!" ControlToValidate="TextBoxDifficulty" Display="Dynamic"
                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                <asp:TextBox ID="TextBoxDifficulty" runat="server" TextMode="SingleLine" CausesValidation="True"
                                    Enabled="false"></asp:TextBox>
                                <a target="MTBCAI" href="http://www.mtbcai.it/scaladifficolta.asp" title="Per saperne di più...">
                                    Per saperne di più...</a>
                            </div>
                        </fieldset>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="UpdatePanelImages" runat="server" ChildrenAsTriggers="true"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button runat="server" CausesValidation="false" ID="ReloadImages" Style="display: none" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <iframe id="UploadImageFrame" runat="server" frameborder="0" width="100%" scrolling="no"
                    height="40px"></iframe>
                <div style="text-align: center">
                    <asp:Button ID="ButtonSave" runat="server" Text="Salva" OnClick="ButtonSave_Click" /></div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
