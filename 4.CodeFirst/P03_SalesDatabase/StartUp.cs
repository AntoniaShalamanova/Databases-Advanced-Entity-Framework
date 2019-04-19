using P03_SalesDatabase.Data;
using P03_SalesDatabase.Data.Models;

namespace P03_SalesDatabase
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var db = new SalesContext())
            {
                Store store = new Store
                {
                    Name = "fruitShop"
                };

                db.Stores.Add(store);

                db.SaveChanges();
            }
        }
    }
}
