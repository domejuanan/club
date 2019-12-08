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
    public interface ISportService
    {
        Task<SportListResponse> ListAsync(int pageNum = 1, int pageSize = 50);
        Task<SportResponse> GetAsync(int id);
        Task<SportResponse> SaveAsync(SportSaveResource sportSaveResource);
        Task<SportResponse> UpdateAsync(int id, SportSaveResource sportSaveResource);
        Task<SportResponse> DeleteAsync(int id);
    }

    public class SportService : ISportService
    {
        private readonly ISportRepository _sportRepository;
        private readonly IMapper _mapper;

        public SportService(ISportRepository sportRepository, IMapper mapper)
        {
            _sportRepository = sportRepository;
            _mapper = mapper;
        }

        public async Task<SportListResponse> ListAsync(int pageNum = 1, int pageSize = 50)
        {
            if (pageNum < 1 || pageSize < 1)
                return new SportListResponse(400, "Wrong pagination", "Pagination", 
                    "The pageNum and pageSize params must be greater than zero.");

            int totalRecords = await _sportRepository.CountAsync();
            var sports = await _sportRepository.ListAsync(pageNum, pageSize);
            var resourcesSports = _mapper.Map<IEnumerable<Sport>, IEnumerable<SportResource>>(sports);
            var resourceListSports = new SportListResource(resourcesSports, pageNum, pageSize, totalRecords);
            
            return new SportListResponse(resourceListSports);
        }

        public async Task<SportResponse> GetAsync(int id)
        {
            var existingItem = await _sportRepository.FindByIdAsync(id);

            if (existingItem == null)
                return new SportResponse(404, "Item id not found", "Id","Sport id not found.");

            var responseResource = _mapper.Map<Sport, SportResource>(existingItem);
            return new SportResponse(responseResource);
        }

        public async Task<SportResponse> SaveAsync(SportSaveResource sportSaveResource)
        {
            try
            {
                var existingItem = await _sportRepository.FindByName(sportSaveResource.Name);

                if (existingItem != null)
                    return new SportResponse(400, "Item already exists", "Name", "Sport name is taken");

                var sport = _mapper.Map<SportSaveResource, Sport>(sportSaveResource);
                await _sportRepository.AddAsync(sport);
                var responseResource = _mapper.Map<Sport, SportResource>(sport);
                return new SportResponse(responseResource);
            }
            catch (Exception ex)
            {
                return new SportResponse(400, "Unexpected error", "Error", ex.Message);
            }
        }

        public async Task<SportResponse> UpdateAsync(int id, SportSaveResource sportSaveResource)
        {
            var existingItem = await _sportRepository.FindByIdAsync(id);
            
            if (existingItem == null)
                return new SportResponse(404, "Item id not found", "Id","Sport id not found.");

            var existingItemWithName = await _sportRepository.FindByName(sportSaveResource.Name);

            if (existingItemWithName.Id != id)
                return new SportResponse(400, "Item already exists", "Name", "Sport name is taken");

            existingItem.Name = sportSaveResource.Name;

            try
            {
                _sportRepository.Update(existingItem);
                var responseResource = _mapper.Map<Sport, SportResource>(existingItem);
                return new SportResponse(responseResource);
            }
            catch (Exception ex)
            {
                return new SportResponse(400, "Unexpected error", "Error", ex.Message);
            }
        }

        public async Task<SportResponse> DeleteAsync(int id)
        {
            var existingItem = await _sportRepository.FindByIdAsync(id);
            
            if (existingItem == null)
                return new SportResponse(404, "Item id not found", "Id", "Sport id not found.");

            try
            {
                _sportRepository.Remove(existingItem);
                var responseResource = _mapper.Map<Sport, SportResource>(existingItem);                
                return new SportResponse(responseResource);
            }
            catch (Exception ex)
            {
                return new SportResponse(400, "Unexpected error", "Error", ex.Message);
            }
        }
    }
}