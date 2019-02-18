using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SaoVietStoring.Models;
using SaoVietStoring.Entites;

namespace SaoVietStoring.Controllers
{
    public class MailAddressReceiveMessageController
    {
        static StoringSystemEntities db = ConnectTo.StoringSystemEntities();
        public static List<MailAddressReceiveMessageModel> Get()
        {
            return db.ExecuteStoreQuery<MailAddressReceiveMessageModel>("spm_SelectMailAddressReceiveMessage").ToList();
        }
    }
}
