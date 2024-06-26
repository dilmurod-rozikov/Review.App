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
    public class CountryControllerUnitTests
    {
        private readonly Mock<ICountryRepository> _countryRepository;
        private readonly Mock<IOwnerRepository> _ownerRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly CountryController _controller;
        private static readonly CountryDTO countryDTO = new()
        {
            Id = 1,
            Name = "Test",
        };
        public CountryControllerUnitTests()
        {
            _countryRepository = new Mock<ICountryRepository>();
            _ownerRepository = new Mock<IOwnerRepository>();
            _mapper = new Mock<IMapper>();
            _controller = new CountryController(_countryRepository.Object, _ownerRepository.Object, _mapper.Object);
        }

        #region GetCountries
        [Fact]
        public void GivenNothing_WhenGetCountriesIsCalled_ThenReturnListOfCountries()
        {
            //Arrange
            List<CountryDTO> countryDTOs = [countryDTO];
            _countryRepository.Setup(x => x.GetCountries())
                .Returns([]);
            _mapper.Setup(mapper => mapper.Map<List<CountryDTO>>(It.IsAny<List<Country>>()))
                .Returns(countryDTOs);

            //Act
            var result = _controller.GetCountries();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualCategories = Assert.IsType<List<CountryDTO>>(okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(JsonSerializer.Serialize(countryDTOs), JsonSerializer.Serialize(actualCategories));
            _countryRepository.Verify(x => x.GetCountries(), Times.Once);
            _mapper.Verify(x => x.Map<List<CountryDTO>>(It.IsAny<List<Country>>()), Times.Once);
        }

        [Fact]
        public void GivenNothing_WhenGetCountriesIsCalled_ThenReturnsBadRequest()
        {
            //Arrange
            _controller.ModelState.AddModelError("", "Model is not valid");

            //Act
            var result = _controller.GetCountries();

            //Assert
            var badRequestResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
        }
        #endregion

        #region GetCountry
        [Fact]
        public void GivenId_WhenGetCountryIsCalled_ThenReturnsCountry()
        {
            //Arrange
            _countryRepository.Setup(x => x.CountryExists(It.IsAny<int>()))
                .Returns(true);
            _countryRepository.Setup(x => x.GetCountry(It.IsAny<int>()))
                .Returns(new Country());
            _mapper.Setup(mapper => mapper.Map<CountryDTO>(It.IsAny<Country>()))
                .Returns(countryDTO);

            //Act
            var result = _controller.GetCountry(countryDTO.Id);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<CountryDTO>(okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(JsonSerializer.Serialize(countryDTO), JsonSerializer.Serialize(actual));
            _countryRepository.Verify(x => x.GetCountry(It.IsAny<int>()), Times.Once);
            _countryRepository.Verify(x => x.CountryExists(It.IsAny<int>()), Times.Once);
            _mapper.Verify(x => x.Map<CountryDTO>(It.IsAny<Country>()), Times.Once);
        }

        [Fact]
        public void GivenId_WhenGetCountryIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            _countryRepository.Setup(x => x.CountryExists(It.IsAny<int>())).Returns(false);

            //Act
            var result = _controller.GetCountry(It.IsAny<int>());

            //Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.NotNull(result);
            _countryRepository.Verify(x => x.CountryExists(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GivenId_WhenGetCountryIsCalled_ThenReturnsBadRequest()
        {
            //Arrange
            _countryRepository.Setup(x => x.CountryExists(It.IsAny<int>()))
                .Returns(true);
            _controller.ModelState.AddModelError("", "Model state is invalid.");

            //Act
            var result = _controller.GetCountry(It.IsAny<int>());

            //Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
            _countryRepository.Verify(x => x.CountryExists(It.IsAny<int>()), Times.Once);
        }
        #endregion

        #region GetCountryOfAnOwner
        [Fact]
        public void GivenId_WhenGetCountryOfAnOwnerIsCalled_ThenReturnsCountryOfTheOwner()
        {
            //Arrange
            _ownerRepository.Setup(x => x.OwnerExists(It.IsAny<int>())).Returns(true);
            _countryRepository.Setup(x => x.GetCountryByOwner(It.IsAny<int>()))
                .Returns(new Country());
            _mapper.Setup(mapper => mapper.Map<CountryDTO>(It.IsAny<Country>()))
                .Returns(countryDTO);

            //Act
            var result = _controller.GetCountryOfAnOwner(It.IsAny<int>());

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<CountryDTO>(okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(JsonSerializer.Serialize(countryDTO), JsonSerializer.Serialize(actual));
            _countryRepository.Verify(x => x.GetCountryByOwner(It.IsAny<int>()), Times.Once);
            _ownerRepository.Verify(x => x.OwnerExists(It.IsAny<int>()), Times.Once);
            _mapper.Verify(x => x.Map<CountryDTO>(It.IsAny<Country>()), Times.Once);
        }

        [Fact]
        public void GivenId_WhenGetCountryOfAnOwnerIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            _ownerRepository.Setup(x => x.OwnerExists(It.IsAny<int>())).Returns(false);

            //Act
            var result = _controller.GetCountryOfAnOwner(It.IsAny<int>());

            //Assert
            var notFound = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFound.StatusCode);
            _ownerRepository.Verify(x => x.OwnerExists(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GivenId_WhenGetCountryOfAnOwnerIsCalled_ThenReturnsBadRequest()
        {
            //Arrange
            _ownerRepository.Setup(x => x.OwnerExists(It.IsAny<int>()))
                .Returns(true);
            _controller.ModelState.AddModelError("", "Model state is invalid.");

            //Act
            var result = _controller.GetCountryOfAnOwner(It.IsAny<int>());

            //Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
            _ownerRepository.Verify(x => x.OwnerExists(It.IsAny<int>()), Times.Once);
        }
        #endregion

        #region CreateCountry
        [Fact]
        public void GivenCountry_WhenCreateCountryIsCalled_ThenReturnsOk()
        {
            //Arrange
            _countryRepository.Setup(x => x.GetCountries())
                .Returns([]);
            _mapper.Setup(mapper => mapper.Map<Country>(It.IsAny<CountryDTO>()))
                .Returns(new Country());
            _countryRepository.Setup(x => x.CreateCountry(It.IsAny<Country>()))
                .Returns(true);

            //Act
            var result = _controller.CreateCountry(countryDTO);

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);
            _countryRepository.Verify(x => x.GetCountries(), Times.Once);
            _countryRepository.Verify(x => x.CreateCountry(It.IsAny<Country>()), Times.Once);
            _mapper.Verify(x => x.Map<Country>(It.IsAny<CountryDTO>()), Times.Once);
        }

        [Fact]
        public void GivenCountry_WhenCreateCountryIsCalled_ThenReturnsBadRequestWhenCountryIsNull()
        {
            //Arrange

            //Act
            var result = _controller.CreateCountry(null);

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public void GivenCountry_WhenCreateCountryIsCalled_ThenReturnsBadRequestWhenModelStateIsInvalid()
        {
            //Arrange
            _countryRepository.Setup(x => x.GetCountries()).Returns([]);
            _controller.ModelState.AddModelError("", "Model state is invalid.");

            //Act
            var result = _controller.CreateCountry(countryDTO);

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
        }

        [Fact]
        public void GivenCountry_WhenCreateCountryIsCalled_ThenReturnsModelErrorWhenCountryAlreadyExists()
        {
            //Arrange
            List<Country> countries = [new Country() { Id = 1, Name = "Test" }];
            _countryRepository.Setup(x => x.GetCountries()).Returns(countries);
            //Act
            var result = _controller.CreateCountry(countryDTO);

            //Assert
            var model = Assert.IsType<ObjectResult>(result);
            Assert.NotNull(result);
            Assert.Equal(422, model.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
            _countryRepository.Verify(x => x.GetCountries(), Times.Once);
        }

        [Fact]
        public void GivenCountry_WhenCreateCountryIsCalled_ThenReturnsServerError()
        {
            //Arrange
            _countryRepository.Setup(x => x.GetCountries())
                .Returns([]);
            _mapper.Setup(mapper => mapper.Map<Country>(It.IsAny<CountryDTO>()))
                .Returns(new Country());
            _countryRepository.Setup(x => x.CreateCountry(It.IsAny<Country>()))
                .Returns(false);

            //Act
            var result = _controller.CreateCountry(countryDTO);

            //Assert
            var model = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, model.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
            _countryRepository.Verify(x => x.GetCountries(), Times.Once);
            _countryRepository.Verify(x => x.CreateCountry(It.IsAny<Country>()), Times.Once);
            _mapper.Verify(x => x.Map<Country>(It.IsAny<CountryDTO>()), Times.Once);
        }
        #endregion

        #region UpdateCountry
        [Fact]
        public void GivenIdAndCountry_WhenUpdateCountryIsCalled_ThenReturnsNoContent()
        {
            //Arrange
            _countryRepository.Setup(x => x.CountryExists(It.IsAny<int>()))
                .Returns(true);
            _mapper.Setup(mapper => mapper.Map<Country>(It.IsAny<CountryDTO>()))
                .Returns(new Country());
            _countryRepository.Setup(x => x.UpdateCountry(It.IsAny<Country>()))
                .Returns(true);

            //Act
            var result = _controller.UpdateCountry(countryDTO.Id, countryDTO);

            //Assert
            var noContent = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContent.StatusCode);
            _countryRepository.Verify(x => x.CountryExists(It.IsAny<int>()), Times.Once);
            _countryRepository.Verify(x => x.UpdateCountry(It.IsAny<Country>()), Times.Once);
            _mapper.Verify(x => x.Map<Country>(It.IsAny<CountryDTO>()), Times.Once);
        }

        [Fact]
        public void GivenIdAndCountry_WhenUpdateCountryIsCalled_ThenReturnsBadRequestWhenCountryIsNull()
        {
            //Arrange

            //Act
            var result = _controller.UpdateCountry(It.IsAny<int>(), null);

            //Assert
            var badRequestResult =  Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public void GivenIdAndCountry_WhenUpdateCountryIsCalled_ThenReturnsBadRequestWhenModelStateIsInvalid()
        {
            //Arrange
            _controller.ModelState.AddModelError("", "Model state is invalid.");

            //Act
            var result = _controller.UpdateCountry(It.IsAny<int>(), countryDTO);

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
        }

        [Fact]
        public void GivenIdAndCountry_WhenUpdateCountryIsCalled_ThenReturnsCountryIsNotFound()
        {
            //Arrange
            _countryRepository.Setup(x => x.CountryExists(It.IsAny<int>())).Returns(false);

            //Act
            var result = _controller.UpdateCountry(countryDTO.Id, countryDTO);

            //Assert
            var model = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, model.StatusCode);
            Assert.True(_controller.ModelState.IsValid);
        }

        [Fact]
        public void GivenIdAndCountry_WhenUpdateCountryIsCalled_ThenReturnsErrorWhenUpdateFails()
        {
            //Arrange
            _countryRepository.Setup(x => x.CountryExists(It.IsAny<int>()))
                .Returns(true);
            _mapper.Setup(mapper => mapper.Map<Country>(It.IsAny<CountryDTO>()))
                .Returns(new Country());
            _countryRepository.Setup(x => x.UpdateCountry(It.IsAny<Country>()))
                .Returns(false);

            //Act
            var result = _controller.UpdateCountry(countryDTO.Id, countryDTO);

            //Assert
            var model = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, model.StatusCode);
            _countryRepository.Verify(x => x.CountryExists(It.IsAny<int>()), Times.Once);
            _countryRepository.Verify(x => x.UpdateCountry(It.IsAny<Country>()), Times.Once);
            _mapper.Verify(x => x.Map<Country>(It.IsAny<CountryDTO>()), Times.Once);
        }
        #endregion

        #region DeleteCountry
        [Fact]
        public void GivenId_WhenDeleteCountryIsCalled_ThenReturnsNoContent()
        {
            //Arrange
            _countryRepository.Setup(x => x.CountryExists(It.IsAny<int>())).Returns(true);
            _countryRepository.Setup(x => x.GetCountry(1)).Returns(new Country());

            //Act
            var result = _controller.DeleteCountry(It.IsAny<int>());

            //Assert
            var model = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, model.StatusCode);
            Assert.NotNull(result);
            Assert.IsAssignableFrom<ObjectResult>(result);
            _countryRepository.Verify(x => x.CountryExists(It.IsAny<int>()), Times.Once);
            _countryRepository.Verify(x => x.GetCountry(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GivenId_WhenDeleteCountryIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            _countryRepository.Setup(x => x.CountryExists(It.IsAny<int>())).Returns(false);

            //Act
            var result = _controller.DeleteCountry(It.IsAny<int>());

            //Assert
            var model = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, model.StatusCode);
            Assert.IsAssignableFrom<NotFoundResult>(result);
            _countryRepository.Verify(x => x.CountryExists(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GivenId_WhenDeleteCountryIsCalled_ThenReturnsModelErrorWhenDeleteFails()
        {
            //Arrange
            _countryRepository.Setup(x => x.CountryExists(It.IsAny<int>())).Returns(true);
            _countryRepository.Setup(x => x.GetCountry(It.IsAny<int>())).Returns(new Country());
            _countryRepository.Setup(x => x.DeleteCountry(It.IsAny<Country>())).Returns(false);

            //Act
            var result = _controller.DeleteCountry(It.IsAny<int>());

            //Assert
            var model = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, model.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
            _countryRepository.Verify(x => x.CountryExists(It.IsAny<int>()), Times.Once);
            _countryRepository.Verify(x => x.GetCountry(It.IsAny<int>()), Times.Once);
            _countryRepository.Verify(x => x.DeleteCountry(It.IsAny<Country>()), Times.Once);
        }
        #endregion
    }
}
