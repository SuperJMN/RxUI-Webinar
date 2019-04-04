using System;
using System.Net.Http;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;
using ReactiveUI;

namespace WebinarRx.Sample3
{
    public class ViewModel : ReactiveObject
    {
        private string url;
        private readonly ObservableAsPropertyHelper<string> html;

        public ViewModel()
        {
            var canLoad = this.WhenAnyValue(x => x.Url).Select(x => !string.IsNullOrEmpty(x));
            LoadUrlCommand = ReactiveCommand.CreateFromTask(() => LoadHtml(Url), canLoad);
            html = LoadUrlCommand.ToProperty(this, x => x.Html);

            LoadUrlCommand
                .ThrownExceptions
                .Subscribe(async exception => await new MessageDialog("Cannot load the website", "Error").ShowAsync());
        }

        private static async Task<string> LoadHtml(string urlToLoad)
        {
            using (var client = new HttpClient())
            {
                return await client.GetStringAsync(urlToLoad);
            }
        }

        public ReactiveCommand<Unit, string> LoadUrlCommand { get; }

        public string Html => html.Value;

        public string Url
        {
            get => url;
            set => this.RaiseAndSetIfChanged(ref url, value);
        }
    }
}