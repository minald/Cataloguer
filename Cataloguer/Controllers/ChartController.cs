using Cataloguer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cataloguer.Controllers
{
    [Authorize]
    public class ChartController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private Repository Repository { get; set; }

        public ChartController(UserManager<ApplicationUser> userManager, Repository repository)
        {
            Repository = repository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            List<Track> chart = Repository.GetTopTracks(amount: 20);
            ApplicationUser currentUser = await _userManager.GetUserAsync(User);
            List<Track> userRating = Repository.GetTopUserTracks(currentUser.Id);
            return View(new ChartViewModel(chart, userRating));
        }

        public IActionResult FullChart() => View(Repository.GetTopTracks(amount: 1000));

        public async Task<IActionResult> ChangeChart()
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(User);
            List<Track> userRating = Repository.GetTopUserTracks(currentUser.Id);
            return View(userRating);
        }
    }
}
