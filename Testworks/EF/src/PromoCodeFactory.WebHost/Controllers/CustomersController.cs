using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.WebHost.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using PromoCodeFactory.DataAccess.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.Core.Domain.Administration;
using System.Collections.Generic;
using PromoCodeFactory.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Клиенты
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController
        : ControllerBase
    {
        private readonly EfRepository<Customer> _customerRepository;
        private readonly EfDbContext _dbContext;
        public CustomersController(EfRepository<Customer> customerRepository, EfDbContext dbContext)
        {
            _customerRepository = customerRepository;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Получить данные всех клиентов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<CustomerShortResponse>> GetCustomersAsync()
        {
            //fsdfdfs
            var customers = await _customerRepository.GetAllAsync();
            var customersModelList = customers
                .Select(
                    x => new CustomerShortResponse()
                    {
                        Id = x.Id,
                        Email = x.Email,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                    })
                .ToList();

            return customersModelList;
        }

        /// <summary>
        /// Получить данные клиента по id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
                return NotFound();

            var preferenceResponseList = customer.Preferences.
                Select(
                    p => new PreferenceResponse()
                    {
                        Id = p.Id,
                        Name = p.Name
                    })
                .ToList();

            var promocodeShortResponceList = customer.PromoCodes.
                Select(
                    p => new PromoCodeShortResponse()
                    {
                        Id = p.Id,
                        Code = p.Code,
                        ServiceInfo = p.ServiceInfo,
                        BeginDate = p.BeginDate.ToString(),
                        EndDate = p.EndDate.ToString(),
                        PartnerName = p.PartnerName
                    })
                .ToList();

            var customerModel = new CustomerResponse()
            {
                Id = customer.Id,
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Preferences = preferenceResponseList,
                PromoCodes = promocodeShortResponceList
            };

            return customerModel;
        }

        /// <summary>
        /// Создать запись клиента
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            var newCustomer = new Customer()
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email
            };

            foreach (var preferenceId in request.PreferenceIds)
            {
                if (await _dbContext.Preferences.FindAsync(preferenceId) == null)
                {
                    return NotFound();
                }
            }

            var customerPreferenceList = request.PreferenceIds.
                Select(
                    p => new CustomerPreference()
                    {
                        CustomerId = newCustomer.Id,
                        PreferenceId = p
                    })
                .ToList();

            _dbContext.CustomerPreferences.AddRange(customerPreferenceList);
            _dbContext.Customers.Add(newCustomer);
            await _dbContext.SaveChangesAsync();
            return Ok("Клиент сохранен");
        }

        /// <summary>
        /// Изменить запись клиента
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
                return NotFound();

            foreach (var preferenceId in request.PreferenceIds)
            {
                if (await _dbContext.Preferences.FindAsync(preferenceId) == null)
                {
                    return NotFound();
                }
            }

            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;
            customer.Email = request.Email;

            var customerPreferenceList = new List<CustomerPreference>();
            foreach (var preferenceId in request.PreferenceIds)
            {
                if (await _dbContext.CustomerPreferences.FirstOrDefaultAsync(cp => (cp.CustomerId == id && cp.PreferenceId == preferenceId)) != null)
                    continue;

                customerPreferenceList.Add(
                    new CustomerPreference()
                    {
                        CustomerId = id,
                        PreferenceId = preferenceId
                    }
                );
            }

            _dbContext.CustomerPreferences.AddRange(customerPreferenceList);
            await _dbContext.SaveChangesAsync();
            return Ok("Изменения сохранены");
        }

        /// <summary>
        /// Удаляет клиента по id
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
                return NotFound();

            _dbContext.Customers.Remove(customer);
            await _dbContext.SaveChangesAsync();
            return Ok("Клиент удален");
        }
    }
}