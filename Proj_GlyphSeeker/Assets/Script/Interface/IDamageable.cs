public interface IDamageable // Interfaccia da assegnare a giocatore e nemici che possono essere danneggiati
{
    void TakeDamage(int damage) // Funzione per prendere danno
    {

    }

    void CheckDeath() // Funzione chiamata in TakeDamage per controllare se la salute è arrivata a 0
    {

    }

    void InstantDeath() // Funzione chiamata in CheckDeath quando la salute arriva a 0, distrugge o disattiva il giocatore o nemico
    {

    }
}