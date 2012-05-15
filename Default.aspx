<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default"
	MasterPageFile="~/MasterPage.master" %>

<%@ Register Src="HorizontalSpot.ascx" TagName="Spot" TagPrefix="uc1" %>
<%@ Register Src="~/Video.ascx" TagName="Video" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<title>Mountain Bike Group Scout</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPanel" runat="Server">
	<div id="ContentPanel" class="ContentPanel">
		<uc1:Spot ID="Spot2" runat="server" />
		<h3>
			Gli Scout</h3>
		<p>
			Siamo un gruppo di amici uniti dalla passione per la Mountain Bike, sport a cui
			ci avviciniamo con spirito non agonistico, vivendolo piuttosto come mezzo di comunione
			con la natura e occasione per sperimentare momenti di serena libertà, lontano da
			strade trafficate e rumorose.</p>
			<p>Perché Scout? Con questo termine ci vogliamo accomunare agli scout indiani, pellerossa che conoscevano molto bene il territorio di insediamento ed erano impiegati come guide dall'esercito 
                americano; ci piace pensare a loro come incarnazione di una libertà minacciata 
                che va difesa a tutti i costi.</p>
		<p>
			L&#39;organizzazione, senza fini di lucro, si prefigge di diffondere la pratica
			di questo sport e la conoscenza del territorio, promuovendo iniziative che possano
			incentivare un turismo equilibrato e sostenibile, rispettoso dell&#39;ambiente e
			della natura.</p>
		<p>
			La nostra attività si concretizza nell&#39;organizzazione di gare ed eventi, nell&#39;apertura
			di nuovi percorsi e manutenzione di sentieri esistenti, nonché nella loro descrizione
			e pubblicizzazione, principalmente attraverso questo sito.
		</p>
		<div style="padding-top: 20px;">
		 <div class="widget">
		       <div style="padding: 20px;">
		       <a href = "school/School.aspx" title="Scuola di Mountain Bike">
				   <img style = "width:200px; height:300px" src="Images/School.JPG" alt="Scuola di Mountain Bike"/>
				   </a>
				</div>
			</div>
			<div class="widget">
				<div style="padding: 20px;">
					<span>Guarda i nostri percorsi registrati, scarica la traccia GPS e buon divertimento!</span>
					<asp:UpdatePanel ID="routePreview" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
						<ContentTemplate>
							<div id="ImageLayer">
								<asp:HyperLink runat="server" ID="A1">
									<asp:Image ID="RandomImage1" runat="server" Style="left: 0px;" /></asp:HyperLink>
								<asp:HyperLink runat="server" ID="A2">
									<asp:Image ID="RandomImage2" runat="server" Style="left: 200px;" /></asp:HyperLink>
								<asp:HyperLink runat="server" ID="A3">
									<asp:Image ID="RandomImage3" runat="server" Style="left: 400px;" /></asp:HyperLink>
								<input type="submit" id="reloadImages" />
							</div>
						</ContentTemplate>
					</asp:UpdatePanel>
					<br />
					<span id="RouteTitle" style="padding-top: 30px;"></span>
				</div>
			</div>
		    <div class="widget">
		       <div style="padding: 20px;">
				    <iframe width="420" height="300" src="http://www.youtube.com/embed/kXIRZKHNm7Y" frameborder="0" allowfullscreen></iframe>
				</div>
			</div>
			
			 <div class="widget">
				<div style="padding: 20px;">
				    <iframe width="420" height="300" src="http://www.youtube.com/embed/nS9_EyzyeMs" frameborder="0" allowfullscreen></iframe>
				</div>
			</div>
			<div class="widget">
				<div style="padding: 20px;">
					<a title="Anello dei Fieschi, video discesa Casale - Pontenero" href="/public/Routes/AnelloFieschi/AnelloFieschi.aspx">
						<span>Anello dei Fieschi, video discesa Casale - Pontenero</span>
						<uc2:Video ID="Video1" VideoHeight="280px" runat="server" VideoUrl="~/public/routes/AnelloFieschi/casale_pontenero.flv"
							PreviewUrl="~/public/routes/AnelloFieschi/casale_pontenero.jpg" />
					</a>
				</div>
			</div>
			
			<uc1:Spot ID="Spot1" runat="server" />
			<h3>
				Pensare <i>All Mountain</i></h3>
			<p>
				Appuntamento ogni domenica mattina per partire, zaino in spalla, verso sentieri
				di altura che ci costringeranno spesso a pedalare duro, a volte a spingere, a volte
				a districarci nella boscaglia in cui si è improvvisamente immerso un sentiero che
				poco prima sembrava un&#39;autostrada.</p>
			<p>
				Alcuni si impegnano per arrivare primi, altri non si lasciano coinvolgere e preferiscono
				godersi il panorama, altri ancora sostengono che la vera difficoltà sta nell&#39;affrontare
				quella discesa tecnica oppure &quot;fare a zero&quot; quel passaggio (mutuando la
				terminologia dei trialisti), e magari ciascuno di noi è stato protagonista, almeno
				una volta, in ognuno di questi ruoli; ma alla fine, la vera soddisfazione ci viene
				da quella strana mescolanza di fatica, di sudore, di tracciati che serpeggiano fra
				accidentati profili montuosi, di prati erbosi, di colori autunnali, di sole che
				filtra fra le fronde degli alberi, di vento che congela le dita e neve che luccica
				al sole; sono <i><b>le sensazioni</b></i>, a volte piacevoli a volte spiacevoli,
				che ci fanno apprezzare questo sport e ci fanno sentire vivi.</p>
			<p>
				I percorsi che facciamo non sono certamente quelli tipici del paesaggio Trentino,
				costellato di strade bianche che permettono di raggiungere ogni angolo del territorio;
				anzi, spesso esasperano quello che è il carattere ligure, spigoloso sia nella persona
				sia nel profilo orografico; discese lungo sentieri stretti e rocciosi, salite spesso
				poco pedalabili, tracciati non sempre evidenti o liberi da vegetazione.</p>
			<p>
				Nella sezione <a href="/Routes/Routes.aspx">Percorsi</a> ne elenchiamo alcuni, con
				l&#39;avvertimento che si tratta di descrizioni sommarie e che talvolta potrebbero
				essere non aggiornate vista la mutevolezza delle condizioni del territorio.</p>
			<p>
				Ci riuniamo ogni domenica a Montoggio alle 9.00 (pioggia permettendo), chi volesse
				aggregarsi a noi per escursioni o volesse chiarimenti in merito ad alcuni tracciati
				può utilizzare la nostra <a href="Appointments.aspx">pagina degli appuntamenti</a> oppure <a href="mailto:info@mtbscout.it">contattarci via mail</a>.</p>
		</div>
		<div id="dummyForHomePage" />
	</div>
    </div>
</asp:Content>
