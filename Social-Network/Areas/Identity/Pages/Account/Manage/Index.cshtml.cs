using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Social_Network.Data.social_network;

namespace Social_Network.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [ViewData]
            [Display(Name = "User name")]
            public string User_Name { get; set; }

            [ViewData]
            [Display(Name = "Real name")]
            public string Surname_Name { get; set; }

            [ViewData]
            [Display(Name = "User status")]
            public string Status { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Photo")]
            public string AvatartUrl { get; set; }

        }

        private async Task LoadAsync(User user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var userStatus = user.Status;
          

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                User_Name = userName,
                Status = userStatus,
                Surname_Name = user.Surname_Name,
                AvatartUrl = user.AvatarURL
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile fileToSocialNetwork)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var username = await _userManager.GetUserNameAsync(user);
            

            if (Input.User_Name != username)
            {
                var setUserName = await _userManager.SetUserNameAsync(user, Input.User_Name);
                if (!setUserName.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set user name.";
                    return RedirectToPage();
                }
            }

            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

           

            if (Input.Status == user.Status && Input.Surname_Name == user.Surname_Name && Input.AvatartUrl == user.AvatarURL)
            {
                StatusMessage = "Your profile has been updated.";
                return RedirectToPage();
            }

            user.Status = Input.Status;
            user.Surname_Name = Input.Surname_Name;
          
            var userURL = await Helpers.Media.UploadImage(fileToSocialNetwork, "AvatarURL");

            if (fileToSocialNetwork != null)
            {
                user.AvatarURL = userURL;
            }

            var updateResult = await _userManager.UpdateAsync(user);

            if(!updateResult.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to update information.";
                return RedirectToPage();
            }
            
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
