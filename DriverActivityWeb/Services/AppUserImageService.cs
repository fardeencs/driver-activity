﻿using AutoMapper;
using DriverActivityWeb.Contracts;
using DriverActivityWeb.Data;
using DriverActivityWeb.Helper;
using DriverActivityWeb.Models;
using DriverActivityWeb.ViewModels;

namespace DriverActivityWeb.Services
{
    public class AppUserImageService : IAppUserImageService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;

        public AppUserImageService(IMapper mapper,
            ApplicationDbContext dbContext)
        {
            this._mapper = mapper;
            this._dbContext = dbContext;
        }

        private string GetFileId()
        {
            Guid obj = Guid.NewGuid();
            return obj.ToString();
        }

        public async Task<AppUserImageVM> SaveOrUpdateData(AppUserImageVM message)
        {
            if (message == null)
                throw new CustomException("Request param not found");

            DateTime date = DateTime.Now;
            AppUserImage saveObj = this._mapper.Map<AppUserImage>(message);

            if (AppUtility.IsNotEmpty(message.FileID))
            {
                var updateObj = this._dbContext.AppUserImage.Where(x => x.FileID == message.FileID
                            && x.IsDeleted == false).FirstOrDefault();
                if (updateObj == null)
                    throw new CustomException("Data not found");

                updateObj.ImgContent = message.ImgContent;
                updateObj.UpdatedDate = date;
                updateObj.UpdatedBy = message.SessionUserId;

                this._dbContext.Update(updateObj);

                message.UpdatedDate = date;
                message.UpdatedBy = message.SessionUserId;
            }
            else
            {
                saveObj.FileID = GetFileId();
                saveObj.CreatedBy = message.SessionUserId.Value;
                saveObj.CreatedDate = date;
                await this._dbContext.AddAsync(saveObj);
                message.FileID = saveObj.FileID;

                message.CreatedDate = date;
                message.CreatedBy = message.SessionUserId.Value;
            }

            await this._dbContext.SaveChangesAsync();
            message.File = null;

            return message;


        }

        public async Task<bool> ChangeStatus(string fileID, bool status, long userId)
        {
            bool result = false;

            if (string.IsNullOrWhiteSpace(fileID) || string.IsNullOrEmpty(fileID))
                throw new KeyNotFoundException("File ID not found");

            var exsitObj = this._dbContext.AppUserImage.Where(x => x.FileID == fileID
                            && x.IsDeleted == false).FirstOrDefault();
            if (exsitObj == null)
                throw new BadHttpRequestException("Dat not found");

            DateTime date = DateTime.Now;

            exsitObj.IsActive = status;
            exsitObj.UpdatedDate = date;
            exsitObj.UpdatedBy = userId;

            this._dbContext.Update(exsitObj);
            await this._dbContext.SaveChangesAsync();
            result = true;

            return result;
        }

        public async Task<bool> DeleteData(string fileID, long userId)
        {
            bool result = false;

            if (string.IsNullOrWhiteSpace(fileID) || string.IsNullOrEmpty(fileID))
                throw new KeyNotFoundException("File ID not found");

            var exsitObj = this._dbContext.AppUserImage.Where(x => x.FileID == fileID
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
