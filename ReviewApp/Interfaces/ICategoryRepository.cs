using ReviewApp.Models;

namespace ReviewApp.Interfaces
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();

        Category? GetCategory(int id);

        ICollection<Pokemon> GetPokemonsByCategory(int categoryId);

        bool CategoryExists(int categoryId);

        bool CreateCategory(Category category);

        bool UpdateCategory(Category category);
    }
}
