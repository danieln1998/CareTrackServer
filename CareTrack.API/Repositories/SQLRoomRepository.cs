using CareTrack.API.Data;
using CareTrack.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CareTrack.API.Repositories
{
    public class SQLRoomRepository : IRoomRepository
    {
        private readonly CareTrackDbcontext dbContext;

        public SQLRoomRepository(CareTrackDbcontext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Room>> GetAllAsync(int pageNumber = 1, int pageSize = 1000)
        {
            var rooms = dbContext.Rooms.AsQueryable();
            // Pagination
            var skipResults = (pageNumber - 1) * pageSize;

            return await rooms.Skip(skipResults).Take(pageSize).ToListAsync();


        }

        public async Task<Room?> GetByIdAsync(Guid id)
        {
            return await dbContext.Rooms.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Room> CreateAsync(Room room)
        {
            await dbContext.Rooms.AddAsync(room);
            await dbContext.SaveChangesAsync();
            return room;
        }

        public async Task<Room?> UpdateAsync(Guid id, Room room)
        {
            var existingRoom = await dbContext.Rooms.FirstOrDefaultAsync(r => r.Id == id);

            if (existingRoom == null)
            {
                return null;
            }

            existingRoom.RoomNumber = room.RoomNumber;
           
            await dbContext.SaveChangesAsync();

            return existingRoom;
        }

        public async Task<Room?> DeleteAsync(Guid id)
        {
            var existingRoom = await dbContext.Rooms.FirstOrDefaultAsync(r => r.Id == id);
            if (existingRoom == null)
            {
                return null;
            }
            dbContext.Rooms.Remove(existingRoom);
            await dbContext.SaveChangesAsync();

            return existingRoom;

        }

    }
}
