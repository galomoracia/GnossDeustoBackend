﻿// Copyright (c) UTE GNOSS - UNIVERSIDAD DE DEUSTO
// Licenciado bajo la licencia GPL 3. Ver https://www.gnu.org/licenses/gpl-3.0.html
// Proyecto Hércules ASIO Backend SGI. Ver https://www.um.es/web/hercules/proyectos/asio
// Controlador repositorios
using System;
using System.Collections.Generic;
using System.Linq;
using ApiCargaWebInterface.Extra.Exceptions;
using ApiCargaWebInterface.Models.Services;
using ApiCargaWebInterface.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NCrontab;

namespace ApiCargaWebInterface.Controllers
{
    
    public class RepositoryConfigController : Controller
    {
        readonly ICallRepositoryConfigService _serviceApi;
        readonly CallRepositoryJobService _respositoryJobService;
        public RepositoryConfigController(ICallRepositoryConfigService serviceApi, CallRepositoryJobService respositoryJobService)
        {
            _serviceApi = serviceApi;
            _respositoryJobService = respositoryJobService;
        }
        public IActionResult Index()
        {
            List<RepositoryConfigViewModel> result = _serviceApi.GetRepositoryConfigs();
            return View(result);
        }

        [Route("[Controller]/{id}")]
        public IActionResult Details(Guid id)
        {
            RepositoryConfigViewModel result = _serviceApi.GetRepositoryConfig(id);
            result.ListRecurringJobs = _respositoryJobService.GetRecurringJobsOfRepo(id);
            result.ListJobs = _respositoryJobService.GetJobsOfRepo(id);
            result.ListScheduledJobs = _respositoryJobService.GetScheduledJobsOfRepo(id);
            if (result.ListJobs != null && result.ListJobs.Count > 0)
            {
                var job = result.ListJobs.OrderByDescending(item => item.ExecutedAt).FirstOrDefault();
                result.LastJob = job.Id;
                result.LastState = job.State;
                int succed = result.ListJobs.Count(item => item.State.Equals("Succeed"));
                double percentage = ((double)succed / result.ListJobs.Count)*100;
                result.PorcentajeTareas = Math.Round(percentage, 2);
            }
            if (result != null)
            {
                return View(result);
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult Edit(Guid id)
        {
            RepositoryConfigViewModel result = _serviceApi.GetRepositoryConfig(id);
            if (result != null)
            {
                return View("Create", result);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult Edit(RepositoryConfigViewModel repositoryConfigView)
        {
            try
            {
                _serviceApi.ModifyRepositoryConfig(repositoryConfigView);

                return RedirectToAction("Details",new { id = repositoryConfigView.RepositoryConfigID });
            }
            catch(BadRequestException)
            {
                return BadRequest();
            }
            
        }

        public IActionResult Delete(Guid id)
        {
            bool result = _serviceApi.DeleteRepositoryConfig(id);
            if (result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(RepositoryConfigViewModel repositoryConfigView)
        {
            try
            {
                repositoryConfigView.RepositoryConfigID = Guid.NewGuid();
                RepositoryConfigViewModel result = _serviceApi.CreateRepositoryConfigView(repositoryConfigView);
                return RedirectToAction("Details", new { id = result.RepositoryConfigID });
                
            }
            catch (BadRequestException)
            {
                return BadRequest();
            }
        }
    }
}