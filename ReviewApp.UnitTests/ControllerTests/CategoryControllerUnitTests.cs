using AutoMapper;
using Microsoft.AspNetCore.Http;
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
            List<CategoryDTO> categoryDTOs = [categoryDTO];
            _categoryRepositry.Setup(x => x.GetCategories())
                .Returns([]);
            _mapper.Setup(mapper => mapper.Map<List<CategoryDTO>>(It.IsAny<IEnumerable<Category>>()))
                .Returns(categoryDTOs);

            //Act
            var result = _controller.GetCategories();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<List<CategoryDTO>>(okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(JsonSerializer.Serialize(categoryDTOs), JsonSerializer.Serialize(actual));
            _categoryRepositry.Verify(x => x.GetCategories(), Times.Once);
            _mapper.Verify(x => x.Map<List<CategoryDTO>>(It.IsAny<IEnumerable<Category>>()), Times.Once);
        }

        [Fact]
        public void GivenNothing_WhenGetCategoriesIsCalled_ThenReturnsBadRequest()
        {
            //Arrange
            _controller.ModelState.AddModelError("", "Model is not valid");

            //Act
            var result = _controller.GetCategories();

            //Assert
            var badRequestResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
        }
        #endregion

        #region GetCategory
        [Fact]
        public void GivenCategoryId_WhenGetCategoryIsCalled_ThenReturnsCategory()
        {
            //Arrange
            _categoryRepositry.Setup(x => x.CategoryExists(It.IsAny<int>()))
                .Returns(true);
            _categoryRepositry.Setup(x => x.GetCategory(It.IsAny<int>()))
                .Returns(new Category());
            _mapper.Setup(mapper => mapper.Map<CategoryDTO>(It.IsAny<Category>()))
                .Returns(categoryDTO);

            //Act
            var result = _controller.GetCategory(It.IsAny<int>());

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<CategoryDTO>(okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(JsonSerializer.Serialize(categoryDTO), JsonSerializer.Serialize(actual));
            _categoryRepositry.Verify(x => x.GetCategory(It.IsAny<int>()), Times.Once);
            _categoryRepositry.Verify(x => x.CategoryExists(It.IsAny<int>()), Times.Once);
            _mapper.Verify(x => x.Map<CategoryDTO>(It.IsAny<Category>()), Times.Once);
        }

        [Fact]
        public void GivenCategoryId_WhenGetCategoryIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            _categoryRepositry.Setup(x => x.CategoryExists(It.IsAny<int>())).Returns(false);

            //Act
            var result = _controller.GetCategory(It.IsAny<int>());

            //Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
            _categoryRepositry.Verify(x => x.CategoryExists(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GivenCategoryId_WhenGetCategoryIsCalled_ThenReturnsBadRequest()
        {
            //Arrange
            _categoryRepositry.Setup(x => x.CategoryExists(It.IsAny<int>()))
                .Returns(true);
            _controller.ModelState.AddModelError("", "Model state is invalid.");

            //Act
            var result = _controller.GetCategory(It.IsAny<int>());

            //Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(badRequestObjectResult.StatusCode, 400);
            Assert.False(_controller.ModelState.IsValid);
            _categoryRepositry.Verify(x => x.CategoryExists(It.IsAny<int>()), Times.Once);
        }
        #endregion

        #region GetPokemonByCategoryId
        [Fact]
        public void GivenCategoryId_WhenGetPokemonByCategoryIdIsCalled_ThenReturnsListOfPokemons()
        {
            //Arrange
            List<PokemonDTO> pokemons = [new() { }];

            _categoryRepositry.Setup(x => x.CategoryExists(It.IsAny<int>())).Returns(true);
            _categoryRepositry.Setup(x => x.GetPokemonsByCategory(It.IsAny<int>()))
                .Returns([]);
            _mapper.Setup(mapper => mapper.Map<List<PokemonDTO>>(It.IsAny<List<Pokemon>>()))
                .Returns(pokemons);

            //Act
            var result = _controller.GetPokemonsByCategory(It.IsAny<int>());

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var actual = Assert.IsType<List<PokemonDTO>>(okResult.Value);
            Assert.Equal(JsonSerializer.Serialize(pokemons), JsonSerializer.Serialize(actual));
            _categoryRepositry.Verify(x => x.GetPokemonsByCategory(It.IsAny<int>()), Times.Once);
            _categoryRepositry.Verify(x => x.CategoryExists(It.IsAny<int>()), Times.Once);
            _mapper.Verify(x => x.Map<List<PokemonDTO>>(It.IsAny<List<Pokemon>>()), Times.Once);
        }

        [Fact]
        public void GivenCategoryId_WhenGetPokemonsByCategoryIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            _categoryRepositry.Setup(x => x.CategoryExists(It.IsAny<int>())).Returns(false);

            //Act
            var result = _controller.GetPokemonsByCategory(It.IsAny<int>());

            //Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
            _categoryRepositry.Verify(x => x.CategoryExists(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GivenCategoryId_WhenGetPokemonsByCategoryIsCalled_ThenReturnsBadRequest()
        {
            //Arrange
            _categoryRepositry.Setup(x => x.CategoryExists(It.IsAny<int>()))
                .Returns(true);
            _controller.ModelState.AddModelError("", "Model state is invalid.");

            //Act
            var result = _controller.GetPokemonsByCategory(It.IsAny<int>());

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
            _categoryRepositry.Verify(x => x.CategoryExists(It.IsAny<int>()), Times.Once);
        }
        #endregion

        #region CreateCategory
        [Fact]
        public void GivenCategory_WhenCreateCategoryIsCalled_ThenReturnsOk()
        {
            //Arrange
            _categoryRepositry.Setup(x => x.GetCategories())
                .Returns([]);
            _mapper.Setup(mapper => mapper.Map<Category>(It.IsAny<CategoryDTO>()))
                .Returns(new Category());
            _categoryRepositry.Setup(x => x.CreateCategory(It.IsAny<Category>()))
                .Returns(true);

            //Act
            var result = _controller.CreateCategory(categoryDTO);

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);
            _categoryRepositry.Verify(x => x.GetCategories(), Times.Once);
            _categoryRepositry.Verify(x => x.CreateCategory(It.IsAny<Category>()), Times.Once);
            _mapper.Verify(x => x.Map<Category>(It.IsAny<CategoryDTO>()), Times.Once);
        }

        [Fact]
        public void GivenCategory_WhenCreateCategoryIsCalled_ThenReturnsBadRequestWhenCategoryIsNull()
        {
            //Arrange

            //Act
            var result = _controller.CreateCategory(null);

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public void GivenCategory_WhenCreateCategoryIsCalled_ThenReturnsBadRequestWhenModelStateIsInvalid()
        {
            //Arrange
            _controller.ModelState.AddModelError("", "Model state is invalid.");

            //Act
            var result = _controller.CreateCategory(categoryDTO);

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
        }

        [Fact]
        public void GivenCategory_WhenCreateCategoryIsCalled_ThenReturnsModelErrorWhenCategoryAlreadyExists()
        {
            //Arrange
            List<Category> categories = [new Category() { Id = 1, Name = "Test" }];
            _categoryRepositry.Setup(x => x.GetCategories()).Returns(categories);

            //Act
            var result = _controller.CreateCategory(categoryDTO);

            //Assert
            var model = Assert.IsType<ObjectResult>(result);
            Assert.Equal(422, model.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
            _categoryRepositry.Verify(x => x.GetCategories(), Times.Once);
        }

        [Fact]
        public void GivenCategory_WhenCreateCategoryIsCalled_ThenReturnsServerError()
        {
            //Arrange
            List<Category> categories = [new Category() { Id = 1, Name = "Test 2" }];
            _categoryRepositry.Setup(x => x.GetCategories())
                .Returns(categories);
            _mapper.Setup(mapper => mapper.Map<Category>(It.IsAny<CategoryDTO>()))
                .Returns(new Category());
            _categoryRepositry.Setup(x => x.CreateCategory(It.IsAny<Category>()))
                .Returns(false);

            //Act
            var result = _controller.CreateCategory(categoryDTO);

            //Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
            _categoryRepositry.Verify(x => x.GetCategories(), Times.Once);
            _categoryRepositry.Verify(x => x.CreateCategory(It.IsAny<Category>()), Times.Once);
            _mapper.Verify(x => x.Map<Category>(It.IsAny<CategoryDTO>()), Times.Once);
        }
        #endregion

        #region UpdateCategory
        [Fact]
        public void GivenIdAndCategory_WhenUpdateCategoryIsCalled_ThenReturnsNoContent()
        {
            //Arrange
            var category = new Category { Id = 1, Name = "New Category" };
            _categoryRepositry.Setup(x => x.CategoryExists(category.Id))
                .Returns(true);
            _mapper.Setup(mapper => mapper.Map<Category>(It.IsAny<CategoryDTO>()))
                .Returns(category);
            _categoryRepositry.Setup(x => x.UpdateCategory(It.IsAny<Category>()))
                .Returns(true);

            //Act
            var result = _controller.UpdateCategory(category.Id, categoryDTO);

            //Assert
            var noContent = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContent.StatusCode);
            _categoryRepositry.Verify(x => x.CategoryExists(category.Id), Times.Once);
            _categoryRepositry.Verify(x => x.UpdateCategory(It.IsAny<Category>()), Times.Once);
            _mapper.Verify(x => x.Map<Category>(It.IsAny<CategoryDTO>()), Times.Once);
        }

        [Fact]
        public void GivenIdAndCategory_WhenUpdateCategoryIsCalled_ThenReturnsBadRequestWhenCategoryIsNull()
        {
            //Arrange

            //Act
            var result = _controller.UpdateCategory(1, null);

            //Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
        }

        [Fact]
        public void GivenIdAndCategory_WhenUpdateCategoryIsCalled_ThenReturnsBadRequestWhenModelStateIsInvalid()
        {
            //Arrange
            _controller.ModelState.AddModelError("", "Model state is invalid.");

            //Act
            var result = _controller.UpdateCategory(categoryDTO.Id, categoryDTO);

            //Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
        }

        [Fact]
        public void GivenIdAndCategory_WhenUpdateCategoryIsCalled_ThenReturnsCategoryIsNotFound()
        {
            //Arrange
            _categoryRepositry.Setup(x => x.CategoryExists(It.IsAny<int>())).Returns(false);

            //Act
            var result = _controller.UpdateCategory(categoryDTO.Id, categoryDTO);

            //Assert
            var model = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, model.StatusCode);
            Assert.True(_controller.ModelState.IsValid);
        }

        [Fact]
        public void GivenIdAndCategory_WhenUpdateCategoryIsCalled_ThenReturnsErrorWhenUpdateFails()
        {
            //Arrange
            _categoryRepositry.Setup(x => x.CategoryExists(It.IsAny<int>()))
                .Returns(true);
            _mapper.Setup(mapper => mapper.Map<Category>(It.IsAny<CategoryDTO>()))
                .Returns(new Category());
            _categoryRepositry.Setup(x => x.UpdateCategory(It.IsAny<Category>()))
                .Returns(false);

            //Act
            var result = _controller.UpdateCategory(categoryDTO.Id, categoryDTO);

            //Assert
            var model = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, model.StatusCode);
            _categoryRepositry.Verify(x => x.CategoryExists(It.IsAny<int>()), Times.Once);
            _categoryRepositry.Verify(x => x.UpdateCategory(It.IsAny<Category>()), Times.Once);
            _mapper.Verify(x => x.Map<Category>(It.IsAny<CategoryDTO>()), Times.Once);
        }
        #endregion

        #region DeleteCategory
        [Fact]
        public void GivenId_WhenDeleteCategoryIsCalled_ThenReturnsNoContent()
        {
            //Arrange
            _categoryRepositry.Setup(x => x.CategoryExists(It.IsAny<int>())).Returns(true);
            _categoryRepositry.Setup(x => x.GetCategory(It.IsAny<int>())).Returns(new Category());

            //Act
            var result = _controller.DeleteCategory(It.IsAny<int>());

            //Assert
            var model = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, model.StatusCode);
            Assert.IsAssignableFrom<ObjectResult>(result);
            _categoryRepositry.Verify(x => x.CategoryExists(It.IsAny<int>()), Times.Once);
            _categoryRepositry.Verify(x => x.GetCategory(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GivenId_WhenDeleteCategoryIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            _categoryRepositry.Setup(x => x.CategoryExists(It.IsAny<int>())).Returns(false);

            //Act
            var result = _controller.DeleteCategory(It.IsAny<int>());

            //Assert
            var model = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, model.StatusCode);
            Assert.IsAssignableFrom<NotFoundResult>(result);
            _categoryRepositry.Verify(x => x.CategoryExists(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GivenId_WhenDeleteCategoryIsCalled_ThenReturnsModelErrorWhenDeleteFails()
        {
            //Arrange
            _categoryRepositry.Setup(x => x.CategoryExists(It.IsAny<int>())).Returns(true);
            _categoryRepositry.Setup(x => x.GetCategory(It.IsAny<int>())).Returns(new Category());
            _categoryRepositry.Setup(x => x.DeleteCategory(It.IsAny<Category>())).Returns(false);

            //Act
            var result = _controller.DeleteCategory(It.IsAny<int>());

            //Assert
            var model = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, model.StatusCode);
            Assert.False(_controller.ModelState.IsValid);
            _categoryRepositry.Verify(x => x.CategoryExists(It.IsAny<int>()), Times.Once);
            _categoryRepositry.Verify(x => x.GetCategory(It.IsAny<int>()), Times.Once);
            _categoryRepositry.Verify(x => x.DeleteCategory(It.IsAny<Category>()), Times.Once);
        }
        #endregion
    }
}
