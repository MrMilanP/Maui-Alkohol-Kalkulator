using Microsoft.Maui.Controls;

namespace Maui_Alkohol_Kalkulator.Utils
{
    // Ova klasa definiše behavior za Entry komponentu koji omogućava:
    // 1. Ograničenje unosa samo na brojeve.
    // 2. Kontrolu vidljivosti dugmeta za brisanje na osnovu unosa.
    public class NumericEntryBehavior : Behavior<Entry>
    {
        // Dugme za brisanje povezano sa Entry komponentom.
        public ImageButton? ClearButton { get; set; }

        // Metoda koja se poziva kada je Behavior povezan sa Entry-jem.
        protected override void OnAttachedTo(Entry entry)
        {
            // Dodajemo event handler koji prati promene unosa.
            entry.TextChanged += OnTextChanged;
            base.OnAttachedTo(entry);  // Pozivamo osnovnu (parent) implementaciju.
        }

        // Metoda koja se poziva kada Behavior više nije povezan sa Entry-jem.
        protected override void OnDetachingFrom(Entry entry)
        {
            // Uklanjamo event handler kada se Behavior odvoji, kako bi se izbegli memory leak-ovi.
            entry.TextChanged -= OnTextChanged;
            base.OnDetachingFrom(entry);  // Poziv osnovne implementacije.
        }

        // Ova metoda se poziva svaki put kada se promeni tekst u Entry-ju.
        private void OnTextChanged(object? sender, TextChangedEventArgs e)
        {
            // Provera da li je sender null ili nije Entry.
            if (sender is not Entry entry) return;

            // Provera da li je entry.Text null.
            string newText = entry.Text ?? string.Empty;

            // Ako unos sadrži nevalidne znakove, vraćamo stari validni tekst.
            if (!string.IsNullOrEmpty(entry.Text) && !IsAllDigits(entry.Text))
            {
                entry.Text = RemoveNonNumeric(entry.Text, e.OldTextValue);
            }

            // Ako je dugme za brisanje dodeljeno, ažuriramo njegovu vidljivost.
            // Dugme je vidljivo samo ako Entry nije prazan.
            if (ClearButton != null)
            {
                ClearButton.IsVisible = !string.IsNullOrEmpty(entry.Text);
            }
        }

        // Proverava da li string sadrži isključivo brojeve.
        private bool IsAllDigits(string str)
        {
            foreach (char c in str)
            {
                // Ako pronađemo karakter koji nije cifra, unos nije validan.
                if (!char.IsDigit(c))
                    return false;
            }
            // Vraćamo true samo ako su svi karakteri cifre.
            return true;
        }

        // Uklanja sve nevalidne (nebrojčane) znakove iz novog unosa i vraća prethodni validan tekst.
        private string RemoveNonNumeric(string newText, string oldText)
        {
            foreach (char c in newText)
            {
                // Ako pronađemo karakter koji nije cifra, vraćamo stari validni tekst.
                if (!char.IsDigit(c))
                    return oldText;
            }
            // Ako je novi unos validan, vraćamo ga.
            return newText;
        }
    }
}
