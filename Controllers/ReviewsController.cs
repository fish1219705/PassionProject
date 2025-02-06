using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PassionProject.Data;
using PassionProject.Models;

namespace PassionProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Reviews
        [HttpGet(template:"List")]
        public async Task<ActionResult<IEnumerable<Review>>> ListReviews()
        {
            return await _context.Reviews.ToListAsync();
        }

        // GET: api/Reviews/5
        [HttpGet(template:"Find/{id}")]
        public async Task<ActionResult<Review>> FindReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            return review;
        }

        // PUT: api/Reviews/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut(template:"Update/{id}")]
        public async Task<IActionResult> UpdateReview(int id, Review review)
        {
            if (id != review.ReviewId)
            {
                return BadRequest();
            }

            _context.Entry(review).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Reviews
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost(template:"Add")]
        public async Task<ActionResult<Review>> AddReview(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReview", new { id = review.ReviewId }, review);
        }

        // DELETE: api/Reviews/5
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.ReviewId == id);
        }

        [HttpGet(template: "ListForDessert/{id}")]
        public async Task<IActionResult> ListReviewsForDessert(int id)
        {

        }
    }
}
