﻿using CronConfigure.Models.Entitties;
using Hangfire;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CronConfigure.Models.Services
{
    public class ProgramingMethodsService
    {
        private CallApiService _serviceApi;
        private HangfireEntityContext _context;

        public ProgramingMethodsService(CallApiService serviceApi, HangfireEntityContext context)
        {
            _serviceApi = serviceApi;
            _context = context;
        }

        [AutomaticRetry(Attempts = 0, DelaysInSeconds = new int[] { 3600 })]
        public string PublishRepositories(Guid idRepositoryGuid)
        {
            string idRepository = idRepositoryGuid.ToString();
            try
            {
                string result = _serviceApi.CallPostApi($"sync/execute/{idRepository}", idRepository);
                result = JsonConvert.DeserializeObject<string>(result);
                return result;
            }
            catch (Exception ex)
            {
                string timeStamp = CreateTimeStamp();
                CreateLoggin(timeStamp, idRepository);
                Log.Error($"{ex.Message}\n{ex.StackTrace}\n");
                throw new Exception(ex.Message);
                //return ex.Message;
            } 
        }

        public static void ProgramRecurringJob(Guid idRepository, string nombreCron, string cronExpression)
        {
            ConfigUrlService serviceUrl = new ConfigUrlService();
            CallApiService serviceApi = new CallApiService(serviceUrl);
            ProgramingMethodsService service = new ProgramingMethodsService(serviceApi, null);

            RecurringJob.AddOrUpdate(nombreCron, () => service.PublishRepositories(idRepository), cronExpression);
        }

        public string ProgramPublishRepositoryJob(Guid idRepository, DateTime fechaInicio)
        {
            string id = BackgroundJob.Schedule(() => PublishRepositories(idRepository), fechaInicio);
            JobRepository jobRepository = new JobRepository()
            {
                IdJob = id,
                IdRepository = idRepository
            };
            _context.JobRepository.Add(jobRepository);
            _context.SaveChanges();
            return id;
        }
        public void ProgramPublishRepositoryRecurringJob(Guid idRepository, string nombreCron, string cronExpression, DateTime fechaInicio)
        {
            string id = BackgroundJob.Schedule(() => ProgramRecurringJob(idRepository,nombreCron,cronExpression), fechaInicio);
            JobRepository jobRepository = new JobRepository()
            {
                IdJob = $"{id}_{nombreCron}",
                IdRepository = idRepository
            };
            _context.JobRepository.Add(jobRepository);
            _context.SaveChanges();
        }

        private void CreateLoggin(string pTimestamp, string id)
        {
            Log.Logger = new LoggerConfiguration().Enrich.FromLogContext().WriteTo.File($"logs/job_{id}/log_{pTimestamp}.txt").CreateLogger();
        }

        private string CreateTimeStamp()
        {
            DateTime time = DateTime.Now;
            string month = time.Month.ToString();
            if (month.Length == 1)
            {
                month = $"0{month}";
            }
            string day = time.Day.ToString();
            if (day.Length == 1)
            {
                day = $"0{day}";
            }
            string timeStamp = $"{time.Year.ToString()}{month}{day}";
            return timeStamp;
        }
    }
}