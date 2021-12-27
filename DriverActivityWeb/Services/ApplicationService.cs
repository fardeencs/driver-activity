using AutoMapper;
using DriverActivityWeb.Contracts;
using DriverActivityWeb.Data;
using DriverActivityWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace DriverActivityWeb.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;

        public ApplicationService(IMapper mapper,
            ApplicationDbContext dbContext)
        {
            this._mapper = mapper;
            this._dbContext = dbContext;
        }

        public async Task<SystemSetting> GetSetting(string bgKey)
        {
            var result = await this._dbContext.SystemSetting.Where(x => x.IsDeleted == false && x.BgKey == bgKey)
                            .FirstOrDefaultAsync();
            return result;
        }

        public async Task<IEnumerable<SystemSetting>> GetSetting(List<string> bgKeys)
        {
            var result = await this._dbContext.SystemSetting.Where(x => x.IsDeleted == false && bgKeys.Contains(x.BgKey))
                            .ToListAsync();
            return result;
        }
    }
}
