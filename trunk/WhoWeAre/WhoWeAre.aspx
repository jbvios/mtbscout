<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WhoWeAre.aspx.cs" MasterPageFile="~/MasterPage.master"
	Inherits="WhoWeAre" %>

<%@ Register Src="../ImageIterator.ascx" TagName="ImageIterator" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<title>Mountain Bike Group Scout Montoggio</title>
	<style type="text/css">
		div.ScoutImage
		{
			background-color: Red;
			height: 100px;
			width: 200px;
			margin: 20px;
			padding: 20px;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPanel" runat="Server">
	<div id="ContentPanel" class="ContentPanel">
		<h1>
			Gli Scout</h1>
		<h3>
			Chi siamo</h3>
		<p>
			Siamo un gruppo di amici uniti dalla passione per la Mountain Bike, sport a cui
			ci avviciniamo con spirito non agonistico, vivendolo piuttosto come mezzo di comunione
			con la natura e occasione per sperimentare momenti di serena libertà, lontano da
			strade trafficate e rumorose.</p>
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
			accidentati profili montuosi, di prati erbosi, di colori autunnali, di sole che filtra fra le fronde
			degli alberi, di vento che congela le dita e neve che luccica al sole; sono <i><b>le
				sensazioni</b></i>, a volte piacevoli a volte spiacevoli, che ci fanno apprezzare
			questo sport e ci fanno sentire vivi.</p>
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
			può <a href="mailto:info@mtbscout.it">contattarci via mail</a>.</p>
		<uc1:ImageIterator ID="ImageIterator1" runat="server" Title="Le nostre foto" />
	</div>
</asp:Content>
