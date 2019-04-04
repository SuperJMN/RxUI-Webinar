using System;
using ReactiveUI;

namespace WebinarRx.Sample0
{
    public class SimplestViewModel : ReactiveObject
    {
        public SimplestViewModel()
        {
            IObservable<string> nameObservable = this.WhenAnyValue(x => x.Name);
        }

        private string name;

        public string Name
        {
            get => name;
            set => this.RaiseAndSetIfChanged(ref name, value);
        }
    }
}