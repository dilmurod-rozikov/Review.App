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
    public class PokemonControllerUnitTests
    {
        private readonly Mock<IOwnerRepository> _ownerRepositry;
        private readonly Mock<IPokemonRepository> _pokemonRepository;
        private readonly Mock<ICategoryRepository> _categoryRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly PokemonController _controller;
        private static readonly PokemonDTO pokemonDTO = new()
        {
            Id = 1,
            Name = "Test 1",
            BirthDate = DateOnly.MinValue,
        };

        private static readonly Review review = new()
        {
            Id = 1,
            Rating = 5,
        };

        private static readonly PokemonOwner pokemonOwner = new()
        {
            OwnerId = 1,
            PokemonId = 1,
            Owner = new(),
            Pokemon = new(),
        };

        private static readonly Pokemon pokemon = new()
        {
            Id = 1,
            Name = "Test 2",
            BirthDate = DateOnly.MinValue,
            Reviews = [review],
            PokemonOwners = [pokemonOwner]
        };

        public PokemonControllerUnitTests()
        {
            _ownerRepositry = new Mock<IOwnerRepository>();
            _categoryRepository = new Mock<ICategoryRepository>();
            _pokemonRepository = new Mock<IPokemonRepository>();
            _mapper = new Mock<IMapper>();
            _controller = new PokemonController(
                _pokemonRepository.Object,
                _ownerRepositry.Object,
                _categoryRepository.Object,
                _mapper.Object);
        }

        #region GetPokemons
        [Fact]
        public void GivenNothing_WhenGetPokemonsIsCalled_ThenReturnListOfPokemons()
        {
            //Arrange
            List<PokemonDTO> pokemonDTOs = [pokemonDTO];
            _pokemonRepository.Setup(x => x.GetPokemons()).Returns([]);
            _mapper.Setup(mapper => mapper.Map<List<PokemonDTO>>(It.IsAny<IEnumerable<Pokemon>>()))
                .Returns(pokemonDTOs);

            //Act
            var result = _controller.GetPokemons();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<List<PokemonDTO>>(okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(JsonSerializer.Serialize(pokemonDTOs), JsonSerializer.Serialize(actual));
            _pokemonRepository.Verify(x => x.GetPokemons(), Times.Once);
            _mapper.Verify(x => x.Map<List<PokemonDTO>>(It.IsAny<IEnumerable<Pokemon>>()), Times.Once);
        }

        [Fact]
        public void GivenNothing_WhenGetPokemonsIsCalled_ThenReturnsBadRequest()
        {
            //Arrange            
            _controller.ModelState.AddModelError("", "Model is not valid");

            //Act
            var result = _controller.GetPokemons();

            //Assert
            var badRequestResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
        }
        #endregion

        #region GetPokemon
        [Fact]
        public void GivenId_WhenGetPokemonIsCalled_ThenReturnsPokemon()
        {
            //Arrange
            _pokemonRepository.Setup(x => x.PokemonExists(It.IsAny<int>()))
                .Returns(true);
            _pokemonRepository.Setup(x => x.GetPokemon(It.IsAny<int>()))
                .Returns(new Pokemon());
            _mapper.Setup(mapper => mapper.Map<PokemonDTO>(It.IsAny<Pokemon>()))
                .Returns(pokemonDTO);

            //Act
            var result = _controller.GetPokemon(It.IsAny<int>());

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<PokemonDTO>(okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(JsonSerializer.Serialize(pokemonDTO), JsonSerializer.Serialize(actual));
            _pokemonRepository.Verify(x => x.GetPokemon(It.IsAny<int>()), Times.Once);
            _pokemonRepository.Verify(x => x.PokemonExists(It.IsAny<int>()), Times.Once);
            _mapper.Verify(x => x.Map<PokemonDTO>(It.IsAny<Pokemon>()), Times.Once);
        }

        [Fact]
        public void GivenId_WhenGetPokemonIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            _pokemonRepository.Setup(x => x.PokemonExists(It.IsAny<int>())).Returns(false);

            //Act
            var result = _controller.GetPokemon(It.IsAny<int>());

            //Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
            _pokemonRepository.Verify(x => x.PokemonExists(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GivenId_WhenGetPokemonIsCalled_ThenReturnsBadRequest()
        {
            //Arrange
            _pokemonRepository.Setup(x => x.PokemonExists(It.IsAny<int>()))
                .Returns(true);
            _controller.ModelState.AddModelError("", "Model state is invalid.");

            //Act
            var result = _controller.GetPokemon(It.IsAny<int>());

            //Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
            _pokemonRepository.Verify(x => x.PokemonExists(It.IsAny<int>()), Times.Once);
        }
        #endregion

        #region GetPokemonRating
        [Fact]
        public void GivenId_WhenGetPokemonRatingIsCalled_ThenReturnsPokemonRating()
        {
            //Arrange
            _pokemonRepository.Setup(x => x.PokemonExists(It.IsAny<int>()))
                .Returns(true);
            _pokemonRepository.Setup(x => x.GetPokemonRating(It.IsAny<int>()))
                .Returns(review.Rating);

            //Act
            var result = _controller.GetPokemonRating(It.IsAny<int>());

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<decimal>(okResult.Value);
            Assert.Equal(review.Rating, actual);
            Assert.Equal(200, okResult.StatusCode);
            _pokemonRepository.Verify(x => x.GetPokemonRating(It.IsAny<int>()), Times.Once);
            _pokemonRepository.Verify(x => x.PokemonExists(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GivenId_WhenGetPokemonByOwnerIdIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            _pokemonRepository.Setup(x => x.PokemonExists(It.IsAny<int>())).Returns(false);

            //Act
            var result = _controller.GetPokemonRating(It.IsAny<int>());

            //Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
            _pokemonRepository.Verify(x => x.PokemonExists(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GivenId_WhenGetPokemonsByOwnerIsCalled_ThenReturnsBadRequest()
        {
            //Arrange
            _pokemonRepository.Setup(x => x.PokemonExists(It.IsAny<int>()))
                .Returns(true);
            _controller.ModelState.AddModelError("", "Model state is invalid.");

            //Act
            var result = _controller.GetPokemonRating(It.IsAny<int>());

            //Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
            _pokemonRepository.Verify(x => x.PokemonExists(It.IsAny<int>()), Times.Once);
        }
        #endregion

        #region CreatePokemon
        [Fact]
        public void GivenOwnerIdAndCountryIdAndPokemon_WhenCreateOwnerIsCalled_ThenReturnsOk()
        {
            //Arrange
            _pokemonRepository.Setup(x => x.GetPokemons())
                .Returns([pokemon]);
            _ownerRepositry.Setup(x => x.OwnerExists(It.IsAny<int>()))
                .Returns(true);
            _categoryRepository.Setup(x => x.CategoryExists(It.IsAny<int>()))
                .Returns(true);
            _mapper.Setup(mapper => mapper.Map<Pokemon>(It.IsAny<PokemonDTO>()))
                .Returns(new Pokemon());
            _pokemonRepository.Setup(x => x.CreatePokemon(
                It.IsAny<int>(), It.IsAny <int>() ,It.IsAny<Pokemon>()))
                .Returns(true);

            //Act
            var result = _controller
                .CreatePokemon(It.IsAny<int>(), It.IsAny<int>(), pokemonDTO);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            _pokemonRepository.Verify(x => x.GetPokemons(), Times.Once);
            _pokemonRepository.Verify(x => x.CreatePokemon(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Pokemon>()),
                Times.Once);
            _ownerRepositry.Verify(x => x.OwnerExists(It.IsAny<int>()), Times.Once);
            _categoryRepository.Verify(x => x.CategoryExists(It.IsAny<int>()), Times.Once);
            _mapper.Verify(x => x.Map<Pokemon>(It.IsAny<PokemonDTO>()), Times.Once);
        }

        [Fact]
        public void GivenOwnerIdAndCountryIdAndPokemon_WhenCreatePokemonIsCalled_ThenReturnsBadRequestWhenPokemonIsNull()
        {
            //Arrange

            //Act
            var result = _controller.CreatePokemon(It.IsAny<int>(), It.IsAny<int>(), null);

            //Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public void GivenOwnerIdAndCountryIdAndPokemon_WhenCreatePokemonIsCalled_ThenReturnsBadRequestWhenModelStateIsInvalid()
        {
            //Arrange
            _controller.ModelState.AddModelError("", "Model state is invalid.");

            //Act
            var result = _controller.CreatePokemon(It.IsAny<int>(), It.IsAny<int>(), pokemonDTO);

            //Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
        }

        [Fact]
        public void GivenOwnerIdAndCountryIdAndPokemon_WhenCreatePokemonIsCalled_ThenReturnsModelErrorWhenPokemonAlreadyExists()
        {
            //Arrange
            List<Pokemon> pokemons = [new Pokemon()
            {
                Id = 2,
                Name = "Test 1",
            }];
            _pokemonRepository.Setup(x => x.GetPokemons()).Returns(pokemons);

            //Act
            var result = _controller.CreatePokemon(It.IsAny<int>(), It.IsAny<int>(), pokemonDTO);

            //Assert
            var model = Assert.IsType<ObjectResult>(result);
            Assert.Equal(422, model.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
            _pokemonRepository.Verify(x => x.GetPokemons(), Times.Once);
        }

        [Fact]
        public void GivenOwnerIdAndCountryIdAndPokemon_WhenCreatePokemonIsCalled_ThenReturnsNotFoundOnOwnerDoNotExist()
        {
            //Arrange
            _pokemonRepository.Setup(x => x.GetPokemons())
                .Returns([pokemon]);
            _ownerRepositry.Setup(x => x.OwnerExists(It.IsAny<int>()))
                .Returns(false);

            //Act
            var result = _controller.CreatePokemon(It.IsAny<int>(), It.IsAny<int>(), pokemonDTO);

            //Assert
            var model = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, model.StatusCode);
            _pokemonRepository.Verify(x => x.GetPokemons(), Times.Once);
            _ownerRepositry.Verify(x => x.OwnerExists(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GivenOwnerIdAndCountryIdAndPokemon_WhenCreatePokemonIsCalled_ThenReturnsNotFoundOnCategoryDoNotExist()
        {
            //Arrange
            _pokemonRepository.Setup(x => x.GetPokemons())
                .Returns([pokemon]);
            _categoryRepository.Setup(x => x.CategoryExists(It.IsAny<int>()))
                .Returns(false);

            //Act
            var result = _controller.CreatePokemon(It.IsAny<int>(), It.IsAny<int>(), pokemonDTO);

            //Assert
            var model = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, model.StatusCode);
            _pokemonRepository.Verify(x => x.GetPokemons(), Times.Once);
            _ownerRepositry.Verify(x => x.OwnerExists(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GivenOwnerIdAndCountryIdAndPokemon_WhenCreatePokemonIsCalled_ThenReturnsServerError()
        {
            //Arrange
            _pokemonRepository.Setup(x => x.GetPokemons())
                .Returns([pokemon]);
            _ownerRepositry.Setup(x => x.OwnerExists(It.IsAny<int>()))
                .Returns(true);
            _categoryRepository.Setup(x => x.CategoryExists(It.IsAny<int>()))
                .Returns(true);
            _mapper.Setup(mapper => mapper.Map<Pokemon>(It.IsAny<PokemonDTO>()))
                .Returns(new Pokemon());
            _pokemonRepository.Setup(x => x.CreatePokemon(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Pokemon>()))
                .Returns(false);

            //Act
            var result = _controller.CreatePokemon(It.IsAny<int>(), It.IsAny<int>(), pokemonDTO);

            //Assert
            var model = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, model.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
            _pokemonRepository.Verify(x => x.GetPokemons(), Times.Once);
            _pokemonRepository.Verify(x => x.CreatePokemon(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Pokemon>()),
                Times.Once);
            _ownerRepositry.Verify(x => x.OwnerExists(It.IsAny<int>()), Times.Once);
            _categoryRepository.Verify(x => x.CategoryExists(It.IsAny<int>()), Times.Once);
            _mapper.Verify(x => x.Map<Pokemon>(It.IsAny<PokemonDTO>()), Times.Once);
        }
        #endregion

        #region UpdatePokemon
        [Fact]
        public void GivenPokemonIdCategoryIdOwnerIdAndPokemon_WhenUpdatePokemonIsCalled_ThenReturnsNoContent()
        {
            //Arrange
            _ownerRepositry.Setup(x => x.OwnerExists(It.IsAny<int>()))
                .Returns(true);
            _pokemonRepository.Setup(x => x.PokemonExists(It.IsAny<int>()))
                .Returns(true);
            _categoryRepository.Setup(x => x.CategoryExists(It.IsAny<int>()))
                .Returns(true);
            _mapper.Setup(mapper => mapper.Map<Pokemon>(It.IsAny<PokemonDTO>()))
                .Returns(pokemon);
            _pokemonRepository.Setup(x => x.UpdatePokemon(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Pokemon>()))
                .Returns(true);

            //Act
            var result = _controller.UpdatePokemon(pokemonDTO.Id, It.IsAny<int>(), It.IsAny<int>(), pokemonDTO);

            //Assert
            var noContent = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContent.StatusCode);
            _ownerRepositry.Verify(x => x.OwnerExists(It.IsAny<int>()), Times.Once);
            _categoryRepository.Verify(x => x.CategoryExists(It.IsAny<int>()), Times.Once);
            _pokemonRepository.Verify(x => x.PokemonExists(It.IsAny<int>()), Times.Once);
            _pokemonRepository.Verify(x => x.UpdatePokemon(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Pokemon>()), Times.Once);
            _mapper.Verify(x => x.Map<Pokemon>(It.IsAny<PokemonDTO>()), Times.Once);
        }

        [Fact]
        public void GivenPokemonIdCategoryIdOwnerIdAndPokemon_WhenUpdatePokemonIsCalled_ThenReturnsBadRequestWhenPokemonIsNull()
        {
            //Arrange

            //Act
            var result = _controller.UpdatePokemon(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), null);

            //Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public void GivenPokemonIdCategoryIdOwnerIdAndPokemon_WhenUpdatePokemonIsCalled_ThenReturnsBadRequestWhenModelStateIsInvalid()
        {
            //Arrange
            _controller.ModelState.AddModelError("", "Model state is invalid.");

            //Act
            var result = _controller.UpdatePokemon(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), null);

            //Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
        }

        [Fact]
        public void GivenPokemonIdCategoryIdOwnerIdAndPokemon_WhenUpdatePokemonIsCalled_ThenReturnsOwnerIsNotFound()
        {
            //Arrange
            _ownerRepositry.Setup(x => x.OwnerExists(It.IsAny<int>())).Returns(false);

            //Act
            var result = _controller.UpdatePokemon(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), pokemonDTO);

            //Assert
            var model = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, model.StatusCode);
            Assert.True(_controller.ModelState.IsValid);
        }

        [Fact]
        public void GivenPokemonIdCategoryIdOwnerIdAndPokemon_WhenUpdatePokemonIsCalled_ThenReturnsCategoryIsNotFound()
        {
            //Arrange
            _categoryRepository.Setup(x => x.CategoryExists(It.IsAny<int>())).Returns(false);

            //Act
            var result = _controller.UpdatePokemon(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), pokemonDTO);

            //Assert
            var model = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, model.StatusCode);
            Assert.True(_controller.ModelState.IsValid);
        }

        [Fact]
        public void GivenPokemonIdCategoryIdOwnerIdAndPokemon_WhenUpdatePokemonIsCalled_ThenReturnsPokemonIsNotFound()
        {
            //Arrange
            _pokemonRepository.Setup(x => x.PokemonExists(It.IsAny<int>())).Returns(false);

            //Act
            var result = _controller.UpdatePokemon(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), pokemonDTO);

            //Assert
            var model = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, model.StatusCode);
            Assert.True(_controller.ModelState.IsValid);
        }

        [Fact]
        public void GivenPokemonIdCategoryIdOwnerIdAndPokemon_WhenUpdatePokemonIsCalled_ThenReturnsErrorWhenUpdateFails()
        {
            //Arrange
            _ownerRepositry.Setup(x => x.OwnerExists(It.IsAny<int>()))
               .Returns(true);
            _pokemonRepository.Setup(x => x.PokemonExists(It.IsAny<int>()))
                .Returns(true);
            _categoryRepository.Setup(x => x.CategoryExists(It.IsAny<int>()))
                .Returns(true);
            _mapper.Setup(mapper => mapper.Map<Pokemon>(It.IsAny<PokemonDTO>()))
                .Returns(pokemon);
            _pokemonRepository.Setup(x => x.UpdatePokemon(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Pokemon>()))
                .Returns(false);

            //Act
            var result = _controller.UpdatePokemon(pokemonDTO.Id, It.IsAny<int>(), It.IsAny<int>(), pokemonDTO);

            //Assert
            var model = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, model.StatusCode);
            _ownerRepositry.Verify(x => x.OwnerExists(It.IsAny<int>()), Times.Once);
            _categoryRepository.Verify(x => x.CategoryExists(It.IsAny<int>()), Times.Once);
            _pokemonRepository.Verify(x => x.PokemonExists(It.IsAny<int>()), Times.Once);
            _pokemonRepository.Verify(x => x.UpdatePokemon(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Pokemon>()), Times.Once);
            _mapper.Verify(x => x.Map<Pokemon>(It.IsAny<PokemonDTO>()), Times.Once);
        }
        #endregion

        #region DeletePokemon
        [Fact]
        public void GivenId_WhenDeletePokemonIsCalled_ThenReturnsNoContent()
        {
            //Arrange
            _pokemonRepository.Setup(x => x.PokemonExists(It.IsAny<int>())).Returns(true);
            _pokemonRepository.Setup(x => x.GetPokemon(It.IsAny<int>())).Returns(new Pokemon());
            _pokemonRepository.Setup(x => x.DeletePokemon(It.IsAny<Pokemon>())).Returns(true);

            //Act
            var result = _controller.DeletePokemon(It.IsAny<int>());

            //Assert
            var model = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, model.StatusCode);
            Assert.IsAssignableFrom<NoContentResult>(result);
            _pokemonRepository.Verify(x => x.PokemonExists(It.IsAny<int>()), Times.Once);
            _pokemonRepository.Verify(x => x.GetPokemon(It.IsAny<int>()), Times.Once);
            _pokemonRepository.Verify(x => x.DeletePokemon(It.IsAny<Pokemon>()), Times.Once);
        }

        [Fact]
        public void GivenId_WhenDDeletePokemonIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            _pokemonRepository.Setup(x => x.PokemonExists(It.IsAny<int>())).Returns(false);

            //Act
            var result = _controller.DeletePokemon(It.IsAny<int>());

            //Assert
            var model = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, model.StatusCode);
            Assert.IsAssignableFrom<NotFoundResult>(result);
            _pokemonRepository.Verify(x => x.PokemonExists(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GivenId_WhenDeletePokemonIsCalled_ThenReturnsModelErrorWhenDeleteFails()
        {
            //Arrange
            _pokemonRepository.Setup(x => x.PokemonExists(It.IsAny<int>())).Returns(true);
            _pokemonRepository.Setup(x => x.GetPokemon(It.IsAny<int>())).Returns(new Pokemon());
            _pokemonRepository.Setup(x => x.DeletePokemon(It.IsAny<Pokemon>())).Returns(false);

            //Act
            var result = _controller.DeletePokemon(It.IsAny<int>());

            //Assert
            var model = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, model.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
            _pokemonRepository.Verify(x => x.PokemonExists(It.IsAny<int>()), Times.Once);
            _pokemonRepository.Verify(x => x.GetPokemon(It.IsAny<int>()), Times.Once);
            _pokemonRepository.Verify(x => x.DeletePokemon(It.IsAny<Pokemon>()), Times.Once);
        }
        #endregion
    }
}
