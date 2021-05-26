using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Relikt_2.Areas.Identity.Pages.Account.Manage
{
    public class Disable2faModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<Disable2faModel> _logger;

        public Disable2faModel(
            UserManager<IdentityUser> userManager,
            ILogger<Disable2faModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Невозможно загрузить пользователя с помощью ID '{_userManager.GetUserId(User)}'.");
            }

            if (!await _userManager.GetTwoFactorEnabledAsync(user))
            {
                throw new InvalidOperationException($"Невозможно отключить двухфакторную аутентификацию для пользователя с ID '{_userManager.GetUserId(User)}' поскольку он в настоящее время не включен.");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Невозможно загрузить пользователя с помощью ID '{_userManager.GetUserId(User)}'.");
            }

            var disable2faResult = await _userManager.SetTwoFactorEnabledAsync(user, false);
            if (!disable2faResult.Succeeded)
            {
                throw new InvalidOperationException($"Произошла непредвиденная ошибка при отключении двухфакторной аутентификации для пользователя с ID '{_userManager.GetUserId(User)}'.");
            }

            _logger.LogInformation("Пользователь с идентификатором '{UserId}' отключил двухфакторную аутентификацию.", _userManager.GetUserId(User));
            StatusMessage = "двухфакторная аутентификация отключена. Вы можете включить двухфакторную аутентификацию при настройке приложения для аутентификации";
            return RedirectToPage("./TwoFactorAuthentication");
        }
    }
}