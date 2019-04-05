﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Search.WebSearch;
using ReactiveUI;

namespace WebinarRx.Sample4
{
    public class ViewModel : ReactiveObject
    {
        private readonly WebSearchClient client;
        private IEnumerable<SearchResult> searchResults;
        private string searchQuery;

        public ViewModel()
        {
            client = new WebSearchClient(new ApiKeyServiceClientCredentials("f891515b49d94b64aa91955aa92c1691"));

            ExecuteSearch = ReactiveCommand.CreateFromTask(Search);
            ExecuteSearch.Subscribe(r => SearchResults = r);

            this.WhenAnyValue(x => x.SearchQuery)
                .Throttle(TimeSpan.FromSeconds(0.8), RxApp.TaskpoolScheduler)
                .Select(query => query?.Trim())
                .DistinctUntilChanged()
                .Where(query => !string.IsNullOrWhiteSpace(query))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Select(s => Unit.Default)
                .InvokeCommand(ExecuteSearch);
        }

        public ReactiveCommand<string, IEnumerable<SearchResult>> ExecuteSearch { get; }


        public IEnumerable<SearchResult> SearchResults
        {
            get => searchResults;
            set => this.RaiseAndSetIfChanged(ref searchResults, value);
        }

        public string SearchQuery
        {
            get => searchQuery;
            set => this.RaiseAndSetIfChanged(ref searchQuery, value);
        }

        private async Task<IEnumerable<SearchResult>> Search(string s)
        {
            var result = await client.Web.SearchAsync(s);
            var results = result.WebPages.Value.Select(x => new SearchResult
            {
                DisplayUrl = x.DisplayUrl,
                Url = x.Url,
                Title = x.Text,
                Description = x.Description
            });

            return results;
        }
    }
}