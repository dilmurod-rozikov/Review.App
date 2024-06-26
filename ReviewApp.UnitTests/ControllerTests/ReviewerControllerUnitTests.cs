using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ReviewApp.Controllers;
using ReviewApp.DTO;
using ReviewApp.Interfaces;
using ReviewApp.Models;
using System.Text.Json;

namespace ReviewApp.UnitTests.ControllerTests
{
    public class ReviewerControllerUnitTests
    {
        private readonly Mock<IReviewerRepository> _reviewerRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly ReviewerController _controller;

        private static readonly ReviewerDTO reviewerDTO = new()
        {
            Id = 1,
            FirstName ="Test 1",
            LastName = "Test 2"
        };

        public ReviewerControllerUnitTests()
        {
            _reviewerRepository = new Mock<IReviewerRepository>();
            _mapper = new Mock<IMapper>();
            _controller = new ReviewerController(_reviewerRepository.Object, _mapper.Object);
        }

        #region GetReviewers
        [Fact]
        public void GivenNothing_WhenGetReviewersIsCalled_ThenReturnListOfReviewers()
        {
            //Arrange
            List<ReviewerDTO> previewerDTOs = [reviewerDTO];
            _reviewerRepository.Setup(x => x.GetAll()).Returns([]);
            _mapper.Setup(mapper => mapper.Map<List<ReviewerDTO>>(It.IsAny<IEnumerable<Reviewer>>()))
                .Returns(previewerDTOs);

            //Act
            var result = _controller.GetReviewers();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<List<ReviewerDTO>>(okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(JsonSerializer.Serialize(previewerDTOs), JsonSerializer.Serialize(actual));
            _reviewerRepository.Verify(x => x.GetAll(), Times.Once);
            _mapper.Verify(x => x.Map<List<ReviewerDTO>>(It.IsAny<IEnumerable<Reviewer>>()), Times.Once);
        }

        [Fact]
        public void GivenNothing_WhenGetReviewersIsCalled_ThenReturnsBadRequest()
        {
            //Arrange            
            _controller.ModelState.AddModelError("", "Model is not valid");

            //Act
            var result = _controller.GetReviewers();

            //Assert
            var badRequestResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
        }
        #endregion

        #region GetReviewer
        [Fact]
        public void GivenId_WhenGetReviewerIsCalled_ThenReturnsReview()
        {
            //Arrange
            _reviewerRepository.Setup(x => x.ReviewerExists(It.IsAny<int>()))
                .Returns(true);
            _reviewerRepository.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(new Reviewer());
            _mapper.Setup(mapper => mapper.Map<ReviewerDTO>(It.IsAny<Reviewer>()))
                .Returns(reviewerDTO);

            //Act
            var result = _controller.GetReviewer(It.IsAny<int>());

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<ReviewerDTO>(okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(JsonSerializer.Serialize(reviewerDTO), JsonSerializer.Serialize(actual));
            _reviewerRepository.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _reviewerRepository.Verify(x => x.ReviewerExists(It.IsAny<int>()), Times.Once);
            _mapper.Verify(x => x.Map<ReviewerDTO>(It.IsAny<Reviewer>()), Times.Once);
        }

        [Fact]
        public void GivenId_WhenGetReviewerIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            _reviewerRepository.Setup(x => x.ReviewerExists(It.IsAny<int>())).Returns(false);

            //Act
            var result = _controller.GetReviewer(It.IsAny<int>());

            //Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
            _reviewerRepository.Verify(x => x.ReviewerExists(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GivenId_WhenGetReviewerIsCalled_ThenReturnsBadRequest()
        {
            //Arrange
            _reviewerRepository.Setup(x => x.ReviewerExists(It.IsAny<int>()))
                .Returns(true);
            _controller.ModelState.AddModelError("", "Model state is invalid.");

            //Act
            var result = _controller.GetReviewer(It.IsAny<int>());

            //Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
            _reviewerRepository.Verify(x => x.ReviewerExists(It.IsAny<int>()), Times.Once);
        }
        #endregion

        #region GetReviewsOfReviewer
        [Fact]
        public void GivenId_WhenGetReviewsOfReviewerIsCalled_ThenReturnsReviewers()
        {
            //Arrange
            List<ReviewerDTO> reviewers = [reviewerDTO];
            _reviewerRepository.Setup(x => x.ReviewerExists(It.IsAny<int>()))
                .Returns(true);
            _reviewerRepository.Setup(x => x.GetReviewsByReviewer(It.IsAny<int>()))
                .Returns([new Review()]);
            _mapper.Setup(mapper => mapper.Map<List<ReviewDTO>>(It.IsAny<List<Review>>()))
                .Returns([]);

            //Act
            var result = _controller.GetReviewsOfReviewer(reviewerDTO.Id);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<List<ReviewDTO>>(okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
            _reviewerRepository.Verify(x => x.GetReviewsByReviewer(It.IsAny<int>()), Times.Once);
            _reviewerRepository.Verify(x => x.ReviewerExists(It.IsAny<int>()), Times.Once);
            _mapper.Verify(x => x.Map<List<ReviewDTO>>(It.IsAny<List<Review>>()), Times.Once);
        }

        [Fact]
        public void GivenId_WhenGetReviewsOfReviewerIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            _reviewerRepository.Setup(x => x.ReviewerExists(It.IsAny<int>())).Returns(false);

            //Act
            var result = _controller.GetReviewsOfReviewer(It.IsAny<int>());

            //Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
            _reviewerRepository.Verify(x => x.ReviewerExists(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GivenId_WhenGetReviewsOfReviewerIsCalled_ThenReturnsBadRequest()
        {
            //Arrange
            _reviewerRepository.Setup(x => x.ReviewerExists(It.IsAny<int>()))
                .Returns(true);
            _controller.ModelState.AddModelError("", "Model state is invalid.");

            //Act
            var result = _controller.GetReviewsOfReviewer(It.IsAny<int>());

            //Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
            _reviewerRepository.Verify(x => x.ReviewerExists(It.IsAny<int>()), Times.Once);
        }
        #endregion

        #region CreateReviewer
        [Fact]
        public void GivenReviewerModel_WhenCreateReviewerIsCalled_ThenReturnsOk()
        {
            //Arrange
            _reviewerRepository.Setup(x => x.GetAll())
                .Returns([]);
            _mapper.Setup(mapper => mapper.Map<Reviewer>(It.IsAny<ReviewerDTO>()))
                .Returns(new Reviewer());
            _reviewerRepository.Setup(x => x.CreateReviewer(It.IsAny<Reviewer>()))
                .Returns(true);

            //Act
            var result = _controller
                .CreateReviewer(reviewerDTO);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            _reviewerRepository.Verify(x => x.CreateReviewer(It.IsAny<Reviewer>()), Times.Once);
            _reviewerRepository.Verify(x => x.GetAll(), Times.Once);
            _mapper.Verify(x => x.Map<Reviewer>(It.IsAny<ReviewerDTO>()), Times.Once);
        }

        [Fact]
        public void GivenReviewerModel_WhenCreateReviewerIsCalled_ThenReturnsBadRequestOnReviewerIsNull()
        {
            //Arrange

            //Act
            var result = _controller.CreateReviewer(null);

            //Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public void GivenReviewModel_WhenCreateReviewerIsCalled_ThenReturnsBadRequestWhenOnStateIsInvalid()
        {
            //Arrange
            _controller.ModelState.AddModelError("", "Model state is invalid.");

            //Act
            var result = _controller.CreateReviewer(reviewerDTO);

            //Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
        }

        [Fact]
        public void GivenReviewIdAndPokemonIdAndReview_WhenCreateReviewIsCalled_ThenReturnsServerError()
        {
            //Arrange
            List<Reviewer> list = [new() {Id = 1, FirstName = "Name", LastName = "surname"}];
            _reviewerRepository.Setup(x => x.GetAll())
                .Returns(list);
            _mapper.Setup(mapper => mapper.Map<Reviewer>(It.IsAny<ReviewerDTO>()))
                .Returns(new Reviewer());
            _reviewerRepository.Setup(x => x.CreateReviewer(It.IsAny<Reviewer>()))
                .Returns(false);

            //Act
            var result = _controller.CreateReviewer(reviewerDTO);

            //Assert
            var model = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, model.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
            _reviewerRepository.Verify(x => x.GetAll(), Times.Once);
            _reviewerRepository.Verify(x => x.CreateReviewer(It.IsAny<Reviewer>()), Times.Once);
            _mapper.Verify(x => x.Map<Reviewer>(It.IsAny<ReviewerDTO>()), Times.Once);
        }
        #endregion

        #region UpdateReviewer
        [Fact]
        public void GivenIdAndReviewer_WhenUpdateReviewerIsCalled_ThenReturnsNoContent()
        {
            //Arrange
            _reviewerRepository.Setup(x => x.ReviewerExists(It.IsAny<int>()))
                .Returns(true);
            _mapper.Setup(mapper => mapper.Map<Reviewer>(It.IsAny<ReviewerDTO>()))
                .Returns(new Reviewer());
            _reviewerRepository.Setup(x => x.UpdateReviewer(It.IsAny<Reviewer>()))
                .Returns(true);

            //Act
            var result = _controller.UpdateReviewer(reviewerDTO.Id, reviewerDTO);

            //Assert
            var noContent = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContent.StatusCode);
            _reviewerRepository.Verify(x => x.ReviewerExists(It.IsAny<int>()), Times.Once);
            _reviewerRepository.Verify(x => x.UpdateReviewer(It.IsAny<Reviewer>()), Times.Once);
            _mapper.Verify(x => x.Map<Reviewer>(It.IsAny<ReviewerDTO>()), Times.Once);
        }

        [Fact]
        public void GivenIdAndReviewer_WhenUpdateReviewerIsCalled_ThenReturnsBadRequestWhenReviewerIsNull()
        {
            //Arrange

            //Act
            var result = _controller.UpdateReviewer(It.IsAny<int>(), null);

            //Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public void GivenIdAndReviewer_WhenUpdateReviewerIsCalled_ThenReturnsBadRequestWhenReviewerIdsDoNotMatch()
        {
            //Arrange

            //Act
            var result = _controller.UpdateReviewer(It.IsAny<int>(), reviewerDTO);

            //Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public void GivenIdAndReviewer_WhenUpdateReviewerIsCalled_ThenReturnsBadRequestWhenModelStateIsInvalid()
        {
            //Arrange
            _controller.ModelState.AddModelError("", "Model state is invalid.");

            //Act
            var result = _controller.UpdateReviewer(It.IsAny<int>(), null);

            //Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
        }

        [Fact]
        public void GivenIdAndReview_WhenUpdateReviewIsCalled_ThenReturnsReviewIsNotFound()
        {
            //Arrange
            _reviewerRepository.Setup(x => x.ReviewerExists(It.IsAny<int>())).Returns(false);

            //Act
            var result = _controller.UpdateReviewer(reviewerDTO.Id, reviewerDTO);

            //Assert
            var model = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, model.StatusCode);
            Assert.True(_controller.ModelState.IsValid);
        }

        [Fact]
        public void GivenPokemonIdCategoryIdOwnerIdAndPokemon_WhenUpdatePokemonIsCalled_ThenReturnsErrorWhenUpdateFails()
        {
            //Arrange
            _reviewerRepository.Setup(x => x.ReviewerExists(It.IsAny<int>()))
                .Returns(true);
            _mapper.Setup(mapper => mapper.Map<Review>(It.IsAny<ReviewDTO>()))
                .Returns(new Review());
            _reviewerRepository.Setup(x => x.UpdateReviewer(It.IsAny<Reviewer>()))
                .Returns(false);

            //Act
            var result = _controller.UpdateReviewer(reviewerDTO.Id, reviewerDTO);

            //Assert
            var noContent = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, noContent.StatusCode);
            _reviewerRepository.Verify(x => x.ReviewerExists(It.IsAny<int>()), Times.Once);
            _reviewerRepository.Verify(x => x.UpdateReviewer(It.IsAny<Reviewer>()), Times.Once);
            _mapper.Verify(x => x.Map<Reviewer>(It.IsAny<ReviewerDTO>()), Times.Once);
        }
        #endregion

        #region DeleteReviewer
        [Fact]
        public void GivenId_WhenDeleteReviewerIsCalled_ThenReturnsNoContent()
        {
            //Arrange
            _reviewerRepository.Setup(x => x.ReviewerExists(It.IsAny<int>())).Returns(true);
            _reviewerRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Reviewer());
            _reviewerRepository.Setup(x => x.DeleteReviewer(It.IsAny<Reviewer>())).Returns(true);

            //Act
            var result = _controller.DeleteReviewer(It.IsAny<int>());

            //Assert
            var model = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, model.StatusCode);
            Assert.IsAssignableFrom<NoContentResult>(result);
            _reviewerRepository.Verify(x => x.ReviewerExists(It.IsAny<int>()), Times.Once);
            _reviewerRepository.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _reviewerRepository.Verify(x => x.DeleteReviewer(It.IsAny<Reviewer>()), Times.Once);
        }

        [Fact]
        public void GivenId_WhenDeleteReviewIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            _reviewerRepository.Setup(x => x.ReviewerExists(It.IsAny<int>())).Returns(false);

            //Act
            var result = _controller.DeleteReviewer(It.IsAny<int>());

            //Assert
            var model = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, model.StatusCode);
            Assert.IsAssignableFrom<NotFoundResult>(result);
            _reviewerRepository.Verify(x => x.ReviewerExists(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GivenId_WhenDeletePokemonIsCalled_ThenReturnsModelErrorWhenDeleteFails()
        {
            //Arrange
            _reviewerRepository.Setup(x => x.ReviewerExists(It.IsAny<int>())).Returns(true);
            _reviewerRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Reviewer());
            _reviewerRepository.Setup(x => x.DeleteReviewer(It.IsAny<Reviewer>())).Returns(false);

            //Act
            var result = _controller.DeleteReviewer(It.IsAny<int>());

            //Assert
            var model = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, model.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
            _reviewerRepository.Verify(x => x.ReviewerExists(It.IsAny<int>()), Times.Once);
            _reviewerRepository.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _reviewerRepository.Verify(x => x.DeleteReviewer(It.IsAny<Reviewer>()), Times.Once);
        }
        #endregion
    }
}
