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
    public interface IMemberService
    {
        Task<MemberListResponse> ListAsync(int pageNum = 1, int pageSize = 50);
        Task<MemberResponse> GetAsync(int id);
        Task<MemberResponse> SaveAsync(MemberSaveResource memberSaveResource);
        Task<MemberResponse> UpdateAsync(int id, MemberSaveResource memberSaveResource);
        Task<MemberResponse> DeleteAsync(int id);
    }

    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IMapper _mapper;

        public MemberService(IMemberRepository memberRepository, IMapper mapper)
        {
            _memberRepository = memberRepository;
            _mapper = mapper;
        }

        public async Task<MemberListResponse> ListAsync(int pageNum = 1, int pageSize = 50)
        {
            if (pageNum < 1 || pageSize < 1)
                return new MemberListResponse(400, "Wrong pagination", "Pagination", 
                    "The pageNum and pageSize params must be greater than zero.");

            int totalRecords = await _memberRepository.CountAsync();
            var items = await _memberRepository.ListAsync(pageNum, pageSize);
            var resources = _mapper.Map<IEnumerable<Member>, IEnumerable<MemberResource>>(items);
            var resourceList = new MemberListResource(resources, pageNum, pageSize, totalRecords);

            return new MemberListResponse(resourceList);
        }

        public async Task<MemberResponse> GetAsync(int id)
        {
            var existingItem = await _memberRepository.FindByIdAsync(id);

            if (existingItem == null)
                return new MemberResponse(404, "Item id not found", "Id", "Member id not found.");

            var responseResource = _mapper.Map<Member, MemberResource>(existingItem);
            return new MemberResponse(responseResource);
        }

        public async Task<MemberResponse> SaveAsync(MemberSaveResource memberSaveResource)
        {
            try
            {
                var item = _mapper.Map<MemberSaveResource, Member>(memberSaveResource);
                await _memberRepository.AddAsync(item);
                var responseResource = _mapper.Map<Member, MemberResource>(item);
                return new MemberResponse(responseResource);
            }
            catch (Exception ex)
            {
                return new MemberResponse(400, "Unexpected error", "Error", ex.Message);
            }
        }

        public async Task<MemberResponse> UpdateAsync(int id, MemberSaveResource memberSaveResource)
        {
            var existingItem = await _memberRepository.FindByIdAsync(id);

            if (existingItem == null)
                return new MemberResponse(404, "Item id not found", "Id", "Member id not found.");

            existingItem.Name = memberSaveResource.Name;
            existingItem.Phone = memberSaveResource.Phone;
            existingItem.Surname = memberSaveResource.Surname;
            existingItem.Address = memberSaveResource.Address;

            try
            {
                _memberRepository.Update(existingItem);
                var responseResource = _mapper.Map<Member, MemberResource>(existingItem);
                return new MemberResponse(responseResource);
            }
            catch (Exception ex)
            {
                return new MemberResponse(400, "Unexpected error", "Error", ex.Message);
            }
        }

        public async Task<MemberResponse> DeleteAsync(int id)
        {
            var existingItem = await _memberRepository.FindByIdAsync(id);
            
            if (existingItem == null)
                return new MemberResponse(404, "Item id not found", "Id", "Member id not found.");

            try
            {
                _memberRepository.Remove(existingItem);
                var responseResource = _mapper.Map<Member, MemberResource>(existingItem);
                return new MemberResponse(responseResource);
            }
            catch (Exception ex)
            {
                return new MemberResponse(400, "Unexpected error", "Error", ex.Message);
            }
        }
    }
}