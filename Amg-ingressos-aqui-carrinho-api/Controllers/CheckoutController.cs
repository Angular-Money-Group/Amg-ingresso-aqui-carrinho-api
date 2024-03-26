using Amg_ingressos_aqui_carrinho_api.Dto;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Amg_ingressos_aqui_carrinho_api.Controllers
{
    [Route("v1/checkout")]
    [Produces("application/json")]
    public class CheckoutController : ControllerBase
    {
        private readonly ILogger<CheckoutController> _logger;
        private readonly ICheckoutService _checkoutService;

        public CheckoutController(
            ILogger<CheckoutController> logger,
            ICheckoutService processTransactionService)
        {
            _logger = logger;
            _checkoutService = processTransactionService;
        }

        /// <summary>
        /// Grava Transação
        /// </summary>
        /// <param name="transaction">Dados para criar Transacao</param>
        /// <returns>200 Transação criado</returns>
        /// <returns>500 Erro inesperado</returns>
        /// <returns>404 Erro tratado</returns>
        [HttpPost]
        [Route("confirm")]
        public async Task<IActionResult> SaveTransactionAsync([FromBody] TransactionDto transaction)
        {
            var result = await _checkoutService.ProcessSaveAsync(transaction);
            if (result.Message != null && result.Message.Any())
            {
                _logger.LogInformation(result.Message);
                return NotFound(result.Message);
            }

            return Ok(result.Data);
        }

        /// <summary>
        /// Grava Transação
        /// </summary>
        /// <param name="idTransaction">atualiza status transacao de dados confirmados</param>
        /// <returns>200 Transação atualizada</returns>
        /// <returns>500 Erro inesperado</returns>
        /// <returns>404 Erro tratado</returns>
        [HttpPut]
        [Route("confirmPersonData/{idTransaction}")]
        public async Task<IActionResult> UpdateTransactionPersonDataAsync([FromRoute] string idTransaction)
        {
            var result = await _checkoutService.UpdateTransactionPersonDataAsync(idTransaction);
            if (result.Message != null && result.Message.Any())
            {
                _logger.LogInformation(result.Message);
                return NotFound(result.Message);
            }

            return Ok(result.Data);
        }

        /// <summary>
        /// Grava Transação
        /// </summary>
        /// <param name="idTransaction">id transacao a ser atualizado</param>
        /// <param name="transactionDto">Dados para atualizar transacao no stage de person DAta</param>
        /// <returns>200 Transação atualizada</returns>
        /// <returns>500 Erro inesperado</returns>
        /// <returns>404 Erro tratado</returns>
        [HttpPut]
        [Route("confirmTicketsData/{idTransaction}")]
        public async Task<IActionResult> UpdateTransactionTicketsDataAsync([FromRoute] string idTransaction, [FromBody] StageTicketDataDto transactionDto)
        {
            var result = await _checkoutService.UpdateTransactionTicketsDataAsync(idTransaction, transactionDto);
            if (result.Message != null && result.Message.Any())
            {
                _logger.LogInformation(result.Message);
                return NotFound(result.Message);
            }

            return Ok(result.Data);
        }

        /// <summary>
        /// Grava Transação
        /// </summary>
        /// <param name="idTransaction">id Transacao a ser realizada confirmacao de dados de pagamento</param>
        /// <param name="transactionDto">Dados para atualizar transacao no stage de person DAta</param>
        /// <returns>200 Transação atualizada</returns>
        /// <returns>500 Erro inesperado</returns>
        /// <returns>404 Erro tratado</returns>
        [HttpPut]
        [Route("confirmPaymentData/{idTransaction}")]
        public async Task<IActionResult> UpdateTransactionPaymentDataAsync([FromRoute] string idTransaction, [FromBody] PaymentMethod transactionDto)
        {
            var result = await _checkoutService.UpdateTransactionPaymentDataAsync(idTransaction, transactionDto);
            if (result.Message != null && result.Message.Any())
            {
                _logger.LogInformation(result.Message);
                return NotFound(result.Message);
            }

            return Ok(result.Data);
        }

        /// <summary>
        /// Realiza pagamento da transacao
        /// </summary>
        /// <param name="idTransaction">Id Transacao a se comunicar com cliente financeiro e realizar pagamento</param>
        /// <returns>200 Pagamento realizado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPut]
        [Route("paymentTransaction/{idTransaction}")]
        public async Task<IActionResult> PaymentTransactionAsync([FromRoute] string idTransaction)
        {
            var resultPayment = await _checkoutService.PaymentTransactionAsync(idTransaction);
            if (resultPayment.Message != null && resultPayment.Message.Any())
            {
                _logger.LogInformation(resultPayment.Message);
                return NotFound(resultPayment.Message);
            }

            return Ok(resultPayment.Data);
        }

        /// <summary>
        /// Finaliza uma transacao enviando qr code e alterando seu status para finalizado
        /// </summary>
        /// <param name="idTransaction">id transacao a ser finalizada</param>
        /// <returns>200 Transação finalizada</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPut]
        [Route("finishedTransaction/{idTransaction}")]
        public async Task<IActionResult> FinishedTransactionAsync([FromRoute] string idTransaction)
        {
            var resultQrcode = await _checkoutService.FinishedTransactionAsync(idTransaction);
            if (resultQrcode.Message != null && resultQrcode.Message.Any())
            {
                _logger.LogInformation(resultQrcode.Message);
                return NotFound(resultQrcode.Message);
            }

            return Ok(resultQrcode.Data);
        }

        /// <summary>
        /// cancela a transacao passando status pra cancelada
        /// </summary>
        /// <param name="idTransaction">idTransacao a ser cancelada</param>
        /// <returns>200 Transação cancelada</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPut]
        [Route("cancel/{idTransaction}")]
        public async Task<IActionResult> CancelTransactionAsync([FromRoute] string idTransaction)
        {
            var result = await _checkoutService.CancelTransactionAsync(idTransaction);
            if (result.Message != null && result.Message.Any())
            {
                _logger.LogInformation(result.Message);
                return NotFound(result.Message);
            }

            return Ok(result.Data);
        }

        /// <summary>
        /// cancela a transacao passando status pra cancelada
        /// </summary>
        /// <param name="idTransaction">idTransacao a ser cancelada</param>
        /// <returns>200 Transação cancelada</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("GetRequestPagbank/{idTransaction}")]
        public async Task<IActionResult> GetDataPagbank([FromRoute] string idTransaction)
        {
            var result = await _checkoutService.GetDataPagbank(idTransaction);
            if (result.Message != null && result.Message.Any())
            {
                _logger.LogInformation(result.Message);
                return NotFound(result.Message);
            }

            return Ok(result.Data);
        }
    }
}