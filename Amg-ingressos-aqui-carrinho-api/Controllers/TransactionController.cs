using Amg_ingressos_aqui_carrinho_api.Consts;
using Amg_ingressos_aqui_carrinho_api.Dtos;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Amg_ingressos_aqui_carrinho_api.Controllers
{
    [Route("v1/transaction")]
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
        /// Grava Transação
        /// </summary>
        /// <param name="transaction">Corpo Transação a ser Gravado</param>
        /// <returns>200 Transação criado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPost]
        public async Task<IActionResult> SaveTransactionAsync(TransactionDto transaction)
        {
            try
            {
                var result = await _transactionService.SaveAsync(transaction);
                if (result.Message!= null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return NoContent();
                }

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.saveTransactionMessage, ex);
                return StatusCode(500, MessageLogErrors.saveTransactionMessage);
            }
        }

        /// <summary>
        /// Grava Transação
        /// </summary>
        /// <param name="transaction">Corpo Transação a ser Gravado</param>
        /// <returns>200 Transação criado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPost]
        [Route("PayTransaction")]
        public async Task<IActionResult> PayTransactionAsync(Transaction transaction)
        {
            try
            {
                var resultPayment = await _transactionService.Payment(transaction);
                if (resultPayment.Message.Any())
                {
                    _logger.LogInformation(resultPayment.Message);
                    return NoContent();
                }

                return Ok("Transação Efetivada");
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.saveTransactionMessage, ex);
                return StatusCode(500, MessageLogErrors.saveTransactionMessage);
            }
        }
    }
}