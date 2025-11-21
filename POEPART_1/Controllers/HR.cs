using Microsoft.AspNetCore.Mvc;
using POEPART_1.Documents;
using POEPART_1.Models;
using QuestPDF.Fluent;
using Rotativa.AspNetCore;

namespace POEPART_1.Controllers
{
    public class HR : Controller
    {
        private sqlconnect _db = new sqlconnect();
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult HRClaims()
        {
            var claims = _db.GetApprovedClaims();
            Console.WriteLine($" Claims count: {claims.Count}");
            return View(claims);
        }

   
        [HttpPost]
        public IActionResult UpdatePayment(int claimId, string paymentStatus)
        {
            if (string.IsNullOrEmpty(paymentStatus))
            {
                // Debug: should never happen
                Console.WriteLine(" paymentStatus is null!");
                return RedirectToAction("HRClaims");
            }

            Console.WriteLine($"Updating claim {claimId} to {paymentStatus}"); // Debug
            _db.UpdatePaymentStatus(claimId, paymentStatus);
            return RedirectToAction("HRClaims");
        }


        public IActionResult HRLecturers(string search)
        {
            List<User> lecturers;

            if (!string.IsNullOrEmpty(search))
            {
                lecturers = _db.SearchLecturers(search);
                ViewBag.SearchTerm = search;
            }
            else
            {
                lecturers = _db.GetAllLecturers();
            }

            return View(lecturers);
        }

        // Update lecturer info
        [HttpPost]
        public IActionResult EditLecturer(int userID, string full_names, string email_address)
        {
            _db.UpdateLecturer(userID, full_names, email_address);
            return RedirectToAction("HRLecturers");
        }

        // GET: show the form for generating invoices
        [HttpGet]
        public IActionResult HRReports()
        {
            var model = new InvoiceRequestViewModel
            {
                ProcessedClaims = _db.GetAllProcessedClaims() // only 'Processed' claims
            };

            return View(model);
        }

        // GET: generate and return PDF file (called by form with lecturerId, month, year)

        [HttpGet]
        public IActionResult DownloadInvoicePdf(int claimId)
        {
            var claim = _db.GetClaimById(claimId); // fetch the single claim

            if (claim == null || claim.Payment_Status != "Processed")
                return NotFound("Processed claim not found.");

            var invoice = new Invoice
            {
                LecturerId = claim.EmployeeNum,
                LecturerName = claim.Lecture_Name,
                MonthYear = claim.Claim_Start_Date.ToString("MMMM yyyy"),
                Claims = new List<Claim> { claim } // only one claim
            };

            var document = new InvoiceDocument(invoice);
            byte[] pdfBytes = document.GeneratePdf(); // QuestPDF API
            var fileName = $"Invoice_{invoice.LecturerName.Replace(' ', '_')}_{claim.Claim_Start_Date:MM_yyyy}.pdf";

            return File(pdfBytes, "application/pdf", fileName);
        }






    }
}
