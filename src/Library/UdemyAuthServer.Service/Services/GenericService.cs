using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dtos;
using System.Linq.Expressions;
using System.Net;
using UdemyAuthServer.Core.Repositories;
using UdemyAuthServer.Core.Services;
using UdemyAuthServer.Core.UnitOfWork;
using UdemyAuthServer.Service.AutoMapper;

namespace UdemyAuthServer.Service.Services
{
    public class GenericService<TEntity, TDto> : IGenericService<TEntity, TDto>
        where TEntity : class
        where TDto : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<TEntity> _genericRepository;

        public GenericService(IUnitOfWork unitOfWork, IGenericRepository<TEntity> genericRepository)
        {
            _unitOfWork = unitOfWork;
            _genericRepository = genericRepository;
        }

        public async Task<Response<TDto>> AddAsync(TDto dto)
        {
            var newEntity = ObjectMapper.Mapper.Map<TEntity>(dto);

            await _genericRepository.AddAsync(newEntity);

            await _unitOfWork.CommitAsync();

            var newDto = ObjectMapper.Mapper.Map<TDto>(newEntity);

            return Response<TDto>.Success(newDto, (int)HttpStatusCode.OK);
        }

        public async Task<Response<IEnumerable<TDto>>> GetAllAsync()
        {
            var entities = await _genericRepository.GetAll().ToListAsync();

            var dtos = ObjectMapper.Mapper.Map<List<TDto>>(entities);

            return Response<IEnumerable<TDto>>.Success(dtos, (int)HttpStatusCode.OK);
        }

        public async Task<Response<TDto>> GetByIdAsync(int id)
        {
            var newEntity = await _genericRepository.GetByIdAsync(id);

            if (newEntity is null)
                return Response<TDto>.Fail("Id not found", (int)HttpStatusCode.NotFound, true);

            var dto = ObjectMapper.Mapper.Map<TDto>(newEntity);

            return Response<TDto>.Success(dto, (int)HttpStatusCode.OK);
        }

        public async Task<Response<NoDataDto>> RemoveAsync(int id)
        {
            var isExistEntity = await _genericRepository.GetByIdAsync(id);

            if (isExistEntity is null)
                return Response<NoDataDto>.Fail("Id not found", (int)HttpStatusCode.NotFound, true);

            _genericRepository.Remove(isExistEntity);

            await _unitOfWork.CommitAsync();

            return Response<NoDataDto>.Success((int)HttpStatusCode.NoContent);
        }

        public async Task<Response<NoDataDto>> UpdateAsync(int id, TDto dto)
        {
            var isExistEntity = await _genericRepository.GetByIdAsync(id);

            if (isExistEntity is null)
                return Response<NoDataDto>.Fail("Id not found", (int)HttpStatusCode.NotFound, true);

            var updateEntity = ObjectMapper.Mapper.Map<TEntity>(dto);
            
            _genericRepository.Update(updateEntity);

            await _unitOfWork.CommitAsync();

            return Response<NoDataDto>.Success((int)HttpStatusCode.NoContent);
        }

        public async Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate)
        {
            var listEntity = _genericRepository.Where(predicate);

            return Response<IEnumerable<TDto>>.Success(ObjectMapper.Mapper.Map<IEnumerable<TDto>>(await listEntity.ToListAsync()), (int)HttpStatusCode.OK);
        }
    }
}