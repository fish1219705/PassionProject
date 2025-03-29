using Microsoft.AspNetCore.Mvc;
using PassionProject.Interfaces;
using PassionProject.Models.ViewModels;
using PassionProject.Models;
using PassionProject.Services;
using Microsoft.AspNetCore.Authorization;

namespace PassionProject.Controllers
{
    public class InstructionPageController : Controller
    {
        private readonly IInstructionService _instructionService;
        private readonly IIngredientService _ingredientService;
        private readonly IDessertService _dessertService;

        // dependency injection of service interface
        public InstructionPageController(IInstructionService InstructionService, IIngredientService IngredientService, IDessertService DessertService)
        {
            _instructionService = InstructionService;
            _ingredientService = IngredientService;
            _dessertService = DessertService;  
        }

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        // GET: InstructionPage/List
        public async Task<IActionResult> List()
        {
            IEnumerable<InstructionDto?> InstructionDtos = await _instructionService.ListInstructions();
            return View(InstructionDtos);
        }

        //GET InstructionPage/Edit/{id}
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            InstructionDto? InstructionDto = await _instructionService.FindInstruction(id);
            IEnumerable<IngredientDto> Ingredients = await _ingredientService.ListIngredients();
            IEnumerable<DessertDto> Desserts = await _dessertService.ListDesserts();
            if (InstructionDto == null)
            {
                return View("Error");
            }
            else
            {
                InstructionEdit InstructionInfo = new InstructionEdit()
                {
                    Instruction = InstructionDto,
                    IngredientOptions = Ingredients,
                    DessertOptions = Desserts
                };
                return View(InstructionInfo);

            }
        }

        //POST InstructionPage/Update/{id}
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(int id, InstructionDto InstructionDto)
        {
            ServiceResponse response = await _instructionService.UpdateInstruction(InstructionDto);

            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                return RedirectToAction("List", "InstructionPage", new { id = id });
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }

        // GET InstructionPage/New
        [Authorize]
        public async Task<IActionResult> New()
        {

            IEnumerable<IngredientDto?> IngredientDtos = await _ingredientService.ListIngredients();

            IEnumerable<DessertDto?> DessertDtos = await _dessertService.ListDesserts();

            InstructionNew Options = new InstructionNew()
            {
                AllDesserts = DessertDtos,
                AllIngredients = IngredientDtos
            };



            return View(Options);
        }

        // POST InstructionPage/Add
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(InstructionDto InstructionDto)
        {
            ServiceResponse response = await _instructionService.AddInstruction(InstructionDto);

            //checking if the item was added
            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                //return RedirectToAction("Details", "InstructionPage",new { id=response.CreatedId });
                return RedirectToAction("List");
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }



        }

        // GET: InstructionPage/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            InstructionDto? InstructionDto = await _instructionService.FindInstruction(id);
           

            if (InstructionDto == null)
            {
                return View("Error", new ErrorViewModel() { Errors = ["Could not find Instruction"] });
            }
            else
            {
                InstructionDetails InstructionInfo = new InstructionDetails()
                {
                    Instruction = InstructionDto
                   
                };
                return View(InstructionInfo);
            }
        }




       // //POST InstructionPage/Add
       //[HttpPost]
       // public async Task<IActionResult> Add(InstructionDto InstructionDto)
       // {
       //     ServiceResponse response = await _instructionService.AddInstruction(InstructionDto);

       //     if (response.Status == ServiceResponse.ServiceStatus.Created)
       //     {
       //         return RedirectToAction("Details", "InstructionPage", new { id = response.CreatedId });
       //     }
       //     else
       //     {
       //         return View("Error", new ErrorViewModel() { Errors = response.Messages });
       //     }
       // }





        //GET InstructionPage/ConfirmDelete/{id}
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            InstructionDto? InstructionDto = await _instructionService.FindInstruction(id);
            if (InstructionDto == null)
            {
                return View("Error");
            }
            else
            {
                return View(InstructionDto);
            }
        }

        //POST InstructionPage/Delete/{id}
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResponse response = await _instructionService.DeleteInstruction(id);

            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("List", "InstructionPage");
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }
    }
}

