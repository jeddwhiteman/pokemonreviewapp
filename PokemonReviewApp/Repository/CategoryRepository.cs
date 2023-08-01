using Microsoft.EntityFrameworkCore.Query.Internal;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDBContext _db;
        public CategoryRepository(ApplicationDBContext Db)
        {
            _db = Db;
        }
        public bool CategoryExists(int id)
        {
            return _db.Categories.Any(c => c.Id == id);
        }

        public ICollection<Category> GetCategories()
        {
            return _db.Categories.OrderBy(c => c.Id).ToList();
        }

        public Category GetCategory(int categoryId)
        {
            return _db.Categories.Where(c => c.Id == categoryId).FirstOrDefault();
        }

        public Category GetCategory(string name)
        {
            return _db.Categories.Where(c => c.CategoryName == name).FirstOrDefault();
        }

        public ICollection<Pokemon> GetPokemonByCategory(int categoryId)
        {
            return _db.PokemonCategories.Where(pc => pc.CategoryId == categoryId).Select(p => p.Pokemon).ToList();
        }
    }
}
