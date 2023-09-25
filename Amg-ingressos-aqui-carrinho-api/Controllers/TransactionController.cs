using Amg_ingressos_aqui_carrinho_api.Consts;
using Amg_ingressos_aqui_carrinho_api.Dto;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Amg_ingressos_aqui_carrinho_api.Model.Querys;
using Amg_ingressos_aqui_carrinho_api.Enum;
using Amg_ingressos_aqui_carrinho_api.Utils;

namespace Amg_ingressos_aqui_carrinho_api.Controllers
{
    [Route("v1/transaction")]
    [Produces("application/json")]
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
            try
            {
                var result = await _transactionService.GetByIdAsync(id);
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

        /// <summary>
        /// Busca Transação por id
        /// </summary>
        /// <param name="idUser">id do usuário</param>
        /// <returns>200 Transação</returns>
        /// <returns>500 Erro inesperado</returns>
        /// <returns>404 Erro tratado</returns>
        [HttpGet]
        [Route("person/{idUser}")]
        public async Task<IActionResult> GetByUserAsync([FromRoute] string idUser)
        {
            try
            {
                var result = await _transactionService.GetByUserAsync(idUser);
                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return NotFound(result.Message);
                }

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.getByPersonTransactionMessage, ex);
                return StatusCode(500, MessageLogErrors.getByPersonTransactionMessage);
            }
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
        [Route("person/{idUser}/tickets")]
        public async Task<IActionResult> GetByUserEventAsync([FromRoute] string idUser, [FromQuery] string? idEvent)
        {
            try
            {
                var result = await _transactionService.GetByUserEventAsync(idUser, idEvent);
                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return NotFound(result.Message);
                }

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.getByPersonTransactionMessage, ex);
                return StatusCode(500, MessageLogErrors.getByPersonTransactionMessage);
            }
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
            try
            {
                var result = await _transactionService.SaveAsync(transaction);
                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return NotFound(result.Message);
                }

                var resultTransactionIten = _transactionService.SaveTransactionItenAsync(
                                        (result.Data as Transaction).Id,
                                        transaction.IdUser,
                                        transaction.TransactionItensDto);
                                        
                if (resultTransactionIten.Result.Message != null &&
                    !string.IsNullOrEmpty(resultTransactionIten.Result.Message))
                    return NotFound(resultTransactionIten.Result.Message);

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
        /// <param name="transactionDto">Dados para atualizar transacao no stage de person DAta</param>
        /// <returns>200 Transação atualizada</returns>
        /// <returns>500 Erro inesperado</returns>
        /// <returns>404 Erro tratado</returns>
        [HttpPut]
        [Route("confirmPersonData/{idTransaction}")]
        public async Task<IActionResult> UpdateTransactionPersonDataAsync([FromRoute] string idTransaction)
        {
            try
            {

                if (idTransaction == string.Empty)
                    return NotFound("Id Transação é Obrigatório");
                var transactionDb = (_transactionService
                    .GetByIdAsync(idTransaction).Result.Data as List<GetTransaction>)
                    .FirstOrDefault();
                var transaction = new Transaction()
                {
                    Id = idTransaction,
                    IdPerson = transactionDb.IdPerson,
                    Stage = Enum.StageTransactionEnum.PersonData,
                    TotalValue = transactionDb.TotalValue
                };

                if (transactionDb.Stage != StageTransactionEnum.Confirm)
                    return NotFound("Estágio fora do padrão");

                var result = await _transactionService.UpdateAsync(transaction);
                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return NotFound(result.Message);
                }

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.updateTransactionMessage, ex);
                return StatusCode(500, MessageLogErrors.updateTransactionMessage);
            }
        }

        /// <summary>
        /// Grava Transação
        /// </summary>
        /// <param name="transactionDto">Dados para atualizar transacao no stage de person DAta</param>
        /// <returns>200 Transação atualizada</returns>
        /// <returns>500 Erro inesperado</returns>
        /// <returns>404 Erro tratado</returns>
        [HttpPut]
        [Route("confirmTicketsData/{idTransaction}")]
        public async Task<IActionResult> UpdateTransactionTicketsDataAsync([FromRoute] string idTransaction, [FromBody] StageTicketDataDto transactionDto)
        {
            try
            {
                if (idTransaction == string.Empty)
                    return NotFound("Id Transação é Obrigatório");
                var transaction = transactionDto.StageTicketDataDtoToTransaction();
                transaction.Id = idTransaction;
                var transactionDb = (_transactionService
                    .GetByIdAsync(transaction.Id).Result.Data as List<GetTransaction>).FirstOrDefault();

                if (transactionDb.Stage != StageTransactionEnum.PersonData)
                    return NotFound("Estágio fora do padrão");

                transaction.IdPerson = transactionDb.IdPerson;
                transaction.TotalValue = transactionDb.TotalValue;
                
                var result = await _transactionService.UpdateAsync(transaction);
                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return NotFound(result.Message);
                }

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.updateTransactionMessage, ex);
                return StatusCode(500, MessageLogErrors.updateTransactionMessage);
            }
        }

        /// <summary>
        /// Grava Transação
        /// </summary>
        /// <param name="transactionDto">Dados para atualizar transacao no stage de person DAta</param>
        /// <returns>200 Transação atualizada</returns>
        /// <returns>500 Erro inesperado</returns>
        /// <returns>404 Erro tratado</returns>
        [HttpPut]
        [Route("confirmPaymentData/{idTransaction}")]
        public async Task<IActionResult> UpdateTransactionPaymentDataAsync([FromRoute] string idTransaction, [FromBody] PaymentMethod transactionDto)
        {
            try
            {
                if (idTransaction == string.Empty)
                    return NotFound("Id Transação é Obrigatório");
                var transaction = transactionDto.StagePaymentDataDtoToTransaction();
                transaction.Id = idTransaction;
                var transactionDb = (_transactionService
                    .GetByIdAsync(transaction.Id).Result.Data as List<GetTransaction>)
                    .FirstOrDefault();

                if (transactionDb.Stage != StageTransactionEnum.TicketsData &&
                transactionDb.Status != StatusPaymentEnum.ErrorPayment)
                    return NotFound("Estágio fora do padrão");

                transaction.IdPerson = transactionDb.IdPerson;
                transaction.TotalValue = transactionDb.TotalValue;
                transaction.Discount = transactionDb.Discount;
                transaction.Tax = transactionDb.Tax;

                var result = await _transactionService.UpdateAsync(transaction);
                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return NotFound(result.Message);
                }

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.updateTransactionMessage, ex);
                return StatusCode(500, MessageLogErrors.updateTransactionMessage);
            }
        }
        
        /// <summary>
        /// Grava Transação
        /// </summary>
        /// <param name="transactionDto">Corpo Transação a ser Gravado</param>
        /// <returns>200 Transação criado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPut]
        [Route("paymentTransaction/{idTransaction}")]
        public async Task<IActionResult> PaymentTransactionAsync([FromRoute] string idTransaction)
        {
            try
            {
                if (idTransaction == string.Empty)
                    return NotFound("Id Transação é Obrigatório");
                var transactionDb = (_transactionService
                    .GetByIdAsync(idTransaction).Result.Data as List<GetTransaction>)
                    .FirstOrDefault();

                if (transactionDb.Stage != StageTransactionEnum.PaymentData)
                    return NotFound("Estágio fora do padrão");

                var transaction = transactionDb.GeTransactionToTransaction();

                var resultPayment = await _transactionService.Payment(transaction);
                
                transaction.Stage = StageTransactionEnum.PaymentTransaction;

                if (resultPayment.Message != null && resultPayment.Message.Any())
                {
                    _logger.LogInformation(resultPayment.Message);
                    return NotFound(resultPayment.Message);
                }

                return Ok(resultPayment.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.paymentTransactionMessage, ex);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Grava Transação
        /// </summary>
        /// <param name="transaction">Corpo Transação a ser Gravado</param>
        /// <returns>200 Transação criado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPut]
        [Route("finishedTransaction/{idTransaction}")]
        public async Task<IActionResult> FinishedTransactionAsync([FromRoute] string idTransaction)
        {
            try
            {
                var transactionDb = (_transactionService
                    .GetByIdAsync(idTransaction).Result.Data as List<GetTransaction>)
                    .FirstOrDefault();
                var transaction = transactionDb.GeTransactionToTransaction();
                var resultQrcode = await _transactionService.FinishedTransactionAsync(transaction);
                if (resultQrcode.Message != null && resultQrcode.Message.Any())
                {
                    _logger.LogInformation(resultQrcode.Message);
                    return NotFound(resultQrcode.Message);
                }

                return Ok(resultQrcode.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.saveTransactionMessage, ex);
                return StatusCode(500, MessageLogErrors.saveTransactionMessage);
            }
        }
    }
}