using System;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;

namespace WebinarRx.Sample2
{
    public class ViewModel : ReactiveObject
    {
        private string message;

        public ViewModel()
        {
            DoSomethingCommand = ReactiveCommand.CreateFromTask(DoSomethingAsync);
            DoSomethingCommand.Subscribe(_ => { this.Message = "Finished"; });
        }

        public string Message
        {
            get => message;
            set => this.RaiseAndSetIfChanged(ref message, value);
        }

        public ReactiveCommand<Unit, Unit> DoSomethingCommand { get; }

        private Task DoSomethingAsync()
        {
            return Task.Delay(1000);
        }
    }
}