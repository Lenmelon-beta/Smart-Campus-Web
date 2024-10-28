using Microsoft.AspNetCore.Mvc;
using CustomAuth.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace CustomAuth.Controllers
{
    public class IncidentReportsController : Controller
    {
        private static List<IncidentReport> _incidentReports = new List<IncidentReport>();
        private readonly string filePath = Path.Combine(Directory.GetCurrentDirectory(), "IncidentReports", "incident_report.txt");

        // GET: IncidentReports/Submit
        public IActionResult Submit()
        {
            return View();
        }

        // POST: IncidentReports/Submit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Submit(IncidentReport model)
        {
            if (ModelState.IsValid)
            {
                // Assign unique ID and submission date
                model.ReportId = _incidentReports.Count + 1;
                model.SubmittedDate = DateTime.Now;

                // Add the report to the list
                _incidentReports.Add(model);

                // Save the report to a text file
                SaveReportToFile(model);

                // Send email notification
                SendEmailNotification(model);

                // Redirect to the List page
                TempData["SuccessMessage"] = "Incident submitted successfully!";
                return RedirectToAction("List");
            }

            return View(model);
        }

        // GET: IncidentReports/List
        public IActionResult List()
        {
            // Load reports from file
            LoadReportsFromFile();

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View(_incidentReports);
        }

        // Save the report to a text file
        private void SaveReportToFile(IncidentReport report)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"{report.ReportId}|{report.SubmittedBy}|{report.Email}|{report.Title}|{report.Description}|{report.SubmittedDate}");
            }
        }

        // Load reports from the text file (if any)
        private void LoadReportsFromFile()
        {
            if (System.IO.File.Exists(filePath))
            {
                _incidentReports.Clear();  // Clear the existing list before loading
                var lines = System.IO.File.ReadAllLines(filePath);

                foreach (var line in lines)
                {
                    var parts = line.Split('|');
                    if (parts.Length == 6)
                    {
                        _incidentReports.Add(new IncidentReport
                        {
                            ReportId = int.Parse(parts[0]),
                            SubmittedBy = parts[1],
                            Email = parts[2],
                            Title = parts[3],
                            Description = parts[4],
                            SubmittedDate = DateTime.Parse(parts[5])
                        });
                    }
                }
            }
        }

        // Method to send an email notification
        private void SendEmailNotification(IncidentReport report)
        {
            try
            {
                var fromAddress = new MailAddress("ryanweitz123@gmail.com", "#INCIDENT REPORT#");
                var toAddress = new MailAddress("ryanweitz123@gmail.com", "Ryan Weitz");
                const string fromPassword = "bwpf zguv gcus rhhj"; // app password
                const string subject = "New Incident Report Submitted";
                string body = $"A new report has been submitted by {report.SubmittedBy}.\n" +
                              $"Title: {report.Title}\nDescription: {report.Description}";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }

                Console.WriteLine("Email sent successfully!");
            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine("SMTP Error: " + smtpEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email sending failed: " + ex.Message);
            }
        }
    }
}
