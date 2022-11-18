using ReactiveUI;
using WalletWasabi.Fluent.Validation;
using WalletWasabi.Models;

namespace WalletWasabi.Fluent.ViewModels.AddWallet.Create;

public partial class RecoveryWordViewModel : ViewModelBase
{
	[AutoNotify] private bool _isSelected;
	[AutoNotify] private bool _isConfirmed;
	[AutoNotify] private string? _selectedWord;

	public RecoveryWordViewModel(int index, string word)
	{
		Index = index;
		Word = word;

		this.WhenAnyValue(x => x.SelectedWord)
			.Subscribe(_ => ValidateWord());
	}

	public int Index { get; }
	public string Word { get; }

	public void Reset()
	{
		SelectedWord = null;
		IsSelected = false;
		IsConfirmed = false;
	}

	private void ValidateWord()
	{
		IsConfirmed =
			SelectedWord is { } &&
			SelectedWord.Equals(Word, StringComparison.InvariantCultureIgnoreCase);
	}

	public override string ToString()
	{
		return $"{Index}. {Word}";
	}
}
