using Microsoft.AspNetCore.Mvc;
using PassionProject.Models;
using PassionProject.Models.ViewModels;
using PassionProject.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace PassionProject.Controllers
{
    public class IngredientPageController : Controller
    {
        private readonly IDessertService _dessertService;
        private readonly IIngredientService _ingredientService;
        private readonly IInstructionService _instructionService;

        // dependency injection of service interface

        public IngredientPageController(IIngredientService IngredientService, IDessertService DessertService, IInstructionService InstructionService)
        {
            _ingredientService = IngredientService;
            _dessertService = DessertService;
            _instructionService = InstructionService;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: IngredientPage/List
        public async Task<IActionResult> List()
        {
            IEnumerable<IngredientDto?> IngredientDtos = await _ingredientService.ListIngredients();
            return View(IngredientDtos);
        }

        // GET: IngredientPage/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            IngredientDto? IngredientDto = await _ingredientService.FindIngredient(id);
            IEnumerable<DessertDto> AssociatedDesserts = await _dessertService.ListDessertsForIngredient(id);
            IEnumerable<DessertDto> Desserts = await _dessertService.ListDesserts();

            //need the instructions for this ingredient
            IEnumerable<InstructionDto> Instructions = await _instructionService.ListInstructionsForIngredient(id);

            if (IngredientDto == null)
            {
                return View("Error", new ErrorViewModel() { Errors = ["Could not find Ingredient"] });
            }
            else
            {
                // information which drives a Ingredient page
                IngredientDetails IngredientInfo = new IngredientDetails()
                {
                    Ingredient = IngredientDto,
                    IngredientDesserts = AssociatedDesserts,
                    AllDesserts = Desserts,
                    IngredientInstructions = Instructions
                };
                return View(IngredientInfo);
            }
        }

        // GET IngredientPage/New
        [Authorize]
        public ActionResult New()
        {
            return View();
        }


        // POST IngredientPage/Add
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(IngredientDto IngredientDto)
        {
            ServiceResponse response = await _ingredientService.AddIngredient(IngredientDto);

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("Details", "IngredientPage", new { id = response.CreatedId });
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }

        //GET IngredientPage/Edit/{id}
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            IngredientDto? IngredientDto = await _ingredientService.FindIngredient(id);
            if (IngredientDto == null)
            {
                return View("Error");
            }
            else
            {
                return View(IngredientDto);
            }
        }

        //POST IngredientPage/Update/{id}
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(int id, IngredientDto IngredientDto)
        {
            ServiceResponse response = await _ingredientService.UpdateIngredient(IngredientDto);

            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                return RedirectToAction("Details", "IngredientPage", new { id = id });
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }

        //GET IngredientPage/ConfirmDelete/{id}
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            // Views/IngredientPage/ConfirmDelete.cshtml
            // find information about this ingredient
            IngredientDto? IngredientDto = await _ingredientService.FindIngredient(id);

            return View(IngredientDto);
        }

        //POST IngredientPage/Delete/{id}
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResponse response = await _ingredientService.DeleteIngredient(id);

            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("List", "IngredientPage");
            }
            else
            {
                return RedirectToAction("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }

        //POST IngredientPage/LinkToDessert
        //DATA: dessertId={dessertId}&ingredientId={ingredientId}
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> LinkToDessert([FromForm] int ingredientId, [FromForm] int dessertId)
        {
            await _dessertService.LinkDessertToIngredient(dessertId, ingredientId);

            return RedirectToAction("Details", new { id = ingredientId });
        }

        //POST IngredientPage/UnlinkFromDessert
        //DATA: dessertId={dessertId}&ingredientId={ingredientId}
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UnlinkFromDessert([FromForm] int ingredientId, [FromForm] int dessertId)
        {
            await _dessertService.UnlinkDessertFromIngredient(dessertId, ingredientId);

            return RedirectToAction("Details", new { id = ingredientId });
        }
    }
}
