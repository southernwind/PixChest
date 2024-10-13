using PixChest.Database;
using PixChest.Database.Tables;

namespace PixChest;

public static class InitialDataRegisterer
{
	public static void Register(PixChestDbContext db) {
		using var transaction = db.Database.BeginTransaction();
		if(db.TagCategories.Count() != 0){
			return;
		}
		db.TagCategories.AddRange([
			new TagCategory() { TagCategoryId = -1, TagCategoryName = "No Category", Tags = [], Detail = "No Category Tags" }
		]);
		transaction.Commit();
		db.SaveChanges();
	}
}
