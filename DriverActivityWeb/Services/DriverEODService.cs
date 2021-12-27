using AutoMapper;
using DriverActivityWeb.Contracts;
using DriverActivityWeb.Data;
using DriverActivityWeb.Helper;
using DriverActivityWeb.Models;
using DriverActivityWeb.ViewModels;

namespace DriverActivityWeb.Services
{
    public class DriverEODService : IDriverEODService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;

        public DriverEODService(IMapper mapper,
            ApplicationDbContext dbContext)
        {
            this._mapper = mapper;
            this._dbContext = dbContext;
        }

        public async Task<DriverEodVM> SaveOrUpdateData(DriverEodVM message)
        {
            if (message == null)
                throw new CustomException("Request param not found");

            DateTime date = DateTime.Now;
            DriverEod saveObj = this._mapper.Map<DriverEod>(message);

            if (message.DriverEodID.HasValue && message.DriverEodID > 0)
            {
                var updateObj = this._dbContext.DriverEod.Where(x => x.DriverEodID == message.DriverEodID
                            && x.IsDeleted == false).FirstOrDefault();
                if (updateObj == null)
                    throw new CustomException("Data not found");

                updateObj.AdditionalDelivery = message.AdditionalDelivery;
                updateObj.Remarks = message.Remarks;
                updateObj.TotalDelivery = message.TotalDelivery;
                updateObj.Delivered = message.Delivered;
                updateObj.Drops = message.Drops;
                updateObj.FailedDelivery = message.FailedDelivery;
                updateObj.UpdatedDate = date;

                updateObj.UpdatedBy = message.SessionUserId;

                if (AppUtility.IsNotEmpty(message.FileID))
                {
                    updateObj.FileID = message.FileID;
                }

                this._dbContext.Update(updateObj);

                message.UpdatedDate = date;
                message.UpdatedBy = message.SessionUserId;
            }
            else
            {
                saveObj.CreatedBy = message.SessionUserId.Value;
                saveObj.CreatedDate = date;
                await this._dbContext.AddAsync(saveObj);

                if (null != message.FileID)
                {
                    var userSign = this._dbContext.AppUserSignature.Where(x => x.FileID == message.FileID
                            && x.IsDeleted == false).FirstOrDefault();
                    if (null != userSign)
                    {
                        userSign.IsActive = true;
                        this._dbContext.Update(userSign);
                    }
                }

                message.FileID = saveObj.FileID;

                message.CreatedDate = date;
                message.CreatedBy = message.SessionUserId.Value;
            }

            await this._dbContext.SaveChangesAsync();


            return message;


        }

    }
}
