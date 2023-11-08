using System.Reactive;
using ReactiveUI;
using WalletWasabi.Fluent.Models.Wallets;
using WalletWasabi.Fluent.Validation;
using WalletWasabi.Fluent.ViewModels.Dialogs.Base;
using WalletWasabi.Models;

namespace WalletWasabi.Fluent.ViewModels.Wallets;

[NavigationMetaData(Title = "Rename Wallet")]
public partial class WalletRenameViewModel : DialogViewModelBase<Unit>
{
	[AutoNotify] private string _newWalletName;

	private WalletRenameViewModel(IWalletModel wallet)
	{
		_newWalletName = wallet.Name;

		this.ValidateProperty(
			x => x.NewWalletName,
			errors =>
			{
				if (string.IsNullOrWhiteSpace(NewWalletName))
				{
					errors.Add(ErrorSeverity.Error, "The name cannot be empty");
				}

				if (NewWalletName.Length > 50)
				{
					errors.Add(ErrorSeverity.Error, "The name is too long");
				}

				if (NewWalletName.TrimStart().Length != NewWalletName.Length)
				{
					errors.Add(ErrorSeverity.Error, "The name should not have leading white spaces");
				}

				if (NewWalletName.TrimEnd().Length != NewWalletName.Length)
				{
					errors.Add(ErrorSeverity.Error, "The name should not have trailing white spaces");
				}
			});

		SetupCancel(enableCancel: true, enableCancelOnEscape: true, enableCancelOnPressed: true);
		var canRename = this.WhenAnyValue(model => model.NewWalletName, selector: newName => !Equals(newName, wallet.Name) && !Validations.Any);
		NextCommand = ReactiveCommand.Create(() => OnRename(wallet), canRename);
	}

	private void OnRename(IWalletModel wallet)
	{
		try
		{
			wallet.Name = NewWalletName;
			Close();
		}
		catch
		{
			UiContext.Navigate().To().ShowErrorDialog($"The wallet cannot be renamed to {NewWalletName}", "Invalid name", "Cannot rename the wallet", NavigationTarget.CompactDialogScreen);
		}
	}
}
