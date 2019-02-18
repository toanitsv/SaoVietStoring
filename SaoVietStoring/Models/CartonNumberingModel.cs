using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaoVietStoring.Models
{
    public class CartonNumberingModel
    {
        public string ProductNo { get; set; }
        public string SizeNo { get; set; }
        public int Quantity { get; set; }
        public double GrossWeight { get; set; }
        public int CartonNoBasic { get; set; }
        public string Barcode { get; set; }
    }
}
