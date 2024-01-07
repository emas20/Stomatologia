namespace Stomatologia.Models
{
    public class Stomatolog
    {
        public int Id { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }

        public string Specjalizacja { get; set; }
        public string ImieNazwisko 
        {
            get { return Imie + " " + Nazwisko; }
        }
    }
}
