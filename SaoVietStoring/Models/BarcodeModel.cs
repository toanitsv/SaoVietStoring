using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaoVietStoring.Models
{
    public class BarcodeModel
    {
        public string Barcode { get; set; }
        public string PackingPlanNo { get; set; }
        public string GTNPONo { get; set; }
        public string ArticleNo { get; set; }
        public int CartonNo { get; set; }
        public string SizeNo { get; set; }
        public int ItemQuantity { get; set; }
        public double GrossWeight { get; set; }
    }
}
