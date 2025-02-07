using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PassionProject.Data;
using PassionProject.Interfaces;
using PassionProject.Models;
using PassionProject.Services;

namespace PassionProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        // dependency injection of service interfaces
        public ReviewController(IReviewService ReviewService)
        {
            _reviewService = ReviewService;
        }

        /// <summary>
        /// Returns a list of Desserts, each represented by a DessertDto with their asscoiated Ingredients and Reviews
        /// </summary>
        /// <returns>
        /// 200 OK
        /// [{DeesertDto}, {DessertDto}, ...]
        /// </returns>
        /// <example>
        /// GET: api/Desserts/List -> [{DessertDto}, {DessertDto}, ...]
        /// </example>
        [HttpGet(template: "List")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> ListReviews()
        {
            IEnumerable<ReviewDto> ReviewDtos = await _reviewService.ListReviews();

            return Ok(ReviewDtos);
        }


        /// <summary>
        /// Returns a single Dessert specified by its {id}
        /// </summary>
        /// <param name="id">The dessert id</param>
        /// <returns>
        /// 200 OK
        /// {DessertDto}
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// GET: api/Desserts/Find/1 -> {DessertDto}
        /// </example>

        [HttpGet(template: "Find/{id}")]
        public async Task<ActionResult<ReviewDto>> FindReview(int id)
        {

            var review = await _reviewService.FindReview(id);

            // if the dessert could not be located, return 404 Not Found
            if (review == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(review);
            }
        }

        // PUT: api/Desserts/5
       
        [HttpPut(template: "Update/{id}")]
        public async Task<ActionResult> UpdateReview(int id, ReviewDto ReviewDto)
        {
            // {id} in URL must match DessertId in POST Body
            if (id != ReviewDto.ReviewId)
            {
                //404 Bad Request
                return BadRequest();
            }

            ServiceResponse response = await _reviewService.UpdateReview(ReviewDto);
            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages);

            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }
            return Created($"api/Review/FindReview/{response.CreatedId}", ReviewDto);
        }

        // POST: api/Desserts

        [HttpPost(template: "Add")]
        public async Task<ActionResult<Review>> AddReview(ReviewDto ReviewDto)
        {
            ServiceResponse response = await _reviewService.AddReview(ReviewDto);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages);
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            // returns 201 Created with Location
            return Created($"api/Review/FindReview/{response.CreatedId}", ReviewDto);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> DeleteReview(int id)
        {
            ServiceResponse response = await _reviewService.DeleteReview(id);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound();
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            return NoContent();

        }


        [HttpGet(template: "ListForDessert/{id}")]
        public async Task<IActionResult> ListReviewsForDessert(int id)
        {
            // empty list of data transfer object ReviewDto
            IEnumerable<ReviewDto> ReviewDtos = await _reviewService.ListReviewsForDessert(id);
            // return 200 OK with ReviewDtos
            return Ok(ReviewDtos);
        }

    }

    

}
