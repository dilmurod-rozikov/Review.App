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
            List<Country> countries = [new() { Id = countryDTO.Id, Name = countryDTO.Name }];
            List<CountryDTO> countryDTOs = [countryDTO];

            _countryRepository.Setup(x => x.GetCountries()).Returns(countries);
            _mapper.Setup(mapper => mapper.Map<List<CountryDTO>>(It.IsAny<IEnumerable<Country>>()))
                .Returns(countryDTOs);

            //Act
            var result = _controller.GetCountries();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualCategories = Assert.IsType<List<CountryDTO>>(okResult.Value);

            Assert.NotNull(result);
            Assert.Equal(JsonSerializer.Serialize(countryDTOs), JsonSerializer.Serialize(actualCategories));
            _countryRepository.Verify(x => x.GetCountries(), Times.Once);
            _mapper.Verify(x => x.Map<List<CountryDTO>>(It.IsAny<IEnumerable<Country>>()), Times.Once);
        }

        [Fact]
        public void GivenNothing_WhenGetCategoriesIsCalled_ThenReturnsBadRequest()
        {
            //Arrange
            List<Country> countries = [new() { Id = countryDTO.Id, Name = countryDTO.Name }];
            List<CountryDTO> countryDTOs = [countryDTO];

            _countryRepository.Setup(x => x.GetCountries()).Returns(countries);
            _mapper.Setup(mapper => mapper.Map<List<CountryDTO>>(It.IsAny<IEnumerable<Country>>()))
                .Returns(countryDTOs);
            _controller.ModelState.AddModelError("", "Model is not valid");

            //Act
            var result = _controller.GetCountries();

            //Assert
            var okResult = Assert.IsType<BadRequestResult>(result);
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
            _countryRepository.Verify(x => x.GetCountries(), Times.Once);
            _mapper.Verify(x => x.Map<List<CountryDTO>>(It.IsAny<IEnumerable<Country>>()), Times.Once);
        }
        #endregion

        #region GetCountry
        [Fact]
        public void GivenCategoryId_WhenGetCountryIsCalled_ThenReturnsCategory()
        {
            //Arrange
            _countryRepository.Setup(x => x.CountryExists(countryDTO.Id)).Returns(true);
            _countryRepository.Setup(x => x.GetCountry(countryDTO.Id)).Returns(It.IsAny<Country>());
            _mapper.Setup(mapper => mapper.Map<CountryDTO>(It.IsAny<Country>()))
                .Returns(countryDTO);

            //Act
            var result = _controller.GetCountry(countryDTO.Id);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<CountryDTO>(okResult.Value);

            Assert.NotNull(result);
            Assert.Equal(JsonSerializer.Serialize(countryDTO), JsonSerializer.Serialize(actual));
            _countryRepository.Verify(x => x.GetCountry(countryDTO.Id), Times.Once);
            _countryRepository.Verify(x => x.CountryExists(countryDTO.Id), Times.Once);
            _mapper.Verify(x => x.Map<CountryDTO>(It.IsAny<Country>()), Times.Once);
        }

        [Fact]
        public void GivenCategoryId_WhenGetCountryIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            _countryRepository.Setup(x => x.CountryExists(countryDTO.Id)).Returns(false);

            //Act
            var result = _controller.GetCountry(countryDTO.Id);

            //Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.NotNull(result);
            _countryRepository.Verify(x => x.CountryExists(countryDTO.Id), Times.Once);
        }

        [Fact]
        public void GivenCategoryId_WhenGetCountryIsCalled_ThenReturnsBadRequest()
        {
            //Arrange
            _countryRepository.Setup(x => x.CountryExists(countryDTO.Id)).Returns(true);
            _countryRepository.Setup(x => x.GetCountry(countryDTO.Id)).Returns(It.IsAny<Country>());
            _mapper.Setup(mapper => mapper.Map<CountryDTO>(It.IsAny<Country>()))
                .Returns(countryDTO);
            _controller.ModelState.AddModelError("", "Model state is invalid.");

            //Act
            var result = _controller.GetCountry(countryDTO.Id);

            //Assert
            var okResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
            _countryRepository.Verify(x => x.GetCountry(countryDTO.Id), Times.Once);
            _countryRepository.Verify(x => x.CountryExists(countryDTO.Id), Times.Once);
            _mapper.Verify(x => x.Map<CountryDTO>(It.IsAny<Country>()), Times.Once);
        }
        #endregion

        #region GetCountryOfAnOwner
        [Fact]
        public void GivenCategoryId_WhenGetCountryOfAnOwnerIsCalled_ThenReturnsCountryOfTheOwner()
        {
            //Arrange
            var country = new Country() { Id = countryDTO.Id, Name = countryDTO.Name };
            _ownerRepository.Setup(x => x.OwnerExists(It.IsAny<int>())).Returns(true);
            _countryRepository.Setup(x => x.GetCountryByOwner(It.IsAny<int>()))
                .Returns(country);
            _mapper.Setup(mapper => mapper.Map<CountryDTO>(It.IsAny<Country>()))
                .Returns(countryDTO);

            //Act
            var result = _controller.GetCountryOfAnOwner(countryDTO.Id);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<CountryDTO>(okResult.Value);

            Assert.NotNull(result);
            Assert.Equal(JsonSerializer.Serialize(countryDTO), JsonSerializer.Serialize(actual));
            _countryRepository.Verify(x => x.GetCountryByOwner(It.IsAny<int>()), Times.Once);
            _ownerRepository.Verify(x => x.OwnerExists(It.IsAny<int>()), Times.Once);
            _mapper.Verify(x => x.Map<CountryDTO>(It.IsAny<Country>()), Times.Once);
        }

        [Fact]
        public void GivenCategoryId_WhenGetCountryOfAnOwnerIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            _ownerRepository.Setup(x => x.OwnerExists(It.IsAny<int>())).Returns(false);

            //Act
            var result = _controller.GetCountryOfAnOwner(It.IsAny<int>());

            //Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.NotNull(result);
            _ownerRepository.Verify(x => x.OwnerExists(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GivenCategoryId_WhenGetCountryOfAnOwnerIsCalled_ThenReturnsBadRequest()
        {
            //Arrange
            var country = new Country() { Id = countryDTO.Id, Name = countryDTO.Name };
            _ownerRepository.Setup(x => x.OwnerExists(It.IsAny<int>())).Returns(true);
            _countryRepository.Setup(x => x.GetCountryByOwner(It.IsAny<int>()))
                .Returns(country);
            _mapper.Setup(mapper => mapper.Map<CountryDTO>(It.IsAny<Country>()))
                .Returns(countryDTO);
            _controller.ModelState.AddModelError("", "Model state is invalid.");

            //Act
            var result = _controller.GetCountryOfAnOwner(It.IsAny<int>());

            //Assert
            var okResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
            _countryRepository.Verify(x => x.GetCountryByOwner(It.IsAny<int>()), Times.Once);
            _ownerRepository.Verify(x => x.OwnerExists(It.IsAny<int>()), Times.Once);
            _mapper.Verify(x => x.Map<CountryDTO>(It.IsAny<Country>()), Times.Once);
        }
        #endregion

        #region CreateCountry
        [Fact]
        public void GivenCategory_WhenCreateCountryIsCalled_ThenReturnsOk()
        {
            //Arrange
            List<Country> countries = [new Country() { Id = 1, Name = "Test 2"}];
            _countryRepository.Setup(x => x.GetCountries()).Returns(countries);
            _mapper.Setup(mapper => mapper.Map<Country>(It.IsAny<CountryDTO>()))
                .Returns(It.IsAny<Country>());
            _countryRepository.Setup(x => x.CreateCountry(It.IsAny<Country>())).Returns(true);

            //Act
            var result = _controller.CreateCountry(countryDTO);

            //Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(result);
            _countryRepository.Verify(x => x.GetCountries(), Times.Once);
            _countryRepository.Verify(x => x.CreateCountry(It.IsAny<Country>()), Times.Once);
            _mapper.Verify(x => x.Map<Country>(It.IsAny<CountryDTO>()), Times.Once);
        }

        [Fact]
        public void GivenCategory_WhenCreateCountryIsCalled_ThenReturnsBadRequestWhenCountryIsNull()
        {
            //Arrange

            //Act
            var result = _controller.CreateCountry(null);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public void GivenCategory_WhenCreateCountryIsCalled_ThenReturnsBadRequestWhenModelStateIsInvalid()
        {
            //Arrange
            List<Country> countries = [new Country() { Id = 1, Name = "Test 2" }];
            _countryRepository.Setup(x => x.GetCountries()).Returns(countries);
            _controller.ModelState.AddModelError("", "Model state is invalid.");

            //Act
            var result = _controller.CreateCountry(countryDTO);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
        }

        [Fact]
        public void GivenCategory_WhenCreateCountryIsCalled_ThenReturnsModelErrorWhenCountryAlreadyExists()
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
        public void GivenCategory_WhenCreateCountryIsCalled_ThenReturnsServerError()
        {
            //Arrange
            List<Country> countries = [new Country() { Id = 1, Name = "Test 2" }];
            _countryRepository.Setup(x => x.GetCountries()).Returns(countries);
            _mapper.Setup(mapper => mapper.Map<Country>(It.IsAny<CountryDTO>()))
                .Returns(It.IsAny<Country>);
            _countryRepository.Setup(x => x.CreateCountry(It.IsAny<Country>())).Returns(false);

            //Act
            var result = _controller.CreateCountry(countryDTO);

            //Assert
            var model = Assert.IsType<ObjectResult>(result);
            Assert.NotNull(result);
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
            _countryRepository.Setup(x => x.CountryExists(It.IsAny<int>())).Returns(true);
            _mapper.Setup(mapper => mapper.Map<Country>(It.IsAny<CountryDTO>()))
                .Returns(It.IsAny<Country>);
            _countryRepository.Setup(x => x.UpdateCountry(It.IsAny<Country>()))
                .Returns(true);

            //Act
            var result = _controller.UpdateCountry(countryDTO.Id, countryDTO);

            //Assert
            Assert.IsType<NoContentResult>(result);
            Assert.NotNull(result);
            _countryRepository.Verify(x => x.CountryExists(It.IsAny<int>()), Times.Once);
            _countryRepository.Verify(x => x.UpdateCountry(It.IsAny<Country>()), Times.Once);
            _mapper.Verify(x => x.Map<Country>(It.IsAny<CountryDTO>()), Times.Once);
        }

        [Fact]
        public void GivenIdAndCountry_WhenUpdateCountryIsCalled_ThenReturnsBadRequestWhenCategoryIsNull()
        {
            //Arrange

            //Act
            var result = _controller.UpdateCountry(1, null);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public void GivenIdAndCountry_WhenUpdateCountryIsCalled_ThenReturnsBadRequestWhenModelStateIsInvalid()
        {
            //Arrange
            _controller.ModelState.AddModelError("", "Model state is invalid.");

            //Act
            var result = _controller.UpdateCountry(1, countryDTO);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
        }

        [Fact]
        public void GivenIdAndCategory_WhenUpdateCategoryIsCalled_ThenReturnsCategoryIsNotFound()
        {
            //Arrange
            _countryRepository.Setup(x => x.CountryExists(It.IsAny<int>())).Returns(false);

            //Act
            var result = _controller.UpdateCountry(countryDTO.Id, countryDTO);

            //Assert
            var model = Assert.IsType<NotFoundObjectResult>(result);
            Assert.NotNull(result);
            Assert.Equal(404, model.StatusCode);
            Assert.True(_controller.ModelState.IsValid);
        }

        [Fact]
        public void GivenIdAndCountry_WhenUpdateCountryIsCalled_ThenReturnsErrorWhenUpdateFails()
        {
            //Arrange
            var category = new Category { Id = countryDTO.Id, Name = "new Category" };
            _countryRepository.Setup(x => x.CountryExists(category.Id)).Returns(true);
            _mapper.Setup(mapper => mapper.Map<Country>(It.IsAny<CountryDTO>()))
                .Returns(It.IsAny<Country>);
            _countryRepository.Setup(x => x.UpdateCountry(It.IsAny<Country>())).Returns(false);

            //Act
            var result = _controller.UpdateCountry(category.Id, countryDTO);

            //Assert
            var model = Assert.IsType<ObjectResult>(result);
            Assert.NotNull(result);
            Assert.Equal(500, model.StatusCode);
            _countryRepository.Verify(x => x.CountryExists(category.Id), Times.Once);
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
            _countryRepository.Setup(x => x.GetCountry(1)).Returns(It.IsAny<Country>());

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
            Assert.NotNull(result);
            Assert.IsAssignableFrom<NotFoundResult>(result);
            _countryRepository.Verify(x => x.CountryExists(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GivenId_WhenDeleteCountryIsCalled_ThenReturnsModelError()
        {
            //Arrange
            _countryRepository.Setup(x => x.CountryExists(It.IsAny<int>())).Returns(true);
            _countryRepository.Setup(x => x.GetCountry(It.IsAny<int>())).Returns(It.IsAny<Country>());
            _controller.ModelState.AddModelError("", "Something wrong with the model");

            //Act
            var result = _controller.DeleteCountry(It.IsAny<int>());

            //Assert
            var model = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, model.StatusCode);
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
            _countryRepository.Verify(x => x.CountryExists(It.IsAny<int>()), Times.Once);
            _countryRepository.Verify(x => x.GetCountry(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GivenId_WhenDeleteCountryIsCalled_ThenReturnsModelErrorWhenDeleteFails()
        {
            //Arrange
            _countryRepository.Setup(x => x.CountryExists(It.IsAny<int>())).Returns(true);
            _countryRepository.Setup(x => x.GetCountry(It.IsAny<int>())).Returns(It.IsAny<Country>());
            _countryRepository.Setup(x => x.DeleteCountry(It.IsAny<Country>())).Returns(false);

            //Act
            var result = _controller.DeleteCountry(It.IsAny<int>());

            //Assert
            var model = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, model.StatusCode);
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
            _countryRepository.Verify(x => x.CountryExists(It.IsAny<int>()), Times.Once);
            _countryRepository.Verify(x => x.GetCountry(It.IsAny<int>()), Times.Once);
            _countryRepository.Verify(x => x.DeleteCountry(It.IsAny<Country>()), Times.Once);
        }
        #endregion
    }
}
