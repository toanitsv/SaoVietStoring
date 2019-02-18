using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaoVietStoring.Models
{
    public class OrdersModel
    {
        public string ProductNo { get; set; }
        public DateTime ETD { get; set; }
        public string ArticleNo { get; set; }
        public string ShoeName { get; set; }
        public int Quantity { get; set; }
        public string PatternNo { get; set; }
        public string MidsoleCode { get; set; }
        public string OutsoleCode { get; set; }
        public string LastCode { get; set; }
        public string Country { get; set; }
        public bool IsEnable { get; set; }
        //
        public string GTNPONo { get; set; }
        public string UCustomerCode { get; set; }
    }
}
