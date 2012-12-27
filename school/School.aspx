<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="School.aspx.cs" Inherits="school_School" %>

<%@ Register Src="../ImageIterator.ascx" TagName="ImageIterator" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Scuola di Mountain Bike</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPanel" runat="Server">

    <div id="ContentPanel" class="ContentPanel">
        <a target="fci" href="http://www.federciclismo.it/" title="Federazione Ciclistica Italiana">
            <img alt="Federazione Ciclistica Italiana" style="float: left; padding: 20px; padding-left: 50px;"
                src="Logo FCI.jpg" /></a> <a target="asso" href="http://www.assomaestri.org/" title="Assomaestri">
                    <img style="float: right; padding: 20px; padding-right: 50px;" alt="Assomaestri"
                        src="assomaestri.png" />
                </a>
        <br />
        <h1>
            Scuola di Mountain Bike Val Pentemina</h1>
        <h4 style="text-align: center">
            Partita IVA 02118870993</h4>
        <h3 style="background-color:Yellow; color: Red; text-align: center; font-weight: bold; font-style: oblique;">
            La scuola è chiusa durante il periodo invernale</h3>
        <br />
        <p>
            La scuola è rivolta principalmente ai bambini e ragazzi dai 7 ai 12 anni ed ha l&#39;obiettivo
            di avvicinarli a questo sport con spirito giocoso e <b><i>non agonistico</i></b>,
            insegnando loro ad apprezzare la natura e il rispetto per l&#39;ambiente. Su appuntamento
            possono partecipare anche i principianti che intendano apprendere le tecniche di
            base del mountain biking, oppure gli escursionisti che desiderino essere accompagnati
            nei percorsi della valle, magari affrontando preliminarmente gli esercizi del campo
            scuola al fine di stabilire quali di questi percorsi possano essere affrontati senza
            difficoltà.</p>
        <iframe id="FBLike" runat="server" frameborder="0" name="I1" scrolling="no" style="border: none;
            width: 330px; height: 50px"></iframe>
        <a title="Seguici su Facebook" href="http://www.facebook.com/ScuolaMtbValPentemina"
            target="_blank">
            <img alt="Seguici su Facebook" src="seguici-facebook.jpg" style="width: 200px; height: 50px" /></a>
        <br />
        <uc1:ImageIterator ID="ImageIterator1" runat="server" HideAds="true" />
        <h3>
            Lezioni per i bambini</h3>
        <p>
            Le lezioni si svolgono ogni <i><b>sabato pomeriggio</b></i> e hanno durata di un&#39;ora,
            nella fascia oraria 17.00 - 19.00; i bambini vengono suddivisi su due turni in base
            al numero di iscritti, alla loro età e alle abilità sviluppate.</p>
        <p>
            Costi:</p>
        <ul>
            <li>lezione singola: 10 euro; </li>
            <li>abbonamento a 10 lezioni (con scadenza 15 settembre): 80 euro; chi ha effettuato
                l&#39;abbonamento potrà accedere ad eventuali ulteriori lezioni al prezzo unitario
                di 8 euro; </li>
            <li>prima lezione di prova: gratuita.</li>
        </ul>
        <p>
            A questo va aggiunto il costo del tesseramento annuo alla Federazione Ciclistica
            Italiana (comprensivo di assicurazione) in base al <a target="fci" href="http://www.federciclismo.it/affiliazione/tesseramento.asp?cod=5">
                tariffario in vigore</a>; il tesseramento verrà effettuato presso la <a href="http://www.genoabike.com/"
                    target="gbike">A.S.D. Genoa Bike</a>, affiliata alla F.C.I. - <a href="http://www.federciclismo.it/affiliazione/societa2012/dettagliosoc.asp?mcodice=06C1178"
                        target="_blank">visualizza lista degli attuali tesserati.</a></p>
        <p>
            Ogni bambino deve essere munito di una fototessera, certificato medico (per attività
            sportiva <b>non agonistica</b>), bici propria e <b><i>casco protettivo</i></b>;
            la fototessera ci serve in formato digitale, se ne hai una tradizionale possiamo
            effettuare noi una scansione della stessa (ad esempio quella della carta d&#39;identità
            o di un altro documento recente, così eviti di farla appositamente); non occorre
            che tu vada a comprare la bici nuova a tuo figlio, fallo venire con quella che ha
            (purché sia in efficiente stato di funzionamento, in particolare i freni), deciderai
            dopo se è il caso di cambiarla in funzione del suo interessamento e della reale
            necessità; per iscrizioni <a href="mailto:info@mtbscout.it">scrivici una mail</a>
            o contatta il 338.3681001.</p>
        <h2>
            Contenuti del corso</h2>
        <p>
            Le lezioni sono prevalentemente a carattere pratico: i bambini e i ragazzi potranno
            divertirsi sviluppando al contempo diverse capacità motorie, condizionali e coordinative
            come l&#39;equilibrio, la destrezza, l’abilità, la capacità reattiva, l’organizzazione
            spazio-temporale, la forza, la resistenza e la velocità, anche grazie all&#39;ausilio
            dei più svariati ostacoli (bascula, gimkana, piccoli salti, passaggi obbligati,
            sottopassi, sentieri in salita, ripidoni in discesa). Non mancheranno peraltro componenti
            teoriche, più o meno &#39;mascherate&#39; all&#39;interno dell&#39;attività ludica,
            volte da un lato a stimolare l&#39;apprendimento di nozioni specifiche quali la
            conoscenza del mezzo e dei comportamenti da osservare durante le escursioni per
            la propria ed altrui sicurezza, dall&#39;altro a principi educativi più generali
            quali il rispetto della natura, l&#39;impegno come mezzo principe per l&#39;ottenimento
            dei risultati, la bici come mezzo ideale di mobilità sostenibile, l&#39;attività
            fisica come veicolo di benessere.</p>
        <iframe width="420" height="315" src="http://www.youtube.com/embed/-Y3TKKbxxJo" frameborder="0"
            allowfullscreen></iframe>
        <h2>
            Il maestro</h2>
        <p>
            Mi chiamo <a href="http://www.linkedin.com/pub/marco-perasso/3a/470/14a" target="marco">
                Marco</a>, ho una laurea in Economia e mi occupo di sviluppo software (in pratica
            sono uno dei colpevoli se i computer si comportano in modo strano e sembrano difficili
            da usare). Mi piace il lavoro che faccio ma... la bici è sicuramente un&#39;altra
            cosa. E&#39; libertà, è un mezzo per sfogare le tensioni, è uno strumento per muoversi
            senza vincoli, è un modo per rimanere bambini (che ogni tanto si sbucciano un ginocchio
            o si infangano fino ai capelli), è una metafora che ti insegna che se vuoi ottenere
            qualcosa ed esserne soddisfatto, te la devi sudare; sono anche fermamente convinto
            che se vogliamo migliorare questa nostra società piuttosto disastrata, anestetizzata
            da shopping, calcio scommesse e grandi fratelli, si debba lavorare sui bambini.
            Sono un fervente sostenitore della bici come strumento di mobilità sostenibile (in
            particolare in città): ogni giorno percorro circa 25 Km per recarmi al lavoro sulla
            mia city bike, sono sufficientemente munito di attrezzatura anti intemperie ma ahimé
            non ho ancora trovato adeguate misure di difesa contro gli automobilisti nevrotici.</p>
        <p>
            Sulla scia di questa mia passione ho ottenuto il diploma di maestro della <a target="fci"
                href="http://www.federciclismo.it/studi/maestri_home.asp">Federazione Ciclistica
                Italiana</a>, mi sono iscritto ad <a target="assom" href="http://www.assomaestri.org">
                    Assomaestri</a> (organizzazione che tutela, anche dal punto di vista assicurativo,
            i maestri di Mountain Bike nella loro attività istituzionale) e ho deciso di aprire
            questa scuola. Non voglio insegnarti ad essere un campione, ad arrivare primo ad
            ogni costo, a surclassare gli altri: se riuscirò a farti divertire e trasmetterti
            solo un po&#39; del mio entusiasmo, potrò ritenermi soddisfatto.</p>
        <p>
            Potrai raccogliere altre informazioni su di me navigando questo sito, che ho curato
            nella forma (sigh, si vede che non sono un grafico...) e nei contenuti. Il mio numero
            di Partita IVA è 02118870993.</p>
        <h2>
            Dov&#39;è la scuola?</h2>
        <p>
            Il campo scuola è sito in Val Pentemina, verde e selvaggia valle che da Montoggio
            conduce a Pentema (conosciuta dai più per il presepe); dalla piazza centrale di
            Montoggio procedere lungo la provinciale 226 in direzione Laccio/Torriglia per 1,5
            Km (superando nell&#39;ordine la banca sulla sinistra, l&#39;ufficio postale sulla
            destra, il ponte sullo Scrivia, la caserma dei Carabinieri sulla destra, un supermercato
            sulla sinistra) quindi imboccare una strada secondaria a sinistra in corrispondenza
            di una curva a novanta gradi, seguendo l&#39;indicazione per Gazzolo. La strada
            sale per 1 Km, quindi inizia a scendere: dopo circa 200 metri troverete il campo
            scuola alla vostra destra.</p>
        <h4>
            <a href="Scuola di Mountain Bike Val Pentemina.pdf" target="modulo">Scarica regolamento
                e modulo di adesione</a>.</h4>
        <h4>
            <a target="schoolmap" href="http://maps.google.it/maps/myplaces?hl=it&ll=44.52607,9.06754&spn=0.001595,0.003613&ctz=-120&t=m&layer=c&cbll=44.526057,9.067671&panoid=_4c7DHvgZ28K-rtTqtjRUA&cbp=12,207.57,,0,24.89&z=19"
                style="color: #0000FF; text-align: left">Visualizza dove si trova il campo scuola
                con Google Street View</a><br />
            (immagini riprese <i>prima</i> che venisse preparato)</h4>
        <iframe width="800" height="600" frameborder="0" scrolling="no" marginheight="0"
            marginwidth="0" src="http://maps.google.it/maps?f=d&amp;source=embed&amp;saddr=Via+Roma%2FSP226&amp;daddr=Localit%C3%A0+Gazzolo&amp;geocode=FS47pwIdXwKKAA%3BFbBppwIdcV2KAA&amp;aq=0&amp;oq=Gazzolo,+mo&amp;sll=44.516736,9.051147&amp;sspn=0.012699,0.028903&amp;hl=it&amp;mra=dme&amp;mrsp=0&amp;sz=16&amp;ie=UTF8&amp;t=m&amp;ll=44.520806,9.056511&amp;spn=0.018359,0.034246&amp;z=15&amp;output=embed">
        </iframe>
        <br />
        <small><a href="http://maps.google.it/maps?f=d&amp;source=embed&amp;saddr=Via+Roma%2FSP226&amp;daddr=Localit%C3%A0+Gazzolo&amp;geocode=FS47pwIdXwKKAA%3BFbBppwIdcV2KAA&amp;aq=0&amp;oq=Gazzolo,+mo&amp;sll=44.516736,9.051147&amp;sspn=0.012699,0.028903&amp;hl=it&amp;mra=dme&amp;mrsp=0&amp;sz=16&amp;ie=UTF8&amp;t=m&amp;ll=44.520806,9.056511&amp;spn=0.018359,0.034246&amp;z=15"
            style="color: #0000FF; text-align: left">Visualizzazione ingrandita della mappa</a></small>
        <br />
    </div>
</asp:Content>
