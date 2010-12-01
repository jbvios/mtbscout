<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
	CodeFile="Register.aspx.cs" Inherits="Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<title>Registrazione nuovo utente</title>
	<style type="text/css">
		.Text
		{
			width: 350px;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPanel" runat="Server">
	<asp:ScriptManager ID="ScriptManager1" runat="server">
	</asp:ScriptManager>
	<div id="ContentPanel" class="ContentPanel">
		<asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Block" UpdateMode="Conditional">
			<ContentTemplate>
				<table style="width: 100%;">
					<tr>
						<td>
							Nome
						</td>
						<td>
							<input id="Name" type="text" class="Text" runat="server" />
						</td>
					</tr>
					<tr>
						<td>
							Cognome
						</td>
						<td>
							<input id="Surname" type="text" class="Text" runat="server" />
							</td>
					</tr>
					<tr>
						<td>
							Indirizzo e-mail
						</td>
						<td>
							<input id="Mail" type="text" class="Text" runat="server" />
						</td>
					</tr>
					<tr>
						<td>
							Indirizzo
						</td>
						<td>
							<input id="Address" type="text" class="Text" runat="server" />
						</td>
					</tr>
					<tr>
						<td>
							Codice postale
						</td>
						<td>
							<input id="ZIP" type="text" class="Text" runat="server" />
						</td>
					</tr>
					<tr>
						<td>
							Città
						</td>
						<td>
							<input id="City" type="text" class="Text" runat="server" />
						</td>
					</tr>
					<tr>
						<td>
							Provincia
						</td>
						<td>
							<input id="Province" type="text" class="Text" runat="server" />
						</td>
					</tr>
					<tr>
						<td>
							Data di nascita
						</td>
						<td>
							<asp:TextBox id="Birthday" type="text" class="Text" runat="server" 
								AutoPostBack="True" CausesValidation="True" ></asp:TextBox>
							<asp:CustomValidator ID="CustomValidator1" runat="server" 
								ControlToValidate="Birthday" ErrorMessage="!" 
								onservervalidate="CustomValidator1_ServerValidate" SetFocusOnError="True"></asp:CustomValidator>
							</td>
					</tr>
					<tr>
						<td>
							Parola chiave
						</td>
						<td>
							<asp:TextBox  id="pwd1" class="Text" runat="server" AutoPostBack="True" CausesValidation="True" TextMode="Password"></asp:TextBox>
							<asp:CustomValidator ID="CustomValidator3" runat="server" 
								ControlToValidate="pwd1" ErrorMessage="!" 
								 SetFocusOnError="True" onservervalidate="CustomValidator3_ServerValidate" 	></asp:CustomValidator>
						</td>
					</tr>
					<tr>
						<td>
							Conferma parola chiave
						</td>
						<td>
							<asp:TextBox  id="pwd2" class="Text" runat="server" AutoPostBack="True" CausesValidation="True" TextMode="Password"></asp:TextBox>
							<asp:CustomValidator ID="CustomValidator2" runat="server" 
								ControlToValidate="pwd2" ErrorMessage="!" 
								 SetFocusOnError="True" onservervalidate="CustomValidator2_ServerValidate" 	></asp:CustomValidator>
							</td>
						</td>
					</tr>
				</table>
			</ContentTemplate>
		</asp:UpdatePanel>
	</div>
</asp:Content>
