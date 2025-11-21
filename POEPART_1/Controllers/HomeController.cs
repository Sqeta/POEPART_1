using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using POEPART_1.Models;
using static POEPART_1.Models.pcview;

namespace POEPART_1.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        //create an instance for class model auto create
        auto_create_instance_db_tables create = new auto_create_instance_db_tables();
        //use object name to call the auto create method 
        create.InitializeSystem();
        return RedirectToAction("Home");
    }
    [HttpGet]
    public IActionResult Register()
    {
        sqlconnect connect = new sqlconnect();

        connect.create_table();

        return View();
    }
    [HttpPost]
    public IActionResult Register(Register user)
    {
        sqlconnect connect = new sqlconnect();

        if (!ModelState.IsValid)
        {
            return View(user);
        }
        else
        {
            connect.store_user(user.full_names,user.email_address,user.password,user.confirm_password,user.role);
            return RedirectToAction("Login");        
        }

            
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult ViewApprove()
    {
        return View();
    }
    public IActionResult GenerateReport()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Lecture()
    {
        return View();
    }


    [HttpPost]
    public IActionResult Lecture(Claim claim)
    {
        if (ModelState.IsValid)
        {
            sqlconnect con = new sqlconnect();

            // Check if EmployeeNum exists
            if (!con.UserExists(claim.EmployeeNum))
            {
                ModelState.AddModelError("", "? Employee number does not exist!");
                return View(claim);
            }

            try
            {
                // Convert IFormFile to byte[]
                byte[] fileData = new byte[0];
                if (claim.SupportingFile != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        claim.SupportingFile.CopyTo(ms);
                        fileData = ms.ToArray();
                    }
                }

                con.StoreClaimTable(
                    claim.EmployeeNum,
                    claim.Lecture_Name,
                    claim.Module_Name,
                    claim.Number_ofSessions,
                    (double)claim.Hourly_rate,
                    claim.Claim_Start_Date,
                    claim.Claim_End_Date,
                    claim.Claim_Description,
                    fileData
                );

                TempData["SuccessMessage"] = "? Claim submitted successfully!";
                return RedirectToAction("Lecture");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "? Error storing claim: " + ex.Message);
            }
        }

        return View(claim);
    }



    // VIEW: Coordinator dashboard
    [HttpGet]
    public IActionResult Programme_Coordinator()
    {
        sqlconnect con = new sqlconnect();
        var allClaims = con.GetAllClaims();

        var viewModel = new POEPART_1.Models.ProgrammeCoordinatorViewModel

        {
            Claims = allClaims
        };

        return View(viewModel);
    }


    // ACTION: Update claim status (Approve or Reject)
    [HttpPost]
    public IActionResult UpdateClaimStatus(int claimId, string newStatus)
    {
        sqlconnect con = new sqlconnect();
        con.UpdateClaimStatus(claimId, newStatus);

        // Redirect back to coordinator dashboard
        return RedirectToAction("Programme_Coordinator");
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(Login Log)
    {
        sqlconnect con = new sqlconnect();

        if (ModelState.IsValid)
        {
            bool isfound = con.LoginUser(Log.email_address,Log.password,Log.role);


            if (isfound)
            {
                switch (Log.role)
                {
                    case "Lecture":
                        return RedirectToAction("Lecture");
                    case "Programme Coordinator":
                        return RedirectToAction("Programme_Coordinator");
                    case "Academic Manager":
                        return RedirectToAction("Academic_Manager");
                    case "HR Personnel":
                        return RedirectToAction("HumanResource");
                }
            }
       
        }
       
       
            Console.WriteLine("cant store");
            return View(Log);
        

    }
    [HttpGet]
    public IActionResult Academic_Manager()
    {
        sqlconnect con = new sqlconnect();
        var allClaims = con.GetAllClaims();

        var viewModel = new AcademicManagerViewModel
        {
            Claims = allClaims
        };

        return View(viewModel);
    }
    [HttpPost]
    public IActionResult UpdateClaimStatusByManager(int claimId, string newStatus)
    {
        sqlconnect con = new sqlconnect();
        con.UpdateClaimStatusByManager(claimId, newStatus);
        return RedirectToAction("Academic_Manager");
    }


    public IActionResult Home()
    {
        return View();
    }

    public IActionResult HumanResource()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
