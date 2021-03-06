using Abstraction.Interfaces.Repositories;
using Abstraction.Interfaces.Services;
using Abstraction.Models;

namespace Service.Implementation
{
    public class PositionService : IPositionService
    {
        private readonly IPositionRepository _positionRepository;

        public PositionService(IPositionRepository positionRepository)
        {
            this._positionRepository = positionRepository;
        }

        public Task<Position> AddAsync(Position entity)
        {
            return this._positionRepository.AddAsync(entity);
        }

        public Task<Position> GetAsync(string stockSymbol, int portfolioId, int userId)
        {
            return _positionRepository.GetAsync(stockSymbol, portfolioId, userId);
        }

        public async Task DeleteAsync(Position position)
        {
            await _positionRepository.DeleteAsync(position);
        }

        public Task<Position> GetAsync(int id)
        {
            return _positionRepository.GetAsync(id);
        }

    }
}