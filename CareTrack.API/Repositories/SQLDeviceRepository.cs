using CareTrack.API.Data;
using CareTrack.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CareTrack.API.Repositories
{
    public class SQLDeviceRepository : IDeviceRepository
    {
        private readonly CareTrackDbcontext dbContext;

        public SQLDeviceRepository(CareTrackDbcontext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Device>> GetAllAsync(int pageNumber = 1, int pageSize = 1000)
        {
            var devices = dbContext.Devices.AsQueryable();
            // Pagination
            var skipResults = (pageNumber - 1) * pageSize;

            return await devices.Skip(skipResults).Take(pageSize).ToListAsync();


        }

        public async Task<Device?> GetByIdAsync(Guid id)
        {
            return await dbContext.Devices.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Device> CreateAsync(Device device)
        {
            await dbContext.Devices.AddAsync(device);
            await dbContext.SaveChangesAsync();
            return device;
        }

        public async Task<Device?> UpdateAsync(Guid id, Device device)
        {
            var existingDevice = await dbContext.Devices.FirstOrDefaultAsync(r => r.Id == id);

            if (existingDevice == null)
            {
                return null;
            }

            existingDevice.DeviceNumber = device.DeviceNumber;

            await dbContext.SaveChangesAsync();

            return existingDevice;
        }

        public async Task<Device?> DeleteAsync(Guid id)
        {
            var existingDevice = await dbContext.Devices.FirstOrDefaultAsync(r => r.Id == id);
            if (existingDevice == null)
            {
                return null;
            }
            dbContext.Devices.Remove(existingDevice);
            await dbContext.SaveChangesAsync();

            return existingDevice;

        }
    }
}
