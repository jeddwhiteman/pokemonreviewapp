using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public ReviewController(IReviewRepository reviewRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Review>))]

        public IActionResult GetReviews()
        {
            var reviews = _mapper.Map<List<ReviewDTO>>(_reviewRepository.GetReviews());

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(reviews);
        }

        [HttpGet("{reviewId}/reviews")]
        [ProducesResponseType(200, Type = typeof(ICollection<Review>))]
        [ProducesResponseType(400)]

        public IActionResult GetReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }
            var review = _mapper.Map<ReviewDTO>(_reviewRepository.GetReview(reviewId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(review);
        }

        [HttpGet("pokemon/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(ICollection<Review>))]
        [ProducesResponseType(400)]

        public IActionResult GetReviewsForAPokemon(int pokeId)
        {
            var reviews = _mapper.Map<List<ReviewDTO>>(_reviewRepository.GetReviewsOfAPokemon(pokeId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]

        public IActionResult CreateReview([FromBody] ReviewDTO reviewCreate, [FromQuery] int reviewerId, [FromQuery] int pokemonId)
        {


            var reviews = _reviewRepository.GetReviews()
                .Where(r => r.Title.Trim().ToUpper() == reviewCreate.Title.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (reviewCreate == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reviewMap = _mapper.Map<Review>(reviewCreate);

            if(!_reviewRepository.CreateReview(reviewerId, pokemonId, reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created!");
        }

    }
}
