using InventoryManagement.Data;
using InventoryManagement.Data.DTO;
using InventoryManagement.Data.Entity;
using InventoryManagement.InterfacesRepositorys;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Repositorys
{
    public class PaymentCustomerRepository : IRepository<PaymentCustomerDto>
    {
        private readonly AppDbContext _appContext;
        private readonly AddEntityBD _addEntityBD;

        public PaymentCustomerRepository(AppDbContext appContext, AddEntityBD addEntityBD)
        {
            _appContext = appContext;
            _addEntityBD = addEntityBD;
        }
        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<PaymentCustomerDto>> GetAll()
        {
           var clients = _appContext.paymentCustomers.ToList();
            var clientDtos = clients.Select(client => new PaymentCustomerDto
            {
                IdCustomer = client.IdCustomer,
                IdCrates = client.IdCrates,
                NumberPayment = client.NumberPayment,
                DatePayment = client.DatePayment,
                Payment = client.Payment
            }).ToList();
            return Task.FromResult(clientDtos);
        }

        public Task<PaymentCustomerDto> GetById(int Id)
        {
            var client = _appContext.paymentCustomers.
                Where(x => x.IdCustomer == Id).FirstOrDefault();
           if (client == null)
            {
                throw new ArgumentException("Payment not found.");
            }
            var clientDto = new PaymentCustomerDto
            {
                IdCustomer = client.IdCustomer,
                IdCrates = client.IdCrates,
                NumberPayment = client.NumberPayment,
                DatePayment = client.DatePayment,
                Payment = client.Payment
            };
            return Task.FromResult(clientDto);
        }

        public async Task Insert(PaymentCustomerDto entity)
        {
            var payment = new Data.Models.PaymentCustomer
            {
                CreationTime = _addEntityBD.ECreationTime(),
                CreatorId = _addEntityBD.ECreatorId(),
                IdCustomer = entity.IdCustomer,
                NumberPayment = entity.NumberPayment,
                DatePayment = entity.DatePayment,
                Payment = entity.Payment,
                IdCrates=entity.IdCrates
            };
            await _appContext.paymentCustomers.AddAsync(payment);
            await _appContext.SaveChangesAsync();
        }

        public Task Update(PaymentCustomerDto entity, int Id)
        {
            throw new NotImplementedException();
        }
    }
}
