using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace OrderProcessing.lib
{
    public class OrderRequest
    {
        private static readonly Regex rxCurrency = new Regex(@"\$(\d+(\.\d+)?)", RegexOptions.Compiled);
        private static readonly string[] delimiters = new string[] { "\r\n" };

        public decimal Freight { get; private set; }
        public string Company { get; private set; }
        public string Shipping { get; private set; }
        public string PlacedBy { get; private set; }
        public string Shipper { get; private set; }
        public DateTime RequiredDate { get; private set; }
        public DateTime OrderDate { get; private set; }
        public IReadOnlyCollection<OrderRequestDetails> OrderDetails { get; private set; }

        public static OrderRequest FromEmail(MailMessage email)
        {
            if (email is null) throw new ArgumentNullException(nameof(email));
            if (email.Subject != "Order Request") throw new ArgumentNullException($"Wrong email type: {email.Subject}");
            if (rxCurrency.Matches(email.Body).Count < 1) throw new ArgumentException("Too few currency symbols found.");

            string[] body = email.Body.Split('\n');

            OrderRequest notice = new OrderRequest
            {
                Company = getValue(body[0]),
                PlacedBy = getValue(body[1]),
                OrderDate = DateTime.Parse(getValue(body[2])),
                RequiredDate = DateTime.Parse(getValue(body[3])),
                Shipper = getValue(body[4]),
                Freight = decimal.Parse(getValue(body[5])),
                Shipping = getValue(body[6])
            };

            string[] lines = email.Body.Split(delimiters, StringSplitOptions.None);

            List<OrderRequestDetails> details = lines.Skip(1).TakeWhile(l => l != "").Select(l => new OrderRequestDetails(l)).ToList();
            notice.OrderDetails = details.AsReadOnly();

            return notice;
        }

        private static string getValue(string line)
        {
            string[] parts = line.Split('\t');
            return parts.Length < 2 ? null : parts[1];
        }
    }

    public class OrderRequestDetails
    {
        internal OrderRequestDetails(string detailLine)
        {
            string[] parts = detailLine.Split('\t');
            Product = parts[0];
            ProductID = parts[1];
            Quantity = int.Parse(parts[2]);
            Discount = decimal.Parse(parts[3].Substring(1));
        }

        public string Product { get; internal set; }
        public string ProductID { get; internal set; }
        public int Quantity { get; internal set; }
        public decimal Discount{ get; internal set; }
    }
}
