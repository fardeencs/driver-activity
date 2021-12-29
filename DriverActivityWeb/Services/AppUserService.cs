namespace DriverActivityWeb.Services
{
    using AutoMapper;
    using DriverActivityWeb.Contracts;
    using DriverActivityWeb.Data;
    using DriverActivityWeb.Helper;
    using DriverActivityWeb.Models;
    using DriverActivityWeb.ViewModels;
    using Microsoft.EntityFrameworkCore;

    public class AppUserService : IAppUserService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;

        public AppUserService(IMapper mapper,
            ApplicationDbContext dbContext)
        {
            this._mapper = mapper;
            this._dbContext = dbContext;
        }

        public async Task<List<AppUserVM>> GetData(SearchEntity message)
        {
            var query = from m in _dbContext.AppUser
                        join ui in this._dbContext.AppUserImage on m.FileID equals ui.FileID into lui
                        from luid in lui.DefaultIfEmpty()
                        join v in this._dbContext.VehicleType on m.VehicleID equals v.VehicleID into lv
                        from lvd in lv.DefaultIfEmpty()
                        where m.IsDeleted == false
                        select new { m, luid, lvd };

            #region search

            //if(null != message.StaffIds && message.StaffIds.Count > 0)
            //{
            //    query = query.Where(w => message.StaffIds.Contains(w.m.StaffId.Value));
            //}
            if(AppUtility.IsNotEmpty(message.QID))
            {
                query = query.Where(w => w.m.QID == Convert.ToInt32(message.QID));
            }

            #endregion

            var selectQurey = query.Select(x => new AppUserVM
            {
                UserId = x.m.UserId,
                CreatedDate = x.m.CreatedDate,
                Name = x.m.NameEn,
                StaffId = Convert.ToString(x.m.StaffId),
                QID = Convert.ToString(x.m.QID),
                VehicleID = Convert.ToString(x.lvd.VehicleID),
                VehicleName = x.lvd.Vehicle,
                FileID = x.m.FileID,
                ImgContent = x.luid.ImgContent
            });
            var result = await selectQurey.ToListAsync();
            return result;
        }

        public async Task<AppUserVM> SaveOrUpdateData(AppUserVM message)
        {
            if (message == null)
                throw new CustomException("Request param not found");

            DateTime date = DateTime.Now;
            AppUser saveObj = this._mapper.Map<AppUser>(message);

            if (message.UserId.HasValue && message.UserId > 0)
            {
                var updateObj = this._dbContext.AppUser.Where(x => x.UserId == message.UserId
                            && x.IsDeleted == false).FirstOrDefault();
                if (updateObj == null)
                    throw new CustomException("Data not found");

                updateObj.NameEn = message.NameEn;
                updateObj.NameAr = message.NameAr;
                updateObj.StaffId = Convert.ToInt32(message.StaffId);
                updateObj.QID = Convert.ToInt32(message.QID);
                //updateObj.VehicleID = Convert.ToInt32(message.VehicleID);
                updateObj.UpdatedDate = date;
                updateObj.UpdatedBy = message.SessionUserId;

                if(AppUtility.IsNotEmpty(message.FileID))
                {
                    updateObj.FileID = message.FileID;
                }

                this._dbContext.Update(updateObj);

                message.UpdatedDate = date;
                message.UpdatedBy = message.SessionUserId;
            }
            else
            {
                saveObj.StaffId = Convert.ToInt32(message.StaffId);
                saveObj.QID = Convert.ToInt32(message.QID);
                //saveObj.VehicleID = Convert.ToInt32(message.VehicleID);
                saveObj.CreatedBy = message.SessionUserId.Value;
                saveObj.CreatedDate = date;
                await this._dbContext.AddAsync(saveObj);

                if (null != message.FileID)
                {
                    var userImage = this._dbContext.AppUserImage.Where(x => x.FileID == message.FileID
                            && x.IsDeleted == false).FirstOrDefault();
                    if(null != userImage)
                    {
                        userImage.IsActive = true;
                        this._dbContext.Update(userImage);
                    }
                }

                message.FileID = saveObj.FileID;

                message.CreatedDate = date;
                message.CreatedBy = message.SessionUserId.Value;
            }

            await this._dbContext.SaveChangesAsync();


            return message;


        }

        public async Task<bool> ChangeStatus(long id, bool status, long userId)
        {
            bool result = false;

            if (id <= 0)
                throw new KeyNotFoundException("User ID not found");

            var exsitObj = this._dbContext.AppUser.Where(x => x.UserId == id
                            && x.IsDeleted == false).FirstOrDefault();
            if (exsitObj == null)
                throw new BadHttpRequestException("Data not found");

            DateTime date = DateTime.Now;

            exsitObj.IsActive = status;
            exsitObj.UpdatedDate = date;
            exsitObj.UpdatedBy = userId;

            this._dbContext.Update(exsitObj);
            await this._dbContext.SaveChangesAsync();
            result = true;

            return result;
        }

        public async Task<bool> DeleteData(long id, long userId)
        {
            bool result = false;

            if (id <= 0)
                throw new KeyNotFoundException("User ID not found");

            var exsitObj = this._dbContext.AppUser.Where(x => x.UserId == id
                            && x.IsDeleted == false).FirstOrDefault();
            if (exsitObj == null)
                throw new BadHttpRequestException("Dat not found");

            DateTime date = DateTime.Now;

            exsitObj.IsDeleted = true;
            exsitObj.UpdatedDate = date;
            exsitObj.UpdatedBy = userId;

            this._dbContext.Update(exsitObj);
            await this._dbContext.SaveChangesAsync();
            result = true;

            return result;
        }

    }
}
