using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaoVietStoring.Models
{
    public class IssuesDetailModel
    {
        public string ProductNo { get; set; }
        public string Barcode { get; set; }
        public int CartonNo { get; set; }
        public string SizeNo { get; set; }
        public int IssuesId { get; set; }
        public double GrossWeight { get; set; }
        public double ActualWeight { get; set; }
        public double DifferencePercent { get; set; }
        public string Process { get; set; }
        public string CheckBy { get; set; }
        public string Location { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
