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
    public class CategoryControllerUnitTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositry;
        private readonly Mock<IMapper> _mapper;
        private readonly CategoryController _controller;
        private static readonly CategoryDTO categoryDTO = new()
        {
            Id = 1,
            Name = "Test",
        };
        public CategoryControllerUnitTests()
        {
            _categoryRepositry = new Mock<ICategoryRepository>();
            _mapper = new Mock<IMapper>();
            _controller = new CategoryController(_categoryRepositry.Object, _mapper.Object);
        }

        #region GetCategories
        [Fact]
        public void GivenNothing_WhenGetCategoriesIsCalled_ThenReturnListOfCategories()
        {
            //Arrange
            List<Category> categories = [new() { Id = categoryDTO.Id, Name = categoryDTO.Name }];
            List<CategoryDTO> categoryDTOs = [categoryDTO];

            _categoryRepositry.Setup(x => x.GetCategories()).Returns(categories);
            _mapper.Setup(mapper => mapper.Map<List<CategoryDTO>>(It.IsAny<IEnumerable<Category>>()))
                .Returns(categoryDTOs);

            //Act
            var result = _controller.GetCategories();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualCategories = Assert.IsType<List<CategoryDTO>>(okResult.Value);

            Assert.NotNull(result);
            Assert.Equal(JsonSerializer.Serialize(categoryDTOs), JsonSerializer.Serialize(actualCategories));
            _categoryRepositry.Verify(x => x.GetCategories(), Times.Once);
        }

        [Fact]
        public void GivenNothing_WhenGetCategoriesIsCalled_ThenReturnsBadRequest()
        {
            //Arrange
            List<Category> categories = [new() { Id = categoryDTO.Id, Name = categoryDTO.Name }];
            List<CategoryDTO> categoryDTOs = [categoryDTO];

            _categoryRepositry.Setup(x => x.GetCategories()).Returns(categories);
            _mapper.Setup(mapper => mapper.Map<List<CategoryDTO>>(It.IsAny<IEnumerable<Category>>()))
                .Returns(categoryDTOs);
            _controller.ModelState.AddModelError("", "Model is not valid");

            //Act
            var result = _controller.GetCategories();

            //Assert
            var okResult = Assert.IsType<BadRequestResult>(result);
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
            _categoryRepositry.Verify(x => x.GetCategories(), Times.Once);
        }
        #endregion

        #region GetCategory
        [Fact]
        public void GivenCategoryId_WhenGetCategoryIsCalled_ThenReturnsCategory()
        {
            //Arrange
            _categoryRepositry.Setup(x => x.CategoryExists(categoryDTO.Id)).Returns(true);
            _categoryRepositry.Setup(x => x.GetCategory(categoryDTO.Id)).Returns(It.IsAny<Category>());
            _mapper.Setup(mapper => mapper.Map<CategoryDTO>(It.IsAny<Category>()))
                .Returns(categoryDTO);

            //Act
            var result = _controller.GetCategory(categoryDTO.Id);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<CategoryDTO>(okResult.Value);

            Assert.NotNull(result);
            Assert.Equal(JsonSerializer.Serialize(categoryDTO), JsonSerializer.Serialize(actual));
            _categoryRepositry.Verify(x => x.GetCategory(categoryDTO.Id), Times.Once);
            _categoryRepositry.Verify(x => x.CategoryExists(categoryDTO.Id), Times.Once);
        }

        [Fact]
        public void GivenCategoryId_WhenGetCategoryIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            _categoryRepositry.Setup(x => x.CategoryExists(categoryDTO.Id)).Returns(false);

            //Act
            var result = _controller.GetCategory(categoryDTO.Id);

            //Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.NotNull(result);
            _categoryRepositry.Verify(x => x.CategoryExists(categoryDTO.Id), Times.Once);
        }

        [Fact]
        public void GivenCategoryId_WhenGetCategoryIsCalled_ThenReturnsBadRequest()
        {
            //Arrange
            _categoryRepositry.Setup(x => x.CategoryExists(categoryDTO.Id)).Returns(true);
            _categoryRepositry.Setup(x => x.GetCategory(categoryDTO.Id)).Returns(It.IsAny<Category>());
            _mapper.Setup(mapper => mapper.Map<CategoryDTO>(It.IsAny<Category>()))
                .Returns(categoryDTO);
            _controller.ModelState.AddModelError("", "Model state is invalid.");

            //Act
            var result = _controller.GetCategory(categoryDTO.Id);

            //Assert
            var okResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
            _categoryRepositry.Verify(x => x.GetCategory(categoryDTO.Id), Times.Once);
            _categoryRepositry.Verify(x => x.CategoryExists(categoryDTO.Id), Times.Once);
        }
        #endregion

        #region GetPokemonByCategoryId
        [Fact]
        public void GivenCategoryId_WhenGetPokemonByCategoryIdIsCalled_ThenReturnsListOfPokemons()
        {
            //Arrange
            List<PokemonDTO> pokemons = [new() { }];

            _categoryRepositry.Setup(x => x.CategoryExists(categoryDTO.Id)).Returns(true);
            _categoryRepositry.Setup(x => x.GetPokemonsByCategory(categoryDTO.Id))
                .Returns(It.IsAny<List<Pokemon>>());
            _mapper.Setup(mapper => mapper.Map<List<PokemonDTO>>(It.IsAny<IEnumerable<Pokemon>>()))
                .Returns(pokemons);

            //Act
            var result = _controller.GetPokemonsByCategory(categoryDTO.Id);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<List<PokemonDTO>>(okResult.Value);

            Assert.NotNull(result);
            Assert.Equal(JsonSerializer.Serialize(pokemons), JsonSerializer.Serialize(actual));
            _categoryRepositry.Verify(x => x.GetPokemonsByCategory(categoryDTO.Id), Times.Once);
            _categoryRepositry.Verify(x => x.CategoryExists(categoryDTO.Id), Times.Once);
        }

        [Fact]
        public void GivenCategoryId_WhenGetPokemonsByCategoryIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            _categoryRepositry.Setup(x => x.CategoryExists(categoryDTO.Id)).Returns(false);

            //Act
            var result = _controller.GetPokemonsByCategory(categoryDTO.Id);

            //Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.NotNull(result);
            _categoryRepositry.Verify(x => x.CategoryExists(categoryDTO.Id), Times.Once);
        }

        [Fact]
        public void GivenCategoryId_WhenGetPokemonsByCategoryIsCalled_ThenReturnsBadRequest()
        {
            //Arrange
            List<PokemonDTO> pokemons = [new() { }];

            _categoryRepositry.Setup(x => x.CategoryExists(categoryDTO.Id)).Returns(true);
            _categoryRepositry.Setup(x => x.GetPokemonsByCategory(categoryDTO.Id))

                .Returns(It.IsAny<List<Pokemon>>());
            _mapper.Setup(mapper => mapper.Map<List<PokemonDTO>>(It.IsAny<IEnumerable<Pokemon>>()))
                .Returns(pokemons);
            _controller.ModelState.AddModelError("", "Model state is invalid.");

            //Act
            var result = _controller.GetPokemonsByCategory(categoryDTO.Id);

            //Assert
            var okResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
            _categoryRepositry.Verify(x => x.GetPokemonsByCategory(categoryDTO.Id), Times.Once);
            _categoryRepositry.Verify(x => x.CategoryExists(categoryDTO.Id), Times.Once);
        }
        #endregion

        #region CreateCategory
        [Fact]
        public void GivenCategory_WhenCreateCategoryIsCalled_ThenReturnsOk()
        {
            //Arrange
            var category = new Category { Id = 1, Name = "New Category" };
            _categoryRepositry.Setup(x => x.GetCategories()).Returns([category]);
            _mapper.Setup(mapper => mapper.Map<CategoryDTO>(It.IsAny<Category>()))
                .Returns(categoryDTO);
            _categoryRepositry.Setup(x => x.CreateCategory(It.IsAny<Category>())).Returns(true);

            //Act
            var result = _controller.CreateCategory(categoryDTO);

            //Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(result);
            _categoryRepositry.Verify(x => x.GetCategories(), Times.Once);
            _categoryRepositry.Verify(x => x.CreateCategory(It.IsAny<Category>()), Times.Once);
        }

        [Fact]
        public void GivenCategory_WhenCreateCategoryIsCalled_ThenReturnsBadRequestWhenCategoryIsNull()
        {
            //Arrange

            //Act
            var result = _controller.CreateCategory(null);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public void GivenCategory_WhenCreateCategoryIsCalled_ThenReturnsBadRequestWhenModelStateIsInvalid()
        {
            //Arrange
            _controller.ModelState.AddModelError("", "Model state is invalid.");

            //Act
            var result = _controller.CreateCategory(categoryDTO);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
        }

        [Fact]
        public void GivenCategory_WhenCreateCategoryIsCalled_ThenReturnsModelErrorWhenCategoryAlreadyExists()
        {
            //Arrange
            var category = new Category { Id = 2, Name = "Test" };
            _categoryRepositry.Setup(x => x.GetCategories()).Returns([category]);

            //Act
            var result = _controller.CreateCategory(categoryDTO);

            //Assert
            var model = Assert.IsType<ObjectResult>(result);
            Assert.NotNull(result);
            Assert.Equal(422, model.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
            _categoryRepositry.Verify(x => x.GetCategories(), Times.Once);
        }

        [Fact]
        public void GivenCategory_WhenCreateCategoryIsCalled_ThenReturnsServerError()
        {
            //Arrange
            var category = new Category { Id = 2, Name = "new Category" };
            _categoryRepositry.Setup(x => x.GetCategories()).Returns([category]);
            _mapper.Setup(mapper => mapper.Map<CategoryDTO>(It.IsAny<Category>()))
                .Returns(categoryDTO);
            _categoryRepositry.Setup(x => x.CreateCategory(It.IsAny<Category>())).Returns(false);

            //Act
            var result = _controller.CreateCategory(categoryDTO);

            //Assert
            var model = Assert.IsType<ObjectResult>(result);
            Assert.NotNull(result);
            Assert.Equal(500, model.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
            _categoryRepositry.Verify(x => x.GetCategories(), Times.Once);
            _categoryRepositry.Verify(x => x.CreateCategory(It.IsAny<Category>()), Times.Once);
        }
        #endregion

        #region UpdateCategory
        [Fact]
        public void GivenIdAndCategory_WhenUpdateCategoryIsCalled_ThenReturnsNoContent()
        {
            //Arrange
            var category = new Category { Id = 1, Name = "New Category" };
            _categoryRepositry.Setup(x => x.CategoryExists(category.Id)).Returns(true);
            _mapper.Setup(mapper => mapper.Map<CategoryDTO>(It.IsAny<Category>()))
                .Returns(categoryDTO);
            _categoryRepositry.Setup(x => x.UpdateCategory(It.IsAny<Category>()))
                .Returns(true);

            //Act
            var result = _controller.UpdateCategory(category.Id, categoryDTO);

            //Assert
            Assert.IsType<NoContentResult>(result);
            Assert.NotNull(result);
            _categoryRepositry.Verify(x => x.CategoryExists(category.Id), Times.Once);
            _categoryRepositry.Verify(x => x.UpdateCategory(It.IsAny<Category>()), Times.Once);
        }

        [Fact]
        public void GivenIdAndCategory_WhenUpdateCategoryIsCalled_ThenReturnsBadRequestWhenCategoryIsNull()
        {
            //Arrange

            //Act
            var result = _controller.UpdateCategory(1, null);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public void GivenIdAndCategory_WhenUpdateCategoryIsCalled_ThenReturnsBadRequestWhenModelStateIsInvalid()
        {
            //Arrange
            _controller.ModelState.AddModelError("", "Model state is invalid.");

            //Act
            var result = _controller.UpdateCategory(1, categoryDTO);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
        }

        [Fact]
        public void GivenIdAndCategory_WhenUpdateCategoryIsCalled_ThenReturnsCategoryIsNotFound()
        {
            //Arrange
            var category = new Category { Id = categoryDTO.Id, Name = "Test" };
            _categoryRepositry.Setup(x => x.CategoryExists(212)).Returns(false);

            //Act
            var result = _controller.UpdateCategory(category.Id, categoryDTO);

            //Assert
            var model = Assert.IsType<NotFoundObjectResult>(result);
            Assert.NotNull(result);
            Assert.Equal(404, model.StatusCode);
            Assert.True(_controller.ModelState.IsValid);
        }

        [Fact]
        public void GivenIdAndCategory_WhenUpdateCategoryIsCalled_ThenReturnsErrorWhenUpdateFails()
        {
            //Arrange
            var category = new Category { Id = categoryDTO.Id, Name = "new Category" };
            _categoryRepositry.Setup(x => x.CategoryExists(category.Id)).Returns(true);
            _mapper.Setup(mapper => mapper.Map<CategoryDTO>(It.IsAny<Category>()))
                .Returns(categoryDTO);
            _categoryRepositry.Setup(x => x.UpdateCategory(It.IsAny<Category>())).Returns(false);

            //Act
            var result = _controller.UpdateCategory(category.Id, categoryDTO);

            //Assert
            var model = Assert.IsType<ObjectResult>(result);
            Assert.NotNull(result);
            Assert.Equal(500, model.StatusCode);
            _categoryRepositry.Verify(x => x.CategoryExists(category.Id), Times.Once);
            _categoryRepositry.Verify(x => x.UpdateCategory(It.IsAny<Category>()), Times.Once);
        }
        #endregion

        #region DeleteCategory
        [Fact]
        public void GivenId_WhenDeleteCategoryIsCalled_ThenReturnsNoContent()
        {
            //Arrange
            var category = new Category { Id = 1, Name = "New Category" };

            _categoryRepositry.Setup(x => x.CategoryExists(category.Id)).Returns(true);
            _categoryRepositry.Setup(x => x.GetCategory(1)).Returns(category);

            //Act
            var result = _controller.DeleteCategory(category.Id);

            //Assert
            var model = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, model.StatusCode);
            Assert.NotNull(result);
            Assert.IsAssignableFrom<ObjectResult>(result);
            _categoryRepositry.Verify(x => x.CategoryExists(category.Id), Times.Once);
            _categoryRepositry.Verify(x => x.GetCategory(1), Times.Once);
        }

        [Fact]
        public void GivenId_WhenDeleteCategoryIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            _categoryRepositry.Setup(x => x.CategoryExists(1)).Returns(false);

            //Act
            var result = _controller.DeleteCategory(1);

            //Assert
            var model = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, model.StatusCode);
            Assert.NotNull(result);
            Assert.IsAssignableFrom<NotFoundResult>(result);
            _categoryRepositry.Verify(x => x.CategoryExists(1), Times.Once);
        }

        [Fact]
        public void GivenId_WhenDeleteCategoryIsCalled_ThenReturnsModelError()
        {
            //Arrange
            var category = new Category { Id = 1, Name = "New Category" };

            _categoryRepositry.Setup(x => x.CategoryExists(category.Id)).Returns(true);
            _categoryRepositry.Setup(x => x.GetCategory(category.Id)).Returns(category);
            _controller.ModelState.AddModelError("", "Something wrong with the model");

            //Act
            var result = _controller.DeleteCategory(category.Id);

            //Assert
            var model = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, model.StatusCode);
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
            _categoryRepositry.Verify(x => x.CategoryExists(category.Id), Times.Once);
            _categoryRepositry.Verify(x => x.GetCategory(category.Id), Times.Once);
        }

        [Fact]
        public void GivenId_WhenDeleteCategoryIsCalled_ThenReturnsModelErrorWhenDeleteFails()
        {
            //Arrange
            var category = new Category { Id = 1, Name = "New Category" };

            _categoryRepositry.Setup(x => x.CategoryExists(category.Id)).Returns(true);
            _categoryRepositry.Setup(x => x.GetCategory(category.Id)).Returns(category);
            _categoryRepositry.Setup(x => x.DeleteCategory(category)).Returns(false);

            //Act
            var result = _controller.DeleteCategory(category.Id);

            //Assert
            var model = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, model.StatusCode);
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
            _categoryRepositry.Verify(x => x.CategoryExists(category.Id), Times.Once);
            _categoryRepositry.Verify(x => x.GetCategory(category.Id), Times.Once);
            _categoryRepositry.Verify(x => x.DeleteCategory(category), Times.Once);
        }
        #endregion
    }
}
