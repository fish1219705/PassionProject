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
        /// Returns a list of Reviews, each represented by a ReviewDto with its asscoiated Dessert
        /// </summary>
        /// <returns>
        /// 200 OK
        /// [{ReviewDto}, {ReviewDto}, ...]
        /// </returns>
        /// <example>
        /// GET: api/Review/List -> [{ReviewDto}, {ReviewDto}, ...]
        /// </example>
        [HttpGet(template: "List")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> ListReviews()
        {
            // returns a list of review dtos
            IEnumerable<ReviewDto> reviewDtos = await _reviewService.ListReviews();
            // return 200 OK with ReviewDtos
            return Ok(reviewDtos);
        }


        /// <summary>
        /// Returns a single Review specified by its {id}, represented by a Review Dto with its associated Dessert
        /// </summary>
        /// <param name="id">The review id</param>
        /// <returns>
        /// 200 OK
        /// {ReviewDto}
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// GET: api/Review/Find/1 -> {ReviewDto}
        /// </example>
        [HttpGet(template: "Find/{id}")]
        public async Task<ActionResult<ReviewDto>> FindReview(int id)
        {

            var review = await _reviewService.FindReview(id);

            // if the review could not be located, return 404 Not Found
            if (review == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(review);
            }
        }

        /// <summary>
        /// Updates a review
        /// </summary>
        /// <param name="id">The ID of Review to update</param>
        /// <param name="reviewDto">The required information to update the review (ReviewId, ReviewNumber, ReviewContent, ReviewTime, OrderId)</param>
        /// <returns>
        /// 400 Bad Request
        /// or
        /// 404 Not Found
        /// or
        /// 204 No Content
        /// </returns>
        [HttpPut(template: "Update/{id}")]
        public async Task<ActionResult> UpdateReview(int id, ReviewDto reviewDto)
        {
            // {id} in URL must match ReviewId in POST Body
            if (id != reviewDto.ReviewId)
            {
                //404 Bad Request
                return BadRequest();
            }

            ServiceResponse response = await _reviewService.UpdateReview(reviewDto);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages);

            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            //Status = Updated
            return NoContent();
        
        }


        /// <summary>
        ///  Adds an Review
        /// </summary>
        /// <param name="reviewDto">The required information to add the review (ReviewNumber, ReviewContent, ReviewTime, OrderId)</param>
        /// <returns>
        /// 201 Created
        /// Location: api/Review/Find/{ReviewId}
        /// {ReviewDto}
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// POST api/Review/Add
        /// </example>
        [HttpPost(template: "Add")]
        public async Task<ActionResult<Review>> AddReview(ReviewDto reviewDto)
        {
            ServiceResponse response = await _reviewService.AddReview(reviewDto);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages);
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            // returns 201 Created with Location
            return Created($"api/Review/FindReview/{response.CreatedId}", reviewDto);
        }

        /// <summary>
        /// Deletes the Review
        /// </summary>
        /// <param name="id">The id of the Review to delete</param>
        /// <returns>
        /// 201 No Content
        /// or
        /// 404 Not Found
        /// </returns>
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

        //ListReviewForDessert
        [HttpGet(template: "ListForDessert/{id}")]
        public async Task<IActionResult> ListReviewsForDessert(int id)
        {
            // empty list of data transfer object ReviewDto
            IEnumerable<ReviewDto> reviewDtos = await _reviewService.ListReviewsForDessert(id);
            // return 200 OK with ReviewDtos
            return Ok(reviewDtos);
        }

    }

    

}
