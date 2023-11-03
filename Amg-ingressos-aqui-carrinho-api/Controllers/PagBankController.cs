using Amg_ingressos_aqui_carrinho_api.Consts;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Amg_ingressos_aqui_carrinho_api.Controllers
{
    [Route("v1/PagBank")]
    [Produces("application/json")]
    public class PagBankController : ControllerBase
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly IPagbankService _pagbankService;

        public PagBankController(
            ILogger<TransactionController> logger,
            IPagbankService transactionService)
        {
            _logger = logger;
            _pagbankService = transactionService;
        }

        /// <summary>
        /// Busca Transação por id
        /// </summary>
        /// <returns>200 Transação</returns>
        /// <returns>500 Erro inesperado</returns>
        /// <returns>404 Erro tratado</returns>
        [HttpGet]
        public async Task<IActionResult> GenerateSession()
        {
            try
            {
                var result = await _pagbankService.GenerateSession();
                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return NotFound(result.Message);
                }

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.getByIdTransactionMessage, ex);
                return StatusCode(500, MessageLogErrors.getByIdTransactionMessage);
            }
        }

    }
}