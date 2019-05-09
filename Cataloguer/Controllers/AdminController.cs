using Cataloguer.Models;
using Cataloguer.Models.NeuralNetwork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cataloguer.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private Repository Repository { get; set; }

        private NeuralNetwork NeuralNetwork { get; set; }

        public AdminController(Repository repository)
        {
            Repository = repository;
            NeuralNetwork = new NeuralNetwork(repository);
        }

        public IActionResult Index()
        {
            return View();
        }

        public /*async Task<IActionResult>*/ IActionResult Learn()
        {
            NeuralNetwork.Learn();
            /*await*/ NeuralNetwork.SaveAsync();
            return View(nameof(Index));
        }
    }
}