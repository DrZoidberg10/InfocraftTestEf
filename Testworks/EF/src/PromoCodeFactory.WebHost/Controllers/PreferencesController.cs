using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;
using PromoCodeFactory.DataAccess.Repositories;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Предпочтения
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PreferencesController
        : ControllerBase
    {
        private readonly EfRepository<Preference> _preferenceRepository;

        public PreferencesController (EfRepository<Preference> preferenceRepository)
        {
            _preferenceRepository = preferenceRepository;
        }

        /// <summary>
        /// Получить список предпочтений
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<PreferenceResponse>> GetPreferencesAsync()
        {
            var preferences = await _preferenceRepository.GetAllAsync();

            var preferencesModelList = preferences.
                Select(
                    p => new PreferenceResponse()
                    {
                        Id = p.Id,
                        Name = p.Name
                    })
                .ToList();

            return preferencesModelList;
        }
    }
}