using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ReviewApp.Controllers;
using ReviewApp.DTO;
using ReviewApp.Interfaces;
using ReviewApp.Models;
using ReviewApp.Repository;
using System.Text.Json;

namespace ReviewApp.UnitTests.ControllerTests
{
    public class ReviewControllerUnitTests
    {
        private readonly Mock<IReviewerRepository> _reviewerRepository;
        private readonly Mock<IPokemonRepository> _pokemonRepository;
        private readonly Mock<IReviewRepository> _reviewRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly ReviewController _controller;

        private static readonly ReviewDTO reviewDTO = new()
        {
            Id = 1,
            Title = "Title",
            Description = "Description",
            Rating = 5,
        };

        public ReviewControllerUnitTests()
        {
            _reviewerRepository = new Mock<IReviewerRepository>();
            _reviewRepository = new Mock<IReviewRepository>();
            _pokemonRepository = new Mock<IPokemonRepository>();
            _mapper = new Mock<IMapper>();
            _controller = new ReviewController(
                _reviewRepository.Object,
                _reviewerRepository.Object,
                _pokemonRepository.Object,
                _mapper.Object);
        }

        #region GetReviews
        [Fact]
        public void GivenNothing_WhenGetReviewsIsCalled_ThenReturnListOfReviews()
        {
            //Arrange
            List<ReviewDTO> previewDTOs = [reviewDTO];
            _reviewRepository.Setup(x => x.GetReviews()).Returns([]);
            _mapper.Setup(mapper => mapper.Map<List<ReviewDTO>>(It.IsAny<IEnumerable<Review>>()))
                .Returns(previewDTOs);

            //Act
            var result = _controller.GetReviews();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<List<ReviewDTO>>(okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(JsonSerializer.Serialize(previewDTOs), JsonSerializer.Serialize(actual));
            _reviewRepository.Verify(x => x.GetReviews(), Times.Once);
            _mapper.Verify(x => x.Map<List<ReviewDTO>>(It.IsAny<IEnumerable<Review>>()), Times.Once);
        }

        [Fact]
        public void GivenNothing_WhenGetReviewsIsCalled_ThenReturnsBadRequest()
        {
            //Arrange            
            _controller.ModelState.AddModelError("", "Model is not valid");

            //Act
            var result = _controller.GetReviews();

            //Assert
            var badRequestResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
        }
        #endregion

        #region GetReview
        [Fact]
        public void GivenId_WhenGetReviewIsCalled_ThenReturnsReview()
        {
            //Arrange
            _reviewRepository.Setup(x => x.ReviewExists(It.IsAny<int>()))
                .Returns(true);
            _reviewRepository.Setup(x => x.GetReview(It.IsAny<int>()))
                .Returns(new Review());
            _mapper.Setup(mapper => mapper.Map<ReviewDTO>(It.IsAny<Review>()))
                .Returns(reviewDTO);

            //Act
            var result = _controller.GetReview(It.IsAny<int>());

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<ReviewDTO>(okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(JsonSerializer.Serialize(reviewDTO), JsonSerializer.Serialize(actual));
            _reviewRepository.Verify(x => x.GetReview(It.IsAny<int>()), Times.Once);
            _reviewRepository.Verify(x => x.ReviewExists(It.IsAny<int>()), Times.Once);
            _mapper.Verify(x => x.Map<ReviewDTO>(It.IsAny<Review>()), Times.Once);
        }

        [Fact]
        public void GivenId_WhenGetReviewIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            _reviewRepository.Setup(x => x.ReviewExists(It.IsAny<int>())).Returns(false);

            //Act
            var result = _controller.GetReview(It.IsAny<int>());

            //Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
            _reviewRepository.Verify(x => x.ReviewExists(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GivenId_WhenGetReviewIsCalled_ThenReturnsBadRequest()
        {
            //Arrange
            _reviewRepository.Setup(x => x.ReviewExists(It.IsAny<int>()))
                .Returns(true);
            _controller.ModelState.AddModelError("", "Model state is invalid.");

            //Act
            var result = _controller.GetReview(It.IsAny<int>());

            //Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
            _reviewRepository.Verify(x => x.ReviewExists(It.IsAny<int>()), Times.Once);
        }
        #endregion

        #region GetReviewsOfPokemon
        [Fact]
        public void GivenId_WhenGetReviewsOfPokemonIsCalled_ThenReturnsPokemon()
        {
            //Arrange
            _reviewRepository.Setup(x => x.ReviewExists(It.IsAny<int>()))
                .Returns(true);
            _reviewRepository.Setup(x => x.GetReviewsOfAPokemon(It.IsAny<int>()))
                .Returns([]);
            _mapper.Setup(mapper => mapper.Map<List<ReviewDTO>>(It.IsAny<List<Review>>()))
                .Returns([]);

            //Act
            var result = _controller.GetReviewsOfPokemon(It.IsAny<int>());

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<List<ReviewDTO>>(okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
            _reviewRepository.Verify(x => x.GetReviewsOfAPokemon(It.IsAny<int>()), Times.Once);
            _reviewRepository.Verify(x => x.ReviewExists(It.IsAny<int>()), Times.Once);
            _mapper.Verify(x => x.Map<List<ReviewDTO>>(It.IsAny<List<Review>>()), Times.Once);
        }

        [Fact]
        public void GivenId_WhenGetReviewsOfPokemonIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            _reviewRepository.Setup(x => x.ReviewExists(It.IsAny<int>())).Returns(false);

            //Act
            var result = _controller.GetReviewsOfPokemon(It.IsAny<int>());

            //Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
            _reviewRepository.Verify(x => x.ReviewExists(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GivenId_WhenGetReviewsOfPokemonIsCalled_ThenReturnsBadRequest()
        {
            //Arrange
            _reviewRepository.Setup(x => x.ReviewExists(It.IsAny<int>()))
                .Returns(true);
            _controller.ModelState.AddModelError("", "Model state is invalid.");

            //Act
            var result = _controller.GetReviewsOfPokemon(It.IsAny<int>());

            //Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
            _reviewRepository.Verify(x => x.ReviewExists(It.IsAny<int>()), Times.Once);
        }
        #endregion

        #region CreateReview
        [Fact]
        public void GivenReviewIdAndPokemonIdAndReview_WhenCreateReviewIsCalled_ThenReturnsOk()
        {
            //Arrange
            _reviewerRepository.Setup(x => x.ReviewerExists(It.IsAny<int>()))
                .Returns(true);
            _pokemonRepository.Setup(x => x.PokemonExists(It.IsAny<int>()))
                .Returns(true);
            _mapper.Setup(mapper => mapper.Map<Review>(It.IsAny<ReviewDTO>()))
                .Returns(new Review());
            _reviewRepository.Setup(x => x.CreateReview(It.IsAny<Review>()))
                .Returns(true);

            //Act
            var result = _controller
                .CreateReview(It.IsAny<int>(), It.IsAny<int>(), reviewDTO);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            _reviewRepository.Verify(x => x.CreateReview(It.IsAny<Review>()), Times.Once);
            _reviewerRepository.Verify(x => x.ReviewerExists(It.IsAny<int>()), Times.Once);
            _pokemonRepository.Verify(x => x.PokemonExists(It.IsAny<int>()), Times.Once);
            _mapper.Verify(x => x.Map<Review>(It.IsAny<ReviewDTO>()), Times.Once);
        }

        [Fact]
        public void GivenReviewIdAndPokemonIdAndReview_WhenCreateReviewIsCalled_ThenReturnsBadRequestWhenPokemonIsNull()
        {
            //Arrange

            //Act
            var result = _controller.CreateReview(It.IsAny<int>(), It.IsAny<int>(), null);

            //Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public void GivenReviewIdAndPokemonIdAndReview_WhenCreateReviewIsCalled_ThenReturnsBadRequestWhenModelStateIsInvalid()
        {
            //Arrange
            _controller.ModelState.AddModelError("", "Model state is invalid.");

            //Act
            var result = _controller.CreateReview(It.IsAny<int>(), It.IsAny<int>(), reviewDTO);

            //Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
        }

        [Fact]
        public void GivenReviewIdAndPokemonIdAndReview_WhenCreateReviewIsCalled_ThenReturnsNotFoundOnReviewerDoesNotExist()
        {
            //Arrange
            _reviewerRepository.Setup(x => x.ReviewerExists(It.IsAny<int>()))
                .Returns(false);

            //Act
            var result = _controller.CreateReview(It.IsAny<int>(), It.IsAny<int>(), reviewDTO);

            //Assert
            var model = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, model.StatusCode);
            _reviewerRepository.Verify(x => x.ReviewerExists(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GivenReviewIdAndPokemonIdAndReview_WhenCreateReviewIsCalled_ThenReturnsNotFoundOnPokemonDoesNotExist()
        {
            //Arrange
            _reviewerRepository.Setup(x => x.ReviewerExists(It.IsAny<int>()))
                .Returns(true);
            _pokemonRepository.Setup(x => x.PokemonExists(It.IsAny<int>()))
                .Returns(false);

            //Act
            var result = _controller.CreateReview(It.IsAny<int>(), It.IsAny<int>(), reviewDTO);

            //Assert
            var model = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, model.StatusCode);
            _reviewerRepository.Verify(x => x.ReviewerExists(It.IsAny<int>()), Times.Once);
            _pokemonRepository.Verify(x => x.PokemonExists(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GivenReviewIdAndPokemonIdAndReview_WhenCreateReviewIsCalled_ThenReturnsServerError()
        {
            //Arrange
            _pokemonRepository.Setup(x => x.PokemonExists(It.IsAny<int>()))
                .Returns(true);
            _reviewerRepository.Setup(x => x.ReviewerExists(It.IsAny<int>()))
                .Returns(true);
            _mapper.Setup(mapper => mapper.Map<Review>(It.IsAny<ReviewDTO>()))
                .Returns(new Review());
            _reviewRepository.Setup(x => x.CreateReview(It.IsAny<Review>()))
                .Returns(false);

            //Act
            var result = _controller.CreateReview(It.IsAny<int>(), It.IsAny<int>(), reviewDTO);

            //Assert
            var model = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, model.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
            _reviewRepository.Verify(x => x.CreateReview(It.IsAny<Review>()), Times.Once);
            _reviewerRepository.Verify(x => x.ReviewerExists(It.IsAny<int>()), Times.Once);
            _pokemonRepository.Verify(x => x.PokemonExists(It.IsAny<int>()), Times.Once);
            _mapper.Verify(x => x.Map<Review>(It.IsAny<ReviewDTO>()), Times.Once);
        }
        #endregion

        #region UpdateReview
        [Fact]
        public void GivenIdAndReview_WhenUpdateReviewIsCalled_ThenReturnsNoContent()
        {
            //Arrange
            _reviewRepository.Setup(x => x.ReviewExists(It.IsAny<int>()))
                .Returns(true);
            _mapper.Setup(mapper => mapper.Map<Review>(It.IsAny<ReviewDTO>()))
                .Returns(new Review());
            _reviewRepository.Setup(x => x.UpdateReview(It.IsAny<Review>()))
                .Returns(true);

            //Act
            var result = _controller.UpdateReview(reviewDTO.Id, reviewDTO);

            //Assert
            var noContent = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContent.StatusCode);
            _reviewRepository.Verify(x => x.ReviewExists(It.IsAny<int>()), Times.Once);
            _reviewRepository.Verify(x => x.UpdateReview(It.IsAny<Review>()), Times.Once);
            _mapper.Verify(x => x.Map<Review>(It.IsAny<ReviewDTO>()), Times.Once);
        }

        [Fact]
        public void GivenIdAndReview_WhenUpdateReviewIsCalled_ThenReturnsBadRequestWhenReviewIsNull()
        {
            //Arrange

            //Act
            var result = _controller.UpdateReview(It.IsAny<int>(), null);

            //Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public void GivenIdAndReview_WhenUpdateReviewIsCalled_ThenReturnsBadRequestWhenReviewIdsDoNotMatch()
        {
            //Arrange

            //Act
            var result = _controller.UpdateReview(It.IsAny<int>(), reviewDTO);

            //Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public void GivenIdAndReview_WhenUpdateReviewIsCalled_ThenReturnsBadRequestWhenModelStateIsInvalid()
        {
            //Arrange
            _controller.ModelState.AddModelError("", "Model state is invalid.");

            //Act
            var result = _controller.UpdateReview(It.IsAny<int>(), null);

            //Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
        }

        [Fact]
        public void GivenIdAndReview_WhenUpdateReviewIsCalled_ThenReturnsReviewIsNotFound()
        {
            //Arrange
            _reviewRepository.Setup(x => x.ReviewExists(It.IsAny<int>())).Returns(false);

            //Act
            var result = _controller.UpdateReview(reviewDTO.Id, reviewDTO);

            //Assert
            var model = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, model.StatusCode);
            Assert.True(_controller.ModelState.IsValid);
        }

        [Fact]
        public void GivenPokemonIdCategoryIdOwnerIdAndPokemon_WhenUpdatePokemonIsCalled_ThenReturnsErrorWhenUpdateFails()
        {
            //Arrange
            _reviewRepository.Setup(x => x.ReviewExists(It.IsAny<int>()))
                .Returns(true);
            _mapper.Setup(mapper => mapper.Map<Review>(It.IsAny<ReviewDTO>()))
                .Returns(new Review());
            _reviewRepository.Setup(x => x.UpdateReview(It.IsAny<Review>()))
                .Returns(false);

            //Act
            var result = _controller.UpdateReview(reviewDTO.Id, reviewDTO);

            //Assert
            var noContent = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, noContent.StatusCode);
            _reviewRepository.Verify(x => x.ReviewExists(It.IsAny<int>()), Times.Once);
            _reviewRepository.Verify(x => x.UpdateReview(It.IsAny<Review>()), Times.Once);
            _mapper.Verify(x => x.Map<Review>(It.IsAny<ReviewDTO>()), Times.Once);
        }
        #endregion

        #region DeleteReview
        [Fact]
        public void GivenId_WhenDeleteReviewIsCalled_ThenReturnsNoContent()
        {
            //Arrange
            _reviewRepository.Setup(x => x.ReviewExists(It.IsAny<int>())).Returns(true);
            _reviewRepository.Setup(x => x.GetReview(It.IsAny<int>())).Returns(new Review());
            _reviewRepository.Setup(x => x.DeleteReview(It.IsAny<Review>())).Returns(true);

            //Act
            var result = _controller.DeleteReview(It.IsAny<int>());

            //Assert
            var model = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, model.StatusCode);
            Assert.IsAssignableFrom<NoContentResult>(result);
            _reviewRepository.Verify(x => x.ReviewExists(It.IsAny<int>()), Times.Once);
            _reviewRepository.Verify(x => x.GetReview(It.IsAny<int>()), Times.Once);
            _reviewRepository.Verify(x => x.DeleteReview(It.IsAny<Review>()), Times.Once);
        }

        [Fact]
        public void GivenId_WhenDeleteReviewIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            _reviewRepository.Setup(x => x.ReviewExists(It.IsAny<int>())).Returns(false);

            //Act
            var result = _controller.DeleteReview(It.IsAny<int>());

            //Assert
            var model = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, model.StatusCode);
            Assert.IsAssignableFrom<NotFoundResult>(result);
            _reviewRepository.Verify(x => x.ReviewExists(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GivenId_WhenDeletePokemonIsCalled_ThenReturnsModelErrorWhenDeleteFails()
        {
            //Arrange
            _reviewRepository.Setup(x => x.ReviewExists(It.IsAny<int>())).Returns(true);
            _reviewRepository.Setup(x => x.GetReview(It.IsAny<int>())).Returns(new Review());
            _reviewRepository.Setup(x => x.DeleteReview(It.IsAny<Review>())).Returns(false);

            //Act
            var result = _controller.DeleteReview(It.IsAny<int>());

            //Assert
            var model = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, model.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
            _reviewRepository.Verify(x => x.ReviewExists(It.IsAny<int>()), Times.Once);
            _reviewRepository.Verify(x => x.GetReview(It.IsAny<int>()), Times.Once);
            _reviewRepository.Verify(x => x.DeleteReview(It.IsAny<Review>()), Times.Once);
        }
        #endregion
    }
}
