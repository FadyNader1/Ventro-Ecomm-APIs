using Ecomm.core.Entities.OrderEntities;
using Ecomm.core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.repository.Repository
{
    public class DeliveryMethodRepo : IDeliveryMethodRepo
    {
        private readonly IUnitOfWork unitOfWork;

        public DeliveryMethodRepo(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task AddDeliveryMethodAsync(DeliveryMethod deliveryMethod)
        {
            await unitOfWork.Repository<DeliveryMethod>().AddAsync(deliveryMethod);
            await unitOfWork.CompleteAsync();
        }

        public async Task DeleteDeliveryMethodAsync(DeliveryMethod deliveryMethod)
        {
            unitOfWork.Repository<DeliveryMethod>().Delete(deliveryMethod);
            await unitOfWork.CompleteAsync();
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethodsAsync()
        {
            return await unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
        }

        public async Task<DeliveryMethod> GetDeliveryMethodByIdAsync(int id)
        {
            return await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(id);
        }

        public async Task UpdateDeliveryMethodAsync(DeliveryMethod deliveryMethod)
        {
            unitOfWork.Repository<DeliveryMethod>().Update(deliveryMethod);
            await unitOfWork.CompleteAsync();
        }
    }
}
