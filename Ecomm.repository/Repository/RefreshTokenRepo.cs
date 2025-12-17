using Ecomm.core.Entities;
using Ecomm.core.Interfaces;
using Ecomm.repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.repository.Repository
{
    public class RefreshTokenRepo : IRefreshTokenRepo
    {
        private readonly IUnitOfWork unitOfWork;

        public RefreshTokenRepo(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
        {
            try
            {
                await unitOfWork.Repository<RefreshToken>().AddAsync(refreshToken);
                await unitOfWork.CompleteAsync();
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RefreshToken?> FindByToken(string token)
        {
           var allRefreshTokens=await  unitOfWork.Repository<RefreshToken>().GetAllAsync();
           var entity=  allRefreshTokens.Where(r => r.Token == token).FirstOrDefault();
              return entity;

        }
    }
}
