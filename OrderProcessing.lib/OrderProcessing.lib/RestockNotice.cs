using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OrderProcessing.lib
{
    public class RestockNotice
    {
        private static readonly Regex rxCurrency = new Regex(@"\$(\d+(\.\d+)?)", RegexOptions.Compiled);
        private static readonly Regex rxSupplier = new Regex(@"supplier (.*)\s+(\(\d+\))", RegexOptions.Compiled);
        private static readonly string[] delimiters = new string[] { "\r\n" };

        public string Supplier { get; private set; }
        public string SupplierID { get; private set; }
        public IReadOnlyCollection<RestockDetails> RestockDetails { get; private set; }
        public int Quantity { get; set; }

        public static RestockNotice FromEmail(MailMessage email)
        {
            if (email is null) throw new ArgumentNullException(nameof(email));
            if (email.Subject != "Restock Notice") throw new ArgumentNullException($"Wrong email type: {email.Subject}");
            if (rxCurrency.Matches(email.Body).Count < 1) throw new ArgumentException("Too few currency symbols found.");

            Match match = rxSupplier.Match(email.Body);

            RestockNotice notice = new RestockNotice
            {
                Supplier = match.Groups[1].Value,
                SupplierID = match.Groups[2].Value
            };

            string[] lines = email.Body.Split(delimiters, StringSplitOptions.None);

            List<RestockDetails> details = lines.Skip(1).TakeWhile(l => l != "").Select(l => new RestockDetails(l)).ToList();
            notice.RestockDetails = details.AsReadOnly();

            return notice;
        }

    }

    public class RestockDetails
    {
        internal RestockDetails(string detailLine)
        {
            string[] parts = detailLine.Split('\t');
            ProductID = parts[0];
            ProductName = parts[1];
            Quantity = int.Parse(parts[2].Substring(1));
        }

        public string ProductID { get; internal set; }
        public string ProductName  { get; internal set; }
        public int Quantity { get; internal set; }

    }
}
