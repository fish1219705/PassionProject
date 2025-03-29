using Microsoft.AspNetCore.Mvc;
using PassionProject.Models;
using PassionProject.Models.ViewModels;
using PassionProject.Interfaces;
using PassionProject.Services;
using Microsoft.AspNetCore.Authorization; 

namespace PassionProject.Controllers
{
    public class ReviewPageController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly IDessertService _dessertService;
 
        // dependency injection of service interface

        public ReviewPageController(IReviewService ReviewService, IDessertService DessertService)
        {
            _reviewService = ReviewService;
            _dessertService = DessertService;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: ReviewPage/List
        public async Task<IActionResult> List()
        {
            IEnumerable<ReviewDto?> ReviewDtos = await _reviewService.ListReviews();
            return View(ReviewDtos);
        }

        // GET: ReviewPage/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            ReviewDto? ReviewDto = await _reviewService.FindReview(id);
            

            if (ReviewDto == null)
            {
                return View("Error", new ErrorViewModel() { Errors = ["Could not find Review"] });
            }
            else
            {
                // information which drives a Review page
                ReviewDetails ReviewInfo = new ReviewDetails()
                {
                    Review = ReviewDto
                };
                return View(ReviewInfo);
            }
        }

        // GET ReviewPage/New
        [Authorize]
        public ActionResult New()
        {
            return View();
        }


        // POST ReviewPage/Add
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(ReviewDto ReviewDto)
        {
            ServiceResponse response = await _reviewService.AddReview(ReviewDto);

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("Details", "ReviewPage", new { id = response.CreatedId });
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }

        //GET ReviewPage/Edit/{id}
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            ReviewDto? ReviewDto = await _reviewService.FindReview(id);
            if (ReviewDto == null)
            {
                return View("Error");
            }
            else
            {
                return View(ReviewDto);
            }
        }

        //POST ReviewPage/Update/{id}
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(int id, ReviewDto ReviewDto, IFormFile ReviewPic)
        {
            //Confirm receipt of review image
            System.Diagnostics.Debug.WriteLine("Review Image "+ReviewPic.Length);
            ServiceResponse imageresponse = await _reviewService.UpdateReviewImage(id, ReviewPic);

            ServiceResponse response = await _reviewService.UpdateReview(ReviewDto);

            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                return RedirectToAction("Details", "ReviewPage", new { id = id });
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }

        //GET ReviewPage/ConfirmDelete/{id}
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            // Views/ReviewPage/ConfirmDelete.cshtml
            // find information about this review
            ReviewDto? ReviewDto = await _reviewService.FindReview(id);

            return View(ReviewDto);
        }

        //POST ReviewPage/Delete/{id}
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResponse response = await _reviewService.DeleteReview(id);

            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("List", "ReviewPage");
            }
            else
            {
                return RedirectToAction("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }
    }
}

