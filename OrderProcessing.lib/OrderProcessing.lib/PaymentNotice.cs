using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OrderProcessing.lib
{
    public class PaymentNotice
    {
        private static readonly Regex rxCurrency = new Regex(@"\$(\d+(\.\d+)?)", RegexOptions.Compiled);
        private static readonly Regex rxDate = new Regex(@"(\d{1,2}-){2}\d{4}", RegexOptions.Compiled);
        private static readonly Regex rxOrderNumber = new Regex(@"order number is (\d+)", RegexOptions.Compiled);
        private static readonly Regex rxCheckNumber = new Regex(@"check number is (\d+)", RegexOptions.Compiled);

        public decimal PaymentAmount { get; private set; }
        public DateTime OrderDate { get; private set; }
        public string OrderNumber { get; private set; }
        public string CheckNumber { get; private set; }

        public static PaymentNotice FromEmail(MailMessage email)
        {
            if (email is null) throw new ArgumentNullException(nameof(email));
            if (email.Subject != "Payment Notice") throw new ArgumentNullException($"Wrong email type: {email.Subject}");
            if (rxCurrency.Matches(email.Body).Count < 1) throw new ArgumentException("Too few currency symbols found.");

            Match matchPaymentAmount = rxCurrency.Match(email.Body);
            Match matchOrderNumber = rxOrderNumber.Match(email.Body);
            Match matchCheckNumber = rxCheckNumber.Match(email.Body);
            Match matchOrderDate = rxDate.Match(email.Body);

            PaymentNotice notice = new PaymentNotice
            {
                PaymentAmount = decimal.Parse(matchPaymentAmount.Groups[1].Value),
                OrderNumber = matchOrderNumber.Groups[1].Value,
                OrderDate = DateTime.Parse(matchOrderDate.Groups[1].Value),
                CheckNumber = matchCheckNumber.Groups[1].Value
            };

            return notice;
        }
    }

}
