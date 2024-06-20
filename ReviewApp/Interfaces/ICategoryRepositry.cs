using ReviewApp.Models;

namespace ReviewApp.Interfaces
{
    public interface ICategoryRepositry
    {
        ICollection<Category> GetCategories();

        Category? GetCategory(int id);

        ICollection<Pokemon> GetPokemonsByCategory(int categoryId);

        bool CategoryExists(int categoryId);
    }
}
