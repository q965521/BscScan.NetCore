﻿using System.Text.Json;
using BscScan.NetCore.Configuration;
using BscScan.NetCore.Constants;
using BscScan.NetCore.Contracts;
using BscScan.NetCore.Extensions;
using BscScan.NetCore.Models.Request.Account;
using BscScan.NetCore.Models.Response.Account;

namespace BscScan.NetCore.Services;

public class BscScanAccountService : BaseHttpClient, IBscScanAccountService
{
    private readonly string _bscScanModule;
    public BscScanAccountService(HttpClient bscScanHttpClient, BscScanConfiguration bscScanConfiguration) :
        base(bscScanHttpClient, bscScanConfiguration)
    {
        _bscScanModule = BscModule.ACCOUNT.AppendApiKey(bscScanConfiguration.BscScanOptions.Token);
    }

    public async Task<BnbBalance?> GetBnbBalanceAsync(BnbBalanceRequest request)
    {
        var queryParameters = $"{_bscScanModule}{request.ToRequestParameters(AccountModuleAction.BALANCE)}";
        using var response = await BscScanHttpClient.GetAsync($"{queryParameters}")
            .ConfigureAwait(false);

        response.EnsureSuccessStatusCode();
        await using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        var result = await JsonSerializer.DeserializeAsync<BnbBalance>(responseStream);
        return result;
    }

    public async Task<BnbMultipleBalance?> GetMultipleBnbBalanceAsync(MultipleBnbBalanceRequest request)
    {
        var queryParameters = $"{_bscScanModule}{request.ToRequestParameters(AccountModuleAction.BALANCE_MULTI)}";
        using var response = await BscScanHttpClient.GetAsync($"{queryParameters}")
            .ConfigureAwait(false);

        response.EnsureSuccessStatusCode();
        await using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        var result = await JsonSerializer.DeserializeAsync<BnbMultipleBalance>(responseStream);
        return result;
    }

    public async Task<NormalTransaction?> GetNormalTransactionsByAddressAsync(NormalTransactionRequest request)
    {
        var queryParameters = $"{_bscScanModule}{request.ToRequestParameters(AccountModuleAction.TRANSACTION_LIST)}";
        using var response = await BscScanHttpClient.GetAsync($"{queryParameters}")
            .ConfigureAwait(false);

        response.EnsureSuccessStatusCode();
        await using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        var result = await JsonSerializer.DeserializeAsync<NormalTransaction>(responseStream);
        return result;
    }


}