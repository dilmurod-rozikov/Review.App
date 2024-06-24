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
    public class OwnerControllerUnitTests
    {
        private readonly Mock<IOwnerRepository> _ownerRepositry;
        private readonly Mock<ICountryRepository> _countryRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly OwnerController _controller;
        private static readonly OwnerDTO ownerDTO = new()
        {
            Id = 1,
            Name = "Test",
            Gym = "New Gym"
        };
        private static readonly List<Owner> owners =
        [
            new Owner()
            {
                Id = 1,
                Name = "Test 2",
                Gym = "New Gym",
                Country = new Country() { Id = 1, Name = "Country" }
            }
        ];
        public OwnerControllerUnitTests()
        {
            _ownerRepositry = new Mock<IOwnerRepository>();
            _countryRepository = new Mock<ICountryRepository>();
            _mapper = new Mock<IMapper>();
            _controller = new OwnerController(_ownerRepositry.Object, _countryRepository.Object, _mapper.Object);
        }

        #region GetOwners
        [Fact]
        public void GivenNothing_WhenGetOwnersIsCalled_ThenReturnListOfOwners()
        {
            //Arrange
            List<OwnerDTO> ownerDTOs = [ownerDTO];

            _ownerRepositry.Setup(x => x.GetOwners()).Returns(owners);
            _mapper.Setup(mapper => mapper.Map<List<OwnerDTO>>(It.IsAny<IEnumerable<Owner>>()))
                .Returns(ownerDTOs);

            //Act
            var result = _controller.GetOwners();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<List<OwnerDTO>>(okResult.Value);
            Assert.NotNull(result);
            Assert.Equal(JsonSerializer.Serialize(ownerDTOs), JsonSerializer.Serialize(actual));
            _ownerRepositry.Verify(x => x.GetOwners(), Times.Once);
            _mapper.Verify(x => x.Map<List<OwnerDTO>>(It.IsAny<IEnumerable<Owner>>()), Times.Once);
        }

        [Fact]
        public void GivenNothing_WhenGetOwnersIsCalled_ThenReturnsBadRequest()
        {
            //Arrange
            List<OwnerDTO> ownerDTOs = [ownerDTO];

            _ownerRepositry.Setup(x => x.GetOwners()).Returns(owners);
            _mapper.Setup(mapper => mapper.Map<List<OwnerDTO>>(It.IsAny<IEnumerable<Owner>>()))
                .Returns(ownerDTOs);
            _controller.ModelState.AddModelError("", "Model is not valid");

            //Act
            var result = _controller.GetOwners();

            //Assert
            var okResult = Assert.IsType<BadRequestResult>(result);
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
            _ownerRepositry.Verify(x => x.GetOwners(), Times.Once);
            _mapper.Verify(x => x.Map<List<OwnerDTO>>(It.IsAny<IEnumerable<Owner>>()), Times.Once);
        }
        #endregion

        #region GetOwner
        [Fact]
        public void GivenId_WhenGetOwnerIsCalled_ThenReturnsCategory()
        {
            //Arrange
            _ownerRepositry.Setup(x => x.OwnerExists(It.IsAny<int>())).Returns(true);
            _ownerRepositry.Setup(x => x.GetOwner(It.IsAny<int>())).Returns(It.IsAny<Owner>());
            _mapper.Setup(mapper => mapper.Map<OwnerDTO>(It.IsAny<Owner>()))
                .Returns(ownerDTO);

            //Act
            var result = _controller.GetOwner(It.IsAny<int>());

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<OwnerDTO>(okResult.Value);
            Assert.NotNull(result);
            Assert.Equal(JsonSerializer.Serialize(ownerDTO), JsonSerializer.Serialize(actual));
            _ownerRepositry.Verify(x => x.GetOwner(It.IsAny<int>()), Times.Once);
            _ownerRepositry.Verify(x => x.OwnerExists(It.IsAny<int>()), Times.Once);
            _mapper.Verify(x => x.Map<OwnerDTO>(It.IsAny<Owner>()), Times.Once);
        }

        [Fact]
        public void GivenId_WhenGetOwnerIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            _ownerRepositry.Setup(x => x.OwnerExists(It.IsAny<int>())).Returns(false);

            //Act
            var result = _controller.GetOwner(It.IsAny<int>());

            //Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.NotNull(result);
            _ownerRepositry.Verify(x => x.OwnerExists(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GivenId_WhenGetOwnerIsCalled_ThenReturnsBadRequest()
        {
            //Arrange
            _ownerRepositry.Setup(x => x.OwnerExists(It.IsAny<int>())).Returns(true);
            _ownerRepositry.Setup(x => x.GetOwner(It.IsAny<int>())).Returns(It.IsAny<Owner>());
            _mapper.Setup(mapper => mapper.Map<OwnerDTO>(It.IsAny<Owner>()))
                .Returns(ownerDTO);
            _controller.ModelState.AddModelError("", "Model state is invalid.");

            //Act
            var result = _controller.GetOwner(It.IsAny<int>());

            //Assert
            var okResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
            _ownerRepositry.Verify(x => x.GetOwner(It.IsAny<int>()), Times.Once);
            _ownerRepositry.Verify(x => x.OwnerExists(It.IsAny<int>()), Times.Once);
            _mapper.Verify(x => x.Map<OwnerDTO>(It.IsAny<Owner>()), Times.Once);
        }
        #endregion

        #region GetPokemonByOwnerId
        [Fact]
        public void GivenId_WhenGetPokemonByOwnerIdIsCalled_ThenReturnsPokemon()
        {
            //Arrange
            List<PokemonDTO> pokemonDTOs =[ new() {Id = 1, Name = "Test" } ];
            List<Pokemon> pokemons = [new() { Id = 1, Name = "Test" }];

            _ownerRepositry.Setup(x => x.OwnerExists(It.IsAny<int>())).Returns(true);
            _ownerRepositry.Setup(x => x.GetPokemonsByOwner(It.IsAny<int>()))
                .Returns(pokemons);
            _mapper.Setup(mapper => mapper.Map<List<PokemonDTO>>(It.IsAny<List<Pokemon>>()))
                .Returns(pokemonDTOs);

            //Act
            var result = _controller.GetPokemonByOwner(It.IsAny<int>());

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<List<PokemonDTO>>(okResult.Value);
            Assert.NotNull(result);
            _ownerRepositry.Verify(x => x.GetPokemonsByOwner(It.IsAny<int>()), Times.Once);
            _ownerRepositry.Verify(x => x.OwnerExists(It.IsAny<int>()), Times.Once);
            _mapper.Verify(x => x.Map<List<PokemonDTO>>(It.IsAny<List<Pokemon>>()), Times.Once);
        }

        [Fact]
        public void GivenId_WhenGetPokemonByOwnerIdIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            _ownerRepositry.Setup(x => x.OwnerExists(It.IsAny<int>())).Returns(false);

            //Act
            var result = _controller.GetPokemonByOwner(It.IsAny<int>());

            //Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.NotNull(result);
            _ownerRepositry.Verify(x => x.OwnerExists(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GivenId_WhenGetPokemonsByOwnerIsCalled_ThenReturnsBadRequest()
        {
            //Arrange
            List<PokemonDTO> pokemons =[ new() { } ];

            _ownerRepositry.Setup(x => x.OwnerExists(It.IsAny<int>())).Returns(true);
            _ownerRepositry.Setup(x => x.GetPokemonsByOwner(It.IsAny<int>()))

                .Returns(It.IsAny<List<Pokemon>>());
            _mapper.Setup(mapper => mapper.Map<List<PokemonDTO>>(It.IsAny<Pokemon>()))
                .Returns(pokemons);
            _controller.ModelState.AddModelError("", "Model state is invalid.");

            //Act
            var result = _controller.GetPokemonByOwner(It.IsAny<int>());

            //Assert
            var okResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
            _ownerRepositry.Verify(x => x.GetPokemonsByOwner(It.IsAny<int>()), Times.Once);
            _ownerRepositry.Verify(x => x.OwnerExists(It.IsAny<int>()), Times.Once);
            _mapper.Verify(x => x.Map<List<PokemonDTO>>(It.IsAny<Pokemon>()), Times.Once);
        }
        #endregion

        #region CreateOwner
        [Fact]
        public void GivenOwnerAndCountryId_WhenCreateOwnerIsCalled_ThenReturnsOk()
        {
            //Arrange
            _ownerRepositry.Setup(x => x.GetOwners()).Returns(owners);
            _countryRepository.Setup(x => x.GetCountry(1)).Returns(new Country() { Id = 1, Name = "Country" });
            _mapper.Setup(mapper => mapper.Map<Owner>(It.IsAny<OwnerDTO>()))
                .Returns(It.IsAny<Owner>);
            _ownerRepositry.Setup(x => x.CreateOwner(It.IsAny<Owner>())).Returns(true);

            //Act
            var result = _controller.CreateOwner(1, ownerDTO);

            //Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(result);
            _ownerRepositry.Verify(x => x.GetOwners(), Times.Once);
            _ownerRepositry.Verify(x => x.CreateOwner(It.IsAny<Owner>()), Times.Once);
            _countryRepository.Verify(x => x.GetCountry(1), Times.Once);
            _mapper.Verify(x => x.Map<Owner>(It.IsAny<OwnerDTO>()), Times.Once);
        }

        [Fact]
        public void GivenOwnerAndCountryId_WhenCreateOwnerIsCalled_ThenReturnsBadRequestWhenCategoryIsNull()
        {
            //Arrange

            //Act
            var result = _controller.CreateOwner(It.IsAny<int>(), null);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public void GivenOwnerAndCountryId_WhenCreateOwnerIsCalled_ThenReturnsBadRequestWhenModelStateIsInvalid()
        {
            //Arrange
            _ownerRepositry.Setup(x => x.GetOwners()).Returns(owners);
            _controller.ModelState.AddModelError("", "Model state is invalid.");

            //Act
            var result = _controller.CreateOwner(It.IsAny<int>(), ownerDTO);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
        }

        [Fact]
        public void GivenOwnerAndCountryId_WhenCreateOwnerIsCalled_ThenReturnsModelErrorWhenCategoryAlreadyExists()
        {
            //Arrange
            _ownerRepositry.Setup(x => x.GetOwners()).Returns(owners);

            //Act
            var result = _controller.CreateOwner(It.IsAny<int>(), ownerDTO);

            //Assert
            var model = Assert.IsType<ObjectResult>(result);
            Assert.NotNull(result);
            Assert.Equal(422, model.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
            _ownerRepositry.Verify(x => x.GetOwners(), Times.Once);
        }

        [Fact]
        public void GivenOwnerAndCountryId_WhenCreateOwnerIsCalled_ThenReturnsServerError()
        {
            //Arrange
            _ownerRepositry.Setup(x => x.GetOwners()).Returns(owners);
            _mapper.Setup(mapper => mapper.Map<Owner>(It.IsAny<OwnerDTO>()))
                .Returns(new Owner());
            _ownerRepositry.Setup(x => x.CreateOwner(It.IsAny<Owner>())).Returns(false);

            //Act
            var result = _controller.CreateOwner(It.IsAny<int>(), ownerDTO);

            //Assert
            var model = Assert.IsType<ObjectResult>(result);
            Assert.NotNull(result);
            Assert.Equal(500, model.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
            _ownerRepositry.Verify(x => x.GetOwners(), Times.Once);
            _ownerRepositry.Verify(x => x.CreateOwner(It.IsAny<Owner>()), Times.Once);
            _mapper.Verify(x => x.Map<Owner>(It.IsAny<OwnerDTO>()), Times.Once);
        }
        #endregion

        #region UpdateOwner
        [Fact]
        public void GivenIdAndOwner_WhenUpdateOwnerIsCalled_ThenReturnsNoContent()
        {
            //Arrange
            var owner = new Owner { Id = 1, Name = "New Owner", Gym = "New Gym" };
            _ownerRepositry.Setup(x => x.OwnerExists(owner.Id)).Returns(true);
            _mapper.Setup(mapper => mapper.Map<Owner>(It.IsAny<OwnerDTO>()))
                .Returns(owner);
            _ownerRepositry.Setup(x => x.UpdateOwner(It.IsAny<Owner>()))
                .Returns(true);

            //Act
            var result = _controller.UpdateOwner(owner.Id, ownerDTO);

            //Assert
            Assert.IsType<NoContentResult>(result);
            Assert.NotNull(result);
            _ownerRepositry.Verify(x => x.OwnerExists(owner.Id), Times.Once);
            _ownerRepositry.Verify(x => x.UpdateOwner(It.IsAny<Owner>()), Times.Once);
            _mapper.Verify(x => x.Map<Owner>(It.IsAny<OwnerDTO>()), Times.Once);
        }

        [Fact]
        public void GivenIdAndOwner_WhenUpdateOwnerIsCalled_ThenReturnsBadRequestWhenCategoryIsNull()
        {
            //Arrange

            //Act
            var result = _controller.UpdateOwner(It.IsAny<int>(), null);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public void GivenIdAndOwner_WhenUpdateOwnerIsCalled_ThenReturnsBadRequestWhenModelStateIsInvalid()
        {
            //Arrange
            _controller.ModelState.AddModelError("", "Model state is invalid.");

            //Act
            var result = _controller.UpdateOwner(It.IsAny<int>(), ownerDTO);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
        }

        [Fact]
        public void GivenIdAndOwner_WhenUpdateOwnerIsCalled_ThenReturnsCategoryIsNotFound()
        {
            //Arrange
            _ownerRepositry.Setup(x => x.OwnerExists(ownerDTO.Id)).Returns(false);

            //Act
            var result = _controller.UpdateOwner(ownerDTO.Id, ownerDTO);

            //Assert
            var model = Assert.IsType<NotFoundObjectResult>(result);
            Assert.NotNull(result);
            Assert.Equal(404, model.StatusCode);
            Assert.True(_controller.ModelState.IsValid);
        }

        [Fact]
        public void GivenIdAndOwner_WhenUpdateOwnerIsCalled_ThenReturnsErrorWhenUpdateFails()
        {
            //Arrange
            Owner owner = new() { Id = ownerDTO.Id };
            _ownerRepositry.Setup(x => x.OwnerExists(It.IsAny<int>())).Returns(true);
            _mapper.Setup(mapper => mapper.Map<Owner>(It.IsAny<OwnerDTO>()))
                .Returns(new Owner());
            _ownerRepositry.Setup(x => x.UpdateOwner(owner)).Returns(false);

            //Act
            var result = _controller.UpdateOwner(ownerDTO.Id, ownerDTO);

            //Assert
            var model = Assert.IsType<ObjectResult>(result);
            Assert.NotNull(result);
            Assert.Equal(500, model.StatusCode);
            _ownerRepositry.Verify(x => x.OwnerExists(It.IsAny<int>()), Times.Once);
            _ownerRepositry.Verify(x => x.UpdateOwner(It.IsAny<Owner>()), Times.Once);
            _mapper.Verify(x => x.Map<Owner>(It.IsAny<OwnerDTO>()), Times.Once);
        }
        #endregion

        #region DeleteOwner
        [Fact]
        public void GivenId_WhenDeleteOwnerIsCalled_ThenReturnsNoContent()
        {
            //Arrange
            _ownerRepositry.Setup(x => x.OwnerExists(It.IsAny<int>())).Returns(true);
            _ownerRepositry.Setup(x => x.GetOwner(It.IsAny<int>())).Returns(new Owner());

            //Act
            var result = _controller.DeleteOwner(It.IsAny<int>());

            //Assert
            var model = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, model.StatusCode);
            Assert.NotNull(result);
            Assert.IsAssignableFrom<ObjectResult>(result);
            _ownerRepositry.Verify(x => x.OwnerExists(It.IsAny<int>()), Times.Once);
            _ownerRepositry.Verify(x => x.GetOwner(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GivenId_WhenDeleteOwnerIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            _ownerRepositry.Setup(x => x.OwnerExists(It.IsAny<int>())).Returns(false);

            //Act
            var result = _controller.DeleteOwner(It.IsAny<int>());

            //Assert
            var model = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, model.StatusCode);
            Assert.NotNull(result);
            Assert.IsAssignableFrom<NotFoundResult>(result);
            _ownerRepositry.Verify(x => x.OwnerExists(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GivenId_WhenDeleteOwnerIsCalled_ThenReturnsModelError()
        {
            //Arrange
            _ownerRepositry.Setup(x => x.OwnerExists(It.IsAny<int>())).Returns(true);
            _ownerRepositry.Setup(x => x.GetOwner(It.IsAny<int>())).Returns(new Owner());
            _controller.ModelState.AddModelError("", "Something wrong with the model");

            //Act
            var result = _controller.DeleteOwner(It.IsAny<int>());

            //Assert
            var model = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, model.StatusCode);
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
            _ownerRepositry.Verify(x => x.OwnerExists(It.IsAny<int>()), Times.Once);
            _ownerRepositry.Verify(x => x.GetOwner(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GivenId_WhenDeleteOwnerIsCalled_ThenReturnsModelErrorWhenDeleteFails()
        {
            //Arrange
            _ownerRepositry.Setup(x => x.OwnerExists(It.IsAny<int>())).Returns(true);
            _ownerRepositry.Setup(x => x.GetOwner(It.IsAny<int>())).Returns(new Owner());
            _ownerRepositry.Setup(x => x.DeleteOwner(It.IsAny<Owner>())).Returns(false);

            //Act
            var result = _controller.DeleteOwner(It.IsAny<int>());

            //Assert
            var model = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, model.StatusCode);
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
            _ownerRepositry.Verify(x => x.OwnerExists(It.IsAny<int>()), Times.Once);
            _ownerRepositry.Verify(x => x.GetOwner(It.IsAny<int>()), Times.Once);
            _ownerRepositry.Verify(x => x.DeleteOwner(It.IsAny<Owner>()), Times.Once);
        }
        #endregion
    }
}
