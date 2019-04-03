using ReactiveUI;

namespace WebinarRx.Sample0
{
    public class ViewModel : ReactiveObject
    {
        private string firstName;
        private string lastName;
        private readonly ObservableAsPropertyHelper<string> fullName;

        public ViewModel()
        {
            fullName = this
                .WhenAnyValue(x => x.FirstName, x => x.LastName, (f, l) => $"{f} {l}")
                .ToProperty(this, x => x.FullName);
        }

        public string FullName => fullName.Value;

        public string FirstName
        {
            get => firstName;
            set => this.RaiseAndSetIfChanged(ref firstName, value);
        }

        public string LastName
        {
            get => lastName;
            set => this.RaiseAndSetIfChanged(ref lastName, value);
        }
    }
}