using Microsoft.AspNetCore.Mvc;
using PassionProject.Interfaces;
using PassionProject.Models;
using PassionProject.Models.ViewModels;

namespace PassionProject.Controllers
{
    public class DessertPageController : Controller
    {
       private readonly IDessertService _dessertService;
        private readonly IIngredientService _ingredientService;
        private readonly IReviewService _reviewService;

        // dependency injection of service interfaces

        public DessertPageController(IDessertService DessertService, IIngredientService IngredientService, IReviewService ReviewService)
        {
            _dessertService = DessertService;
            _ingredientService = IngredientService;
            _reviewService = ReviewService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        // GET: DessertPage/List
        public async Task<IActionResult> List()
        {
            IEnumerable<DessertDto?> DessertDtos = await _dessertService.ListDesserts();
            return View(DessertDtos);
        }

        // GET: DessertPage/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            DessertDto? DessertDto = await _dessertService.FindDessert(id);
            IEnumerable<IngredientDto> AssociatedIngredients = await _ingredientService.ListIngredientsForDessert(id);
            IEnumerable<ReviewDto> AssociatedReviews = await _reviewService.ListReviewsForDessert(id);

            if (DessertDto == null)
            {
                return View("Error", new ErrorViewModel() { Errors = ["Could not find dessert"] });
            }
            else
            {
                // information which drives a dessert page
                DessertDetails DessertInfo = new DessertDetails()
                {
                    Dessert = DessertDto,
                    DessertIngredients = AssociatedIngredients,
                    DessertReviews = AssociatedReviews
                };
                return View(DessertInfo);
            }
        }
        // GET DessertPage/New
        public ActionResult New()
        {
            return View();
        }

        // POST DessertPage/Add
        [HttpPost]
        public async Task<IActionResult> Add(DessertDto DessertDto)
        {
            ServiceResponse response = await _dessertService.AddDessert(DessertDto);

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("Details", "DessertPage", new { id = response.CreatedId });
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }

        //GET DessertPage/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            DessertDto? DessertDto = await _dessertService.FindDessert(id);
            if (DessertDto == null)
            {
                return View("Error");
            }
            else
            {
                return View(DessertDto);
            }
        }

        //POST DessertPage/Update/{id}
        [HttpPost]
        public async Task<IActionResult> Update(int id, DessertDto DessertDto)
        {
            ServiceResponse response = await _dessertService.UpdateDessert(DessertDto);

            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                return RedirectToAction("Details", "DessertPage", new { id = id });
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }

        //GET DessertPage/ConfirmDelete/{id}
        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            DessertDto? DessertDto = await _dessertService.FindDessert(id);
            if (DessertDto == null)
            {
                return View("Error");
            }
            else
            {
                return View(DessertDto);
            }
        }

        //POST DessertPage/Delete/{id}
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResponse response = await _dessertService.DeleteDessert(id);
            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("List", "DessertPage");
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }
    }
}
