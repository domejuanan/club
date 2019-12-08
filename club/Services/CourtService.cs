using System;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using club.Models;
using club.Repositories;
using club.Resources;
using club.Responses;


namespace club.Services
{
    public interface ICourtService
    {
        Task<CourtListResponse> ListAsync(int pageNum = 1, int pageSize = 50);
        Task<CourtResponse> GetAsync(int id);
        Task<CourtResponse> SaveAsync(CourtSaveResource sportSaveResource);
        Task<CourtResponse> UpdateAsync(int id, CourtSaveResource sportSaveResource);
        Task<CourtResponse> DeleteAsync(int id);
    }

    public class CourtService : ICourtService
    {
        private readonly ISportRepository _sportRepository;
        private readonly ICourtRepository _courtRepository;
        private readonly IMapper _mapper;

        public CourtService(ICourtRepository courtRepository, ISportRepository sportRepository, IMapper mapper)
        {
            _courtRepository = courtRepository;
            _sportRepository = sportRepository;
            _mapper = mapper;
        }
        
        public async Task<CourtListResponse> ListAsync(int pageNum = 1, int pageSize = 50)
        {
            if (pageNum < 1 || pageSize < 1) 
                return new CourtListResponse(400, "Wrong pagination", "Pagination", "pageNum and pageSize params must be greater than zero.");
            
            
            int totalRecords = await _courtRepository.CountAsync();
            var items = await _courtRepository.ListAsync(pageNum, pageSize);
            var resources = _mapper.Map<IEnumerable<Court>, IEnumerable<CourtResource>>(items);
            var resourceList = new CourtListResource(resources, pageNum, pageSize, totalRecords);
            
            return new CourtListResponse(resourceList);
        }

        public async Task<CourtResponse> GetAsync(int id)
        {
            var existingItem = await _courtRepository.FindByIdAsync(id);

            if (existingItem == null)
                return new CourtResponse(404,"Item id not found", "Id", "Court id not found.");

            var responseResource = _mapper.Map<Court, CourtResource>(existingItem);
            return new CourtResponse(responseResource);
        }

        public async Task<CourtResponse> SaveAsync(CourtSaveResource sportSaveResource)
        {
            try
            {
                var sport = await _sportRepository.FindByIdAsync(sportSaveResource.SportId);

                if (sport == null)
                    return new CourtResponse(404, "Item id not found", "SportId", "Sport id not found.");

                var item = _mapper.Map<CourtSaveResource, Court>(sportSaveResource);
                await _courtRepository.AddAsync(item);
                var responseResource = _mapper.Map<Court, CourtResource>(item);
                return new CourtResponse(responseResource);
            }
            catch (Exception ex)
            {
                return new CourtResponse(400, "Unexpected error", "Error", ex.Message);
            }
        }

        public async Task<CourtResponse> UpdateAsync(int id, CourtSaveResource sportSaveResource)
        {
            var existingItem = await _courtRepository.FindByIdAsync(id);

            if (existingItem == null)
                return new CourtResponse(404, "Item id not found", "Id","Court id not found.");

            var sport = await _sportRepository.FindByIdAsync(sportSaveResource.SportId);

            if (sport == null)
                return new CourtResponse(404, "Item id not found", "SportId", "Sport id not found.");

            existingItem.Reference = sportSaveResource.Reference;
            existingItem.SportId = sportSaveResource.SportId;

            try
            {
                _courtRepository.Update(existingItem);
                var responseResource = _mapper.Map<Court, CourtResource>(existingItem);
                return new CourtResponse(responseResource);
            }
            catch (Exception ex)
            {
                return new CourtResponse(400, "Unexpected error", "Error", ex.Message);
            }
        }

        public async Task<CourtResponse> DeleteAsync(int id)
        {
            var existingItem = await _courtRepository.FindByIdAsync(id);
            

            if (existingItem == null)
                return new CourtResponse(404, "Item id not found", "Id","Court id not found.");

            try
            {
                _courtRepository.Remove(existingItem);
                var responseResource = _mapper.Map<Court, CourtResource>(existingItem);                
                return new CourtResponse(responseResource);
            }
            catch (Exception ex)
            {
                return new CourtResponse(400, "Unexpected error", "Error", ex.Message);
            }
        }
    }
}