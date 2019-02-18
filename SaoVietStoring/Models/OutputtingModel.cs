using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaoVietStoring.Models
{
    public class OutputingModel
    {
        public string ProductNo { get; set; }
        public string Barcode { get; set; }
        public int CartonNo { get; set; }
        public string SizeNo { get; set; }
        public double GrossWeight { get; set; }
        public double ActualWeight { get; set; }
        public double DifferencePercent { get; set; }
        public bool IsPass { get; set; }
        public string WorkerId { get; set; }
        public int IssuesId { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
