using Ecomm.core.Entities.OrderEntities;
using Ecomm.core.Exceptions;
using Ecomm.core.Interfaces;
using Ecomm.service.InterfaceServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecomm.service.ImplementServices
{
    public class DeliveryMethodService : IDeliveryMethodService
    {
        private readonly IDeliveryMethodRepo deliveryMethodRepo;

        public DeliveryMethodService(IDeliveryMethodRepo deliveryMethodRepo)
        {
            this.deliveryMethodRepo = deliveryMethodRepo;
        }


        public async Task AddDeliveryMethodAsync(DeliveryMethod deliveryMethod)
        {
            if (deliveryMethod == null)
                throw new ArgumentNullException(nameof(deliveryMethod));

            await deliveryMethodRepo.AddDeliveryMethodAsync(deliveryMethod);
        }


        public async Task DeleteDeliveryMethodAsync(DeliveryMethod deliveryMethod)
        {
            if (deliveryMethod == null)
                throw new ArgumentNullException(nameof(deliveryMethod));

            await deliveryMethodRepo.DeleteDeliveryMethodAsync(deliveryMethod);
        }


        public async Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethodsAsync()
        {
            return await deliveryMethodRepo.GetAllDeliveryMethodsAsync();
        }


        public async Task<DeliveryMethod> GetDeliveryMethodByIdAsync(int id)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid DeliveryMethod Id");

            var deliveryMethod = await deliveryMethodRepo.GetDeliveryMethodByIdAsync(id);

            if (deliveryMethod == null)
                throw new BadRequestException("DeliveryMethod not found, please check the Id");

            return deliveryMethod;
        }


        public async Task UpdateDeliveryMethodAsync(DeliveryMethod deliveryMethod)
        {
          
            var existing = await deliveryMethodRepo.GetDeliveryMethodByIdAsync(deliveryMethod.Id);
            // Update fields manually to avoid tracking conflict
            existing.Name = deliveryMethod.Name;
            existing.Price = deliveryMethod.Price;
            existing.DeleveryDays = deliveryMethod.DeleveryDays;
            existing.Description = deliveryMethod.Description;
           
            await deliveryMethodRepo.UpdateDeliveryMethodAsync(existing);
        }
    }
}
