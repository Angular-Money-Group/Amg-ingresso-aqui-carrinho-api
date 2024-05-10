using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Amg_ingressos_aqui_carrinho_api.Controllers
{
    [Route("v1/transaction")]
    [Produces("application/json")]
    //[Authorize(Policy = "PublicSecure")]
    public class TransactionController : ControllerBase
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly ITransactionService _transactionService;

        public TransactionController(
            ILogger<TransactionController> logger,
            ITransactionService transactionService)
        {
            _logger = logger;
            _transactionService = transactionService;
        }

        /// <summary>
        /// Busca Transação por id
        /// </summary>
        /// <param name="idTransaction">id da transacao</param>
        /// <returns>200 Transação</returns>
        /// <returns>500 Erro inesperado</returns>
        /// <returns>404 Erro tratado</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetByIdTransactionAsync(string id)
        {

            var result = await _transactionService.GetByIdAsync(id);
            if (result.Message != null && result.Message.Any())
            {
                _logger.LogInformation(result.Message);
                return NotFound(result.Message);
            }

            return Ok(result.Data);
        }

        /// <summary>
        /// Busca Transação por id
        /// </summary>
        /// <param name="idUser">id do usuário</param>
        /// <returns>200 Transação</returns>
        /// <returns>500 Erro inesperado</returns>
        /// <returns>404 Erro tratado</returns>
        [HttpGet]
        [Route("user/{idUser}/card/active")]
        public async Task<IActionResult> GetByUserActivesAsync([FromRoute] string idUser)
        {
            var result = await _transactionService.GetByUserActivesAsync(idUser);
            if (result.Message != null && result.Message.Any())
            {
                _logger.LogInformation(result.Message);
                return NotFound(result.Message);
            }

            return Ok(result.Data);
        }

        /// <summary>
        /// Busca Transação por id
        /// </summary>
        /// <param name="idUser">id do usuário</param>
        /// <returns>200 Transação</returns>
        /// <returns>500 Erro inesperado</returns>
        /// <returns>404 Erro tratado</returns>
        [HttpGet]
        [Route("user/{idUser}/card/history")]
        public async Task<IActionResult> GetByUserHistoryAsync([FromRoute] string idUser)
        {
            var result = await _transactionService.GetByUserHistoryAsync(idUser);
            if (result.Message != null && result.Message.Any())
            {
                _logger.LogInformation(result.Message);
                return NotFound(result.Message);
            }

            return Ok(result.Data);
        }

        /// <summary>
        /// Busca Transação por id
        /// </summary>
        /// <param name="idUser">id do usuário</param>
        /// <param name="idEvent">id do evento</param>
        /// <returns>200 Transação</returns>
        /// <returns>500 Erro inesperado</returns>
        /// <returns>404 Erro tratado</returns>
        [HttpGet]
        [Route("person/{idUser}/event/{idEvent}")]
        public async Task<IActionResult> GetByUserDataTicketEventAsync([FromRoute] string idUser, string idEvent)
        {
            var result = await _transactionService.GetByUserTicketEventDataAsync(idUser, idEvent);
            if (result.Message != null && result.Message.Any())
            {
                _logger.LogInformation(result.Message);
                return NotFound(result.Message);
            }

            return Ok(result.Data);
        }
    }
}