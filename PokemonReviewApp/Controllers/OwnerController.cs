using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;

        public OwnerController(IOwnerRepository ownerRepository, IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Country>))]

        public IActionResult GetOwners()
        {
            var owners = _mapper.Map<List<OwnerDTO>>(_ownerRepository.GetOwners());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(owners);
        }

        [HttpGet("{ownerId}/owners")]
        [ProducesResponseType(200, Type = typeof(ICollection<Country>))]
        [ProducesResponseType(400)]

        public IActionResult GetOwner(int ownerId)
        {
            if (!_ownerRepository.OwnerExists(ownerId))
            {
                return NotFound();
            }

            var owner = _mapper.Map<OwnerDTO>(_ownerRepository.GetOwner(ownerId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(owner);
        }


        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]

        public IActionResult CreateOwner([FromBody] OwnerDTO ownerCreate, int pokemonId, int countryId)
        {
            if (ownerCreate == null)
            {
                return BadRequest(ModelState);
            }

            var owners = _ownerRepository.GetOwners()
                .Where(o => o.FirstName.Trim().ToUpper() == ownerCreate.FirstName.TrimEnd().ToUpper() &&
                o.LastName.Trim().ToUpper() == ownerCreate.LastName.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (owners != null)
            {
                ModelState.AddModelError("", "Owner already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ownerMap = _mapper.Map<Owner>(ownerCreate);

            if(!_ownerRepository.CreateOwner(pokemonId, countryId, ownerMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created!");
            
        }

        [HttpPut("{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public IActionResult UpdateOwner(int ownerId, [FromBody] OwnerDTO ownerUpdated)
        {
            if (ownerUpdated == null)
            {
                return BadRequest(ModelState);
            }

            if (ownerId != ownerUpdated.Id)
            {
                return BadRequest(ModelState);
            }

            if (!_ownerRepository.OwnerExists(ownerId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ownerMap = _mapper.Map<Owner>(ownerUpdated);

            if (!_ownerRepository.UpdateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something went wrong updating owner.");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
