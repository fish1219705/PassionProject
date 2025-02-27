using PassionProject.Interfaces;
using PassionProject.Models;
using PassionProject.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System;
using System.IO;
using NuGet.Packaging.Signing;


namespace PassionProject.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;
        // dependency injection of database context
        public ReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ReviewDto>> ListReviews()
        {
            // include will join the (r)eview with 1 dessert
            List<Review> Reviews = await _context.Reviews
                .Include(r => r.Dessert)
                .ToListAsync();
            // empty list of data transfer object ReviewDto
            List<ReviewDto> ReviewDtos = new List<ReviewDto>();
            // foreach Review record in database
            foreach (Review Review in Reviews)
            {
                ReviewDto ReviewDto = new ReviewDto()
                {
                    ReviewId = Review.ReviewId,
                    ReviewNumber = Review.ReviewNumber,
                    ReviewContent = Review.ReviewContent,
                    ReviewTime = Review.ReviewTime,
                    ReviewUser = Review.ReviewUser,
                    DessertId = Review.DessertId,
                    DessertName = Review.Dessert.DessertName,
                    HasReviewPic = Review.HasPic
                };

                if (Review.HasPic)
                {
                    ReviewDto.ReviewImagePath = $"/images/reviews/{Review.ReviewId}{Review.PicExtension}";
                }
                // create new instance of ReviewDto, add to list
                ReviewDtos.Add(ReviewDto);
            }
            // return ReviewDtos
            return ReviewDtos;

        }

        public async Task<ReviewDto?> FindReview(int id)
        {
            // include will join (r)eview with 1 dessert
            // first or default async will get the first (r)eview matching the {id}
            var review = await _context.Reviews
                .Include(i => i.Dessert)
                .FirstOrDefaultAsync(r => r.ReviewId == id);

            // no review found
            if (review == null)
            {
                return null;
            }
            // create an instance of reviewDto
            ReviewDto reviewDto = new ReviewDto()
            {
                ReviewId = review.ReviewId,
                ReviewNumber = review.ReviewNumber,
                ReviewContent = review.ReviewContent,
                ReviewTime = review.ReviewTime,
                ReviewUser = review.ReviewUser,
                HasReviewPic = review.HasPic,
                DessertId = review.DessertId,
                DessertName = review.Dessert.DessertName
            };
            if (review.HasPic)
            {
                reviewDto.ReviewImagePath = $"/images/reviews/{review.ReviewId}{review.PicExtension}";
            }
            return reviewDto;

        }

        public async Task<ServiceResponse> UpdateReview(ReviewDto reviewDto)
        {
            ServiceResponse serviceResponse = new();

            Dessert? dessert = await _context.Desserts.FindAsync(reviewDto.DessertId);
            Review? review = await _context.Reviews.FindAsync(reviewDto.ReviewId);

            // Posted data must link to valid entity
            if (dessert == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                //404 Not Found
                return serviceResponse;
            }

            // Create instance of Review
            if (review == null)
            {
                serviceResponse.Messages.Add("Review could not be found");
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                return serviceResponse;
            }

            review.ReviewNumber = reviewDto.ReviewNumber;
            review.ReviewContent = reviewDto.ReviewContent;
            review.ReviewTime = reviewDto.ReviewTime;
            review.ReviewUser = reviewDto.ReviewUser;
            review.Dessert = dessert;
            review.DessertId = reviewDto.DessertId;


            //Review review = new Review()
            //{

            //    ReviewNumber =
            //    ReviewContent = reviewDto.ReviewContent,
            //    ReviewTime = reviewDto.ReviewTime,
            //    ReviewUser = reviewDto.ReviewUser,
            //    Dessert = dessert;
            //    DessertId = reviewDto.DessertId

            //};

            // flags that the object has changed
            _context.Entry(review).State = EntityState.Modified;
            // handled by another method
            _context.Entry(review).Property(r => r.HasPic).IsModified = false;
            _context.Entry(review).Property(r => r.PicExtension).IsModified = false;


            try
            {
                //SQL Equivalent: Update Reviews set ... where ReviewId={id}
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("An error occured updating the record");
                return serviceResponse;
            }
            serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;

            return serviceResponse;
        }

        public async Task<ServiceResponse> AddReview(ReviewDto reviewDto)
        {
            ServiceResponse serviceResponse = new();
            Dessert? dessert = await _context.Desserts.FindAsync(reviewDto.DessertId);

            // Data must link to a valid entity
            if (dessert == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                if (dessert == null)
                {
                    serviceResponse.Messages.Add("Dessert was not found. ");
                }

                return serviceResponse;
            }

                Review review = new Review()
                {
                    ReviewNumber = reviewDto.ReviewNumber,
                    ReviewContent = reviewDto.ReviewContent,
                    ReviewTime = reviewDto.ReviewTime,
                    ReviewUser = reviewDto.ReviewUser,
                    Dessert = dessert,
                    DessertId = reviewDto.DessertId,
                    

                };
                // SQL Equivalent: Insert into reviews (..) values (..)

                try
                {
                    _context.Reviews.Add(review);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                    serviceResponse.Messages.Add("There was an error adding the Review.");
                    serviceResponse.Messages.Add(ex.Message);

                }


                serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
                serviceResponse.CreatedId = review.ReviewId;
                return serviceResponse;
            }



            public async Task<ServiceResponse> DeleteReview(int id)
            {
                ServiceResponse response = new();
                // Review must exist in the first place
                var review = await _context.Reviews.FindAsync(id);
                if (review == null)
                {
                    response.Status = ServiceResponse.ServiceStatus.NotFound;
                    response.Messages.Add("Review cannot be deleted because it does not exist.");
                    return response;
                }

                try
                {
                    _context.Reviews.Remove(review);
                    await _context.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    response.Status = ServiceResponse.ServiceStatus.Error;
                    response.Messages.Add("Error encountered while deleting the review");
                    return response;
                }

                response.Status = ServiceResponse.ServiceStatus.Deleted;

                return response;

            }

            public async Task<IEnumerable<ReviewDto>> ListReviewsForDessert(int id)
            {
                // WHERE dessertid == id
                List<Review> reviews = await _context.Reviews
                    .Include(r => r.Dessert)
                    .Where(r => r.DessertId == id)
                    .ToListAsync();

                // empty list of data transfer object ReviewDto
                List<ReviewDto> reviewDtos = new List<ReviewDto>();
                // foreach Review record in database
                foreach (Review review in reviews)
                {
                    // create new instance of ReviewDto, add to list
                    reviewDtos.Add(new ReviewDto()
                    {
                        ReviewId = review.ReviewId,
                        ReviewNumber = review.ReviewNumber,
                        ReviewContent = review.ReviewContent,
                        ReviewTime = review.ReviewTime,
                        ReviewUser = review.ReviewUser,
                        DessertId = review.DessertId,
                        DessertName = review.Dessert.DessertName
                    });
                }
                // return ReviewDtos
                return reviewDtos;

            }

        public async Task<ServiceResponse> UpdateReviewImage(int id, IFormFile ReviewPic)
        {
            ServiceResponse response = new();

            Review? Review = await _context.Reviews.FindAsync(id);
            if (Review == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add($"Review {id} not found");
                return response;
            }

            if (ReviewPic.Length > 0)
            {


                // remove old picture if exists
                if (Review.HasPic)
                {
                    string OldFileName = $"{Review.ReviewId}{Review.PicExtension}";
                    string OldFilePath = Path.Combine("wwwroot/images/reviews/", OldFileName);
                    if (File.Exists(OldFilePath))
                    {
                        File.Delete(OldFilePath);
                    }

                }


                // establish valid file types (can be changed to other file extensions if desired!)
                List<string> Extensions = new List<string> { ".jpeg", ".jpg", ".png", ".gif" };
                string ReviewPicExtension = Path.GetExtension(ReviewPic.FileName).ToLowerInvariant();
                if (!Extensions.Contains(ReviewPicExtension))
                {
                    response.Messages.Add($"{ReviewPicExtension} is not a valid file extension");
                    response.Status = ServiceResponse.ServiceStatus.Error;
                    return response;
                }

                string FileName = $"{id}{ReviewPicExtension}";
                string FilePath = Path.Combine("wwwroot/images/reviews/", FileName);

                using (var targetStream = File.Create(FilePath))
                {
                    ReviewPic.CopyTo(targetStream);
                }

                // check if file was uploaded
                if (File.Exists(FilePath))
                {
                    Review.PicExtension = ReviewPicExtension;
                    Review.HasPic = true;

                    _context.Entry(Review).State = EntityState.Modified;

                    try
                    {
                        // SQL Equivalent: Update Reviews set HasPic=True, PicExtension={ext} where ReviewId={id}

                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        response.Status = ServiceResponse.ServiceStatus.Error;
                        response.Messages.Add("An error occurred updating the record");

                        return response;
                    }
                }

            }
            else
            {
                response.Messages.Add("No File Content");
                response.Status = ServiceResponse.ServiceStatus.Error;
                return response;
            }

            response.Status = ServiceResponse.ServiceStatus.Updated;



            return response;
        }

    }
    }

