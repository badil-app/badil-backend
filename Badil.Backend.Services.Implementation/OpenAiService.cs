using RestSharp;
using Rystem.OpenAi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Badil.Backend.Services.Implementation
{
    public class OpenAiService(IOpenAiFactory service) : IOpenAiService
    {
        public async Task<string> GenerateSearchTerm(string brandName, string productName)
        {
            var results = await service.CreateChat()
                .RequestWithSystemMessage(@"
You are an AI assistant that, given a brand name and product name, you ONLY RESPOND WITH search terms I can use to get similar products to that one (but not necessarily and preferably not the same brand). Don't be too specific, and give me a response that is one words seperated by comma. Be as little specific as possible because the more items you give, the less results I'll get from the open food facts database. Keep it a little broad. It needs to be single words seperated by commas.")
                .AddUserMessage("Brand: Pepsi,Pepsi-Cola Name: Diet 12floz Can")
                .AddAssistantMessage("cola,diet")
                .AddUserMessage($"Brand: {brandName} Name: {productName}")
                .WithModel("gpt-4-turbo-preview")
                .ExecuteAsync();
            return results.Choices?[0]?.Message?.Content ?? "";
        }
    }
}
