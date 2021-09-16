using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Mvc;
using SmartMed.CodeChalenge.Model;
using SmartMed.Database;
using SmartMed.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartMed.CodeChalenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicationsController : ControllerBase
    {
        private MedicationRepository MedicationRepository;
        private ILog Log;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public MedicationsController(MedicationDbContext context)
        {
            MedicationRepository = new MedicationRepository(context);
            Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            ILoggerRepository logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());

            XmlConfigurator.Configure(logRepository, new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"log4net.config")));
            UpdateLoggerRepo(ref logRepository);
        }


        /// <summary>
        /// Get all medications
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public IEnumerable<Medication> GetAll()
        {
            try
            {
                IEnumerable<Medication> medications = MedicationRepository.Get();
                if (medications.Any())
                    Log.Debug($"Returned {medications.Count()}  medications");
                return medications;
            }
            catch (Exception ex)
            {
                Log.Error($"Request -> {Request.Path.Value} -> {ex}");
                return null;
            }
        }

        /// <summary>
        /// Get All active medications
        /// </summary>
        /// <returns></returns>
        [HttpGet("active")]
        public IEnumerable<Medication> GetActiveMedication()
        {
            try
            {
                IEnumerable<Medication> medications = MedicationRepository.Query(m => DateTime.Now <= m.Expired && m.Active);
                if (medications.Any())
                    Log.Debug($"Returned {medications.Count()} active medications");
                return medications;
            }
            catch (Exception ex)
            {
                Log.Error($"Request -> {Request.Path.Value} -> {ex}");
                return null;
            }
        }

        /// <summary>
        /// Get All expired medication
        /// </summary>
        /// <returns></returns>
        [HttpGet("expired")]
        public IEnumerable<Medication> GetExpiredMedication()
        {
            try
            {
                IEnumerable<Medication> medications = MedicationRepository.Query(m => DateTime.Now > m.Expired || !m.Active);
                if (medications.Any())
                    Log.Debug($"Returned {medications.Count()} expired medications");
                return medications;
            }
            catch (Exception ex)
            {
                Log.Error($"Request -> {Request.Path.Value} -> {ex}");
                return null;
            }
        }

        /// <summary>
        /// Get a medication by id
        /// </summary>
        /// <param name="id">medication guid</param>
        /// <returns></returns>
        [HttpGet("get/{id}")]
        public Medication Get(Guid id)
        {
            try
            {
                Medication medication = MedicationRepository.Get(m => m.Id == id);
                if (medication != null)
                    Log.Debug($"Found medication {id}");
                return medication;
            }
            catch (Exception ex)
            {
                Log.Error($"Request -> {Request.Path.Value} -> {ex}");
                return null;
            }
        }

        /// <summary>
        /// Search by name medication
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("search/{query}")]
        public IEnumerable<Medication> SearchMedication(string query)
        {
            try
            {
                IEnumerable<Medication> medications = MedicationRepository.Query(m => m.Name.Contains(query));
                if (medications.Any())
                    Log.Debug($"Search query returned {medications.Count()} medications");
                return medications;
            }
            catch (Exception ex)
            {
                Log.Error($"Request -> {Request.Path.Value} -> {ex}");
                return null;
            }
        }
        /// <summary>
        /// Add medication
        /// </summary>
        /// <param name="medication"></param>
        [HttpPost("add")]
        public IActionResult Add([FromBody] Medication medication)
        {
            try
            {
                medication = MedicationRepository.Add(medication);
                if (medication != null)
                {
                    Log.Info($"New medication added {Newtonsoft.Json.JsonConvert.SerializeObject(medication)}");
                    return Ok(medication);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Request -> {Request.Path.Value} -> {ex}");
                return BadRequest("An error has occurred please contact, development team for more support!");
            }
        }


        /// <summary>
        /// Set a given medication as inactive
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(Guid id)
        {

            try
            {

                MedicationRepository.Delete(id);
                Log.Info($"Medication with id {id} deleted with success!!!");
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error($"Request -> {Request.Path.Value} -> {ex}");
                return BadRequest("An error has occurred please contact, development team for more support!");
            }
        }


        #region Private methods

        private static void UpdateLoggerRepo(ref ILoggerRepository repo)
        {
            foreach (var appender in repo.GetAppenders())
            {
                if (appender.GetType() == typeof(log4net.Appender.ConsoleAppender))
                {
                    continue;
                }
                log4net.Appender.FileAppender fileAppender = (log4net.Appender.FileAppender)appender;
                fileAppender.File = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "\\logs", "log_");
                fileAppender.ActivateOptions();

            }
        }


        #endregion
    }
}
