using FormulaOne.DataService.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace FormulaOne.DataService.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _appDbContext;
        public IDriverRepository Drivers { get; }
        public IAchievementRepository Achievements { get; }

        public UnitOfWork(
            AppDbContext appDbContext,
            ILoggerFactory loggerFactory)
        {
            _appDbContext = appDbContext;
            var logger = loggerFactory.CreateLogger(categoryName: "logs");
            Drivers = new DriverRepository(_appDbContext, logger);
            Achievements = new AchievementsRepository(_appDbContext, logger);
        }
        public async Task<bool> CompleteAsync()
        {
            var result = await _appDbContext.SaveChangesAsync();
            return result > 0;
        }

        public void Dispose()
        {
            _appDbContext.Dispose();
        }
    }
}
