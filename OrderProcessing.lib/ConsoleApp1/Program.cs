using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SendMail
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter the email letter:    " +
                              "\r\n Press O for OrderRequests:   " +
                              "\r\n Press P for PaymentNotices:  " +
                              "\r\n Press S for ShipmentRequest: " +
                              "\r\n Press R for RestockNotices:  ");

            string input = Console.ReadLine().ToUpper();
            string folder = null, subject = null;

            switch (input)
            {
                case "O":
                    folder = "OrderRequests";
                    subject = "Order request";
                    break;

                case "P":
                    folder = "PaymentNotices";
                    subject = "Payment Received";
                    break;

                case "S":
                    folder = "ShipmentNotices";
                    subject = "Order Shipped";
                    break;

                case "R":
                    folder = "RestockNotices";
                    subject = "Shipment recieved";
                    break;

                default: Console.WriteLine("nothing to do! Bye."); return;
            }
            foreach (string fpath in Directory.GetFiles(folder))
                SendMessages(fpath, subject);
        }    

        static void SendMessages(string fileName,string subject)
        {
            string body = File.ReadAllText(fileName);
            MailMessage msg = new MailMessage("nwadmin@platformbronx.com", "rpa4@platformbronx.com", subject, body);
            SmtpClient client = new SmtpClient("10.0.0.35");
            client.Send(msg);
                       
        }


    }
}
