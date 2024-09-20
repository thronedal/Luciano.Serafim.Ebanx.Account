using System;
using Luciano.Serafim.Ebanx.Account.Core.Abstractions.Services;
using Luciano.Serafim.Ebanx.Account.Core.Abstractions.Transactions;
using Luciano.Serafim.Ebanx.Account.Core.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Luciano.Serafim.Ebanx.Account.Infrastructure.MongoDb;

public class AccountService : IAccountService
{
    private IMongoCollection<Core.Models.Account> accountsCollection;
    private IMongoCollection<AccountConsolidatedBalance> consolidatedBalanceCollection;
    private readonly IUnitOfWork unitOfWork;

    public AccountService(IOptions<MongoDBSettings> databaseSettings, IMongoClient mongoClient, IUnitOfWork unitOfWork)
    {
        var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
        accountsCollection = mongoDatabase.GetCollection<Core.Models.Account>("accounts");
        consolidatedBalanceCollection = mongoDatabase.GetCollection<AccountConsolidatedBalance>("accountConsolidatedBalance");
        this.unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task<bool> InitializeState()
    {
        throw new NotImplementedException();        
    }

    /// <inheritdoc/>
    public async Task<bool> AccountExists(int accountId)
    {
        return (await accountsCollection.Find(unitOfWork.GetSession<IClientSessionHandle>(), a => a.Id == accountId).FirstOrDefaultAsync()) is not null;
    }

    /// <inheritdoc/>
    public async Task<AccountConsolidatedBalance> ConsolidateBalance(Core.Models.Account account, DateOnly date, double balance)
    {
        var consolidated = new AccountConsolidatedBalance( account, DateOnly.FromDateTime(DateTime.UtcNow), balance);
        await consolidatedBalanceCollection.InsertOneAsync(unitOfWork.GetSession<IClientSessionHandle>(), consolidated);
        return consolidated;
    }

    /// <inheritdoc/>
    public async Task<Core.Models.Account> CreateAccount(Core.Models.Account account)
    {
        await accountsCollection.InsertOneAsync(unitOfWork.GetSession<IClientSessionHandle>(), account);
        return account;
    }

    /// <inheritdoc/>
    public async Task<Core.Models.Account?> GetAccountById(int accountId)
    {
        return await accountsCollection.Find(unitOfWork.GetSession<IClientSessionHandle>(), a => a.Id == accountId).FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public async Task<AccountConsolidatedBalance> GetLastConsolidatedBalance(int accountId)
    {
        var maxDate = consolidatedBalanceCollection.AsQueryable(unitOfWork.GetSession<IClientSessionHandle>()).Where(c => c.Account.Id == accountId).Max(m => m.BalanceDate);

        var consolidatedBalance = await consolidatedBalanceCollection.Find(unitOfWork.GetSession<IClientSessionHandle>(),c => c.Account.Id == accountId && c.BalanceDate == maxDate).FirstOrDefaultAsync();

        return consolidatedBalance ?? new AccountConsolidatedBalance(new Core.Models.Account(accountId, accountId.ToString()), DateOnly.MinValue, 0);
    }
}
