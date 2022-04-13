using Abstraction.Interfaces.Repositories;
using Abstraction.Interfaces.Services;
using Abstraction.Models;

namespace Service.Implementation
{
    internal class PositionService: IPositionService
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

        public Task<Position> GetAsync(string stockSymbol, int userId)
        {
            return _positionRepository.GetAsync(stockSymbol, userId);
        }

        public Task<Position> UpdateAsync(Position entity)
        {
            return _positionRepository.UpdateAsync(entity);
        }
    }
}
