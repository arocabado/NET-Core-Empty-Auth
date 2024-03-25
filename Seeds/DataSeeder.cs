using server.Data;

namespace server.Seeds
{
    public class DataSeeder
    {
        private readonly DBContext dbContext;
        public DataSeeder(DBContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public void Seed()
        {
            var geo_seeder = new DataSeeder_Geoportal(dbContext);
            geo_seeder.Seed();

            dbContext.Proyectos.AddRange(Seeds_Proyecto.List);
            dbContext.SaveChanges();
        }
    }
}