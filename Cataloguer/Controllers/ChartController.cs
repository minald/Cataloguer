using Cataloguer.Models;
using Cataloguer.Models.NeuralNetwork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
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

        private async Task<ApplicationUser> GetUserAsync() => await _userManager.GetUserAsync(User);

        public async Task<IActionResult> Index()
        {
            List<Track> chart = Repository.GetTopTracks(amount: 20);
            ApplicationUser currentUser = await GetUserAsync();
            List<Track> userRating = Repository.GetTopUserTracks(currentUser.Id).ToList();
            var neuralNetwork = new NeuralNetwork(Repository);
            List<KeyValuePair<Track, float>> assumptiveRating = neuralNetwork.GetAssumptiveRating(currentUser);
            return View(new ChartViewModel(chart, userRating, assumptiveRating));
        }

        public IActionResult FullChart() => View(Repository.GetTopTracks(amount: 1000));

        public async Task<IActionResult> ChangeChart()
        {
            ApplicationUser currentUser = await GetUserAsync();
            List<Track> userRating = Repository.GetTopUserTracks(currentUser.Id).ToList();
            return View(userRating);
        }

        [HttpPost]
        public async Task<IActionResult> AddToClientChart(string trackName, string artistName, int position)
        {
            Track track = Repository.GetTrack(trackName, artistName);
            ApplicationUser currentUser = await GetUserAsync();
            Repository.InsertRating(new Rating(currentUser, track, position));
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> RemoveFromClientChart(string trackName, string artistName)
        {
            Track track = Repository.GetTrack(trackName, artistName);
            ApplicationUser currentUser = await GetUserAsync();
            Repository.DeleteRating(currentUser, track);
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
