using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalHub.Models.ViewModels
{
    public class GadgetHomeVM
    {
        public string CategoryName { get; set; }
        public List<Gadget> Gadgets { get; set; }
    }
}
