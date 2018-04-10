using System;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using JMA.BusinessLogic.Services;
using JMA.Mapping.DTOs;
using JMA.Mapping.ViewModels;
using JMA.Web.Models;
using AutoMapper;

namespace JMA.Web.Controllers
{
    public class ClaimantController : Controller
    {
        #region Members
        private readonly IClaimService claimService;
        #endregion

        #region Methods
        public ClaimantController(IClaimService claimService)
        {
            this.claimService = claimService;
        }

        // GET: Claimant
        public ActionResult Index()
        {
            Session.Abandon();
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", "")); // forces a new session id
            IndexViewModel model = new IndexViewModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(IndexViewModel model)
        {
            if (ModelState.IsValid)
            {
                var knownClaimantDTO = claimService.GetKnownClaimant(model.Claim8, model.PIN);

                if (knownClaimantDTO != null)
                {
                    if (knownClaimantDTO.SubmitDate == null)
                    {
                        TempData["claim8"] = knownClaimantDTO.Claim8;
                        TempData["pin"] = knownClaimantDTO.PIN;

                        return RedirectToAction("KnownClaimForm");
                    }
                    else
                    {
                        model.Error = "A claim has already been submitted for this Claim Id";
                    }
                }
                else
                {
                    model.Error = "The Claim ID / PIN was not found in our database. Please try again.";
                }
            }

            return View(model);
        }

        public ActionResult UnknownClaimForm()
        {
            ClaimFormViewModel model = new ClaimFormViewModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnknownClaimForm(ClaimFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var claimantDTO = Mapper.Map<ClaimFormViewModel, ClaimantDTO>(model);

                    claimantDTO.SubmitSource = "Web";
                    claimantDTO.SubmitDate = DateTime.Now;
                    claimantDTO.IPAddress = GetIPAddress();
                    claimantDTO.UserAgentString = GetUserAgentString();
                    claimantDTO.SessionID = Session.SessionID;

                    string caseCode = ConfigurationManager.AppSettings["CaseCode"];
                    var claimId = claimService.SaveUnknownClaim(claimantDTO);

                    string caseName = ConfigurationManager.AppSettings["CaseName"];
                    string fullClaimId = caseCode + "-" + claimId;

                    if (!string.IsNullOrEmpty(model.Email))
                    {
                        try
                        {
                            SmtpClient smtp = new SmtpClient();
                            smtp.Send(GetConfirmationMail(model.Email, caseName, claimantDTO.SubmitDate, fullClaimId));
                        }
                        catch (Exception e)
                        {
                            ModelState.AddModelError("", "Unable to send confirmation email.");
                            return RedirectToAction("Index", "Error");
                        }
                    }

                    return RedirectToAction("ClaimSubmitted", new { fullClaimId = fullClaimId });
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your administrator.");
                    return RedirectToAction("Index", "Error");
                }
            }

            return View(model);
        }

        public ActionResult KnownClaimForm()
        {
            var claim8 = TempData["claim8"].ToString();
            var pin = TempData["pin"].ToString();

            if (string.IsNullOrEmpty(claim8) || string.IsNullOrEmpty(pin))
            {
                return RedirectToAction("Index");
            }
            else
            {
                var knownClaimantDTO = claimService.GetKnownClaimant(claim8, pin);
                var model = Mapper.Map<KnownClaimantDTO, KnownClaimFormViewModel>(knownClaimantDTO);
                model.Claim8 = claim8;

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KnownClaimForm(KnownClaimFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var knownClaimantDTO = Mapper.Map<KnownClaimFormViewModel, KnownClaimantDTO>(model);

                    knownClaimantDTO.SubmitSource = "Web";
                    knownClaimantDTO.SubmitDate = DateTime.Now;
                    knownClaimantDTO.IPAddress = GetIPAddress();
                    knownClaimantDTO.UserAgentString = GetUserAgentString();
                    knownClaimantDTO.SessionID = Session.SessionID;

                    string caseCode = ConfigurationManager.AppSettings["CaseCode"];
                    var claimId = claimService.UpdateKnownClaim(knownClaimantDTO);

                    string caseName = ConfigurationManager.AppSettings["CaseName"];
                    string fullClaimId = caseCode + "-" + claimId;

                    if (!string.IsNullOrEmpty(model.Email))
                    {
                        try
                        {
                            SmtpClient smtp = new SmtpClient();
                            smtp.Send(GetConfirmationMail(model.Email, caseName, knownClaimantDTO.SubmitDate, fullClaimId));
                        }
                        catch (Exception)
                        {
                            ModelState.AddModelError("", "Unable to send confirmation email.");
                            return RedirectToAction("Index", "Error");
                        }
                    }

                    return RedirectToAction("ClaimSubmitted", new { fullClaimId = fullClaimId });
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your administrator.");
                    return RedirectToAction("Index", "Error");
                }
            }
            else
            {
                model.IAgree = false;
                return View(model);
            }
        }

        public ActionResult ClaimSubmitted(string fullClaimId)
        {
            if (!string.IsNullOrEmpty(fullClaimId))
            {
                var model = new ClaimSubmittedViewModel();
                model.ClaimID = fullClaimId;

                return View(model);
            }

            return RedirectToAction("Index");
        }

        private string GetIPAddress()
        {
            var ip = (!string.IsNullOrEmpty(Request.ServerVariables["HTTP_X_FORWARDED_FOR"])) ? Request.ServerVariables["HTTP_X_FORWARDED_FOR"] : Request.ServerVariables["REMOTE_ADDR"];

            if (ip.Contains(","))
            {
                ip = ip.Split(',').First().Trim();
            }

            return ip;
        }

        private string GetUserAgentString()
        {
            string userAgentString = Request.ServerVariables["HTTP_USER_AGENT"];

            if (!string.IsNullOrEmpty(userAgentString))
            {
                int maxLength = 255;

                if (userAgentString.Length > maxLength)
                {
                    userAgentString = userAgentString.Substring(0, maxLength);
                }
            }

            return userAgentString;
        }

        private MailMessage GetConfirmationMail(string email, string caseName, DateTime? submitDate, string fullClaimId)
        {
            string emailFrom = ConfigurationManager.AppSettings["EmailFrom"];

            MailMessage message = new MailMessage(emailFrom, email);
            message.BodyEncoding = Encoding.UTF8;
            message.Subject = "Confirmation of receipt of your online " + caseName + " Claim";

            StringBuilder sb = new StringBuilder();
            sb.Append("<html><head><meta content=\"text/html; charset=utf-8\" /></head>");
            sb.Append("<body><table width=\"600\" >");
            sb.Append("<tr><td width=\"125\"></td><td style=\"font:14px Times,serif;\">");
            sb.AppendFormat("<p>SUBJECT: {0}</p>", message.Subject);
            sb.AppendFormat("<p>Thank you for filing your claim online on {0}.<p>", submitDate);
            sb.AppendFormat("<p>Your <b>Claim ID</b> is: <b>{0}</b></p>", fullClaimId);
            sb.Append("<p>Please retain this information in your records and use the Claim ID in any communications with KCC regarding this case.</p>");
            sb.Append("</td></tr></table></body></html>");
            message.Body = sb.ToString();
            message.IsBodyHtml = true;

            return message;
        }
        #endregion
    }
}