using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ReviewApp.DTO;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : Controller
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
        {
            _mapper = mapper;
            _reviewerRepository = reviewerRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewers()
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var reviewers = _mapper
                .Map<List<ReviewerDTO>>(_reviewerRepository.GetAll());

            return Ok(reviewers);
        }

        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewer = _mapper
                .Map<ReviewerDTO>(_reviewerRepository.GetById(reviewerId));

            return Ok(reviewer);
        }

        [HttpGet("{reviewerId}/reviews")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetReviewsOfReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviews = _mapper
                .Map<List<ReviewDTO>>(_reviewerRepository.GetReviewsByReviewer(reviewerId));

            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult CreateReviewer([FromBody] ReviewerDTO reviewerDTO)
        {
            if (reviewerDTO is null || !ModelState.IsValid)
                return BadRequest(ModelState);

            var trimmedReviewerName = reviewerDTO.LastName.Trim() + reviewerDTO.FirstName.Trim();
            var reviewerExists = _reviewerRepository.GetAll()
                .Any(x => (x.LastName.Trim() + x.FirstName.Trim())
                .Equals(trimmedReviewerName, StringComparison.OrdinalIgnoreCase));

            if (reviewerExists)
            {
                ModelState.AddModelError("", "Reviewer already exists");
                return StatusCode(422, ModelState);
            }

            var reviewerMap = _mapper.Map<Reviewer>(reviewerDTO);

            try
            {
                if (!_reviewerRepository.CreateReviewer(reviewerMap))
                {
                    ModelState.AddModelError("", "Something went wrong while saving.");
                    return StatusCode(500, ModelState);
                }
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", $"Database Update Exception: {ex.InnerException?.Message ?? ex.Message}");
                return StatusCode(500, ModelState);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{reviewerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReviewer(int reviewerId, [FromBody] ReviewerDTO reviewer)
        {
            if (reviewer is null)
                return BadRequest(ModelState);

            if (reviewerId != reviewer.Id)
                return BadRequest(ModelState);

            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewerMap = _mapper.Map<Reviewer>(reviewer);

            if (!_reviewerRepository.UpdateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();

            var reviewerDelete = _reviewerRepository.GetById(reviewerId);

            if (!_reviewerRepository.DeleteReviewer(reviewerDelete))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
