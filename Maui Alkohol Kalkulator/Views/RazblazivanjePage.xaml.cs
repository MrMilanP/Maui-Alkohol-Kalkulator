using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Maui_Alkohol_Kalkulator.Views
{
    public partial class RazblazivanjePage : ContentPage
    {
        // Originalni niz etanola
        private readonly double[] etanola = { 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75 };
        private readonly double[] vode = { 68.207, 67.207, 66.185, 65.242, 64.295, 63.347, 62.395, 61.439, 60.476, 59.511, 58.542, 57.570, 56.596, 55.617, 54.635, 53.650, 52.662, 51.670, 50.676, 49.679, 48.679, 47.679, 46.670, 45.661, 44.650, 43.637, 42.620, 41.601, 40.579, 39.555, 38.529, 37.500, 36.469, 35.436, 34.399, 33.360, 32.129, 31.180, 30.229, 29.276, 28.319 };

        public RazblazivanjePage()
        {
            InitializeComponent();
            SetupClearButton(etPocetnaKolicina, clearBtnKolicina);
            SetupClearButton(etPocetnaJacina, clearBtnPocetna);
            SetupClearButton(etKrajnjaJacina, clearBtnZeljena);
        }

        private void OnClearBtnClicked(object sender, EventArgs e)
        {
            if (sender is ImageButton button)
            {
                if (button == clearBtnKolicina)
                {
                    etPocetnaKolicina.Text = string.Empty;
                }
                else if (button == clearBtnPocetna)
                {
                    etPocetnaJacina.Text = string.Empty;
                }
                else if (button == clearBtnZeljena)
                {
                    etKrajnjaJacina.Text = string.Empty;
                }
                button.IsVisible = false;
            }
        }

        private void OnCalculateClicked(object sender, EventArgs e)
        {
           Izracunaj();
        }


        private void Izracunaj()
        {

            try
            {

                if (!double.TryParse(etPocetnaKolicina.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double pocetnaKolicina) ||
                    !double.TryParse(etPocetnaJacina.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double pocetnaJacina) ||
                    !double.TryParse(etKrajnjaJacina.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double krajnaJacina))
                {
                    tvRezultat.Text = "Molimo unesite validne numeričke vrednosti.";
                    return;
                }

                // Provera da li su sva polja validna ili prazna
                if (pocetnaKolicina <= 0 || pocetnaJacina <= 0 || krajnaJacina <= 0)
                {
                    tvRezultat.Text = "Vrednosti moraju biti veće od nule.";
                    return;
                }

                // Računanje krajnje količine
                double krajnjaKolicina = (pocetnaJacina / krajnaJacina) * pocetnaKolicina;

                // Pronalaženje indeksa za početnu i krajnju jačinu u tabeli
                int pocetniIndeks = NadjiIndeks(pocetnaJacina);
                int krajnjiIndeks = NadjiIndeks(krajnaJacina);

                if (pocetniIndeks == -1 || krajnjiIndeks == -1)
                {
                    tvRezultat.Text = "Jačina alkohola nije pronađena u tabeli.";
                    return;
                }

                // Izračunavanje vode na osnovu tabele
                double pocetnaVoda = pocetnaKolicina * vode[pocetniIndeks] / 100;
                double krajnjaVoda = krajnjaKolicina * vode[krajnjiIndeks] / 100;

                // Izračunavanje količine vode koja treba da se doda
                double kolicinaVodeZaDodati = krajnjaVoda - pocetnaVoda;

                // Prikaz rezultata
                tvRezultat.Text = string.Format("Dodajte {0:F2} litara destilovane vode. Konačna količina sa sažimanjem: {1:F2} litara.", kolicinaVodeZaDodati, krajnjaKolicina);
            }
            catch (ArgumentException ex)
            {
                // Prikazuje poruku o grešci ako proračun baci izuzetak.
                tvRezultat.Text = ex.Message;
            }
        }

        // Metoda za pronalaženje indeksa jačine iz tabele
        private int NadjiIndeks(double jacina)
        {
            for (int i = 0; i < etanola.Length; i++)
            {
                if (Math.Abs(etanola[i] - jacina) < 0.001)
                {
                    return i;
                }
            }
            return -1; // Ako nije pronađena jačina
        }

        private void SetupClearButton(Entry entry, ImageButton clearButton)
        {
            entry.TextChanged += (s, e) =>
            {
                clearButton.IsVisible = !string.IsNullOrEmpty(entry.Text);
            };

            clearButton.Clicked += (s, e) =>
            {
                entry.Text = string.Empty;
                clearButton.IsVisible = false;
            };
        }
    }
}
