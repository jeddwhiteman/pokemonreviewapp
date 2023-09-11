using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;
using Microsoft.OpenApi.Validations;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : Controller
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
        {
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Reviewer>))]

        public IActionResult GetReviewers()
        {
            var reviewers = _mapper.Map<List<ReviewerDTO>>(_reviewerRepository.GetReviewers());

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(reviewers);
        }

        [HttpGet("{reviewerId}/reviewers")]
        [ProducesResponseType(200, Type = typeof(ICollection<Review>))]
        [ProducesResponseType(400)]

        public IActionResult GetReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
            {
                return NotFound();
            }
            var review = _mapper.Map<ReviewerDTO>(_reviewerRepository.GetReviewer(reviewerId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(review);
        }

        [HttpGet("{reviewerId}/reviews")]
        [ProducesResponseType(200, Type = typeof(ICollection<Review>))]
        [ProducesResponseType(400)]

        public IActionResult GetReviewsByReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
            {
                return NotFound();
            }
            var reviews = _mapper.Map<List<ReviewDTO>>(_reviewerRepository.GetReviewsByReviewer(reviewerId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult CreateReviewer([FromBody] ReviewerDTO reviewerCreate)
        {
            if (reviewerCreate == null)
            {
                return BadRequest(ModelState);
            }

            var reviewer = _reviewerRepository.GetReviewers()
                .Where(r => r.FirstName.Trim().ToUpper() == reviewerCreate.FirstName.Trim().ToUpper() &&
                        r.LastName.Trim().ToUpper() == reviewerCreate.LastName.Trim().ToUpper())
                .FirstOrDefault();

            if(reviewer != null)
            {
                ModelState.AddModelError("", "Reviewer already exists!");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reviewerMap = _mapper.Map<Reviewer>(reviewerCreate);

            if (!_reviewerRepository.CreateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Reviewer Added!");
        }
    }
}
