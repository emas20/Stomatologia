using Microsoft.AspNetCore.Mvc.Rendering;

namespace Stomatologia.Models
{
    public class UmowWizyteViewModel
    {
        public List<SelectListItem> DostepniStomatolodzy { get; set; }
        public DateTime WybranaData { get; set; }
        public string WybranaGodzina { get; set; }

        public int WybranyStomatologId { get; set; }
    }
}
