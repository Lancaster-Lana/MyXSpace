using System.Linq;
using System.Threading.Tasks;
//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MyXSpace.WebSPA.Api
{
    public class InviteController2// : BaseController<InviteController>
    {
        //private readonly ITenantAppService _tenantManager;
        //private readonly UserManager _userManager;
        //private readonly AuthHelper _authHelper;

        public InviteController2(   
            //    AuthHelper authHelper,
         //   ITenantAppService _tenantManager, UserManager<AppUser> userManager,
        //    ICandidateManager candidateManager,
        //    ILocationManager locationManager,  ITimeZoneManager timezoneManager)
        )
        {
            //    _tenantManager = tenantManager;
            //    _userManager = userManager; 
            //    _candidateManager = candidateManager;
        }

        /*
         [AbpAllowAnonymous]
         public async Task<IActionResult> VerifyTenantUserInvite(InviteTenantUserViewModel model)
         {
             var tenantId = model.TenantId.To<int>();
             var userId = SimpleStringCipher.Instance.Decrypt(model.UserId).To<long>();
             var activationCode = model.InviteActivationCode;
             var user = await _userManager.GetUserByIdAsync(userId);

             if (user == null || activationCode.IsNullOrEmpty())
                 return RedirectToAction("Login", "Account"); //Redirect to login page instead of defining the error for the user (for security purposes)
             else
             {
                 model.ShouldChangePassword = user.ShouldChangePasswordOnNextLogin;
                 model.FirstName = user.Name;
                 model.LastName = user.Surname;
                 model.EmailAddress = user.EmailAddress;
                 model.TimeZones = _timezoneManager.GetSystemTimeZones().ToList();
             }

             return View(model);
         }

         [HttpPost]
         [UnitOfWork]
         [AbpAllowAnonymous]
         public async Task<IActionResult> CompleteTenantUserInvite(InviteTenantUserFormModel model)
         {
             //grab querystring info
             var tenantId = model.TenantId.To<int>();
             var userId = SimpleStringCipher.Instance.Decrypt(model.UserId).To<long>();
             var activationCode = model.InviteActivationCode;
             using (UnitOfWorkManager.Current.SetTenantId(tenantId))
             {

                 var tenant = await _tenantManager.FindByIdAsync(tenantId);
                 var user = await _userManager.GetUserByIdAsync(userId);
                 if (activationCode.IsNullOrEmpty() || user.EmailConfirmationCode != activationCode)
                     user.EmailConfirmationCode = null;
                 if (tenant == null || user == null || user.EmailConfirmationCode.IsNullOrEmpty())
                 {
                     return Json(new AjaxResponse()
                     {
                         Error = new ErrorInfo(L("Tenant.Invite.Verify.Error"), L("Tenant.Invite.Verify.Error_Details")),
                         Success = false
                     });
                 }

                 user.Name = model.FirstName;
                 user.Surname = model.LastName;
                 user.IsActive = true;
                 user.IsEmailConfirmed = true;
                 user.EmailConfirmationCode = null;
                 user.TimeZoneId = model.TimeZoneId;

                 //if user is coming in for the first time, then make sure to set the password
                 if (!model.Password.IsNullOrEmpty())
                 {
                     user.Password = new PasswordHasher().HashPassword(model.Password);
                     user.PasswordResetCode = null;
                     user.ShouldChangePasswordOnNextLogin = false;
                 }
                 await UnitOfWorkManager.Current.SaveChangesAsync();

                 //log user into the application if we have their password, 
                 //otherwise just direct them to their landing page
                 if (!model.Password.IsNullOrEmpty())
                 {
                     var loginResult = await _authHelper.GetLoginResultAsync(
                         user.EmailAddress,
                         model.Password,
                         tenant.TenancyName);
                     await _authHelper.SignInAsync(loginResult.User, loginResult.Identity, false);
                 }

                 return Json(new AjaxResponse()
                 {
                     TargetUrl = _authHelper.GetHomePageUrl()
                 });
             }
         }

         [HttpPost]
         [UnitOfWork]
         [AbpAllowAnonymous]
         public async Task<IActionResult> CompleteClientUserInvite(InviteClientUserFormModel model)
         {
             //grab querystring info
             var tenantId = model.TenantId.To<int>();
             var userId = SimpleStringCipher.Instance.Decrypt(model.UserId).To<long>();
             var clientId = SimpleStringCipher.Instance.Decrypt(model.ClientId).To<long>();

             var activationCode = model.InviteActivationCode;
             using (UnitOfWorkManager.Current.SetTenantId(tenantId))
             {
                 var tenant = await _tenantManager.FindByIdAsync(tenantId);
                 var user = await _userManager.GetUserByIdAsync(userId);
                 var client = _clienManager.GetClientById(clientId);

                 //var clientUser = study.Users.FirstOrDefault(x => x.UserId == user.Id);
                 var confirmationCode = clientUser.ClientEmailConfirmationCode;
                 var clientUsers = _clientUserManager.GetClientUsersByEmailConfirmationCode(confirmationCode);

                 if (tenant == null || user == null || confirmationCode != activationCode)
                 {
                     return Json(new AjaxResponse()
                     {
                         Error = new ErrorInfo(L("Client.Invite.Verify.Error"), L("Client.Invite.Verify.Error_Details")),
                         Success = false
                     });
                 }

                 clientUser.IsActive = true;
                 clientUser.IsClientUserEmailConfirmed = true;
                 clientUser.StudyUserEmailConfirmationCode = null;
 
                 //update user record info
                 user.Name = model.FirstName;
                 user.Surname = model.LastName;
                 user.IsActive = true;
                 user.IsEmailConfirmed = true;
                 user.EmailConfirmationCode = null;
                 user.TimeZoneId = model.TimeZoneId;

                 //if user is coming in for the first time, then make sure to set the password
                 if (!model.Password.IsNullOrEmpty())
                 {
                     user.Password = new PasswordHasher().HashPassword(model.Password);
                     user.PasswordResetCode = null;
                     user.ShouldChangePasswordOnNextLogin = false;
                 }
                 await UnitOfWorkManager.Current.SaveChangesAsync();

                 //log user into the application if we have their password, 
                 //otherwise just direct them to their landing page
                 if (!model.Password.IsNullOrEmpty())
                 {
                     var loginResult = await _authHelper.GetLoginResultAsync(
                         user.EmailAddress,
                         model.Password,
                         tenant.TenancyName);
                     await _authHelper.SignInAsync(loginResult.User, loginResult.Identity, false);
                 }
                 return Json(new AjaxResponse()
                 {
                     TargetUrl = _authHelper.GetHomePageUrl()
                 });
             }
         }


         [AllowAnonymous]
         public async Task<IActionResult> VerifyCandidateUserInvite(InvitecandidateUserViewModel model)
         {
             var tenantId = model.TenantId.To<int>();
             var userId = SimpleStringCipher.Instance.Decrypt(model.UserId).To<long>();
             var candidateId = SimpleStringCipher.Instance.Decrypt(model.candidateId).To<long>();
             var user = await _userManager.GetUserByIdAsync(userId);

             if (user == null || user.EmailConfirmationCode.IsNullOrEmpty())
                 return RedirectToAction("Login", "Account");
             else
             {
                 model.ShouldChangePassword = user.ShouldChangePasswordOnNextLogin;
                 model.InviteNoLongerValid = false;
                 model.FirstName = user.Name;
                 model.LastName = user.Surname;
                 model.EmailAddress = user.EmailAddress;
                 model.TimeZones = _timezoneManager.GetSystemTimeZones().ToList();
             }
             return View(model);
         }


         [HttpPost]
         [UnitOfWork]
         [AllowAnonymous]
         public async Task<IActionResult> CompleteCandidateUserInvite(InviteCandidateUserFormModel model)
         {
             var tenantId = model.TenantId.To<int>();
             var candidateId = SimpleStringCipher.Instance.Decrypt(model.candidateId).To<long>();
             var userId = SimpleStringCipher.Instance.Decrypt(model.UserId).To<long>();
             var activationCode = model.InviteActivationCode;

             using (UnitOfWorkManager.Current.SetTenantId(tenantId))
             {
                 var tenant = await _tenantManager.FindByIdAsync(tenantId);
                 var user = await _userManager.FindByIdAsync(userId);
                 var candidate = _candidateManager.GetCandidateById(candidateId);

                 if (tenant == null || user == null || candidate == null || user.EmailConfirmationCode != activationCode)
                 {
                     return Json(new AjaxResponse()
                     {
                         Error = new ErrorInfo(L("Candidate.Invite.Verify.Error"), L("Candidate.Invite.Verify.Error_Details")),
                         Success = false
                     });
                 }

                 candidate.IsActive = true;
                 candidate.InviteActivationCode = null;
                 candidate.Candidate.IsActive = true;

                 user.Name = model.FirstName;
                 user.Surname = model.LastName;
                 user.UserName = user.FullName.ToLower().TrimSpaces();
                 user.IsActive = true;
                 user.IsEmailConfirmed = true;
                 user.EmailConfirmationCode = null;

                 //if user is coming in for the first time, then make sure to set the password
                 if (!model.Password.IsNullOrEmpty())
                 {
                     user.Password = new PasswordHasher().HashPassword(model.Password);
                     user.PasswordResetCode = null;
                     user.ShouldChangePasswordOnNextLogin = false;
                 }
                 await UnitOfWorkManager.Current.SaveChangesAsync();

                 //log user into the application if we have their password, 
                 //otherwise just direct them to their landing page
                 if (!model.Password.IsNullOrEmpty())
                 {
                     var loginResult = await _authHelper.GetLoginResultAsync(
                         user.EmailAddress,
                         model.Password,
                         tenant.TenancyName);
                     await _authHelper.SignInAsync(loginResult.User, loginResult.Identity, false);
                 }

                 var target = "/invite/verifycandidateconsentform?candidateId=" + candidateId;
                 return Json(new AjaxResponse()
                 {
                     TargetUrl = target
                 });
             }
         }

         [AllowAnonymous]
         public IActionResult VerifyCandidateConsentForm(long id)
         {
             var candidate = _candidateManager.GetCandidateById(id);
             return View(candidate.MapTo<CandidateDto>());
         }

         */
    }
}
