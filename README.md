NB: Tutti gli utenti presenti in database hanno come password
ciao

NB2: Si consiglia di effettuare LogOut prima di chiudere il browser. Se la sessione restituisse null alla compilazione, pulire il cache storage e riavviare il progetto.

Nel progetto è possibile:
> da cliente
	- registrare un ordine per libri con giacenza > 0. Se il cliente pone un ordine per un libro già ordinato precedentemente, viene aggiornata la sua richiesta al nuovo valore (compatibilmente con la giacenza totale e tenendo conto della quantità dell'ordine precedente es) vecchio ordine 2x Libro1 -> nuovo ordine chiede 5x Libro1, la giacenza viene controllata per un totale di 3 nuove copie richieste, non 5)
	- registrare una richiesta per libri con giacenza = 0. Se il cliente pone una richiesta per un libro già richiesto, viene aggiornata la sua richiesta al nuovo valore
	- vedere il proprio storico ordini / richieste e cancellare per ogni libro la richiesta associata.

> da venditore
	- vedere ordini e richieste di ogni cliente. Se non sono già state prese in carico da un altro venditore, può scegliere di prenderle in carico (al cliente verrà mostrato il rispettivo aggiornamento nella pagina personale). Se ha preso in carico un ordine/richiesta, può liberarsi dell'incombenza. Tutto ciò avviene tramite pagina personale del venditore (/SellerArea/Home)
	
> entrambi i ruoli
	- funzione di Login/Logout e associazione tramite variabili di sessione al proprio account.
	- ricerca libri per Autore, Titolo, ISBN, MinPrice, MaxPrice. La ricerca avviene combinando i campi inseriti a scelta dall'utente (da 0 a 5).