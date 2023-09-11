using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using System.Transactions;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;
        public PokemonController(IPokemonRepository pokemonRepository, IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;   
        }

        [HttpGet("GetPokemons")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]

        public IActionResult GetPokemons()
        {
            var pokemons = _mapper.Map<List<PokemonDTO>>(_pokemonRepository.GetPokemons());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(pokemons);
        }

        [HttpGet("{pokeId}/GetPokemonByID")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]

        public IActionResult GetPokemonByID (int pokeId) 
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
                return NotFound();

            var pokemon = _mapper.Map<PokemonDTO>(_pokemonRepository.GetPokemon(pokeId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(pokemon);
        }

        [HttpGet("{pokeId}/GetRating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]

        public IActionResult GetPokemonRating(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId)) 
                return NotFound();

            var rating = _pokemonRepository.GetPokemonAverageRating(pokeId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            return Ok(rating);
        }

        [HttpGet("{pokeId}/RatingByReviewer")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]

        public IActionResult GetPokemonAverageRatingByReviewerID(int reviewerID, int pokeId)
        {
            var rating = _pokemonRepository.GetPokemonAverageRatingByReviewerID(reviewerID, pokeId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(rating);
        }


        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult CreatePokemon([FromQuery] int ownerId, [FromQuery] int categoryId, [FromBody] PokemonDTO pokemonCreate)
        {
            if (pokemonCreate == null)
            {
                return BadRequest(ModelState);
            }

            var pokemons = _pokemonRepository.GetPokemons()
                .Where(p => p.Name.Trim().ToUpper() == pokemonCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (pokemons != null)
            {
                ModelState.AddModelError("", "Pokemon already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pokemonMap = _mapper.Map<Pokemon>(pokemonCreate);

            if (!_pokemonRepository.CreatePokemon(ownerId, categoryId, pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }

            return Ok("Succesfully Created!");
        }

    }
}